namespace inmobiliaria.Servicio;
using System;
using System.Net.Mail;
using System.Net;

public class EmailSender : ISender
{
    private string _password = "leotoloza1133466839";

    public bool SendEmail(string destinatario, string asunto, string mensajeHtml)
    {
        string from = "leotoloza@alwaysdata.net";
        string displayName = "Inmobiliaria T&L";
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from, displayName);
            mail.To.Add(destinatario);

            mail.Subject = asunto;
            mail.Body = mensajeHtml;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp-leotoloza.alwaysdata.net", 25);
            client.Credentials = new NetworkCredential(from, _password);
            client.EnableSsl = false;

            client.Send(mail);
            return true;
        }
        catch (SmtpException ex)
        {
            throw new Exception("Error al enviar el email: " + ex.Message);
        }
    }
}
