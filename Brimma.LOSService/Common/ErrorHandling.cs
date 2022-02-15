using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brimma.LOSService.Common
{
    public static class ErrorHandling
    {
        public static Object GetErrorResponse(int code, string message)
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
