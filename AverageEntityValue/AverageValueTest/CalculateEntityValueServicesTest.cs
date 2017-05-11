using System;
using System.Collections.Generic;
using Xunit;
using Nomnio.AverageEntityValue;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace AverageValueTest
{
    public class CalculateEntityValueServicesTest
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const string sourceConnectionString = "DefaultEndpointsProtocol=https;AccountName=mt1;AccountKey=O9+FoFPCQ4wqqfMJLm5I1zp7sePAgGGfowvDmCnGBt+AKlrdTXGOJ8QuzoQWz7yTsKPiOvBRE/8PfW5kRzzsTg==;EndpointSuffix=core.windows.net";
        const string targetConnectionString = "DefaultEndpointsProtocol=https;AccountName=mt1;AccountKey=O9+FoFPCQ4wqqfMJLm5I1zp7sePAgGGfowvDmCnGBt+AKlrdTXGOJ8QuzoQWz7yTsKPiOvBRE/8PfW5kRzzsTg==;EndpointSuffix=core.windows.net";

        [Theory]
        [InlineData(sourceConnectionString, targetConnectionString,100,670,1)]
        [InlineData(sourceConnectionString, targetConnectionString,250, 799, 2)]
        public async Task Check_If_Average_Is_Calculated_Corectly_Test(string _sourceConnectionString, string _targetConnectionString,int batchSize,double expectedResult,int testDataSize)
        {
            var emptySourceTableName = RandomString(16);
            var emptySourceStorageAccount = GetCloudStorageAccount(_sourceConnectionString);
            var emptySourceTable = await GetTableAsync(emptySourceStorageAccount, emptySourceTableName);
            var fakeResults = PrepareTestData(testDataSize);
            await WriteToTableAsync(emptySourceTable, fakeResults);

            var entityValue = new CalculateEntityValueServices(_targetConnectionString, emptySourceTableName, batchSize);
            var expected = new AverageEntitiesPropertyValues(fakeResults);
            var task = await entityValue.GetAverage();
            await DeleteTableAsync(emptySourceTable);
            Assert.Equal(expectedResult, task.AverageEntityValue);
        }

        private string RandomString(int length)
        {
            var random = new Random();
            string tableName = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return "t" + tableName;
        }

        private CloudStorageAccount GetCloudStorageAccount(string connectionString)
        {
            return CloudStorageAccount.Parse(connectionString);
        }

        private async Task<CloudTable> GetTableAsync(CloudStorageAccount storageAccount, string tableName)
        {
            var sourceTableClient = storageAccount.CreateCloudTableClient();
            var table = sourceTableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        private IEnumerable<AzureTableEntity> PrepareTestData(int numberOfData)
        {
            var testData = new List<AzureTableEntity>();
            if(numberOfData>1)
            {
                testData.Add(new AzureTableEntity("KMS-514aef17-6794-6dec-ed1c-f8cc15e5680c", "SS-SE-0006845067", new DateTimeOffset(), 6845067, "5eb9cc34-89f0-478a-aff3-80e06abad2d9",
                    new Guid("514aef17-6794-6dec-ed1c-f8cc15e5680c"), "KMS", 0, "ChangeDailyTimetable", "google-oauth2|105376100120990910344", new Guid("dc83d736-21f5-418e-9b25-5030aa37f795"),
                    "DailyTimetableChanged", "{\"TimetableCode\":\"P1\",\"DayOfWeek\":\"Wednesday\",\"Intervals\":[{\"Key\":0,\"Value\":47},{\"Key\":52,\"Value\":44}],\"CircuitCode\":\"HC2\"}",
                    new DateTime(2016, 11, 20, 11, 3, 4, 68)));
            }
            testData.Add(new AzureTableEntity("KMS-514aef17-6794-6dec-ed1c-f8cc15e5680c", "SS-SE-0006824287", new DateTimeOffset(), 6824287, "OYpLaQHEsEjdNb5TnpoWx6Ih6GumOtVt",
                    new Guid("514aef17-6794-6dec-ed1c-f8cc15e5680c"), "KMS", 0, "UpdateFromRegistries", "System", new Guid("63652686-23f6-4952-b8dd-57d995a8b465"),
                    "RelayStateChanged", "{\"State\":true,\"RelayCode\":\"R4\"}", new DateTime(2016, 11, 18, 18, 7, 21, 11)));

            return testData;
        }

        private async Task WriteToTableAsync(CloudTable table, IEnumerable<AzureTableEntity> enteties)
        {
            foreach (var item in enteties)
            {
                TableOperation insert = TableOperation.InsertOrReplace(item);
                await table.ExecuteAsync(insert);
            }
        }

        private async Task DeleteTableAsync(CloudTable table)
        {
            await table.DeleteIfExistsAsync();
        }
    }
}
