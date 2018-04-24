using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace Siteimprove.Integration.Sitefinity.Resources
{
    /// <summary>
    /// 
    /// </summary>
    [ObjectInfo(typeof(SiteimproveResources), Title = "SiteimproveResourcesTitle", Description = "SiteimproveResourcesDescription")]
    public class SiteimproveResources : Resource
    {
        public SiteimproveResources() { }

        public SiteimproveResources(ResourceDataProvider dataProvider) : base(dataProvider) { }

        [ResourceEntry("SiteimproveResourcesTitle", Value = "Siteimprove Plugin", Description = "Localizes the Title of the SiteimproveResources class")]
        public string SiteimproveResourcesTitle
        {
            get
            {
                return this["SiteimproveResourcesTitle"];
            }
        }

        [ResourceEntry("SiteimproveResourcesDescription", Value = "This resource labels resource class helps with localization of the Siteimprove Plugin's interface", Description = "Holds the value of the Description field of the SiteimproveResources class")]
        public string SiteimproveResourcesDescription
        {
            get
            {
                return this["SiteimproveResourcesDescription"];
            }
        }

        [ResourceEntry("SiteimproveModuleDescription", Value = "The Siteimprove CMS Plugin bridges the gap between Progress Sitefinity CMS and the Siteimprove Intelligence Platform. You are able to put your Siteimprove results to use where they are most valuable – during your content creation and editing process.", Description = "Shows user-friendly description for the Siteimprove module")]
        public string SiteimproveModuleDescription
        {
            get
            {
                return this["SiteimproveModuleDescription"];
            }
        }

        [ResourceEntry("SiteimproveConfig", Value = "Siteimprove Configuration", Description = "The title of the SiteimproveConfig section")]
        public string SiteimproveConfig
        {
            get
            {
                return this["SiteimproveConfig"];
            }
        }

        [ResourceEntry("ScriptUrlTitle", Value = "Script Url", Description = "The title of the ScriptUrl configuration property")]
        public string ScriptUrlTitle
        {
            get
            {
                return this["ScriptUrlTitle"];
            }
        }

        [ResourceEntry("ScriptUrlDescription", Value = "Configure the Url for the main script that will be used by Siteimprove to load its libraries", Description = "The description for the ScriptUrl configuration property")]
        public string ScriptUrlDescription
        {
            get
            {
                return this["ScriptUrlDescription"];
            }
        }

        [ResourceEntry("LogActivityInTheConsoleTitle", Value = "Log Activity in the Browser Console", Description = "The title of the LogActivityInTheConsoleTitle configuration property")]
        public string LogActivityInTheConsoleTitle
        {
            get
            {
                return this["LogActivityInTheConsoleTitle"];
            }
        }

        [ResourceEntry("LogActivityInTheConsoleDescription", Value = "If True, the Siteimprove Overlay JavaScript will log its activity in the web browser console", Description = "The description for the LogActivityInTheConsoleDescription configuration property")]
        public string LogActivityInTheConsoleDescription
        {
            get
            {
                return this["LogActivityInTheConsoleDescription"];
            }
        }

        [ResourceEntry("TokensTitle", Value = "Api Tokens", Description = "Used for the title of the Tokens configuration property")]
        public string TokensTitle
        {
            get
            {
                return this["TokensTitle"];
            }
        }

        [ResourceEntry("TokensDescription", Value = "Configure the tokens that are used to manage the included domains in Siteimprove", Description = "Used for the description of the Tokens configuration property")]
        public string TokensDescription
        {
            get
            {
                return this["TokensDescription"];
            }
        }

        [ResourceEntry("DomainTitle", Value = "Domain", Description = "Used as title of the Domain configuration property")]
        public string DomainTitle
        {
            get
            {
                return this["DomainTitle"];
            }
        }

        [ResourceEntry("DomainDescription", Value = "Domain managed in Siteimprove, i.e. https://www.mydomain.com", Description = "Used to describe the Domain configuration property")]
        public string DomainDescription
        {
            get
            {
                return this["DomainDescription"];
            }
        }

        [ResourceEntry("TokenTitle", Value = "Token", Description = "Used as title of the Token configuration property")]
        public string TokenTitle
        {
            get
            {
                return this["TokenTitle"];
            }
        }

        [ResourceEntry("TokenDescription", Value = "The token that will be used by Siteimprove to manage the respective domain, i.e. 8669723c3a4e475cbe2b303b690a3db6", Description = "Used to describe the Token configuration property")]
        public string TokenDescription
        {
            get
            {
                return this["TokenDescription"];
            }
        }

        [ResourceEntry("ErrorScriptUrlNotValid", Value = "There was an error with the Siteimprove script Url not valid. You must provide a valid Url for the main script used by Siteimprove, i.e. https://cdn.siteimprove.net/cms/overlay.js. Please check the Siteimprove configurations in the Advanced setting section", Description = "Error message for the Siteimprove Script Not Valid")]
        public string ErrorScriptUrlNotValid
        {
            get
            {
                return this["ErrorScriptUrlNotValid"];
            }
        }

        [ResourceEntry("ErrorOverlayWidgetLoading", Value = "There was an error loading the Siteimprove Overlay widget onto the: ", Description = "Error message for the Overlay widget not loading")]
        public string ErrorOverlayWidgetLoading
        {
            get
            {
                return this["ErrorOverlayWidgetLoading"];
            }
        }

        [ResourceEntry("ErrorRecheckPageResponse", Value = "Failed to recheck a page with Url: {0} using the {1} endpoint. The service returned status code {2} with the following contnet:", Description = "Error message for failed response of a recheck page operation")]
        public string ErrorRecheckPageResponse
        {
            get
            {
                return this["ErrorRecheckPageResponse"];
            }
        }

        [ResourceEntry("ErrorRecheckPageException", Value = "Could not request a recheck of the {0} page. Please ensure that no Firewall is blocking the Siteimprove url at {1}", Description = "Error message used in the exception for a page recheck")]
        public string ErrorRecheckPageException
        {
            get
            {
                return this["ErrorRecheckPageException"];
            }
        }
        
        [ResourceEntry("ErrorFetchTokenResponse", Value = "Request for token at {0} did not return status 200 OK, but status {1} instead. The returned content was {2}", Description = "Error message used upon failed response on the fetching token")]
        public string ErrorFetchTokenResponse
        {
            get
            {
                return this["ErrorFetchTokenResponse"];
            }
        }

        [ResourceEntry("ErrorFetchTokenException", Value = "Failed to fetch token from Siteimprove", Description = "Error message used when fetching tokens fails with exception")]
        public string ErrorFetchTokenException
        {
            get
            {
                return this["ErrorFetchTokenException"];
            }
        }

        [ResourceEntry("ErrorControllerType", Value = "The host is only allowed to host Types that are descendants of System.Web.Mvc.Controller", Description = "Error for when the controller type requirement is not met")]
        public string ErrorControllerType
        {
            get
            {
                return this["ErrorControllerType"];
            }
        }

        [ResourceEntry("ErrorSavingTokenInConfigs", Value = "Siteimprove Plugin: Cannot set a new Token. Please ensure that configuration files are not write protected.", Description = "Used to display error if Siteimprove token cannot be saved into the Sitefinity configs")]
        public string ErrorSavingTokenInConfigs
        {
            get
            {
                return this["ErrorSavingTokenInConfigs"];
            }
        }
    }
}
