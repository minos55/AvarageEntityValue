using System.Threading.Tasks;
using Serilog;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Nomnio.AverageEntityValue
{
    public class CalculateEntityValueServices
    {
        private ILogger myLog;
        private string ConnectionString;
        private string TableName;
        private CloudStorageAccount StorageAccount;
        private int BatchSize;
        private bool ShowLogOutput;

        public CalculateEntityValueServices(string connectionString, string tableName, int batchSize, bool showLogOutput)
        {
            myLog = Log.ForContext<CalculateEntityValueServices>();
            ConnectionString = connectionString;
            TableName = tableName;
            BatchSize = batchSize;
            StorageAccount = CloudStorageAccount.Parse(ConnectionString);
            myLog.Information("Connected to {Connection}", StorageAccount);
            ShowLogOutput = showLogOutput;
        }

        public async Task<AverageEntityValues> GetAverage()
        {
            var sourceTable = await GetTableAsync();
            var tableQuery = new TableQuery<DynamicTableEntity>().Take(BatchSize);
            TableContinuationToken continuationToken = null;
            var result = new AverageEntityValues();
            do
            {
                // Retrieve a segment (up to 100 entities).
                var tableQuerySegment = await sourceTable.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQuerySegment.ContinuationToken;

                foreach (DynamicTableEntity entity in tableQuerySegment.Results)
                {
                    result.IncrementSize("PartitionKey", entity.PartitionKey);
                    result.IncrementSize("RowKey", entity.PartitionKey);
                    result.IncrementSize("Timestamp", entity.Timestamp);
                    result.IncrementSize("ETag", entity.ETag);
                    foreach (var property in entity.Properties)
                    {
                        result.IncrementSize(property.Key, property.Value.PropertyAsObject);
                    }
                }

                // Loop until a null continuation token is received, indicating the end of the table.

                myLog.Information("Average values of properties in bytes for {NumberOfEntiteis} enteties", result.GetNumberOfEntities());
                

            } while (continuationToken != null);

            myLog.Information("Average value of all entities is {EntityAverage}", result);

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
