using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace ClientApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", PrintIncomingMessage);
            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));

            Console.WriteLine("Please enter the server IP and port in the format xxx.xxx.xxx.xxx:xxxx:");
            string serverInfo = Console.ReadLine();

            string serverIP = serverInfo.Split(':').First();
            int serverPort = int.Parse(serverInfo.Split(':').Last());

            while (true)
            {
                Console.Write("Enter a message:");
                string messageToSend = Console.ReadLine();
                Console.WriteLine("Sending message to server saying '" + messageToSend + "'");

                NetworkComms.SendObject("Message", serverIP, serverPort, messageToSend);

                //Check if user wants to go around the loop
                Console.WriteLine("\nPress q to quit or any other message to send again.");
                if (Console.ReadKey(true).Key == ConsoleKey.Q)
                    break;
            }

            //We have used comms so we make sure to call shutdown
            NetworkComms.Shutdown();
        }

        private static void PrintIncomingMessage(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine("\nA message was received from " + connection.ToString() + " which said '" + message + "'.");
        }
    }
}