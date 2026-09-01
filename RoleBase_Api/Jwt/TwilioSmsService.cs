using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace RoleBase_Api.Jwt
{
    public class TwilioSmsService
    {
        private readonly TwilioSettings _twilioSettings;

        public TwilioSmsService(IOptions<TwilioSettings> twilioSettings)
        {
            _twilioSettings = twilioSettings.Value;
        }
        public async Task SendSmsAsync(string toPhoneNumber, string message)
        {
            if (!toPhoneNumber.StartsWith("+"))
            {
                toPhoneNumber = "+91" + toPhoneNumber;
            }

            TwilioClient.Init(
                _twilioSettings.AccountSid,
                _twilioSettings.AuthToken);


            await MessageResource.CreateAsync(
            body: $"Your OTP is {message}",
            from: new PhoneNumber(_twilioSettings.PhoneNumber),
            to: new PhoneNumber(toPhoneNumber));
        }
    }
}
