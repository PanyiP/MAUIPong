using MAUIPong.Models;

namespace MAUIPong.Views;

public class PongDrawable : View, IDrawable
{
   private int playAreaWidth = 400;
   private int playAreaHeight = 500;

   private readonly int playerYOffsetFromPlayAreaBorder = 20;

   public Ball Ball { get; set; }

   public Player Player1 { get; set; }
   public Player Player2 { get; set; }

   public PongDrawable(double fps)
   {
      Ball = new Ball(fps);
   }

   public void Draw(ICanvas canvas, RectF dirtyRect)
   {
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
      canvas.StrokeColor = Player2.PlayerColor;
      canvas.FillColor = Player2.PlayerColor;
      canvas.FillCircle(dirtyRect.Center.X + (float)Ball.ballDistanceFromCenter.X, dirtyRect.Center.Y + (float)Ball.ballDistanceFromCenter.Y, Ball.ballRadius);

      // Get points of the ball
      Point bottomOfTheBall = new Point(dirtyRect.Center.X + (float)Ball.ballDistanceFromCenter.X,
                                        dirtyRect.Center.Y + (float)Ball.ballDistanceFromCenter.Y + Ball.ballRadius);
      Point leftSideOfTheBall = new Point(dirtyRect.Center.X + (float)Ball.ballDistanceFromCenter.X - Ball.ballRadius,
                                          dirtyRect.Center.Y + (float)Ball.ballDistanceFromCenter.Y);
      Point rightSideOfTheBall = new Point(dirtyRect.Center.X + (float)Ball.ballDistanceFromCenter.X + Ball.ballRadius,
                                           dirtyRect.Center.Y + (float)Ball.ballDistanceFromCenter.Y);

      // Check if ball hits the left side of the play area
      if (leftSideOfTheBall.X <= dirtyRect.Center.X - (playAreaWidth / 2))
      {

      }

      // Check if ball hits the right side of the play area
      if (rightSideOfTheBall.X >= dirtyRect.Center.X + (playAreaWidth / 2))
      {

      }

      // Check if ball hits the bottom of the play area
      if (bottomOfTheBall.Y >= dirtyRect.Center.Y + (playAreaHeight / 2))
      {
         canvas.FontColor = Colors.White;
         canvas.DrawString("Vesztettél", dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);
      }
   }
}
