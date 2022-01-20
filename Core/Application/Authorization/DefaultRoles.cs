using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    public enum DefaultRoles
    {
        SUPER_ADMIN = 0,
        ADMIN = 1,
        VIEW_ONLY = 2,
        USER = 3,
        BLOCKED = 4
    }
}
