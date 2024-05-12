namespace inmobiliaria.Servicio
{
    public interface ISender
    {
        bool SendEmail(string destinatario, string asunto, string mensajeHtml);
    }
}
