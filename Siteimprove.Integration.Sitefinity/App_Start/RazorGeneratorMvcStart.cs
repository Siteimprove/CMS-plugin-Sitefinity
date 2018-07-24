using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using RazorGenerator.Mvc;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Siteimprove.Integration.Sitefinity.RazorGeneratorMvcStart), "Start")]

namespace Siteimprove.Integration.Sitefinity {
    public static class RazorGeneratorMvcStart {
        public static void Start() {
       
        }
    }
}
