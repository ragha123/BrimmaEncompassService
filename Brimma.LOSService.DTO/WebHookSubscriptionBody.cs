namespace Brimma.LOSService.DTO
{
    public class WebHookSubscriptionBody
    {
        public string endpoint { get; set; }
        public string resource { get; set; }
        public string[] events { get; set; }
        public filters filters { get; set; }
    }
    public class filters
    {
        public string[] attributes { get; set; }
    }
}
