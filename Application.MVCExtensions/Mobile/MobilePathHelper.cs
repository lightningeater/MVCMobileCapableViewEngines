using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Globalization;

namespace Application.MVCExtensions.Mobile
{
    public static class MobilePathHelper
    {
        public static string Resolve(System.Web.HttpRequestBase request, string viewName)
        {
            string mobileViewName = viewName;

            // "layout engine" does not always exist so we need to check a reliable source if it's null (PreferredRenderingType)
            switch ((request.Browser.Capabilities["layoutEngine"] ?? request.Browser.PreferredRenderingType).ToString())
            {
                case "html4": //preferredRenderingType.
                case "WebKit": //layoutEngine.
                    mobileViewName = "Mobile/WebKit/" + viewName;
                    break;
                default: //everyone else gets wap
                    mobileViewName = "Mobile/WAP/" + viewName;
                    break;
            }

            return mobileViewName;
        }

        public static string Resolve(IMobileViewEngine engine, System.Web.HttpRequestBase request, string viewName)
        {
            if (engine == null || engine.DeviceFolders == null || engine.DeviceFolders.Count.Equals(0))
            {
                return Resolve(request, viewName);
            }

            string mobileViewName = string.Empty;

            mobileViewName = string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Mobile/{0}/{1}",
                                    RetrieveDeviceFolderName(request.Browser.Browser, engine.DeviceFolders),
                                    viewName);
            return mobileViewName;
        }

        /// <summary>
        /// Get the device folder associated with the name of the browser.
        /// </summary>
        /// <param name="browser">Name of the browser.</param>
        /// <returns>The associated folder name.</returns>
        private static string RetrieveDeviceFolderName(string browser, StringDictionary deviceFolders)
        {
            if (deviceFolders.ContainsKey(browser))
            {
                return deviceFolders[browser.Trim()];
            }
            else
            {
                return "unknown";
            }
        }

    }
}
