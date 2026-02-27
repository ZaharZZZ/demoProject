using System.Collections.Generic;

using System.Collections.Generic;

namespace DemoLib
{
    public class Order
    {
        private List<OrderRecord> records = new List<OrderRecord>();

        public IReadOnlyList<OrderRecord> Records => records.AsReadOnly();

        public void AddRecord(OrderRecord record)
        {
            records.Add(record);
        }

        public void RemoveRecord(OrderRecord record)
        {
            records.Remove(record);
        }

        public void Clear()
        {
            records.Clear();
        }
    }
}