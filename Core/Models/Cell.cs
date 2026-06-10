using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Cell
    {
        public int CellId { get; set; }
        public int? OrderId { get; set; }
        public bool OrderIsPaid { get; set; }
        public bool CellHasPackage { get; set; }
        public int isClosed { get; set; }
        public bool isTaken { get; set; }
    }
}
