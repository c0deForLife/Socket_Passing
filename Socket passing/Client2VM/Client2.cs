using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client2VM
{
    class Client2
    {
        static void Main(string[] args)
        {
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // host running the application.
            string client2IP = args[0];
            IPAddress client2IPAddress = IPAddress.Parse(client2IP);
            IPEndPoint localEndPoint = new IPEndPoint(client2IPAddress, 2010);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.

                Console.WriteLine("Client2: I am waiting for Client1 to send me the socket object.");
                // Program is suspended while waiting for an incoming connection.
                Socket handler = listener.Accept();

                // An incoming connection needs to be processed.

                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                
                SocketInformation sockInfo = new SocketInformation();
                byte[] protocolInfo = new byte[bytesRec];
                Array.Copy(bytes, protocolInfo, bytesRec);
                sockInfo.ProtocolInformation = protocolInfo;
                
                listener.Close();
                listener = null;

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

                Console.WriteLine("Client2: Successfully received the socket object.");

                Socket duplicatedSocket = new Socket(sockInfo);
                
                Console.WriteLine("Client2: I am already connected to server {0}",duplicatedSocket.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.

                byte[] msg = Encoding.ASCII.GetBytes("Client2 says hey there!\n ");
                // Send the data through the socket.
                Console.WriteLine("Client2: Let me send a  message to the Server");
                int bytesSent = duplicatedSocket.Send(msg);
                msg = Encoding.ASCII.GetBytes("<EOF>");
                duplicatedSocket.Send(msg);
                Console.WriteLine("Client2: Message sent to the server successfully!");
                Console.WriteLine("Client2: Closing the socket");
                duplicatedSocket.Shutdown(SocketShutdown.Both);
                duplicatedSocket.Close();            

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.Read();
        }
    }
}
