using System;
using System.Net.Mail;
using System.IO;
using System.Reflection;

public static class MailMessageExt
{
    //private static readonly System.Reflection.BindingFlags Flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
    //private static readonly System.Type MailWriter = typeof(System.Net.Mail.SmtpClient).Assembly.GetType("System.Net.Mail.MailWriter");
    //private static readonly System.Reflection.ConstructorInfo MailWriterConstructor = MailWriter.GetConstructor(Flags, null, new[] { typeof(System.IO.Stream) }, null);
    //private static readonly System.Reflection.MethodInfo CloseMethod = MailWriter.GetMethod("Close", Flags);
    //private static readonly System.Reflection.MethodInfo SendMethod = typeof(System.Net.Mail.MailMessage).GetMethod("Send", Flags);

    ///// <summary>
    ///// A little hack to determine the number of parameters that we
    ///// need to pass to the SaveMethod.
    ///// </summary>
    //private static readonly bool IsRunningInDotNetFourPointFive = SendMethod.GetParameters().Length == 3;

    ///// <summary>
    ///// The raw contents of this MailMessage as a MemoryStream.
    ///// </summary>
    ///// <param name="self">The caller.</param>
    ///// <returns>A MemoryStream with the raw contents of this MailMessage.</returns>
    //public static System.IO.MemoryStream RawMessage(this System.Net.Mail.MailMessage self)
    //{
    //    var result = new System.IO.MemoryStream();
    //    var mailWriter = MailWriterConstructor.Invoke(new object[] { result });
    //    SendMethod.Invoke(self, Flags, null, IsRunningInDotNetFourPointFive ? new[] { mailWriter, true, true } : new[] { mailWriter, true }, null);
    //    result = new System.IO.MemoryStream(result.ToArray());
    //    CloseMethod.Invoke(mailWriter, Flags, null, new object[] { }, null);
    //    return result;
    //}

    public static void Save(this MailMessage Message, string FileName)
    {
        Assembly assembly = typeof(SmtpClient).Assembly;
        Type _mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");

        using (FileStream _fileStream = new FileStream(FileName, FileMode.Create))
        {
            // Get reflection info for MailWriter contructor
            ConstructorInfo _mailWriterContructor =
                _mailWriterType.GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new Type[] { typeof(Stream) },
                    null);

            // Construct MailWriter object with our FileStream
            object _mailWriter = _mailWriterContructor.Invoke(new object[] { _fileStream });

            // Get reflection info for Send() method on MailMessage
            MethodInfo _sendMethod =
                typeof(MailMessage).GetMethod(
                    "Send",
                    BindingFlags.Instance | BindingFlags.NonPublic);

            // Call method passing in MailWriter
            _sendMethod.Invoke(
                Message,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new object[] { _mailWriter, true, true },
                null);

            // Finally get reflection info for Close() method on our MailWriter
            MethodInfo _closeMethod =
                _mailWriter.GetType().GetMethod(
                    "Close",
                    BindingFlags.Instance | BindingFlags.NonPublic);

            // Call close method
            _closeMethod.Invoke(
                _mailWriter,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new object[] { },
                null);
        }
    }
}

