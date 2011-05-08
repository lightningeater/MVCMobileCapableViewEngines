using System.Web.Mvc;

namespace Application.MVCExtensions.Mobile
{
    public class MobileCapableRazorViewEngine : RazorViewEngine, IMobileViewEngine
    {
        public MobileCapableRazorViewEngine()
        {
            this.mobileOverrideQueryStringParam = "M2W";
            this.deviceFolders = new System.Collections.Specialized.StringDictionary {
                { "Pocket IE", "WindowsMobile" },
                { "AppleMAC-Safari", "iPhone" }
            };
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialPath, bool useCache)
        {
            return MobileViewEngine.FindPartialView<IMobileViewEngine>(this, controllerContext, partialPath, useCache);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return MobileViewEngine.FindView<IMobileViewEngine>(this, controllerContext, viewName, masterName, useCache);
        }


        #region IMobileViewEngine Members
        private string mobileOverrideQueryStringParam;
        public string MobileOverrideQueryStringParam { get { return this.mobileOverrideQueryStringParam; } set { this.mobileOverrideQueryStringParam = value; } }
        private System.Collections.Specialized.StringDictionary deviceFolders;
        public System.Collections.Specialized.StringDictionary DeviceFolders { get { return this.deviceFolders; } }

        public virtual ViewEngineResult ResolvePartialView(ControllerContext controllerContext, string partialPath, bool useCache)
        {
            return base.FindPartialView(controllerContext, partialPath, useCache);
        }

        public virtual ViewEngineResult ResolveView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        #endregion
    }
}