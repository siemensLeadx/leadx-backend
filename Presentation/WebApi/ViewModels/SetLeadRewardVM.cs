using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class SetLeadRewardVM
    {
        public long lead_id { get; set; }
        public int? reward_class_id { get; set; }
        public int? reward_criteria_id { get; set; }
    }
}
