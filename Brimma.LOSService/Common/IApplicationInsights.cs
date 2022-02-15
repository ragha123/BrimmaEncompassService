using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brimma.LOSService.Common
{
   public interface IApplicationInsights
    {
        void AddCustomPropertiesInAppInsights(string controller, string method, object request, object response, string purpose, string apiURL, string loanGuid);
    }
}
