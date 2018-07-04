using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBatch.Model
{
    public class OrderSummary
    {
        public string BoutiqueId { get; set; }

        public string OrderId { get; set; }

        public decimal TotalValue { get; set; }
    }
}
