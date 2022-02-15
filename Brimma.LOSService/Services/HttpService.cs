using Brimma.LOSService.Common;
using Brimma.LOSService.Config;
using Brimma.LOSService.DTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Mime;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Brimma.LOSService.Services
{
    public class HttpService : IHttpService
    {
        //private HttpClient client;
        private readonly EncompassAPIs encompassAPIs;
        private readonly AuthServiceAPIs authServiceAPIs;
        private readonly FloifyServiceAPIs floifyAPIs;
        private readonly ILogger<HttpService> logger;
        public HttpService(IOptions<EncompassAPIs> encompassAPIsOptions, IOptions<AuthServiceAPIs> authServiceAPIsOptions, IOptions<FloifyServiceAPIs> floifyAPIsOptions, ILogger<HttpService> logger)
        {
            encompassAPIs = encompassAPIsOptions.Value;
            authServiceAPIs = authServiceAPIsOptions.Value;
            floifyAPIs = floifyAPIsOptions.Value;
            this.logger = logger;
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string apiURL, bool isSDKAPICall = false, bool isAppraisalOrderCall = false)
        {
            ApiResponse<T> response = new ApiResponse<T>();
            try
            {
                HttpClient httpClient = CreateHttpClient(isSDKAPICall, isAppraisalOrderCall);
                
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(apiURL).ConfigureAwait(false);
                response = await GetApiResponseAsync<T>(httpResponseMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                SetExceptionToResponse<T>(ref response, ex);
            }
            return response;
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string apiURL, Object request, bool uploadAttachment = false, HttpContent httpContent = null, string authorizationHeader = "")
        {
            ApiResponse<T> response = new ApiResponse<T>();
            try
            {
                HttpClient httpClient = CreateHttpClient(uploadAttachment: uploadAttachment, authorizationHeader: authorizationHeader);
                HttpResponseMessage httpResponseMessage;
                if (uploadAttachment)
                {
                    httpResponseMessage = await httpClient.PutAsync(new Uri(apiURL), httpContent).ConfigureAwait(false);
                }
                else
                {
                    httpResponseMessage = await httpClient.PutAsJsonAsync(apiURL, request).ConfigureAwait(false);
                }

                response = await GetApiResponseAsync<T>(httpResponseMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                SetExceptionToResponse<T>(ref response, ex);
            }
            return response;
        }

        public async Task<ApiResponse<T>> PatchAsync<T>(string apiURL, Object request)
        {
            ApiResponse<T> response = new ApiResponse<T>();
            try
            {
                HttpClient httpClient = CreateHttpClient();
                string content = JsonConvert.SerializeObject(request);
                StringContent stringContent = new StringContent(content, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await httpClient.PatchAsync(apiURL, stringContent).ConfigureAwait(false);
                response = await GetApiResponseAsync<T>(httpResponseMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                SetExceptionToResponse<T>(ref response, ex);
            }
            return response;
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string apiURL, Object request, bool needHttpResponseHeader = false, bool isSDKAPICall = false, bool isAppraisalOrderCall = false, bool isWebhookSubscriptionCall = false, bool isFloifyAPI = false, bool isFloifyAPIKey = false)
        {
            ApiResponse<T> response = new ApiResponse<T>();
            try
            {
                HttpClient httpClient = CreateHttpClient(isSDKAPICall, isAppraisalOrderCall, isFloifyAPI: isFloifyAPI, isFloifyAPIKey: isFloifyAPIKey);
                string Serialized = string.Empty;
                HttpResponseMessage httpResponseMessage;
                if (isAppraisalOrderCall || isWebhookSubscriptionCall)
                {
                    httpResponseMessage = await httpClient.PostAsJsonWithNoCharSetAsync(apiURL, request).ConfigureAwait(false);
                }
                else
                {
                    StringContent payload = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                    httpResponseMessage = await httpClient.PostAsync(apiURL, payload).ConfigureAwait(false);
                }
                response = await GetApiResponseAsync<T>(httpResponseMessage, needHttpResponseHeader, request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                SetExceptionToResponse<T>(ref response, ex);
            }
            return response;
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string apiURL)
        {
            ApiResponse<T> response = new ApiResponse<T>();
            try
            {
                HttpClient httpClient = CreateHttpClient();
                
                HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(apiURL).ConfigureAwait(false);
                response = await GetApiResponseAsync<T>(httpResponseMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                SetExceptionToResponse<T>(ref response, ex);
            }
            return response;
        }

        private async Task<ApiResponse<T>> GetApiResponseAsync<T>(HttpResponseMessage httpResponseMessage, bool needHttpResponseHeader = false, object request = null)
        {
            ApiResponse<T> response = new ApiResponse<T>();
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                response.Data = JsonConvert.DeserializeObject<T>(result);
                if (needHttpResponseHeader)
                {
                    response.HttpResponseHeaders = httpResponseMessage.Headers;
                }
                response.Success = true;
            }
            else
            {
                response = await GetApiErrorResponseAsync<T>(httpResponseMessage, request).ConfigureAwait(false);
            }
            return response;
        }

        private async Task<ApiResponse<T>> GetApiErrorResponseAsync<T>(HttpResponseMessage httpResponseMessage, object request = null)
        {
            ApiResponse<T> response = new ApiResponse<T>
            {
                Success = false
            };
            string errorMessage = string.Empty;
            var errorInfo = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            logger.LogError("Method : GetApiErrorResponseAsync.  Status Code : " + httpResponseMessage.StatusCode + ";   ErrorInfo : " + errorInfo);

            if (errorInfo.StartsWith("<"))
            {
                errorMessage = errorInfo;
            }
            else
            {
                JObject errorDetails = JsonConvert.DeserializeObject<JObject>(errorInfo);
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    if (errorDetails != null && errorDetails.ContainsKey("errors"))
                    {
                        int count = 0;
                        foreach (var error in errorDetails["errors"])
                        {
                            if (count > 0)
                            {
                                break;
                            }
                            errorMessage = error["details"].ToString();
                            count++;
                        }
                    }
                    else if (errorDetails != null && errorDetails.ContainsKey("details"))
                    {
                        errorMessage = errorDetails["details"].ToString();
                    }
                    else if (errorDetails != null && errorDetails.ContainsKey("error"))
                    {
                        errorMessage = errorDetails["error"]["message"].ToString();
                    }
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    if (errorDetails != null && errorDetails.ContainsKey("message"))
                    {
                        errorMessage = errorDetails["message"].ToString();
                    }
                    /*Added for LOA and LO assignments where summary and details is returned on error*/
                    if (errorDetails != null && errorDetails.ContainsKey("details"))
                    {
                        errorMessage = errorDetails["details"].ToString();
                    }
                }
                else
                {
                    if (httpResponseMessage.StatusCode.GetHashCode() == 500 && errorDetails != null && errorDetails.ContainsKey("error") && errorDetails["error"]["message"] != null)
                    {
                        errorMessage = errorDetails["error"]["message"].ToString();
                    }
                    else if (errorDetails != null && errorDetails.ContainsKey("details"))
                    {
                        errorMessage = errorDetails["details"].ToString();
                    }
                    else
                    {
                        errorMessage = httpResponseMessage.ReasonPhrase;
                    }
                }
            }

            response.ErrorResponse = new ErrorResponse
            {
                Error = new Error
                {
                    Code = httpResponseMessage.StatusCode.GetHashCode(),
                    Message = !string.IsNullOrEmpty(errorMessage) ? errorMessage : httpResponseMessage.ReasonPhrase
                }
            };
            return response;
        }

        private void SetExceptionToResponse<T>(ref ApiResponse<T> response, Exception exception)
        {
            logger.LogError("StackTrace :" + exception.StackTrace + ";   Error Message: " + exception.Message);
            response.Success = false;
            response.ErrorResponse = new ErrorResponse
            {
                Error = new Error
                {
                    Code = 500,
                    Message = exception.Message
                }
            };
        }

        public HttpClient CreateHttpClient(bool isSDKAPICall = false, bool isAppraisalOrderCall = false, bool uploadAttachment = false, string authorizationHeader = "", bool isFloifyAPI = false, bool isFloifyAPIKey = false)
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(encompassAPIs.HttpTimeout);
            httpClient.MaxResponseContentBufferSize = 2147483647;

            if (isFloifyAPIKey)
            {
                httpClient.BaseAddress = new Uri(authServiceAPIs.APIServer);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("ApiKey", GetEncodedApiKey());
            }
            else if (isFloifyAPI)
            {
                httpClient.BaseAddress = new Uri(floifyAPIs.APIServer);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppData.FloifyAuthResponse.Token);
                httpClient.DefaultRequestHeaders.Add("X-API-KEY", AppData.FloifyAuthResponse.APIKey);
            }
            else
            {
                if (AppData.AuthResponse != null && !string.IsNullOrEmpty(AppData.AuthResponse.AccessToken))
                {
                    /*Added if call is for SDK API*/
                    if (!isSDKAPICall)
                    {
                        httpClient.BaseAddress = new Uri(encompassAPIs.APIServer);
                    }
                    else
                    {
                        httpClient.BaseAddress = new Uri(encompassAPIs.SDKServer);
                    }
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    if (isAppraisalOrderCall)
                    {
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                    }
                    else if (uploadAttachment)
                    {
                        // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                    }
                    else
                    {
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    if (uploadAttachment)
                    {
                        if (!(string.IsNullOrEmpty(authorizationHeader)))
                        {
                            httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                        }
                        else
                        {
                            httpClient.DefaultRequestHeaders.Add("Authorization", (AppData.AuthResponse.TokenType + " " + AppData.AuthResponse.AccessToken));
                        }
                    }
                    else
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", (AppData.AuthResponse.TokenType + " " + AppData.AuthResponse.AccessToken));
                    }
                }
            }
            return httpClient;
        }
        private string GetEncodedApiKey()
        {
            string apiKey = authServiceAPIs.ApiKey + ":" + DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(apiKey));
        }

    }
}