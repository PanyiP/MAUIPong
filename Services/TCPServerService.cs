using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MAUIPong.Services;

public class TCPServerService
{
   #region Singleton Pattern
   private TCPServerService()
   {
      // Start TcpServer background thread 		
      tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
      tcpListenerThread.IsBackground = true;
      tcpListenerThread.Start();
   }
   public static TCPServerService Instance { get; } = new TCPServerService();
   #endregion

   /// <summary> 	
   /// TCPListener to listen for incomming TCP connection 	
   /// requests. 	
   /// </summary> 	
   private TcpListener tcpListener;
   /// <summary> 
   /// Background thread for TcpServer workload. 	
   /// </summary> 	
   private Thread tcpListenerThread;
   /// <summary> 	
   /// Create handle to connected tcp client. 	
   /// </summary> 	
   private TcpClient connectedTcpClient;

   /// <summary> 	
   /// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
   /// </summary> 	
   private void ListenForIncommingRequests()
   {
      try
      {
         // Create listener on localhost port 27015. 			
         tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 27015);
         tcpListener.Start();
         Debug.WriteLine("Server is listening");
         Byte[] bytes = new Byte[1024];
         while (true)
         {
            using (connectedTcpClient = tcpListener.AcceptTcpClient())
            {
               // Get a stream object for reading 					
               using (NetworkStream stream = connectedTcpClient.GetStream())
               {
                  int length;
                  // Read incomming stream into byte arrary. 						
                  while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                  {
                     var incommingData = new byte[length];
                     Array.Copy(bytes, 0, incommingData, 0, length);
                     // Convert byte array to string message. 							
                     string clientMessage = Encoding.ASCII.GetString(incommingData);
                     Debug.WriteLine("client message received as: " + clientMessage);
                  }
               }
            }
         }
      }
      catch (SocketException socketException)
      {
         Debug.WriteLine("SocketException " + socketException.ToString());
      }
   }

   /// <summary> 	
   /// Send message to client using socket connection. 	
   /// </summary> 	
   public void SendMessage(string serverMessage = "This is a message from your server.")
   {
      if (connectedTcpClient == null)
      {
         return;
      }

      try
      {
         // Get a stream object for writing. 			
         NetworkStream stream = connectedTcpClient.GetStream();
         if (stream.CanWrite)
         {
            // Convert string message to byte array.                 
            byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
            // Write byte array to socketConnection stream.               
            stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
            Debug.WriteLine("Server sent his message - should be received by client");
         }
      }
      catch (SocketException socketException)
      {
         Debug.WriteLine("Socket exception: " + socketException);
      }
   }
}
