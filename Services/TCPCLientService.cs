using MAUIPong.Models;
using System.Diagnostics;
using System.Net.Sockets;

namespace MAUIPong.Services;

public class TCPClientService
{
   #region Singleton Pattern
   private TCPClientService()
   {
      PortNumber = 27015;
   }
   public static TCPClientService Instance { get; } = new TCPClientService();
   #endregion

   #region Events
   public class PlayerEventArgs : EventArgs
   {
      public Player Player { get; set; }
   }

   public class BallEventArgs : EventArgs
   {
      public Ball Ball { get; set; }
   }

   public event EventHandler<PlayerEventArgs> OnTCPClientConnectionSuccessful;
   public event EventHandler<PlayerEventArgs> OnTCPClientPlayerDataReceived;
   public event EventHandler<BallEventArgs> OnTCPClientBallDataReceived;
   #endregion

   private TcpClient socketConnection;
   private Thread clientReceiveThread;

   public string IPAddress { get; set; }
   public int PortNumber { get; set; }

   /// <summary> 	
   /// Setup socket connection. 	
   /// </summary> 	
   public void ConnectToTcpServer(Player player)
   {
      try
      {
         socketConnection = new TcpClient(IPAddress, PortNumber);

         TCPMessage msg = new TCPMessage()
         {
            Type = TCPMessageType.PlayerConnected,
            Player = player,
         };
         SendMessage(TCPHelperService.Serialize(msg));

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
         Byte[] bytes = new Byte[1024];
         while (true)
         {
            // Get a stream object for reading
            using NetworkStream stream = socketConnection.GetStream();
            int length;
            // Read incomming stream into byte arrary.
            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
               var incomingData = new byte[length];
               Array.Copy(bytes, 0, incomingData, 0, length);
               // Convert byte array to object
               TCPMessage msg = TCPHelperService.Deserialize<TCPMessage>(incomingData);

               switch (msg.Type)
               {
                  case TCPMessageType.PlayerConnected:
                     OnTCPClientConnectionSuccessful?.Invoke(this, new PlayerEventArgs() { Player = msg.Player });
                     Debug.WriteLine("server message received: PlayerConnected");
                     break;
                  case TCPMessageType.PlayerDisconnected:
                     break;
                  case TCPMessageType.PlayerDataUpdate:
                     OnTCPClientPlayerDataReceived?.Invoke(this, new PlayerEventArgs() { Player = msg.Player });
                     Debug.WriteLine("server message received: PlayerDataUpdate");
                     break;
                  case TCPMessageType.BallDataUpdate:
                     OnTCPClientBallDataReceived?.Invoke(this, new BallEventArgs() { Ball = msg.Ball });
                     Debug.WriteLine("server message received: BallDataUpdate");
                     break;
                  default:
                     break;
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
   public void SendMessage(byte[] message)
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
            // Write byte array to socketConnection stream.
            stream.Write(message, 0, message.Length);
            Debug.WriteLine("Client sent his message - should be received by server");
         }
      }
      catch (SocketException socketException)
      {
         Debug.WriteLine("Socket exception: " + socketException);
      }
   }
}
