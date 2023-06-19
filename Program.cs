using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    [Obsolete]
    static string GetIP() {
        try
        {
            using (var client = new WebClient())
            {
                return client.DownloadString("https://api.ipify.org");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            return string.Empty;
        }
    }

    static void Main()
    {
        string ip = GetIP();
        Console.WriteLine("Your public IPv4 address is: " + ip);


        // Start a thread to act as the server
        var serverThread = new Thread(new ParameterizedThreadStart((obj) =>
        {
            StartServer((string)obj);
        }));
        
        serverThread.Start(ip);

        // Wait for a moment to ensure the server is running
        Thread.Sleep(1000);

        // Start a thread to act as the client
        var clientThread = new Thread(StartClient);
        clientThread.Start();

        Console.ReadLine();
    }

    static void StartServer(string ip)
    {
        try
        {
            Console.WriteLine(ip);
            // Set up the server endpoint
            var serverIP = IPAddress.Parse("127.0.0.1");
            var serverPort = 12345;
            var serverEndPoint = new IPEndPoint(serverIP, serverPort);

            // Create the server socket and bind it to the endpoint
            var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(serverEndPoint);
            serverSocket.Listen(1);

            Console.WriteLine("Server started. Waiting for a client connection...");

            // Accept a client connection
            var clientSocket = serverSocket.Accept();
            Console.WriteLine("Client connected.");

            // Start receiving messages from the client
            var receiveThread = new Thread(() => ReceiveMessages(clientSocket));
            receiveThread.Start();

            // Send messages to the client
            SendMessages(clientSocket);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred in the server: " + ex.Message);
        }
    }

    static void StartClient()
    {
        try
        {
            // Set up the server endpoint
            var serverIP = IPAddress.Parse("127.0.0.1");
            var serverPort = 12345;
            var serverEndPoint = new IPEndPoint(serverIP, serverPort);

            // Create the client socket and connect to the server
            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(serverEndPoint);

            Console.WriteLine("Client connected to the server.");

            // Start receiving messages from the server
            var receiveThread = new Thread(() => ReceiveMessages(clientSocket));
            receiveThread.Start();

            // Send messages to the server
            SendMessages(clientSocket);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred in the client: " + ex.Message);
        }
    }

    static void SendMessages(Socket socket)
    {
        try
        {
            while (true)
            {
                // Read input from the console
                var message = Console.ReadLine();

                // Convert the message to bytes
                var messageBytes = Encoding.ASCII.GetBytes(message);

                // Send the message to the peer
                socket.Send(messageBytes);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while sending a message: " + ex.Message);
        }
    }

    static void ReceiveMessages(Socket socket)
    {
        try
        {
            while (true)
            {
                // Create a buffer to hold the received data
                var buffer = new byte[1024];

                // Receive data from the peer
                var bytesRead = socket.Receive(buffer);

                // Convert the received data to a string
                var message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                Console.WriteLine("Received message: " + message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while receiving a message: " + ex.Message);
        }
    }
}