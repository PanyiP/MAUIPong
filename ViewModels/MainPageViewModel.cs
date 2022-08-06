using MAUIPong.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIPong.ViewModels
{
   public class MainPageViewModel : BaseViewModel
   {
      private string iPAddress;
      public string IPAddress
      {
         get => iPAddress;
         set { SetProperty(ref iPAddress, value); }
      }

      public Command StartServerCommand { get; }
      public Command ConnectToServerCommand { get; }
      public Command PlayCommand { get; }

      public MainPageViewModel()
      {
         IPAddress = "127.0.0.1";

         StartServerCommand = new Command(() =>
         {
            TCPServerService server = TCPServerService.Instance;
         });

         ConnectToServerCommand = new Command(() =>
         {
            TCPCLientService client = TCPCLientService.Instance;
            client.IPAddress = IPAddress;
            client.ConnectToTcpServer();
         });

         PlayCommand = new Command(() =>
         {
            Shell.Current.GoToAsync("//GamePage");
         });
      }
   }
}
