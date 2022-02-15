using Brimma.LOSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brimma.LOSService.Services
{
    public interface IDocumentService
    {
        /// <summary>
        /// Retrieves a loan Document details.
        /// </summary>
        /// <param name="loanGuid">Loan ID</param>
        /// <returns>Loan Document details</returns>
        Task<object> GetDocuments(string loanGuid);
    }
}
