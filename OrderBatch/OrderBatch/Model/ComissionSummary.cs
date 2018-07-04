using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBatch.Model
{
    public class ComissionSummary
    {
        public string BoutiqueId { get; set; }

        public decimal TotalSubjectOrders { get; set; }

        public decimal TotalComission => TotalSubjectOrders * 0.1M;
    }
}
