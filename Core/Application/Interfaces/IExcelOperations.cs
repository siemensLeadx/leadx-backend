using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExcelOperations
    {
        Task ExportExcel<T>(IEnumerable<T> data, string filename, string sheetName);
    }
}
