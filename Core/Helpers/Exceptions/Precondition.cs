using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Exceptions
{
    public static class Precondition
    {
        public static void Requires(bool condition, string message, string property = null)
        {
            if (!condition)
                if (string.IsNullOrWhiteSpace(property))
                    throw new Exception(message);
                else
                    throw new ArgumentException(message, property);

        }
    }
}
