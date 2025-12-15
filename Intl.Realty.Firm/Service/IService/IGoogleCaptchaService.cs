namespace Intl.Realty.Firm.Service.IService
{
    public interface IGoogleCaptchaService
    {
        Task<bool> VerifyToken(string? token, string expectedAction, double minScore = 0.5);
    }
}
