using RoyPhishingProj.BusinessLogicLayer.dto;
using RoyPhishingProj.BusinessLogicLayer.Enums;
using RoyPhishingProj.BusinessLogicLayer.Helpers;
using RoyPhishingProj.DBLayer;

namespace RoyPhishingProj.BusinessLogicLayer
{
    public interface IPhishingEmailService
    {
        Task<Result<bool>> SendPhishingEmailAsync(string name, string email);

        Task<Result<bool>> UpdatePhishingEmailClickedAsync(string name, string hashEmail);

        Task<Result<List<EmailDto>>> GetAllPhishingAttempts();
    }

    public class PhishingEmailService : IPhishingEmailService
    {
        private readonly IEmailService _emailService;
        private readonly MongoDBService _mongoDBService;
        public PhishingEmailService(IEmailService emailService, MongoDBService mongoDBService)
        {
            _emailService = emailService;
            _mongoDBService = mongoDBService;
        }

        public async Task<Result<bool>> SendPhishingEmailAsync(string name, string email)
        {
            try
            {
                await _emailService.SendEmailAsync(email, name);

                Console.WriteLine($"Phishing email sent to {name} <{email}>.");

                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                await _mongoDBService.UpdateEmailDto(email, Status.Failed.ToString());
                Console.WriteLine(e);
                return Result<bool>.Fail(e.Message);
            }
        }

        public async Task<Result<bool>> UpdatePhishingEmailClickedAsync(string name, string hashEmail)
        {
            try
            {
                var emailMappingList = await _mongoDBService.GetAllEmailMapping();
                var emailMappingDto = emailMappingList.FirstOrDefault(x => x.HashEmail == hashEmail);
                if (emailMappingDto == null)
                {
                    return Result<bool>.Fail("Invalid email on url");
                }

                await _mongoDBService.UpdateEmailDto(emailMappingDto.Email,Status.Clicked.ToString());

                Console.WriteLine($"Phishing email sent to {name} <{emailMappingDto.Email}>.");

                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<bool>.Fail(e.Message);
            }
        }

        public async Task<Result<List<EmailDto>>> GetAllPhishingAttempts()
        {
            try
            {
                var result= await _mongoDBService.GetAllPhishingAttempts();
                return Result<List<EmailDto>>.Success(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<List<EmailDto>>.Fail(e.Message);
            }
        }
    }
}
