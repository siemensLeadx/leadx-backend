using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class DashboardLeadsVM
    {
        public int page_number { get; set; }
        public int page_size { get; set; }
        public string name { get; set; }
    }
}
