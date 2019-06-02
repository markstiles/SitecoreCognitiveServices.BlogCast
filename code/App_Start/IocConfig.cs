using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using SitecoreCognitiveServices.Feature.BlogCast.Areas.SitecoreCognitiveServices.Controllers;

namespace SitecoreCognitiveServices.Feature.BlogCast
{
    public class IocConfig : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(BlogCastController));
        }
    }
}