using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ServerVM
{
    class Server
    {
        static void Main(string[] args)
        {
             byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
             string serverIP = args[0];// "192.168.56.1";
             IPAddress ipAddress = IPAddress.Parse(serverIP);
             IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1010);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("This is a Server ");
            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Server: Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket handler = listener.Accept();
                    string data = null;
                    // An incoming connection needs to be processed.                   
                        bytes = new byte[1024];
                        while (true)
                        {
                            
                            int bytesRec = handler.Receive(bytes);
                            data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            if (data.IndexOf("<EOF>") > -1)
                            {
                                Console.WriteLine("Client is done sending all the text");
                                break;
                            }
                            if (bytesRec == 0)
                            {
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Text received from client: " + data);
                                bytesRec = 0;
                                data = "";
                            }
                        }                   
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.Read();
        }
    }
}