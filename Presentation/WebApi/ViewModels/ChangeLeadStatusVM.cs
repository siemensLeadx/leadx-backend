using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class ChangeLeadStatusVM
    {
        public long lead_id { get; set; }
        public int status_id { get; set; }
        public string notes { get; set; }
    }
}
