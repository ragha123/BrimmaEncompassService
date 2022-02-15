using Brimma.LOSService.Common;
using Brimma.LOSService.DTO;
using Brimma.LOSService.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
    Positive test case – Get User Profile for existing user
    Negative test case – Get Error for non-existing user
*/
namespace Brimma.LOSService.Test
{
    [TestFixture]
    public class OrderOutUserProfileTest
    {
        private DependencyResolverHelpercs _serviceProvider;
        private string bearerTokenValue = "0007rqvbsiz8PGsP7IkIzUN2zJ2b";
        private string existingUserIdPass = "zvelez";           
        private string nonexistingUserIdFail = "notauser";        
        public OrderOutUserProfileTest()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()                
                .UseEnvironment(env)               
                .Build();
            _serviceProvider = new DependencyResolverHelpercs(webHost);
        }
        #region Positive test cases
        [Test]
        public async Task Should_Return_SuccessResponse_For_Existing_UserId()
        {                      
            await positiveTestCase(existingUserIdPass);
        }        
        #endregion
        #region Negative test cases        
        [Test]
        public async Task Should_Return_FalseResponse_For_NonExisting_UserId()
        {            
            await negativeTestCase(nonexistingUserIdFail);
        }        
        #endregion
        #region Private methods
        private void GetEncompassAccessToken()
        {
            AppData.AuthResponse = new AuthResponse
            {
                TokenType = "Bearer",
                AccessToken = bearerTokenValue
            };
        }
        private async Task positiveTestCase(string userId)
        {
            GetEncompassAccessToken();
            var userService = _serviceProvider.GetService<IOrderOutService>();            
            var response = await userService.GetUserProfile(userId);
            Assert.IsNotNull(response);
            if (response!=null)
            {
                object objJValueUserId = response?.GetType().GetProperty("userId")?.GetValue(response, null);
                Assert.AreEqual(((JValue)objJValueUserId).Value, userId);
            }            
        }
        private async Task negativeTestCase(string userId)
        {
            GetEncompassAccessToken();
            var userService = _serviceProvider.GetService<IOrderOutService>();            
            var response = await userService.GetUserProfile(userId);
            Assert.IsNotNull(response);
            if (response != null)
            {                
                Assert.IsNotNull(((ErrorResponse)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value).Error.Message);                
            }
        } 
        #endregion
    }
}