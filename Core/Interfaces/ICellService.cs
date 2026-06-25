using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Models;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICellService
    {
        Task<bool> OpenCellAsync(int cellId);
        Task<bool> CloseCellAsync(int cellId);
        Task<List<Cell>> GetStatusCells();
    }
}
