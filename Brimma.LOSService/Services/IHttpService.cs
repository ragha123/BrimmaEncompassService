using Brimma.LOSService.DTO;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Brimma.LOSService.Services
{
    public interface IHttpService
    {
        /// <summary>
        /// HttpClient Get method
        /// </summary>
        /// <typeparam name="T">Generic class for Data property in ApiResponse class</typeparam>
        /// <param name="apiURL">apiURL</param>
        /// <param name="IsSDKAPICall">IsSDKAPICall</param>
        /// <param name="isAppraisalOrderCall">isAppraisalOrderCall</param>
        /// <returns>Returns Api response</returns>
        Task<ApiResponse<T>> GetAsync<T>(string apiURL, bool IsSDKAPICall = false, bool isAppraisalOrderCall = false);

        /// <summary>
        /// HttpClient Post method
        /// </summary>
        /// <typeparam name="T">Generic class for Data property in ApiResponse class</typeparam>
        /// <param name="apiURL">API url</param>
        /// <param name="request">request</param>
        /// <param name="needHttpResponseHeader">needHttpResponseHeader</param>
        /// <param name="IsSDKAPICall">IsSDKAPICall</param>
        /// <param name="isAppraisalOrderCall">isAppraisalOrderCall</param>
        /// <param name="isWebhookSubscriptionCall">isWebhookSubscriptionCall</param>
        /// <returns>Returns Api response</returns>
        Task<ApiResponse<T>> PostAsync<T>(string apiURL, Object request, bool needHttpResponseHeader = false, bool IsSDKAPICall = false, bool isAppraisalOrderCall = false, bool isWebhookSubscriptionCall = false, bool isFloifyAPI = false, bool isFloifyAPIKey = false);

        /// <summary>
        /// HttpClient Patch method
        /// </summary>
        /// <typeparam name="T">Generic class for Data property in ApiResponse class</typeparam>
        /// <param name="apiURL">API url</param>
        /// <param name="request">request</param>
        /// <returns>Returns Api response</returns>
        Task<ApiResponse<T>> PatchAsync<T>(string apiURL, Object request);

        /// <summary>
        /// HttpClient Put method
        /// </summary>
        /// <typeparam name="T">Generic class for Data property in ApiResponse class</typeparam>
        /// <param name="apiURL">API url</param>
        /// <param name="request">request</param>
        /// <param name="uploadAttachment"></param>
        /// <param name="httpContent"></param>
        /// <param name="authorizationHeader"></param>
        /// <returns>Returns Api response</returns>
        Task<ApiResponse<T>> PutAsync<T>(string apiURL, Object request, bool uploadAttachment = false, HttpContent httpContent = null, string authorizationHeader = "");

        /// <summary>
        /// HttpClient Delete method
        /// </summary>
        /// <typeparam name="T">Generic class for Data property in ApiResponse class</typeparam>
        /// <param name="apiURL">API url</param>
        /// <returns>Returns Api response</returns>
        Task<ApiResponse<T>> DeleteAsync<T>(string apiURL);
    }
}
