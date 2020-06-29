using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;
using System.Net.Http;
using System.Security.Cryptography;

namespace GraphIntegrationWebHook
{
    public static class GroupMembershipFunction
    {
        private static HttpClient httpClient = new HttpClient();
        private static string FlowUrl = "https://prod-30.canadacentral.logic.azure.com:443/workflows/c259d1eaf83247468b2e2e6f1855efb8/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=oGkatiqwnj0JWv6mpHHkLhL0EF4lw9P6S9CMAcYTTKU";
        public static async Task NotifyFlow(string input)
        {
            var response = await httpClient.PostAsync(FlowUrl, new StringContent(input));
            // Rest of function
        }

        [FunctionName("GroupMembershipFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            if(req.Method == "POST")
            {
                if(req.Query.ContainsKey("validationToken"))
                {
                    var token = System.Web.HttpUtility.HtmlDecode(req.Query["validationToken"]);
                    return new OkObjectResult(token);
                }
                else
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    //dynamic data = JsonConvert.DeserializeObject(requestBody);
                    log.LogInformation(requestBody);
                    await NotifyFlow(requestBody);
                    log.LogInformation("sent");
                    //var name = data?.name;
                    var result = new ObjectResult("");
                    result.StatusCode = StatusCodes.Status202Accepted;
                    return result;

                }
            }
            return (ActionResult)new OkObjectResult("Get");

           
        }
    }
}
