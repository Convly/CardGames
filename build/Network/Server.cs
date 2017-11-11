using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Network.Lock;
using System.Threading;
using System.Timers;

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
            set
            {
                instance = value;
            }
        }

        public Dictionary<string, InfosClient> Clients { get => clients; set => clients = value; }
        public LockManager Lock_m { get => lock_m; set => lock_m = value; }

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

        private LockManager     lock_m = new LockManager();
        private string          _serverIP;
        private int             _serverPort;
        public int              _currentId = 0;
        private Dictionary<string, InfosClient> clients = new Dictionary<string, InfosClient>();

        public static Func<Object, int>  CallBackFct;

        /// <summary>
        /// Get ip of the pc
        /// </summary>
        /// <returns></returns>
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
                NetworkComms.AppendGlobalIncomingPacketHandler<string>("Chat", MsgRequest);
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

        public void Stop()
        {
            Console.WriteLine("Server stopped!");
            NetworkComms.RemoveGlobalIncomingPacketHandler<string>("Message", ServerRequest);
            NetworkComms.RemoveGlobalIncomingPacketHandler<string>("Chat", MsgRequest);
            this.Clients = new Dictionary<string, InfosClient>();
            Connection.StopListening();
        }

        /// <summary>
        /// Delete a client to the server
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteClient(string name)
        {
            try
            {
                clients.Remove(name);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Send an object to a client
        /// </summary>
        /// <param name="name">Name of the client you want to send something</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public void SendDataToClient(string name, CardGameResources.Net.Packet data)
        {
            try
            {
                InfosClient value;

                if (!Clients.TryGetValue(name, out value))
                {
                    throw new Exception("Can't find player " + name + " in the clients list");
                }

                uint key = this.Lock_m.Add(name);
                data.Key = key;

                NetworkComms.SendObject("Message", value._ip, value._port, JsonConvert.SerializeObject(data));

                this.Lock_m.Lock(key);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                DeleteClient(name);
                throw new Exception("Lead given to the AI");
            }
        }

        /// <summary>
        /// Send a msg to all the clients for the chat
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SendMsgChat(string msg)
        {
            try
            {
                foreach (var client in Clients)
                {
                    NetworkComms.SendObject("Chat", client.Value._ip, client.Value._port, msg);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get a Msg from a client for the chat
        /// </summary>
        /// <param name="header"></param>
        /// <param name="connection"></param>
        /// <param name="msg"></param>
        public static void MsgRequest(PacketHeader header, Connection connection, string msg)
        {
            Server.Instance.SendMsgChat(msg);
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

            bool reg = dataObject.Registration;

            if (reg)
            {
                if (!Server.Instance.Clients.ContainsKey(dataObject.Name.ToString()))
                {
                    InfosClient infosClient = new InfosClient()
                    {
                        _ip = clientIP,
                        _port = clientPort
                    };
                    Server.Instance.Clients.Add(dataObject.Name.ToString(), infosClient);
                }
                else
                {
                    NetworkComms.SendObject("Message", clientIP, clientPort, "Error: Name already exist");
                    return;
                }
            }
            CallBackFct(dataObject);
        }
    }
}
