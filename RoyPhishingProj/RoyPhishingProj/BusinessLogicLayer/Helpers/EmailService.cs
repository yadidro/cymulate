using System.Security.Cryptography;
using System.Text;
using RoyPhishingProj.BusinessLogicLayer.dto;
using RoyPhishingProj.DBLayer;
using mailslurp.Api;
using mailslurp.Client;
using mailslurp.Model;
using RoyPhishingProj.BusinessLogicLayer.Enums;

public interface IEmailService
{
    Task SendEmailAsync(string email, string name);
}

public class EmailService : IEmailService
{
    private readonly MongoDBService _mongoDBService;
    private readonly string _apiKey = "825441fd39c542a08d748950925feae2aab4dd0ee80615ffb418a1ff347a6e10";
    private readonly InboxControllerApi _inboxController;
    private readonly Configuration _config;


    public EmailService(MongoDBService mongoDbService)
    {
        _mongoDBService = mongoDbService;
        _config = new Configuration();
        _config.ApiKey.Add("x-api-key", _apiKey);
        _inboxController = new InboxControllerApi(_config);

    }

    public async Task SendEmailAsync(string email, string name)
    {
        var emailHash = HashEmail(email);
        var emailMappingList = await _mongoDBService.GetAllEmailMapping();
        var emailMappingDto = emailMappingList.FirstOrDefault(x => x.HashEmail == emailHash);
        if (emailMappingDto == null)
        {
            await _mongoDBService.SetEmailMapping(new EmailMappingDto() { Email = email, HashEmail = emailHash });
            await _mongoDBService.SetEmailDto(new EmailDto() { Email = email, Name = name, Status = Status.Created.ToString()});
        }

        var baseUrl = "http://localhost:5252/phishing/Update";
        var callbackUrl = $"{baseUrl}?&name={name}&{emailHash}";
        var emailBody = $"Please click <a href='{callbackUrl}'>here</a> to update your information.";

        var inbox = await _inboxController.CreateInboxAsync();

        // Send an email from this inbox
        var options = new SendEmailOptions(
            to: new List<string> { email },
            subject: "Click on This Mail",
            body: emailBody,
            isHTML: false
        );

        await _inboxController.SendEmailAndConfirmAsync(inbox.Id, options);

        Console.WriteLine($"Email sent from: {inbox.EmailAddress}");
    }

    private string HashEmail(string email)
    {
        using (var sha256 = SHA256.Create())
        {
            var normalizedEmail = email.Trim().ToLower();
            byte[] emailBytes = Encoding.UTF8.GetBytes(normalizedEmail);
            byte[] hashBytes = sha256.ComputeHash(emailBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}