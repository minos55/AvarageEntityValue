using System.Threading.Tasks;
using Nomnio.AverageEntityValue.Interfaces;
using Serilog;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Nomnio.AverageEntityValue
{
    public class CalculateEntityValueService : IEntityValue
    {
        private ILogger myLog;
        private string ConnectionString;
        private string TableName;
        CloudStorageAccount StorageAccount;

        public CalculateEntityValueService(string connectionString, string tableName)
        {
            myLog = Log.ForContext<CalculateEntityValueService>();
            ConnectionString = connectionString;
            TableName = tableName;
            StorageAccount = CloudStorageAccount.Parse(ConnectionString);
            myLog.Information("Connected to {Connection}", StorageAccount);
        }

        public async Task<AverageEntitiesPropertyValues> GetAverage()
        {
            var sourceTable = await GetTableAsync();
            var list = new List<AzureTableEntity>();
            AverageEntitiesPropertyValues result;
            var tableQuery = new TableQuery<AzureTableEntity>();
            TableContinuationToken continuationToken = null;
            do
            {
                // Retrieve a segment (up to 100 entities).
                var tableQuerySegment = await sourceTable.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQuerySegment.ContinuationToken;
                
                foreach (AzureTableEntity item in tableQuerySegment.Results)
                {
                    var entity = new AzureTableEntity(item.PartitionKey, item.RowKey, item.Timestamp, item.Version, item.CorrelationId, item.ActorId, item.ActorCode, item.Schema, item.CausedBy, item.User, item.Id, item.Type, item.Data, item.Emitted);
                    
                    list.Add(entity);
                }
                // Loop until a null continuation token is received, indicating the end of the table.
                result = new AverageEntitiesPropertyValues(list);
                myLog.Information("Averege PartitionKey value of enteties is({AveragePartitionKeyValue}) and averege RowKey value is ({RowKeyString}).",
                    result.AveragePartitionKeyPropertyValue, result.AverageRowKeyPropertyValue);
            } while (continuationToken != null);

            return result;
        }

        private async Task<CloudTable> GetTableAsync()
        {
            var sourceTableClient = StorageAccount.CreateCloudTableClient();
            var table = sourceTableClient.GetTableReference(TableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }
    }
}
