using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace MAUIPong.Services;

public class TCPCLientService
{
   #region Singleton Pattern
   private TCPCLientService()
   {
      IPAddress = "127.0.0.1";
      PortNumber = 27015;
   }
   public static TCPCLientService Instance { get; } = new TCPCLientService();
   #endregion

   private TcpClient socketConnection;
   private Thread clientReceiveThread;

   public string IPAddress { get; set; }
   public int PortNumber { get; set; }

   /// <summary> 	
   /// Setup socket connection. 	
   /// </summary> 	
   public void ConnectToTcpServer()
   {
      try
      {
         clientReceiveThread = new Thread(new ThreadStart(ListenForData));
         clientReceiveThread.IsBackground = true;
         clientReceiveThread.Start();
      }
      catch (Exception e)
      {
         Debug.WriteLine("On client connect exception " + e);
      }
   }

   /// <summary> 	
   /// Runs in background clientReceiveThread; Listens for incomming data. 	
   /// </summary>     
   private void ListenForData()
   {
      try
      {
         socketConnection = new TcpClient(IPAddress, PortNumber);
         Byte[] bytes = new Byte[1024];
         while (true)
         {
            // Get a stream object for reading 				
            using (NetworkStream stream = socketConnection.GetStream())
            {
               int length;
               // Read incomming stream into byte arrary. 					
               while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
               {
                  var incommingData = new byte[length];
                  Array.Copy(bytes, 0, incommingData, 0, length);
                  // Convert byte array to string message. 						
                  string serverMessage = Encoding.ASCII.GetString(incommingData);
                  Debug.WriteLine("server message received as: " + serverMessage);
               }
            }
         }
      }
      catch (SocketException socketException)
      {
         Debug.WriteLine("Socket exception: " + socketException);
      }
   }

   /// <summary> 	
   /// Send message to server using socket connection. 	
   /// </summary> 	
   public void SendMessage(string clientMessage = "This is a message from one of your clients.")
   {
      if (socketConnection == null)
      {
         return;
      }
      try
      {
         // Get a stream object for writing. 			
         NetworkStream stream = socketConnection.GetStream();
         if (stream.CanWrite)
         {
            // Convert string message to byte array.                 
            byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
            // Write byte array to socketConnection stream.                 
            stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            Debug.WriteLine("Client sent his message - should be received by server");
         }
      }
      catch (SocketException socketException)
      {
         Debug.WriteLine("Socket exception: " + socketException);
      }
   }
}
