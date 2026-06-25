using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public int CellId { get; set; }
        public bool IsTaken { get; set; }
        public DateTime? TakenAt { get; set; }
    }
}
