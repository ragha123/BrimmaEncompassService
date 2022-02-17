using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brimma.LOSService.Extensions;
using Brimma.LOSService.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Brimma.LOSService.Controllers
{
    [ApiVersion("1.0")]
    [Route("encompass/v{v:apiVersion}/borrowerContactSelector")]
    [ApiController]
    public class BorrowerController : ControllerBase
    {
        private readonly IBorrowerService borrowerService;

        public BorrowerController(IBorrowerService borrowerService)
        {
            this.borrowerService = borrowerService;
        }

        /// <summary>
        /// Returns list of Borrower Contacts
        /// </summary>
        /// <param name="request">Borrower Contact Info</param>
        /// <param name="startLimit">Start Limit</param>
        /// <param name="endLimit">End Limit</param>
        /// <returns>List of Borrower Contacts</returns>
        [HttpPost]
        [Produces("application/json")]
        [ApiExplorerSettings(GroupName = "v1")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        public async Task<Object> GetBorrowerContacts([FromBody] Object request,[FromQuery] int startLimit, [FromQuery] int endLimit)
        {

            return await borrowerService.GetBorrowerContacts(request, startLimit, endLimit).ConfigureAwait(false);
        }
    }
}