Type.registerNamespace("Siteimprove.Integration.Sitefinity.FieldControls");

Siteimprove.Integration.Sitefinity.FieldControls.OverlayScriptField = function (element) {
    Siteimprove.Integration.Sitefinity.FieldControls.OverlayScriptField.initializeBase(this, [element]);
}

Siteimprove.Integration.Sitefinity.FieldControls.OverlayScriptField.prototype =
    {
        initialize: function () {
            Siteimprove.Integration.Sitefinity.FieldControls.OverlayScriptField.callBaseMethod(this, "initialize");
        },

        dispose: function () {
            Siteimprove.Integration.Sitefinity.FieldControls.OverlayScriptField.callBaseMethod(this, "dispose");
        }
    };

Siteimprove.Integration.Sitefinity.FieldControls.OverlayScriptField.registerClass("Siteimprove.Integration.Sitefinity.FieldControls.OverlayScriptField", Telerik.Sitefinity.Web.UI.Fields.FieldControl);

