using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using eCaseBinderService.Services;
using eCaseBinderService.Extensions;

namespace eCaseBinderService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class DraftController : ControllerBase
    {
        private readonly IDraftService draftService;
        public DraftController(IDraftService draftService)
        {
            this.draftService = draftService;
        }

        ///// <summary>
        ///// Create Database.
        ///// </summary>
        ///// <param name="dbName">Database Name.</param>
        ///// <returns></returns>
        //[HttpGet("createdatabase")]
        //[Produces("application/json")]
        ////[MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        //public async Task<IActionResult> CreateDatabase(string dbName)
        //{
        //    var result = await draftService.CreateDatabase(dbName);
        //    return Ok(result);
        //}

        ///// <summary>
        ///// Create Collection.
        ///// </summary>
        ///// <param name="collectionName">Collection Name.</param>
        ///// <returns></returns>
        //[HttpGet("createcollection")]
        //[Produces("application/json")]
        ////[MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        //public async Task<IActionResult> CreateCollection(string collectionName)
        //{
        //    var result = await draftService.CreateCollection(collectionName);
        //    return Ok(result);
        //}

        /// <summary>
        /// Save a new loan in Cosmos DB.
        /// </summary>
        /// <param name="request">Loan data elements to save a new loan. Request is same as Encompass loan create request.</param>
        /// <returns></returns>
        [HttpPost("createloan")]
        [Produces("application/json")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<IActionResult> CreateLoan([FromBody] JObject request)
        {
            var result = await draftService.CreateLoan(request);
            return Ok(result);
        }

        /// <summary>
        /// Returns list of loans on drafts with pagination.
        /// </summary>
        /// <param name="userName">UserName who created the loans</param>
        /// <param name="start">Zero-based starting index</param>
        /// <param name="limit">The maximum number of items to return in a page</param>
        /// <returns></returns>
        [HttpGet("getloans")]
        [Produces("application/json")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<IActionResult> GetLoans(string userName, [FromQuery] int start, [FromQuery] int limit)
        {
            var result = await draftService.GetLoans(userName, start, limit);
            return Ok(result);
        }

        /// <summary>
        /// Returns a loan information
        /// </summary>
        /// <param name="id"> each loan have Unique Id in Cosmos DB</param>
        /// <returns></returns>
        [HttpGet("getloan/{id}")]
        [Produces("application/json")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<IActionResult> GetLoan(string id)
        {
            var result = await draftService.GetLoan(id);
            return Ok(result);
        }

        /// <summary>
        /// Update a existing loan in Cosmos DB
        /// </summary>
        /// <param name="id"> each loan have Unique Id in Cosmos DB</param>
        /// <param name="request">Loan data elements to update a existing loan. Request is same as Encompass loan update request.</param>
        /// <returns></returns>
        [HttpPut("updateloan/{id}")]
        [Produces("application/json")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<IActionResult> UpdateLoan(string id, [FromBody] JObject request)
        {
            var result = await draftService.UpdateLoan(id, request);
            return Ok(result);
        }

        /// <summary>
        /// Delete a existing loan in Cosmos DB
        /// </summary>
        /// <param name="id"> each loan have Unique Id in Cosmos DB</param>        
        /// <returns></returns>
        [HttpDelete("deleteloan/{id}")]
        [Produces("application/json")]
        [MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        public async Task<IActionResult> DeleteLoan(string id)
        {
            var result = await draftService.DeleteLoan(id);
            return Ok(result);
        }

        ///// <summary>
        ///// Delete a existing Collection in Cosmos DB.
        ///// </summary>
        ///// <param name="collectionName">Collection Name.</param>
        ///// <returns></returns>
        //[HttpDelete("deletecollection/{collectionName}")]
        //[Produces("application/json")]
        ////[MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        //public async Task<IActionResult> DeleteCollection(string collectionName)
        //{
        //    var result = await draftService.DeleteCollection(collectionName);
        //    return Ok(result);
        //}

        ///// <summary>
        ///// Delete a existing Database in Cosmos DB.
        ///// </summary>
        ///// <param name="dbName">Database Name.</param>
        ///// <returns></returns>
        //[HttpDelete("deletedatabase/{dbName}")]
        //[Produces("application/json")]
        ////[MiddlewareFilter(typeof(CustomAuthorizationPipeline))]
        //public async Task<IActionResult> DeleteDatabase(string dbName)
        //{
        //    var result = await draftService.DeleteDatabase(dbName);
        //    return Ok(result);
        //}
    }
}
