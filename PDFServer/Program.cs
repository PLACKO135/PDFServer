// A C# Program for Server
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        // Main Method
        static void Main(string[] args)
        {
            ExecuteServer();
        }

        public static void ExecuteServer()
        {
            // Establish the local endpoint 
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

            // Creation TCP/IP Socket using 
            // Socket Class Constructor
            Socket listener = new Socket(ipAddr.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

            // Array of categories to choose from
            char[] categories = { 'a', 'b', 'c' };
            Random random = new Random();

            try
            {
                // Bind the socket
                listener.Bind(localEndPoint);

                // Listen for incoming connections
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for connection ... ");

                    // Accept incoming connection
                    Socket clientSocket = listener.Accept();

                    // Data buffer
                    byte[] bytes = new Byte[1024];
                    string data = null;

                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, numByte);

                        // Check for end of message
                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    Console.WriteLine("Text received -> {0} ", data);

                    // Randomly select a category
                    char randomCategory = categories[random.Next(categories.Length)];

                    // Create the new message
                    string newMessage = data.TrimEnd("<EOF>".ToCharArray()) + randomCategory;

                    // Send the modified message back to the client
                    byte[] message = Encoding.ASCII.GetBytes(newMessage);
                    clientSocket.Send(message);

                    // Close client Socket
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}