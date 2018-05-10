
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace AzureFunction1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["textoTweet"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var regex = new Regex(@"(?<=#)\w+");
            var matches = regex.Matches(name);
            string textoNuevo = "";
            foreach (Match m in matches)
            {
                textoNuevo = name.Replace(m.Value, "").ToString().Replace("#", "").ToString();

            }


            var result = textoNuevo.ToString();

            ObjectResult error = new ObjectResult("Texto vacío");
            ActionResult actionResult = new OkObjectResult(result);

            BadRequestObjectResult badRequestObjectResult = new BadRequestObjectResult(error);

            if (name != null)
            {
                return (actionResult);
            }
            else
            {
                return (error);
            }
            
        }
    }
}
