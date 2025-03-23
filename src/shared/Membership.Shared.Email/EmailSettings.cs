namespace Membership.Shared.Email;

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; }
    public string SenderUser { get; set; }
    public string SenderPassword { get; set; }
}