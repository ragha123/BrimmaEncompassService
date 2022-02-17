using Brimma.LOSService.Common;
using Brimma.LOSService.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Brimma.LOSService.Services
{
    public class BorrowerService : ControllerBase, IBorrowerService
    {
        private readonly IHttpService httpService;
        private readonly EncompassAPIs encompassAPIs;
        private readonly SaveNLog saveNLogger;

        public BorrowerService(IHttpService httpService, IOptions<EncompassAPIs> encompassAPIsOptions, SaveNLog saveNLogger)
        {
            this.httpService = httpService;
            encompassAPIs = encompassAPIsOptions.Value;
            this.saveNLogger = saveNLogger;
        }

        /// <summary>
        /// Retrive Loan Document Details
        /// </summary>
        /// <param name="request"> The </param>
        /// <param name="startLimit"></param>
        /// <param name="endlimit"></param>
        /// <returns> List of Loan Document Details</returns>
        public async Task<Object> GetBorrowerContacts(Object request, int startLimit, int endlimit)
        {
            Object response = new Object();
            try
            {
                var apiResponse = await httpService.PostAsync<Object>
                                                  (string.Format(encompassAPIs.RetrieveBorrowerContacts, startLimit, endlimit), request)
                                                  .ConfigureAwait(false);
                if (apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
                else if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                {
                    return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                }
            }
            catch (Exception ex)
            {

                saveNLogger.SaveLogFile("BorrowerService", "GetBororwerContacts", ex.StackTrace, ex.Message);
            }
            return response;
        }
    }
}
