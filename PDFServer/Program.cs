using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpServerExample
{
    static void Main(string[] args)
    {
        // Define the server's IP address and port
        int port = 8080; // Change this to your desired port
        TcpListener server = null;

        try
        {
            // Set up the TCP listener
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine("Server started, waiting for a connection...");

            // Accept a client connection
            using (TcpClient client = server.AcceptTcpClient())
            {
                Console.WriteLine("Client connected.");

                // Get the network stream
                NetworkStream stream = client.GetStream();

                // Buffer for reading data
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received: {0}", message);

                // Send a response back to the client
                string response = "Hello, Client!";
                byte[] data = Encoding.ASCII.GetBytes(response);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", response);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
        }
        finally
        {
            // Stop the server
            server?.Stop();
        }
    }
}