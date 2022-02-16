using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brimma.LOSService.Common;
using Brimma.LOSService.Config;
using Brimma.LOSService.DTO;
using Brimma.LOSService.Extensions;
using Brimma.LOSService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Brimma.LOSService.Controllers
{
    [ApiVersion("1.0")]
    [Route("encompass/v{v:apiVersion}/loans")]
    [ApiController]
    public class DocumentController : ControllerBase
    {

        private readonly IDocumentService documentService;
        private readonly ErrorMessages errorMessages;

        public DocumentController(IDocumentService documentService, IOptions<ErrorMessages> errorMessagesoptions)
        {
            this.documentService = documentService;
            this.errorMessages = errorMessagesoptions.Value;
        }

        /// <summary>
        /// Retrieves loan Documents.
        /// </summary>
        /// <param name="loanGuid">Loan ID</param>
        /// <returns>A collection of Documents for a loan.</returns>
        /// <response code="200">Successfully retrieved Documents for a loan.</response>
        /// <response code="404">No loan found for given loanId.</response>
        /// <response code="403">Action not permitted for specified API key.</response>
        /// <response code="401">Access token is missing or invalid.</response>
        [HttpGet]
        [Route("{loanGuid}/documents")]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v1")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        [ProducesResponseType(typeof(List<DocumentInfo>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<Object> GetDocuments(string loanGuid)
        {
            if(loanGuid == null)
            {
                return BadRequest(ErrorHandling.GetErrorResponse(400, errorMessages.LoanGuidBadRequestError));
            }

           return await documentService.GetDocuments(loanGuid).ConfigureAwait(false);
        }
    }
}
