using MAUIPong.Models;

namespace MAUIPong.Views;

public class PongDrawable : View, IDrawable
{
   private int playAreaWidth = 400;
   private int playAreaHeight = 500;

   private readonly int playerYOffsetFromPlayAreaBorder = 20;

   public Player Player1 { get; set; }
   public Player Player2 { get; set; }

   public void Draw(ICanvas canvas, RectF dirtyRect)
   {
      //canvas.FontColor = Colors.White;
      //canvas.DrawString("A string drawn by canvas", dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);

      // Draw the area to play in
      canvas.StrokeColor = Colors.White;
      canvas.DrawRectangle(dirtyRect.Center.X - (playAreaWidth / 2), dirtyRect.Center.Y - (playAreaHeight / 2), playAreaWidth, playAreaHeight);

      // Draw player 1
      canvas.StrokeColor = Player1.PlayerColor;
      canvas.FillColor = Player1.PlayerColor;
      canvas.FillRectangle(dirtyRect.Center.X + ((playAreaWidth - Player1.PlayerWidth) / 2) * (float)Player1.XAxis - (Player1.PlayerWidth / 2),
                           dirtyRect.Center.Y - (playAreaHeight / 2) + playAreaHeight - playerYOffsetFromPlayAreaBorder,
                           Player1.PlayerWidth,
                           Player1.PlayerHeight);

      // Draw player 2
      canvas.StrokeColor = Player2.PlayerColor;
      canvas.FillColor = Player2.PlayerColor;
      canvas.FillRectangle(dirtyRect.Center.X - (Player1.PlayerWidth / 2),
                           dirtyRect.Center.Y - (playAreaHeight / 2) + (playerYOffsetFromPlayAreaBorder / 2),
                           Player1.PlayerWidth,
                           Player1.PlayerHeight);

      // Draw the Ball
   }
}
