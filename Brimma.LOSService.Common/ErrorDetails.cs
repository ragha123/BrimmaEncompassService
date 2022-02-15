namespace Brimma.LOSService.Common
{
    public class ErrorDetails
    {
        public object CreateErrorResponse(int code, string message)
        {
            var error = new
            {
                error = new
                {
                    code,
                    message
                }
            };
            return error;
        }
    }
}
