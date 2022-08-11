using MAUIPong.Models;
using MAUIPong.Services;
using System.Net;

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

      private string playerName;
      public string PlayerName
      {
         get => playerName;
         set { SetProperty(ref playerName, value); }
      }

      private string messageLabelText;
      public string MessageLabelText
      {
         get => messageLabelText;
         set { SetProperty(ref messageLabelText, value); }
      }

      private bool isPlayButtonEnabled;
      public bool IsPlayButtonEnabled
      {
         get => isPlayButtonEnabled;
         set { SetProperty(ref isPlayButtonEnabled, value); }
      }

      private Player player1;
      public Player Player1
      {
         get => player1;
         set { SetProperty(ref player1, value); }
      }

      public Command StartServerCommand { get; }
      public Command ConnectToServerCommand { get; }
      public Command PlayCommand { get; }

      public MainPageViewModel()
      {
         IPAddress = TCPHelperService.GetLocalAddress();
         IsPlayButtonEnabled = false;

         StartServerCommand = new Command(() =>
         {
            TCPServerService server = TCPServerService.Instance;
            server.OnTCPServerSetupSuccessful += Server_OnTCPServerSetupSuccessful;
            server.IPAddress = IPAddress;
            server.StartServer();
         });

         ConnectToServerCommand = new Command(() =>
         {
            Player1 = new Player()
            {
               Name = PlayerName
            };

            TCPClientService client = TCPClientService.Instance;
            client.OnTCPClientConnectionSuccessful += Client_OnTCPClientConnectionSuccessful;
            client.IPAddress = IPAddress;
            client.ConnectToTcpServer(Player1);
         });

         PlayCommand = new Command(() =>
         {
            Shell.Current.GoToAsync("//GamePage");
         });
      }

      private void Server_OnTCPServerSetupSuccessful(object sender, EventArgs e)
      {
         TCPServerService server = TCPServerService.Instance;
         server.OnTCPServerSetupSuccessful -= Server_OnTCPServerSetupSuccessful;

         MessageLabelText += "Server started!\n";

         ConnectToServerCommand.Execute(this);
      }

      private void Client_OnTCPClientConnectionSuccessful(object sender, TCPClientService.PlayerEventArgs e)
      {
         TCPClientService client = TCPClientService.Instance;
         client.OnTCPClientConnectionSuccessful -= Client_OnTCPClientConnectionSuccessful;

         IsPlayButtonEnabled = true;
         MessageLabelText += $"{e.Player.Name} connected to server!\n";
      }
   }
}
