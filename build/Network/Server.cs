using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Network
{
    public class InfosClient
    {
        public string   _ip;
        public int      _port;
    }

    public class Server
    {
        private static Server instance = null;
        public static Server Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Server();
                }
                return instance;
            }
        }

        public Dictionary<string, InfosClient> Clients { get => clients; set => clients = value; }

        private Server() { }

        ~Server()
        {
            try
            {
                NetworkComms.Shutdown();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private string          _serverIP;
        private int             _serverPort;
        public int              _currentId = 0;
        private Dictionary<string, InfosClient> clients = new Dictionary<string, InfosClient>();

        public static Func<Object, int>  CallBackFct;

        public IPAddress GetIpAddr()
        {
            foreach (IPAddress addr in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    return addr;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callBackFct">Function called when the client receive a request from the client</param>
        /// <returns></returns>
        public string Start(Func<Object, int> callBackFct)
        {
            try
            {
                CallBackFct = callBackFct;
                NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", ServerRequest);
                _serverIP = GetIpAddr().ToString();
                _serverPort = 8989;
                Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(IPAddress.Parse(_serverIP), _serverPort));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return null;
            }
            return _serverIP + ":" + _serverPort;
        }

        /// <summary>
        /// Send an object to a client
        /// </summary>
        /// <param name="name">Name of the client you want to send something</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool sendDataToClient(string name, Object data)
        {
            InfosClient value;
            if (!Clients.TryGetValue(name, out value))
            {
                return false;
            }
            NetworkComms.SendObject("Message", value._ip, value._port, JsonConvert.SerializeObject(data));
            return true;
        }

        /// <summary>
        /// Send an object to all client
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SendToAllClient(Object data)
        {
            foreach (var user in this.Clients)
            {
                InfosClient value = user.Value;
                NetworkComms.SendObject("Message", value._ip, value._port, JsonConvert.SerializeObject(data));
            }
            return true;
        }

        /// <summary>
        /// Function trigered by the server when the client send a request
        /// </summary>
        /// <param name="header"></param>
        /// <param name="connection"></param>
        /// <param name="data">Data sent by the client</param>
        public static void  ServerRequest(PacketHeader header, Connection connection, string data)
        {
            string  clientIP = connection.ConnectionInfo.RemoteEndPoint.ToString().Split(':').First();
            int     clientPort = int.Parse(connection.ConnectionInfo.RemoteEndPoint.ToString().Split(':').Last());
            dynamic dataObject = JsonConvert.DeserializeObject<dynamic>(data);

            if (!Server.Instance.Clients.ContainsKey(dataObject.Name.ToString()))
            {
                InfosClient infosClient = new InfosClient()
                {
                    _ip = clientIP,
                    _port = clientPort
                };
                Server.Instance.Clients.Add(dataObject.Name.ToString(), infosClient);
            }
            CallBackFct(dataObject);
        }
    }
}
