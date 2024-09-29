namespace Intl.Realty.Firm.Service.IService
{
    public interface IGoogleCaptchaService
    {
        public Task<bool> VerifyToken(string token);
    }
}
