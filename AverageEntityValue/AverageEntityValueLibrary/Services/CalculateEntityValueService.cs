using System.Threading.Tasks;
using Nomnio.AverageEntityValue.Interfaces;
using Serilog;
using Microsoft.WindowsAzure.Storage;

namespace Nomnio.AverageEntityValue
{
    class CalculateEntityValueService : IEntityValue
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

        public async Task Average()
        {
            
        }
    }
}
