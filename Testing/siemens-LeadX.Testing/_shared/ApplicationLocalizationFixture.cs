using Helpers.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace siemens_LeadX.Testing._shared
{
    public class ApplicationLocalizationFixture : IDisposable
    {
        public ApplicationLocalization localizer { get; private set; }
        public ApplicationLocalizationFixture()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            var stringLocalizer = new StringLocalizer<SharedResource>(factory);

            localizer = new ApplicationLocalization(stringLocalizer);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
