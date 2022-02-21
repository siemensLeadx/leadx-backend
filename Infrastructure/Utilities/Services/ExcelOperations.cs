using Application.Interfaces;
using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Services
{
    public class ExcelOperations : IExcelOperations
    {
        public async Task ExportExcel<T>(IEnumerable<T> data, string filename, string sheetName)
        {
            var excel = new ExcelMapper();

            await excel.SaveAsync(filename, data, sheetName);
        }
    }
}
