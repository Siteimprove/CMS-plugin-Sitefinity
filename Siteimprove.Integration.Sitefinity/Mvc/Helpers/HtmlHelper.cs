using System.Web;
using System.Web.Mvc;

namespace Siteimprove.Integration.Sitefinity.Mvc.Helpers
{
    /// <summary>
    /// Helps with general Html render logic for Razor views
    /// </summary>
    public static class HtmlHelper
    {
        public static HtmlString RenderIfTrue(this System.Web.Mvc.HtmlHelper helper, bool condition, string text)
        {
            var resultText = string.Empty;
            if (condition)
                resultText = text;

            return new HtmlString(resultText);
        }
    }
}
