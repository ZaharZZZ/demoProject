using Google.Protobuf.WellKnownTypes;
using System;
using System.ComponentModel;

using System;

namespace DemoLib
{
    public class OrderRecord
    {
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public DateTime SaleDate { get; set; }
    }
}