using Brimma.LOSService.Config;
using Brimma.LOSService.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Brimma.LOSService.Services
{
    public class DocumentService : ControllerBase, IDocumentService
    {
        private readonly IHttpService httpService;
        private readonly EncompassAPIs encompassAPIs;

        public DocumentService(IHttpService httpService, IOptions<EncompassAPIs> encompassAPIsOptions)
        {
            this.httpService = httpService;
            encompassAPIs = encompassAPIsOptions.Value;
        }

        /// <summary>
        /// Retrive Loan Document Details
        /// </summary>
        /// <param name="loanGuid"></param>
        /// <returns> List of Loan Document Details</returns>
        public async Task<object> GetDocuments(string loanGuid)
        {
            Object documenentResponse = new Object();

           var documentListResponse = await httpService.GetAsync<List<DocumentInfo>>
                                             (string.Format(encompassAPIs.RetrieveLoanDocuments, loanGuid)).ConfigureAwait(false);
              
                if (documentListResponse.Data != null)
                {
                    List<DocumentInfo> documents = documentListResponse.Data;

                    if (documents != null )
                    { 
                    documenentResponse = documents;
                    }
                }

                else if (documentListResponse.ErrorResponse != null && documentListResponse.ErrorResponse.Error != null)
                {
                    return StatusCode(documentListResponse.ErrorResponse.Error.Code, documentListResponse.ErrorResponse);
                }
            
            return documenentResponse;
        }
    }
}
