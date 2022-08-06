using MAUIPong.Models;

namespace MAUIPong.Views;

public class PongGraphicsView : GraphicsView
{
   private const double fps = 50;

   public static readonly BindableProperty Player1Property = BindableProperty.Create(nameof(Player1),
     typeof(Player),
     typeof(PongGraphicsView),
     propertyChanged: (b, o, n) =>
     {
        Drawable.Player1 = (Player)n;
     });

   public static readonly BindableProperty Player2Property = BindableProperty.Create(nameof(Player1),
     typeof(Player),
     typeof(PongGraphicsView),
     propertyChanged: (b, o, n) =>
     {
        Drawable.Player2 = (Player)n;
     });

   public static readonly BindableProperty Player1XAxisProperty = BindableProperty.Create(nameof(Player1XAxis),
     typeof(double),
     typeof(PongGraphicsView),
     0.5,
     propertyChanged: (b, o, n) => {
        Drawable.Player1.XAxis = (double)n;
     });

   public double Player1XAxis
   {
      get => (double)GetValue(Player1XAxisProperty);
      set => SetValue(Player1XAxisProperty, value);
   }

   public Player Player1
   {
      get => (Player)GetValue(Player1Property);
      set => SetValue(Player1Property, value);
   }

   public Player Player2
   {
      get => (Player)GetValue(Player2Property);
      set => SetValue(Player2Property, value);
   }

   public static new PongDrawable Drawable;

   public PongGraphicsView()
   {
      base.Drawable = Drawable = new PongDrawable();

      var ms = 1000.0 / fps;
      var ts = TimeSpan.FromMilliseconds(ms);
      Dispatcher.StartTimer(ts, TimerLoop);
   }

   private bool TimerLoop()
   {
      Invalidate();

      return true;
   }
}
