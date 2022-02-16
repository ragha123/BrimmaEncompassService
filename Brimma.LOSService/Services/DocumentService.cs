using Brimma.LOSService.Common;
using Brimma.LOSService.Config;
using Brimma.LOSService.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brimma.LOSService.Services
{
    public class DocumentService : ControllerBase, IDocumentService
    {
        private readonly IHttpService httpService;
        private readonly EncompassAPIs encompassAPIs;
        private readonly SaveNLog saveNLogger;

        public DocumentService(IHttpService httpService, IOptions<EncompassAPIs> encompassAPIsOptions,SaveNLog saveNLogger)
        {
            this.httpService = httpService;
            encompassAPIs = encompassAPIsOptions.Value;
            this.saveNLogger = saveNLogger;
        }

        /// <summary>
        /// Retrive Loan Document Details
        /// </summary>
        /// <param name="loanGuid"></param>
        /// <returns> List of Loan Document Details</returns>
        public async Task<object> GetDocuments(string loanGuid)
        {
            Object documenentResponse = new Object();

            try
            {
                var documentListResponse = await httpService.GetAsync<List<DocumentInfo>>
                                             (string.Format(encompassAPIs.RetrieveLoanDocuments, loanGuid)).ConfigureAwait(false);

                if (documentListResponse.Data != null)
                {
                    List<DocumentInfo> documents = documentListResponse.Data;

                    if (documents != null)
                    {
                        documenentResponse = documents;
                    }
                }
                else if (documentListResponse.ErrorResponse != null && documentListResponse.ErrorResponse.Error != null)
                {
                    return StatusCode(documentListResponse.ErrorResponse.Error.Code, documentListResponse.ErrorResponse);
                }
            }
            catch(Exception ex)
            {
                saveNLogger.SaveLogFile("DocumentService", "GetDocuments", ex.StackTrace, ex.Message);
                return StatusCode(500, ErrorHandling.GetErrorResponse(500, ex.Message));
            }
            
            return documenentResponse;
        }
    }
}
