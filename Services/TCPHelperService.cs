using System.Net;
using System.Xml.Serialization;

namespace MAUIPong.Services;

public static class TCPHelperService
{
   public static byte[] Serialize<T>(T anySerializableObject)
   {
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      MemoryStream stream = new MemoryStream();
      serializer.Serialize(stream, anySerializableObject);
      return stream.ToArray();
   }

   public static T Deserialize<T>(byte[] message)
   {
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      MemoryStream stream = new MemoryStream(message);

      object result = serializer.Deserialize(stream);

      return (T)result;
   }

   public static string GetLocalAddress()
   {
      IPAddress[] IpAddress;
      if (DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
      {
         IpAddress = Dns.GetHostAddresses(Dns.GetHostName());
      }
      else
      {
         IpAddress = new IPAddress[0];// TODO: Figure out what is the issue on mobile
      }

      foreach (var item in IpAddress)
      {
         if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            return item.ToString();
      }

      return "127.0.0.1";
   }
}
