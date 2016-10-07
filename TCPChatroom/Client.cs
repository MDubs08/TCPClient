using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace TCPChatroom
{
    class Client
    {
        private TcpClient client;
        StreamReader reader;
        StreamWriter writer;
        public Client()
        {
            client = new TcpClient();
        }
        public void Connect()
        {
            try
            {
                Console.WriteLine("Connecting...");
                client.Connect("10.2.20.26", 8008);
                EndPoint endPoint = client.Client.RemoteEndPoint;

                if (client.Connected)
                {
                    Console.WriteLine("Entered {0}", endPoint);
                    reader = new StreamReader(client.GetStream());
                    writer = new StreamWriter(client.GetStream());
                    Thread GetMessages = new Thread(getMessages);
                    GetMessages.Start();
                    Thread SendMessages = new Thread(sendMessages);
                    SendMessages.Start();
                }
                else
                {
                    Console.WriteLine("Unable to connect to server {0}", endPoint);
                    Console.WriteLine("Trying to Reconnect...");
                    Connect();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..." + e.Message );
                reader.Close();
            }
        }
        private void getMessages()
        {
            while (client.Connected)
            {
                try
                {
                    string serverMessage = reader.ReadLine();
                    Console.WriteLine(serverMessage);
                    reader.DiscardBufferedData();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        private void sendMessages()
        {
            while (client.Connected)
            {
                try
                {
                    string userMessage = Console.ReadLine();
                    writer.WriteLine(userMessage);
                    writer.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}