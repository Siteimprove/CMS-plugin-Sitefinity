using Siteimprove.Integration.Sitefinity.Configuration;
using System.Collections.Generic;
using System.Web.UI;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Web.UI.Fields;

namespace Siteimprove.Integration.Sitefinity.FieldControls
{
    public class OverlayScriptField : FieldControl
    {
        protected override string LayoutTemplateName
        {
            get
            {
                return null;
            }
        }

        public override string LayoutTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(base.LayoutTemplatePath))
                    base.LayoutTemplatePath = OverlayScriptField.layoutTemplatePath;
                return base.LayoutTemplatePath;
            }
            set
            {
                base.LayoutTemplatePath = value;
            }
        }

        protected override void InitializeControls(GenericContainer container)
        {
        }

        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            var scripts = new List<ScriptReference>(base.GetScriptReferences());
            scripts.Add(new ScriptReference(OverlayScriptField.ViewScript, typeof(OverlayScriptField).Assembly.FullName));
            scripts.Add(new ScriptReference(Config.Get<SiteimproveConfig>().ScriptUrl));
            return scripts;
        }

        internal const string ViewScript = "Siteimprove.Integration.Sitefinity.FieldControls.OverlayScriptField.OverlayScriptField.js";
        private const string layoutTemplatePath = "~/Siteimprove/Siteimprove.Integration.Sitefinity.FieldControls.OverlayScriptField.OverlayScriptField.ascx";
    }
}
