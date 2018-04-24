using System.Web;
using System.Reflection;
using System.Runtime.InteropServices;
using Siteimprove.Integration.Sitefinity.App_Start;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Siteimprove.Integration.Sitefinity")]
[assembly: AssemblyDescription("The Siteimprove CMS Plugin bridges the gap between Progress Sitefinity CMS and the Siteimprove Intelligence Platform. You are able to put your Siteimprove results to use where they are most valuable – during your content creation and editing process")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Siteimprove")]
[assembly: AssemblyProduct("Siteimprove.Integration.Sitefinity")]
[assembly: AssemblyCopyright("Copyright ©  2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b3396756-abc2-4313-80ca-9c333f4e319c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("10.2.6600.0")]
[assembly: AssemblyFileVersion("10.2.6600.0")]
[assembly: PreApplicationStartMethod(typeof(PluginStartup), PluginStartup.InitilizeMethodName)]
[assembly: ControllerContainer()]
