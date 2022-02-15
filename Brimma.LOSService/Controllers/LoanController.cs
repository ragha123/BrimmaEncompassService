using Brimma.LOSService.Common;
using Brimma.LOSService.Config;
using Brimma.LOSService.DTO;
using Brimma.LOSService.Extensions;
using Brimma.LOSService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Brimma.LOSService.Controllers
{
    [ApiVersion("1.0")]
    [Route("encompass/v{v:apiVersion}/loans")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService loanService;
        private readonly ErrorMessages errorMessages;
        private Guid loanId;

        public LoanController(ILoanService loanService, IOptions<ErrorMessages> errorMessagesoptions)
        {
            this.loanService = loanService;
            errorMessages = errorMessagesoptions.Value;
        }

        /// <summary>
        /// Returns the loan information.
        /// </summary>
        /// <param name="loanGuid">Encompass LoanGuid</param>
        /// <param name="entities"></param>
        /// <returns></returns>
        [HttpGet("{loanGuid}")]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v1")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<Object> GetLoan(string loanGuid, [FromQuery] string entities = "")
        {
            return await loanService.GetLoan(loanGuid, entities).ConfigureAwait(false);
        }

        
        /// <summary>
        /// Updates an existing loan by modifying the values of the loan data elements passed.
        /// </summary>
        /// <param name="loanGuid">The unique identifier assigned to the loan.</param>
        /// <param name="request">Loan data elements to update. Request is same as Encompass loan update request.</param>
        /// <returns>Returns the ID assigned to the loan.</returns>
        [HttpPatch("{loanGuid}")]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v1")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<Object> UpdateLoan(string loanGuid, [FromBody] Object request)
        {
            return await loanService.UpdateLoan(loanGuid, request).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new loan assigns LO and returns the created loan Guid.
        /// </summary>
        /// <param name="loanTemplate">loanTemplate path.Which type of template we want to create.</param>
        /// <param name="loId">LO ID to which the loan will be assigned.</param>
        /// <param name="loaId">LOA ID which will be assigned to the loan.</param>
        /// <param name="request">Loan data elements to create a new loan. Request is same as Encompass loan create request.</param>
        /// <param name="userId">User Id</param>
        /// <param name="loanProgramPath">Loan program userqualifiedpath</param>
        /// <param name="closingCostPath">Closing cost userqualifiedpath</param>
        /// <param name="createFlow">Flow create in FLoify when it's true</param>
        /// <returns>Returns the Created Loan GUID.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v1")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<Object> CreateLoan([FromQuery] string loanTemplate, [FromQuery] string loId, [FromQuery] string loaId, [FromBody] object request,[FromQuery] string userId, [FromQuery] string loanProgramPath, [FromQuery] string closingCostPath, [FromQuery] bool createFlow)
        {
            return await loanService.CreateLoan(loanTemplate, loId, loaId, request, userId, loanProgramPath, closingCostPath, createFlow).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns list of fields with values
        /// </summary>
        /// <param name="fieldIds">field Id to return the value for it</param>
        /// <param name="loanGuid">loan Id for returning the field properties of that loan</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{loanGuid}/loanfieldsinfo")]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v1")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<Object> GetLoanFieldValues(string loanGuid, [FromQuery] string fieldIds)
        {
            return await loanService.GetLoanFieldValues(loanGuid, fieldIds).ConfigureAwait(false);
        }

        /// <summary>
        /// Submit a rate lock request
        /// </summary>
        /// <param name="rateLockRequest">request object to extend the rate lock</param>
        /// <param name="loanGuid">loan Id for returning the field properties of that loan</param>
        /// /// <param name="action">actions (extend) to be performed while submitting a rate lock request</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{loanGuid}/ratelock")]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v1")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<Object> SubmitRateLock(Guid loanGuid, [FromQuery] [Required] DTO.Action action, [FromBody] [Required] RateLockRequest rateLockRequest)
        {
            if (rateLockRequest == null)
                return BadRequest(ErrorHandling.GetErrorResponse(400, errorMessages.RequestEmptyBadRequest));
            
            return await loanService.SubmitRateLock(loanGuid, rateLockRequest, action).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the loan information for Qualifier to get the EPPS Rates.
        /// </summary>
        /// <param name="loanGuid">Encompass LoanGuid</param>
        /// <returns></returns>
        [HttpPost("{loanGuid}/qualifier")]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v1")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<Object> GetLoanQualifer(string loanGuid, [FromBody] [Required] JObject request)
        {
            if (string.IsNullOrEmpty(loanGuid) || (!string.IsNullOrEmpty(loanGuid) && !Guid.TryParse(loanGuid, out loanId)))
            {
                return BadRequest(ErrorHandling.GetErrorResponse(400, errorMessages.LoanGuidBadRequestError));
            }
            else if (request == null)
            {
                return BadRequest(ErrorHandling.GetErrorResponse(400, errorMessages.RequestEmptyBadRequest));
            }
            return await loanService.GetLoanQualifer(loanGuid, request).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates an existing loan by modifying the values of the loan data elements passed and by applying a loan template, loan program template and closing cost template.
        /// </summary>
        /// <param name="loanGuid">The unique identifier assigned to the loan.</param>        
        /// <param name="loanTemplatePath">Path to the loan template that will be applied to the loan.</param>
        /// <param name="loanProgramPath">Loan program userqualifiedpath</param>
        /// <param name="closingCostPath">Closing cost userqualifiedpath</param>
        /// <param name="request">Loan data elements to update. Request is same as Encompass loan update request.</param>
        /// <returns>Returns the ID assigned to the loan.</returns>
        [HttpPatch("{loanGuid}/applytemplate")]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v1")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<Object> UpdateLoanWithTemplate(string loanGuid,[FromQuery] string loanTemplatePath, [FromQuery] string loanProgramPath, [FromQuery] string closingCostPath, [FromBody] Object request)
        {
            if (string.IsNullOrEmpty(loanGuid) || !Guid.TryParse(loanGuid, out loanId))
            {
                return BadRequest(ErrorHandling.GetErrorResponse(400, errorMessages.LoanGuidBadRequestError));
            }            
            else if (request == null)
            {
                return BadRequest(ErrorHandling.GetErrorResponse(400, errorMessages.RequestEmptyBadRequest));
            }
            return await loanService.UpdateLoanWithTemplate(loanGuid, loanTemplatePath, loanProgramPath, closingCostPath, request).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates an existing loan by applying a loan program template or clsoing cost template.
        /// </summary>
        /// <param name="loanGuid">The unique identifier assigned to the loan.</param>
        /// <param name="loanProgramPath">Loan program userqualifiedpath</param>
        /// <param name="closingCostPath">Closing cost userqualifiedpath</param>
        /// <returns>Returns the ID assigned to the loan.</returns>
        [HttpPost("{loanGuid}")]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v1")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<Object> ApplyLoanProgramClosingCost(string loanGuid, [FromQuery] string loanProgramPath, [FromQuery] string closingCostPath)
        {
            if (string.IsNullOrEmpty(loanGuid) || !Guid.TryParse(loanGuid, out loanId))
            {
                return BadRequest(ErrorHandling.GetErrorResponse(400, errorMessages.LoanGuidBadRequestError));
            }
            if (string.IsNullOrEmpty(loanProgramPath) && string.IsNullOrEmpty(closingCostPath))
            {
                if (string.IsNullOrEmpty(loanProgramPath))
                {
                    return BadRequest(ErrorHandling.GetErrorResponse(400, errorMessages.LoanProgramTemplatePathBadRequestError));
                }
                else if (string.IsNullOrEmpty(closingCostPath))
                {
                    return BadRequest(ErrorHandling.GetErrorResponse(400, errorMessages.ClosingCostTemplatePathBadRequestError));
                }                           
            }
            return await loanService.ApplyLoanProgramClosingCost(loanGuid, loanProgramPath, closingCostPath).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the loan information.
        /// </summary>
        /// <param name="loanGuid">Encompass LoanGuid</param>
        /// <param name="entities"></param>
        /// <returns></returns>
        [ApiVersion("2.0")]
        [HttpGet("{loanGuid}")]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v2")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<Object> GetLoanV2(string loanGuid, [FromQuery] string entities = "")
        {
            return await loanService.GetLoanV2(loanGuid, entities).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates an existing loan by modifying the values of the loan data elements passed.
        /// </summary>
        /// <param name="loanGuid">The unique identifier assigned to the loan.</param>
        /// <param name="request">Loan data elements to update. Request is same as Encompass loan update request.</param>
        /// <returns>Returns the ID assigned to the loan.</returns>
        [ApiVersion("2.0")]
        [HttpPatch("{loanGuid}")]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v2")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<Object> UpdateLoanV2(string loanGuid, [FromBody] Object request)
        {
            return await loanService.UpdateLoanV2(loanGuid, request).ConfigureAwait(false);
        }
    }
}