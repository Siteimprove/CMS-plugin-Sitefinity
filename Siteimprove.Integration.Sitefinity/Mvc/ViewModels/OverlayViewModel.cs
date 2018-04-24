using System.Globalization;

namespace Siteimprove.Integration.Sitefinity.Mvc.ViewModels
{
    /// <summary>
    /// The main view model used to structure the Overlay Widget's View
    /// </summary>
    public class OverlayViewModel
    {
        public string Token { get; set; }

        public string Domain { get; set; }

        public string PageUrl { get; set; }

        public bool ShouldLogActivity { get; set; }

        public ExternalScriptViewModel ExternalScript { get; set; }
    }
}
