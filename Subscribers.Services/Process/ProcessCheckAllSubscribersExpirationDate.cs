using Coravel.Invocable;
using Subscribers.Models.Subscribers;
using Subscribers.Services.Services.Interfaces;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace Subscribers.Services.Process;

public class ProcessCheckAllSubscribersExpirationDate : IInvocable
{
    private readonly ISubscribersService _subscribersService;

    public ProcessCheckAllSubscribersExpirationDate(ISubscribersService subscribersService)
    {
        _subscribersService = subscribersService;
    }

    public async Task Invoke()
    {
        var watch = new Stopwatch();
        watch.Start();

        while (true)
        {
            try
            {
                var expiredSubscribers = await _subscribersService.Search(new SubscriberSearch { ExpirationDateTo = DateTime.UtcNow.Date });

                if (!expiredSubscribers.Any())
                {
                    break;
                }

                var tasks = new List<Task>(expiredSubscribers.Count);
                foreach (var subscriber in expiredSubscribers)
                {
                    tasks.Add(ProcessSendEmailForSubscriber(subscriber.Email));
                }

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error message:{ex.Message}");
            }
        }

        watch.Stop();
        Console.WriteLine($"Process: {nameof(ProcessCheckAllSubscribersExpirationDate)} finished");
    }

    private async Task ProcessSendEmailForSubscriber(string receiverEmail)
    {
        var mailMessage = BuildMailMessage(receiverEmail);
        var smtpClient = CreateSmtpClient();
        smtpClient.Send(mailMessage);

        Console.WriteLine($"Email Sent Successfully for subscriber: {receiverEmail}");
    }


    private static MailMessage BuildMailMessage(string receiverEmail)
    {
        MailAddress to = new MailAddress(receiverEmail);
        MailAddress from = new MailAddress("SenderEmail");

        MailMessage mailMessage = new MailMessage(from, to);
        mailMessage.Subject = "Info for AcmeGroup subscriber";
        mailMessage.Body = "Hello, AcmeGroup subscriber. Inform you about the expiry date of the subscription.";

        return mailMessage;
    }

    private static SmtpClient CreateSmtpClient()
    {
        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Host = "smtp.server.address";
        smtpClient.Port = 587;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("smtp_username", "smtp_password");
        smtpClient.EnableSsl = true;

        return smtpClient;
    }

}