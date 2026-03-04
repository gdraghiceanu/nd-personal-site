using PersonalSite.Api.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PersonalSite.Api.Services;

public sealed class SendGridEmailService : IEmailService
{
    private readonly ISendGridClient _client;
    private readonly IConfiguration _config;
    private readonly ILogger<SendGridEmailService> _logger;

    public SendGridEmailService(
        ISendGridClient client,
        IConfiguration config,
        ILogger<SendGridEmailService> logger)
    {
        _client = client;
        _config = config;
        _logger = logger;
    }

    public async Task<bool> SendContactEmailAsync(ContactRequest request, CancellationToken ct = default)
    {
        var recipientEmail = _config["Email:RecipientEmail"]!;
        var senderEmail    = _config["Email:SenderEmail"]!;
        var senderName     = _config["Email:SenderName"] ?? "Personal Site";

        var msg = new SendGridMessage
        {
            From        = new EmailAddress(senderEmail, senderName),
            Subject     = $"Message from nataliadraghiceanu.com - {request.Name}",
            PlainTextContent = BuildPlainText(request),
            HtmlContent      = BuildHtml(request)
        };

        msg.AddTo(new EmailAddress(recipientEmail));
        msg.SetReplyTo(new EmailAddress(request.Email, request.Name));

        var response = await _client.SendEmailAsync(msg, ct);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Body.ReadAsStringAsync(ct);
            _logger.LogError("SendGrid error {StatusCode}: {Body}", response.StatusCode, body);
            return false;
        }

        _logger.LogInformation("Contact email sent from {Email}", request.Email);
        return true;
    }

    private static string BuildPlainText(ContactRequest r) =>
        $"Name: {r.Name}\nEmail: {r.Email}\n\n{r.Message}";

    private static string BuildHtml(ContactRequest r) =>
        $"""
        <h2>New contact message</h2>
        <p><strong>Name:</strong> {r.Name}</p>
        <p><strong>Email:</strong> <a href="mailto:{r.Email}">{r.Email}</a></p>
        <hr/>
        <p>{r.Message.Replace("\n", "<br/>")}</p>
        """;
}
