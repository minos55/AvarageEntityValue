using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nomnio.AverageEntityValue
{
    public class AverageEntitiesPropertyValues
    {
        public AverageEntitiesPropertyValues(IEnumerable<AzureTableEntity> entityList)
        {
            AveragePartitionKeyPropertyValue = entityList.Average(r => r.PartitionKeyPropertyValue);

            AverageRowKeyPropertyValue = entityList.Average(r => r.RowKeyPropertyValue);
            AverageTimestampPropertyValue = entityList.Average(r => r.TimestampPropertyValue);
            AverageVersionPropertyValue = entityList.Average(r => r.VersionPropertyValue);
            AverageCorrelationIdPropertyValue = entityList.Average(r => r.CorrelationIdPropertyValue);
            AverageActorIdPropertyValue = entityList.Average(r => r.ActorIdPropertyValue);
            AverageActorCodePropertyValue = entityList.Average(r => r.ActorCodePropertyValue);
            AverageSchemaPropertyValue = entityList.Average(r => r.SchemaPropertyValue);
            AverageCausedByPropertyValue = entityList.Average(r => r.CausedByPropertyValue);
            AverageUserPropertyValue = entityList.Average(r => r.UserPropertyValue);
            AverageIdPropertyValue = entityList.Average(r => r.IdPropertyValue);
            AverageTypePropertyValue = entityList.Average(r => r.TypePropertyValue);
            AverageDataPropertyValue = entityList.Average(r => r.DataPropertyValue);
            AverageEmittedPropertyValue = entityList.Average(r => r.EmittedPropertyValue);
            AverageEntityValue = entityList.Average(r => r.EntityValue);
        }

        public double AveragePartitionKeyPropertyValue { get; set; }
        public double AverageRowKeyPropertyValue { get; set; }
        public double AverageTimestampPropertyValue { get; set; }
        public double AverageVersionPropertyValue { get; set; }
        public double AverageCorrelationIdPropertyValue { get; set; }
        public double AverageActorIdPropertyValue { get; set; }
        public double AverageActorCodePropertyValue { get; set; }
        public double AverageSchemaPropertyValue { get; set; }
        public double AverageCausedByPropertyValue { get; set; }
        public double AverageUserPropertyValue { get; set; }
        public double AverageIdPropertyValue { get; set; }
        public double AverageTypePropertyValue { get; set; }
        public double AverageDataPropertyValue { get; set; }
        public double AverageEmittedPropertyValue { get; set; }
        public double AverageEntityValue { get; set; }

    }
}
