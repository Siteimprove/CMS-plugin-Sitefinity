using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Abstractions;

namespace Siteimprove.Integration.Sitefinity.App_Start
{
    public static class PluginStartup
    {
        public static void OnPreApplicationStart()
        {
            Bootstrapper.Initialized += Bootstrapper_Initialized;
        }

        private static void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "RegisterRoutes")
            {
                SiteimproveModule.Register();
            }

        }

        public const string InitilizeMethodName = "OnPreApplicationStart";
    }
}
