// A C# Program for Server
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

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
                    Console.WriteLine("Server Started");
                    Console.WriteLine("Waiting for connection ... ");

                    // Accept incoming connection
                    Socket clientSocket = listener.Accept();

                    // Data buffer
                    byte[] bytes = new Byte[10240];
                    StringBuilder data = new StringBuilder();

                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);
                        data.Append(Encoding.ASCII.GetString(bytes, 0, numByte));

                        // Check for end of message
                        if (data.ToString().IndexOf("<EOF>") > -1)
                            break;
                    }

                    Console.WriteLine("Connected to client");
                    Console.WriteLine("Text received -> {0} ", data.ToString());

                    // Use regex to split while keeping "Synopsis"
                    string pattern = @"(?<=Synopsis:)";
                    string[] vulnerabilities = Regex.Split(data.ToString().TrimEnd("<EOF>".ToCharArray()), pattern);

                    // Create a StringBuilder to hold the new message
                    StringBuilder newMessageBuilder = new StringBuilder();

                    // Process each vulnerability and append a random category before "Synopsis"
                    foreach (var vulnerability in vulnerabilities)
                    {
                        // Trim whitespace
                        string trimmedVulnerability = vulnerability.Trim();
                        if (!string.IsNullOrEmpty(trimmedVulnerability))
                        {
                            // Randomly select a category
                            char randomCategory = categories[random.Next(categories.Length)];

                            // Check if "Synopsis" exists in the vulnerability
                            if (trimmedVulnerability.Contains("Synopsis"))
                            {
                                // Insert the random category before "Synopsis"
                                trimmedVulnerability = trimmedVulnerability.Replace("Synopsis", $"(Category: {randomCategory}) Synopsis");
                            }

                            // Append the modified vulnerability
                            newMessageBuilder.AppendLine(trimmedVulnerability);
                        }
                    }

                    // Send the modified message back to the client
                    byte[] message = Encoding.ASCII.GetBytes(newMessageBuilder.ToString() + "<EOF>");
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