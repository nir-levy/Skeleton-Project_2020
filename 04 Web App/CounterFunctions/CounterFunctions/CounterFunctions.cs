using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CounterFunctions
{
    public static class CounterFunctions
    {
        private static readonly AzureSignalR SignalR = new AzureSignalR(Environment.GetEnvironmentVariable("AzureSignalRConnectionString"));

        [FunctionName("negotiate")]
        public static async Task<SignalRConnectionInfo> NegotiateConnection(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage request,
            ILogger log)
        {
            try
            {
                ConnectionRequest connectionRequest = await ExtractContent<ConnectionRequest>(request);
                log.LogInformation($"Negotiating connection for user: <{connectionRequest.UserId}>.");

                string clientHubUrl = SignalR.GetClientHubUrl("CounterHub");
                string accessToken = SignalR.GenerateAccessToken(clientHubUrl, connectionRequest.UserId);
                return new SignalRConnectionInfo { AccessToken = accessToken, Url = clientHubUrl };
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to negotiate connection.");
                throw;
            }
        }

        [FunctionName("update-counter")]
        public static async Task UpdateCounter(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage request,
            [Table("CountersTable")] CloudTable cloudTable,
            [SignalR(HubName = "CounterHub")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            log.LogInformation("Updating counter.");

            Counter counterRequest = await ExtractContent<Counter>(request);

            Counter cloudCounter = await GetOrCreateCounter(cloudTable, counterRequest.Id);
            cloudCounter.Count++;
            TableOperation updateOperation = TableOperation.Replace(cloudCounter);
            await cloudTable.ExecuteAsync(updateOperation);

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "CounterUpdate",
                    Arguments = new object[] { cloudCounter }
                });
        }

        [FunctionName("get-counter")]
        public static async Task<Counter> GetCounter(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-counter/{id}")] HttpRequestMessage request,
            [Table("CountersTable")] CloudTable cloudTable,
            string id,
            ILogger log)
        {
            log.LogInformation("Getting counter.");

            return await GetOrCreateCounter(cloudTable, int.Parse(id));
        }

        private static async Task<T> ExtractContent<T>(HttpRequestMessage request)
        {
            string connectionRequestJson = await request.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(connectionRequestJson);
        }

        private static async Task<Counter> GetOrCreateCounter(CloudTable cloudTable, int counterId)
        {
            TableQuery<Counter> idQuery = new TableQuery<Counter>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, counterId.ToString()));

            TableQuerySegment<Counter> queryResult = await cloudTable.ExecuteQuerySegmentedAsync(idQuery, null);
            Counter cloudCounter = queryResult.FirstOrDefault();
            if (cloudCounter == null)
            {
                cloudCounter = new Counter { Id = counterId };

                TableOperation insertOperation = TableOperation.InsertOrReplace(cloudCounter);
                cloudCounter.PartitionKey = "counter";
                cloudCounter.RowKey = cloudCounter.Id.ToString();
                TableResult tableResult = await cloudTable.ExecuteAsync(insertOperation);
                return await GetOrCreateCounter(cloudTable, counterId);
            }

            return cloudCounter;
        }
    }
}
