using System.Threading.Tasks;
using Nomnio.AverageEntityValue.Interfaces;
using Serilog;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Nomnio.AverageEntityValue
{
    public class CalculateEntityValueServices : IEntityValue
    {
        private ILogger myLog;
        private string ConnectionString;
        private string TableName;
        CloudStorageAccount StorageAccount;
        int BatchSize;
        public CalculateEntityValueServices(string connectionString, string tableName,int batchSize)
        {
            myLog = Log.ForContext<CalculateEntityValueServices>();
            ConnectionString = connectionString;
            TableName = tableName;
            BatchSize = batchSize;
            StorageAccount = CloudStorageAccount.Parse(ConnectionString);
            myLog.Information("Connected to {Connection}", StorageAccount);

        }

        public async Task<AverageEntitiesPropertyValues> GetAverage()
        {
            var sourceTable = await GetTableAsync();
            var list = new List<AzureTableEntity>();
            AverageEntitiesPropertyValues result=new AverageEntitiesPropertyValues();
            var tableQuery = new TableQuery().Take(BatchSize);
            TableContinuationToken continuationToken = null;
            do
            {
                // Retrieve a segment (up to 100 entities).
                var tableQuerySegment = await sourceTable.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQuerySegment.ContinuationToken;
                
                
                foreach (DynamicTableEntity item in tableQuerySegment.Results)
                {
                    var partitionKey = item.PartitionKey;
                    var rowKey = item.RowKey;

                    //var entity = new AzureTableEntity(item.PartitionKey, item.RowKey, item.Timestamp, item.Version, item.CorrelationId, item.ActorId, item.ActorCode, item.Schema, item.CausedBy, item.User, item.Id, item.Type, item.Data, item.Emitted);
                    
                    //list.Add(entity);
                }
                // Loop until a null continuation token is received, indicating the end of the table.
                //result = new AverageEntitiesPropertyValues(list);
                /*myLog.Information("Averege PartitionKey value of the first {NumberOfEnteties} enteties is({AveragePartitionKeyValue}),averege RowKey value is ({AverageRowKeyValue}) and averege entetie value is ({AverageEntetieValue}).",
                    list.Count,result.AveragePartitionKeyPropertyValue, result.AverageRowKeyPropertyValue,result.AverageEntityValue);*/
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
