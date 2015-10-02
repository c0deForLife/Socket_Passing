using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Client1VM
{
    class Client1
    {
        static void Main(string[] args)
        {
            byte[] bytes = new byte[1024];
            // Connect to a remote device.
        try {
            // Establish the remote endpoint for the socket.
            // This example uses port 11000 on the local computer.
            string serverIP = args[0]; //"192.168.56.1";
            string client2IP = args[1];//192.168.56.1";
            IPAddress ipAddress = IPAddress.Parse(serverIP); 
            IPEndPoint remoteEP = new IPEndPoint(ipAddress,1010);
            Console.WriteLine("Client1: I will create a socket object to talk to the server");
            // Create a TCP/IP  socket.
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
            // Connect the socket to the remote endpoint. Catch any errors.
            try {
                sender.Connect(remoteEP);
                Console.WriteLine("Client1: Got connected to {0}",sender.RemoteEndPoint.ToString());
                
                
                byte[] msg = Encoding.ASCII.GetBytes("This is Client1 saying hello!\n ");
                // Send the data through the socket.[
                Console.WriteLine("Client1: Sending message to server");
              
                int bytesSent = sender.Send(msg);

                Console.WriteLine("Client1: Message sent successfully! ");
                System.Net.IPHostEntry ipHostEntry = System.Net.Dns.GetHostEntry(client2IP);
                Process clientTwoProcess = Process.GetProcessesByName("Client2VM", ipHostEntry.HostName)[0];

                SocketInformation sinfo = sender.DuplicateAndClose(clientTwoProcess.Id);

                IPAddress client2IPAddress = IPAddress.Parse(client2IP);
                remoteEP = new IPEndPoint(client2IPAddress, 2010);

                // Create a TCP/IP  socket.
                Socket sender1 = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
                sender1.Connect(remoteEP);
                Console.WriteLine("Client1: Let me send the socket object to Client2");             
                bytesSent = sender1.Send(sinfo.ProtocolInformation);
                sender1.Close();
                Console.WriteLine("Client1: GoodBye!");
                Console.ReadLine();

            } catch (ArgumentNullException ane) {
                Console.WriteLine("ArgumentNullException : {0}",ane.ToString());
            } catch (SocketException se) {
                Console.WriteLine("SocketException : {0}",se.ToString());
            } catch (Exception e) {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        } catch (Exception e) {
            Console.WriteLine( e.ToString());
        }
        }
    }
}
