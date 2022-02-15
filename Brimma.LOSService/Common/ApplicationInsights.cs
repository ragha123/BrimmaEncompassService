using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brimma.LOSService.Common
{
    public class ApplicationInsights: IApplicationInsights
    {
        private IConfiguration Configuration { get; set; }

        public ApplicationInsights(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void AddCustomPropertiesInAppInsights(string controller, string method, object request, object response, string purpose, string apiURL, string loanGuid)
        {
            TelemetryConfiguration telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.InstrumentationKey = Configuration["ApplicationInsights:InstrumentationKey"];
            TelemetryClient telemetryClient = new TelemetryClient(telemetryConfiguration);
            Dictionary<string, string> customProperties = new Dictionary<string, string>
            {
                {"Controller", controller },
                {"Method", method },
                {"API URL", apiURL },
                {"LoanGuid", loanGuid },
                {"Request", JsonConvert.SerializeObject(request) },
                {"Response", JsonConvert.SerializeObject(response) },
                {"Purpose", purpose },
            };
            //telemetryClient.TrackEvent("SSPL Details", customProperties);
            telemetryClient.TrackTrace("Encompass Data service", SeverityLevel.Information, customProperties);
            telemetryClient.Flush();
            telemetryConfiguration.Dispose();
        }
    }
}
