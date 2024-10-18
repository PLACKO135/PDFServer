using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpClientExample
{
    static void Main(string[] args)
    {
        // Define the server address and port
        string serverIp = "127.0.0.1"; // Change this to the server's IP address
        int port = 8080; // Change this to the server's port

        // Array of characters to choose from
        char[] categories = { 'a', 'b', 'c' };
        Random random = new Random();

        try
        {
            // Create a TCP client
            using (TcpClient client = new TcpClient())
            {
                // Connect to the server
                client.Connect(IPAddress.Parse(serverIp), port);
                Console.WriteLine("Connected to the server.");

                // Get the network stream
                NetworkStream stream = client.GetStream();

                // Send a message to the server
                string message = "Hello, Server!";
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);

                // Receive a response from the server
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received: {0}", response);

                // Randomly select a category
                char randomCategory = categories[random.Next(categories.Length)];

                // Create the new message
                string newMessage = response + "added category" +randomCategory;

                // Send the new message back to the server
                byte[] newData = Encoding.ASCII.GetBytes(newMessage);
                stream.Write(newData, 0, newData.Length);
                Console.WriteLine("Sent back: {0}", newMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
        }
    }
}