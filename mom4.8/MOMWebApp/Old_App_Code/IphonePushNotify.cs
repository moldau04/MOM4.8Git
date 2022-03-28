using System;
using System.Security.Cryptography.X509Certificates;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.IO;

/// <summary>
/// Summary description for IphonePushNotify
/// </summary>
public class IphonePushNotify
{
    public IphonePushNotify()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string PushToiPhone(string deviceId, string alertMsg)
    {
        int port = 2195;
        String hostname = "gateway.sandbox.push.apple.com";

        //------- Without the last argument it works only locally, but if we deploy or publish it, this code doesn't work ------- !
        X509Certificate2 clientCertificate = new X509Certificate2(System.Web.HttpContext.Current.Server.MapPath("~/APNS-Cert.p12"), "ideavate123", X509KeyStorageFlags.MachineKeySet);

        X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

        TcpClient client = new TcpClient(hostname, port);

        SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

        try
        {
            sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Default, false);
        }
        catch (Exception e)
        {
            string exce = e.Message.ToString() + "-------" + e.StackTrace.ToString() + "-------" + e.InnerException.ToString();

            client.Close();

            return "failure";
        }

        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);

        writer.Write((byte)0);  //------- The command
        writer.Write((byte)0);  //------- The first byte of the deviceId length (big-endian first byte)
        writer.Write((byte)32); //------- The deviceId length (big-endian second byte)

        String deviceID = deviceId; //"30b0bc753a8da842524e540fab4530df2b932beb1cd4e7200953f28d5ce031d3";
        writer.Write(HexStringToByteArray(deviceID.ToUpper()));

        String payload = "{\"aps\":{\"alert\":\"" + alertMsg + "\",\"badge\":0,\"sound\":\"default\"}}";

        // also sending push notification with custom button ....... !
        //String payload = "{\"aps\":{\"alert\":{ \"body\":\"" + alertMsg + "\", \"action-loc-key\" : \"Ga naar app!\" },\"badge\":0,\"sound\":\"default\"}}";

        // String payload = "{\"aps\":{\"alert\":{ \"body\":" + alertMsg + "},\"badge\":0,\"sound\":\"default\"}}";

        //string payload = "{\"aps\":{\"alert\":{ \"body\":\" alertMsg\","action-loc-key" :"Ga naar app!" },"badge":0,"sound":"default"}}";
        writer.Write((byte)0);
        writer.Write((byte)payload.Length);

        byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
        writer.Write(b1);
        writer.Flush();

        byte[] array = memoryStream.ToArray();
        sslStream.Write(array);
        sslStream.Flush();

        client.Close();

        return "successfully sended to iPhone";
    }

    public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    public static byte[] HexStringToByteArray(String s)
    {
        s = s.Replace(" ", "");

        byte[] buffer = new byte[s.Length / 2];

        for (int i = 0; i < s.Length; i += 2)
        {
            buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
        }

        return buffer;
    }
}
