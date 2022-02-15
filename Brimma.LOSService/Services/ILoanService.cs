using Brimma.LOSService.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Brimma.LOSService.Services
{
    public interface ILoanService
    {
        Task<Object> GetLoan(string loanGuid, string entities);
        Task<Object> UpdateLoan(string loanGuid, Object request);        
        Task<Object> CreateLoan(string loanTemplate, string loId, string loaId, object request, string userId, string loanProgramPath, string closingCostPath, bool createFlow);
        Task<Object> GetLoanFieldValues(string loanGuid, string fieldIds);
        Task<Object> SubmitRateLock(Guid loanGuid, RateLockRequest rateLockRequest, DTO.Action action);
        Task<Object> GetLoanQualifer(string loanGuid, JObject request);
        Task<Object> UpdateLoanWithTemplate(string loanGuid, string loanTemplatePath, string loanProgramPath, string closingCostPath, Object request);
        Task<Object> ApplyLoanProgramClosingCost(string loanGuid, string loanProgramPath, string closingCostPath);
        Task<Object> GetLoanV2(string loanGuid, string entities);
        Task<Object> UpdateLoanV2(string loanGuid, Object request);
    }
}
