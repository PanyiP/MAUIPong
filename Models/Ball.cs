namespace MAUIPong.Models;

[Serializable]
public class Ball
{
   public readonly int ballRadius = 10;
   public Point ballDistanceFromCenter = new Point(0, 0); // the position of the ball on the canvas

   private double ballSpeedRatio = 1;
   public double ballSpeed;

   public Ball() { }
   public Ball(double fps)
   {
      ballSpeed = ballSpeedRatio / fps;
   }
}
