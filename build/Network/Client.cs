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
    /// <summary>
    /// Client class for the Network library.
    /// This class allow the user to start a transmission in order to receive and send requests to a single <see cref="Server"/>.
    /// </summary>
    public class Client
    {
        private static Client instance = null;

        /// <summary>
        /// Getter for the singleton instance of a <see cref="Client"/>
        /// </summary>
        public static Client Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Client();
                }
                return instance;
            }
        }

        private Client() { }

        /// <summary>
        /// Destructor of the <see cref="Client"/>. When called, it'll shutdown all the transmissions
        /// </summary>
        ~Client()
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

        private string _serverIP = null;
        private int _serverPort;

        /// <summary>
        /// Static callback method for the server request
        /// </summary>
        public static Func<Object, int> CallBackFct;
        /// <summary>
        /// Static callback method for the chat server request
        /// </summary>
        public static Func<string, int> MsgCallbackFct;

        /// <summary>
        /// Launch the ClientNetwork
        /// </summary>
        /// <param name="callBackFct">Function called when the client receive a message from the server</param>
        /// <param name="msgCallbackFct">Function called when the client received a chat message from the server</param>
        /// <param name="serverIP">Ip of the server you want to connect to</param>
        /// <param name="serverPort">Port of the server you want to connect to</param>
        public void Start(Func<Object, int> callBackFct, Func<string, int> msgCallbackFct, string serverIP, int serverPort)
        {
            try
            {
                if (_serverIP == null)
                {
                    _serverIP = serverIP;
                    _serverPort = serverPort;
                    CallBackFct = callBackFct;
                    MsgCallbackFct = msgCallbackFct;
                    NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", ClientRequest);
                    NetworkComms.AppendGlobalIncomingPacketHandler<string>("Chat", MsgRequest);
                    Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Send an object to the server (you will minimum need "public string name" in your object
        /// </summary>
        /// <param name="data"></param>
        public void SendDataToServer(Object data)
        {
            try
            {
                NetworkComms.SendObject("Message", _serverIP, _serverPort, JsonConvert.SerializeObject(data));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Send a msg for the chat to the server
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsgChat(string msg)
        {
            try
            {
                NetworkComms.SendObject("Chat", _serverIP, _serverPort, msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Get a msg for the chat from the server
        /// </summary>
        /// <param name="header"></param>
        /// <param name="connection"></param>
        /// <param name="msg"></param>
        public static void MsgRequest(PacketHeader header, Connection connection, string msg)
        {
            MsgCallbackFct(msg);
        }

        /// <summary>
        /// Trigered function when the server want to communicate with the client
        /// </summary>
        /// <param name="header"></param>
        /// <param name="connection"></param>
        /// <param name="data">Data send by the server</param>
        public static void ClientRequest(PacketHeader header, Connection connection, string data)
        {
            if (data.ToString().StartsWith("Error:"))
            {
                CallBackFct(data.ToString());
            }
            else
            {
                dynamic dataObject = JsonConvert.DeserializeObject<dynamic>(data);
                CallBackFct(dataObject);
            }
        }
    }
}
