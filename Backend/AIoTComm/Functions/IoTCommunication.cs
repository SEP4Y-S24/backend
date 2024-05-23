using System.Net.Sockets;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace AIoTComm.Functions
{
    public static class IoTCommunication
    {
        
        [Function(nameof(IoTCommunication))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(IoTCommunication));
            logger.LogInformation("Connect to IoTDevice.");
            var outputs = new List<string>();
            
            await context.CallActivityAsync<string>(nameof(ConnectToDevice), "London");
            
            return outputs;
        }
        
        [Function(nameof(ConnectToDevice))]
        public static async Task ConnectToDevice([ActivityTrigger] string name, FunctionContext executionContext)
        {
            TcpServer.GetInstance();
        }
        


        [Function("StartIoTCommunication")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "connect")] 
            HttpRequestData req, [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("ConnectToIoTDevice");

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(IoTCommunication));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);
            
            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return await client.CreateCheckStatusResponseAsync(req, instanceId);
        }
    }
}
