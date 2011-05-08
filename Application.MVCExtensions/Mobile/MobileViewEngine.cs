using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Globalization;

namespace Application.MVCExtensions.Mobile
{
    public interface IMobileViewEngine
    {
        string MobileOverrideQueryStringParam { get; }
        StringDictionary DeviceFolders { get; }
        ViewEngineResult ResolvePartialView(ControllerContext controllerContext, string partialPath, bool useCache);
        ViewEngineResult ResolveView(ControllerContext controllerContext, string viewName, string masterName, bool useCache);
    }

    internal static class MobileViewEngine
    {
        internal static ViewEngineResult FindPartialView<T>(T engine, ControllerContext controllerContext, string partialPath, bool useCache) where T : IMobileViewEngine
        {
            var request = controllerContext.HttpContext.Request;
            ViewEngineResult result = null;

            if (MobileDetect.IsMobile(controllerContext.HttpContext) && !MobileDetect.OverrideMobile(controllerContext.HttpContext, engine.MobileOverrideQueryStringParam))
            {
                /*
                 *  Device or capabilities specific mobile path. 
                 *  i.e. /Mobile/iPhone/ViewName or /Mobile/WebKit/ViewName
                */
                string mobileViewName = MobilePathHelper.Resolve(engine, request, partialPath);
                result = engine.ResolvePartialView(controllerContext, mobileViewName, useCache);

                if (result == null || result.View == null)
                {
                    /*  Fallback:
                     *  Non-Device/capabilities specific mobile path. 
                     *  i.e. /Mobile/ViewName
                    */
                   mobileViewName = string.Format(CultureInfo.InvariantCulture, "Mobile/{0}", partialPath);

                   result = engine.ResolvePartialView(controllerContext, mobileViewName, useCache);
                }
            }

            if (result == null || result.View == null)
            {
                result = engine.ResolvePartialView(controllerContext, partialPath, useCache);
            }

            return result;
        }

        internal static ViewEngineResult FindView<T>(T engine, ControllerContext controllerContext, string viewName, string masterName, bool useCache) where T : IMobileViewEngine
        {
            var request = controllerContext.HttpContext.Request;
            ViewEngineResult result = null;
            if (MobileDetect.IsMobile(controllerContext.HttpContext) && !MobileDetect.OverrideMobile(controllerContext.HttpContext, engine.MobileOverrideQueryStringParam))
            {
                /*
                 *  Device or capabilities specific mobile path. 
                 *  i.e. /Mobile/iPhone/ViewName or /Mobile/WebKit/ViewName
                */
                string mobileViewName = MobilePathHelper.Resolve(engine, request, viewName);
                result = engine.ResolveView(controllerContext, mobileViewName, masterName, useCache);
                
                if (result == null || result.View == null)
                {
                    /*  Fallback:
                     *  Non-Device/capabilities specific mobile path. 
                     *  i.e. /Mobile/ViewName
                    */
                    mobileViewName = string.Format(CultureInfo.InvariantCulture, "Mobile/{0}", viewName);
                    result = engine.ResolveView(controllerContext, mobileViewName, masterName, useCache);
                }
            }

            if (result == null || result.View == null)
            {
                result = engine.ResolveView(controllerContext, viewName, masterName, useCache);
            }

            return result;
        }
    }
}
