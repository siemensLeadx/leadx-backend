using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace siemens_LeadX.Testing._shared
{
    [CollectionDefinition("AppCollection")]
    public class AppCollection : ICollectionFixture<ApplicationLocalizationFixture>
    {
    }
}
