using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Nomnio.AverageEntityValue
{
    public class AzureTableEntity : TableEntity
    {
        public AzureTableEntity()
        {
        }

        public AzureTableEntity(string partitionKey, string rowKey, DateTimeOffset timestamp, int version, string correlationId, Guid actorId, string actorCode,
            int schema, string causedBy, string user,Guid id, string type, string data, DateTime emitted)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Timestamp = timestamp;
            Version = version;
            CorrelationId = correlationId;
            ActorId = actorId;
            ActorCode = actorCode;
            Schema = schema;
            CausedBy = causedBy;
            User = user;
            Id = id;
            Type = type;
            Data = data;
            Emitted = emitted;
            CalculatePropertyValues();
        }

        private void CalculatePropertyValues()
        {
            //4 bytes + Len(PartitionKey + RowKey) * 2 bytes + For - Each Property(8 bytes + Len(Property Name) * 2 bytes + Sizeof(.Net Property Type))
            PartitionKeyPropertyValue = PartitionKey.Length * 2;
            RowKeyPropertyValue = RowKey.Length * 2;
            TimestampPropertyValue = 8 + nameof(Timestamp).Length * 2 + 8;
            VersionPropertyValue = 8 + nameof(Version).Length * 2 + sizeof(int);
            CorrelationIdPropertyValue = 8 + nameof(CorrelationId).Length * 2 + CorrelationId.Length * 2 + 4;
            ActorIdPropertyValue = 8 + nameof(ActorId).Length * 2 + 16;
            ActorCodePropertyValue = 8 + nameof(ActorCode).Length * 2 + ActorCode.Length * 2 + 4;
            SchemaPropertyValue = 8 + nameof(Schema).Length * 2 + sizeof(int);
            CausedByPropertyValue = 8 + nameof(CausedBy).Length * 2 + CausedBy.Length * 2 + 4;
            UserPropertyValue = 8 + nameof(User).Length * 2 + User.Length * 2 + 4;
            IdPropertyValue = 8 + nameof(Id).Length * 2 + 16;
            TypePropertyValue = 8 + nameof(Type).Length * 2 + Type.Length * 2 + 4;
            DataPropertyValue = 8 + nameof(Data).Length * 2 + Data.Length * 2 + 4;
            EmittedPropertyValue = 8 + nameof(Emitted).Length * 2 + 8;

            EntityValue = 4 + PartitionKeyPropertyValue + RowKeyPropertyValue + TimestampPropertyValue + VersionPropertyValue + CorrelationIdPropertyValue + ActorIdPropertyValue +
                ActorCodePropertyValue + SchemaPropertyValue + CausedByPropertyValue + UserPropertyValue + IdPropertyValue + TypePropertyValue + DataPropertyValue + EmittedPropertyValue;
        }

        public int Version { get; set; }
        public string CorrelationId { get; set; } = string.Empty;
        public Guid ActorId { get; set; }
        public string ActorCode { get; set; } = string.Empty;
        public int Schema { get; set; }
        public string CausedBy { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public DateTime Emitted { get; set; }

        public int PartitionKeyPropertyValue { get; set; }
        public int RowKeyPropertyValue { get; set; }
        public int TimestampPropertyValue { get; set; }
        public int VersionPropertyValue { get; set; }
        public int CorrelationIdPropertyValue { get; set; }
        public int ActorIdPropertyValue { get; set; }
        public int ActorCodePropertyValue { get; set; }
        public int SchemaPropertyValue { get; set; }
        public int CausedByPropertyValue { get; set; }
        public int UserPropertyValue { get; set; }
        public int IdPropertyValue { get; set; }
        public int TypePropertyValue { get; set; }
        public int DataPropertyValue { get; set; }
        public int EmittedPropertyValue { get; set; }
        public int EntityValue { get; set; }

    }
}
