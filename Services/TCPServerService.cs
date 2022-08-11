using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace MAUIPong.Services;

public class TCPServerService
{
   #region Singleton Pattern
   private TCPServerService()
   {
      connectedTcpClients = new List<TcpClient>();
      PortNumber = 27015;
   }
   public static TCPServerService Instance { get; } = new TCPServerService();
   #endregion

   #region Events
   public event EventHandler<EventArgs> OnTCPServerSetupSuccessful;
   #endregion

   /// <summary>
   /// TCPListener to listen for incomming TCP connection requests.
   /// </summary>
   private TcpListener tcpListener;
   /// <summary>
   /// Background thread for TcpServer workload.
   /// </summary>
   private Thread tcpListenerThread;
   /// <summary>
   /// Create handle to connected tcp client.
   /// </summary>
   private readonly List<TcpClient> connectedTcpClients;

   public string IPAddress { get; set; }
   public int PortNumber { get; set; }

   public void StartServer()
   {
      // Start TcpServer background thread
      tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
      tcpListenerThread.IsBackground = true;
      tcpListenerThread.Start();
   }

   /// <summary>
   /// Runs in background TcpServerThread; Handles incomming TcpClient requests
   /// </summary>
   private void ListenForIncommingRequests()
   {
      try
      {
         // Create listener on IPAddress and PortNumber.
         tcpListener = new TcpListener(System.Net.IPAddress.Parse(IPAddress), PortNumber);
         tcpListener.Start();
         OnTCPServerSetupSuccessful?.Invoke(this, new EventArgs());
         Debug.WriteLine($"Server is listening on {IPAddress}:{PortNumber}");
         // Read incoming data
         Byte[] bytes = new Byte[1024];
         while (true)
         {
            connectedTcpClients.Add(tcpListener.AcceptTcpClient());
            foreach (var readDataFromTCPClient in connectedTcpClients)
            {
               // Get a stream object for reading
               using NetworkStream stream = readDataFromTCPClient.GetStream();
               int length;
               // Read incomming stream into byte arrary.
               while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
               {
                  var incommingData = new byte[length];
                  Array.Copy(bytes, 0, incommingData, 0, length);

                  foreach (var sendDataToAllOtherTCPClient in connectedTcpClients)
                  {
                     if (!readDataFromTCPClient.Equals(sendDataToAllOtherTCPClient))
                     {
                        SendMessage(sendDataToAllOtherTCPClient, incommingData);
                     }
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
   private void SendMessage(TcpClient client, byte[] message)
   {
      try
      {
         // Get a stream object for writing.
         NetworkStream stream = client.GetStream();
         if (stream.CanWrite)
         {
            // Write byte array to socketConnection stream.
            stream.Write(message, 0, message.Length);
            Debug.WriteLine("Server sent his message - should be received by client");
         }
      }
      catch (SocketException socketException)
      {
         Debug.WriteLine("Socket exception: " + socketException);
      }
   }
}
