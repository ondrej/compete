using System.Linq;
using System.Web.Mvc;
using Compete.Core.Infrastructure;
using Compete.Site.Infrastructure;
using Compete.Site.Refereeing;
using Compete.Site.Startup;

using Machine.Container;
using Machine.Container.Plugins;
using Machine.Core;

namespace Compete.Site
{
  public class SiteServices : IServiceCollection
  {
    public void RegisterServices(ContainerRegisterer register)
    {
      register.Type<WebServerStartup>();
      register.Type<AdministratorAuthentication>();
      register.Type<RefereeThread>();
      register.Type<ScoreKeeper>();
      register.Type<IFormsAuthentication>().ImplementedBy<FormsAuthenticationService>();
      register.Type<ISignin>().ImplementedBy<SigninService>();
      register.Type<IInitialSetup>().ImplementedBy<InitialSetupService>();
      register.Type<MatchStarter>();

      GetType().Assembly.GetExportedTypes().Where(x => typeof(Controller).IsAssignableFrom(x)).Each(
        x => register.Type(x).AsTransient()
        );
    }
  }
}