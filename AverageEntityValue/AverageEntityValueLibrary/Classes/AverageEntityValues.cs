using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Nomnio.AverageEntityValue
{
    public class AverageEntityValues
    {
        // Use this Dictionary store table's properties. 
        public IDictionary<string, double> Properties { get; private set; }
        private double EntityCounter;
        public AverageEntityValues()
        {
            Properties = new Dictionary<string, double>();
            EntityCounter = 0;
        }

        #region SetAndGetMethods
        public double GetPropertyAverage(string PropertyName)
        {
            if (!Properties.ContainsKey(PropertyName))
                return 0;
            var result = Properties[PropertyName] / EntityCounter;
            return result;
        }

        public double GetEntityAverage()
        {
            var SumAverage=(Properties.Sum(y => y.Value)+4*EntityCounter);
            var result = SumAverage / EntityCounter;
            return result;
        }

        public void IncrementSize(string PropertyName, object value)
        {
            double SizeOfProperty;
            if (PropertyName == "PartitionKey"|| PropertyName == "RowKey")
            {
                if(PropertyName == "PartitionKey")
                {
                    EntityCounter++;
                }
                SizeOfProperty = value.ToString().Length * 2;
            }
            else
            {
                SizeOfProperty = 8+ PropertyName.Length*2+(double)GetSizeOfProperty(value);
            }
            
            if (Properties.ContainsKey(PropertyName))
            {
                Properties[PropertyName] += SizeOfProperty;
            }
            else
            {
                Properties.Add(PropertyName, SizeOfProperty);
            }
        }

        public double GetNumberOfEntities()
        {
            return EntityCounter;
        }

        #endregion

        private int GetSizeOfProperty(object value)
        {
            if (value == null) return 0;
            if (value.GetType() == typeof(byte[]))
                return System.Runtime.InteropServices.Marshal.SizeOf((byte[])value);
            if (value.GetType() == typeof(bool))
                return sizeof(bool);
            if (value.GetType() == typeof(DateTimeOffset))
                return 10;
            if (value.GetType() == typeof(DateTime))
                return 8;
            if (value.GetType() == typeof(double))
                return sizeof(double);
            if (value.GetType() == typeof(Guid))
                return 16;
            if (value.GetType() == typeof(int))
                return sizeof(int);
            if (value.GetType() == typeof(long))
                return sizeof(long);
            if (value.GetType() == typeof(string))
                return value.ToString().Length * 2 + 4;
            throw new Exception(string.Format("This value type {0} is not supported", value.GetType()));
        }
    }
}
