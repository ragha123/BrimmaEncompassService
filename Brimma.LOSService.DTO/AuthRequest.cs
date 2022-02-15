namespace Brimma.LOSService.DTO
{
    public class AuthRequest
    {
        public string GrantType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }

    }
}
