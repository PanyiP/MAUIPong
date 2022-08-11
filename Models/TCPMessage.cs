namespace MAUIPong.Models;

public enum TCPMessageType
{
   PlayerConnected = 0,
   PlayerDisconnected = 1,
   PlayerDataUpdate = 2,
   BallDataUpdate = 3,
}

public class TCPMessage
{
   public TCPMessageType Type { get; set; }

   public Player Player { get; set; }

   public Ball Ball { get; set; }
}
