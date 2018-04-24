namespace Siteimprove.Integration.Sitefinity.Mvc.ViewModels
{
    /// <summary>
    /// A POCO representaion of an external JavaScript reference
    /// </summary>
    public class ExternalScriptViewModel
    {
        public string Url { get; set; }

        public bool LoadAsync { get; set; }
    }
}
