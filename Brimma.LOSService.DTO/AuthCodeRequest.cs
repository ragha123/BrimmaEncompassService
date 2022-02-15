namespace Brimma.LOSService.DTO
{
    public class AuthCodeRequest
    {
        public string AuthorizationCode { get; set; }
        public string RedirectUrl { get; set; }
    }
}