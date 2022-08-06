namespace MAUIPong.Models;

public class Player
{
   public string Name { get; set; } = "Default";
   public Color PlayerColor { get; set; } = Colors.White;

   public double XAxis { get; set; } = 0.5;
   public int PlayerWidth { get; set; } = 100;
   public int PlayerHeight { get; set; } = 10;
   public int Score { get; set; } = 0;
}
