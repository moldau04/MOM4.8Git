using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MobilePushNotification
{
    public class AndroidPushNotification
    {
        /// <summary>
        /// Send Push notification to Android Device
        /// </summary>
        /// <param name="serverUrl"> </param>
        /// <param name="message">Message Which send in Phone</param>
        /// <param name="registrationID"> </param>
        /// <param name="pushNotificationMessageType">
        /// Type of pushNotificationMessage(Phone Block-3,Phoen Delete-1,Transaction-2)
        /// </param>
        /// <param name="googleAppID"> </param>
        /// <param name="senderID"> </param>
        /// <param name="dateNTime"> </param>
        /// <returns></returns>
        public string PushToAndroid(string serverUrl, string message, string googleAppID, string senderID,
          string dateNTime, string registrationID)
        {
            string sResponseFromServer = "";

            try
            {
                var value = message;
                WebRequest tRequest;
                tRequest = WebRequest.Create(serverUrl);
                tRequest.Method = "post";
                tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", googleAppID));

                tRequest.Headers.Add(string.Format("Sender: id={0}", senderID));

                string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=false&data.message=" + value +
                    "&data.time=" + dateNTime + "&registration_id=" + registrationID;

                Console.WriteLine(postData);
                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();

                StreamReader tReader = new StreamReader(dataStream);

                sResponseFromServer = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();

                return sResponseFromServer;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }


    public class IOSPushNotification
    {
        /// <summary>
        /// Send Push notification to iOS Device //Added by Nitin Chotwani (ideavate solutions)
        /// </summary>
        /// <param name="serverUrl">The URL for iPhone Push Notification Server</param>
        /// <param name="serverPort">The port for iPhone Push Notification Server.</param>
        /// <param name="certificatePath">The certificate path for iPhone.</param>
        /// <param name="title">Title of the notification</param>
        /// <param name="tokenID">The token id provided by APNS to iPhone device.</param>
        /// <param name="alertMsg">The alert message for push notification.</param>
        /// <param name="phoneId">The phone id of the iPhone device.</param>
        /// <param name="soundFile">Sound file name</param>
        /// <param name="surveyId">Survey Id</param>
        /// <returns></returns>


        public string PushToiPhone(string tokenID, string message, string CertificatePath, string notificationType, string hostname, string strRandom)
        {

            string sResponseFromServer = "1";
            try
            {
                int serverPort = 2195;

                X509Certificate2 clientCertificate = new X509Certificate2(CertificatePath, "", X509KeyStorageFlags.MachineKeySet);
                X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

                TcpClient client = new TcpClient(hostname, serverPort);

                SslStream sslStream = new SslStream(client.GetStream(),
                                                    false,
                                                    new RemoteCertificateValidationCallback(ValidateServerCertificate),
                                                    null);

                try
                {
                    sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Default, false);
                }
                catch (Exception e)
                {
                    return e.Message;
                }

                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);

                writer.Write((byte)0); //------- The command
                writer.Write((byte)0); //------- The first byte of the deviceId length (big-endian first byte)
                writer.Write((byte)32); //------- The deviceId length (big-endian second byte)               
                String deviceID = tokenID;
                writer.Write(HexStringToByteArray(deviceID.ToUpper()));

                //  String payload = "{\"aps\":{\"alert\":\"" + message + "\",\"badge\":0,\"sound\":\"sound.caf\",\"notificationType\" : \"" + notificationType + "\"}}";  

                //String payload = " {\"aps\": { \"alert\" : \"" + message + "\", \"badge\":0, \"sound\":\"\", \"notificationType\" : \"" + notificationType + "\" , \"Random\" : \"" + strRandom + "\", \"content-available\" : 1   }}";

                String payload = " {\"aps\": { \"alert\" : \"" + message + "\", \"badge\":0, \"sound\":\"\", \"notificationType\" : \"\" , \"Random\" : \"" + strRandom + "\", \"content-available\" : 1}}";

                writer.Write((byte)0);
                writer.Write((byte)payload.Length);

                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                writer.Write(b1);
                writer.Flush();

                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();

                client.Close();
                writer.Close();


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return sResponseFromServer;
        }

        /// <summary>
        /// Validates the server certificate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="chain">The chain.</param>
        /// <param name="sslPolicyErrors">The SSL policy errors.</param>
        /// <returns>true if certificate is valid else false</returns>
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Convert the string to byte array.
        /// </summary>
        /// <param name="stringToConvert">The string to convert.</param>
        /// <returns>the byte array</returns>
        public static byte[] HexStringToByteArray(String stringToConvert)
        {
            stringToConvert = stringToConvert.Replace(" ", "");

            byte[] buffer = new byte[stringToConvert.Length / 2];

            for (int i = 0; i < stringToConvert.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(stringToConvert.Substring(i, 2), 16);
            }

            return buffer;
        }


    }
}
