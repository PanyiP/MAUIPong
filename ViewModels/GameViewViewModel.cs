using MAUIPong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIPong.ViewModels
{
   public class GameViewViewModel : BaseViewModel
   {

      private Player player1;
      public Player Player1
      {
         get => player1;
         set { SetProperty(ref player1, value); }
      }

      private Player player2;
      public Player Player2
      {
         get => player2;
         set { SetProperty(ref player2, value); }
      }

      public GameViewViewModel()
      {
         player1 = new Player
         {
            Name = "Málna úr",
            PlayerColor = Colors.OrangeRed
         };

         player2 = new Player
         {
            Name = "MR. ANDERSON",
            PlayerColor = Colors.LightGoldenrodYellow
         };
      }
   }
}
