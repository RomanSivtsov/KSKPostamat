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
        public bool CellHasPackage { get; set; }
        public bool IsClosed { get; set; }
    }
}
