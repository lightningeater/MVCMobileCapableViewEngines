using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections;

namespace Application.MVCExtensions.Mobile
{
    public static class MobileDetect
    {
        private static string StoreName = "MOBILEOVERRIDE";
        private static Regex MOBILE_REGEX
        {
            get
            {
                string exp = (ConfigurationManager.AppSettings.Get("MobileDevices") ?? "nomatch");
                return new Regex(exp, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this browser instance is a mobile device.
        /// </summary>
        /// <remarks>
        /// Checks the HttpRequest.Browser.IsMobileDevice first returns if true; 
        /// Otherwise, if false, an additional check is done against the 
        /// HttpRequest.Browser.UserAgent string using a Regular Expression from AppSettings.
        /// AppSetting key is MobileDevices.
        /// </remarks>
        /// <value><c>true</c> if this instance is mobile; otherwise, <c>false</c>.</value>
        public static bool IsMobile(HttpContextBase context)
        {
            if (context != null)
            {
                HttpRequestBase request = context.Request;

                if (request.Browser.IsMobileDevice)
                    return true;

                if (!string.IsNullOrEmpty(request.UserAgent) && MOBILE_REGEX.IsMatch(request.UserAgent))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Overrides the mobile views if either a cookie exists 
        /// or QueryString parameters contain the given <paramref name="param"/> name.
        /// </summary>
        /// <param name="param">The parameter name to look for.</param>
        /// <returns><c>true</c> when the QueryString contains the given 
        /// <paramref name="param"/> name; Otherwise, <c>false</c>.</returns>
        public static bool OverrideMobile(HttpContextBase context, string param)
        {
            if (context == null || context.Request == null || context.Request.Cookies == null)
            {
                return false;
            }
            else
            {
                if (ReadOverrideState(context)) { return true; }
                HttpRequestBase request = context.Request;
                if (request.QueryString.HasKeys())
                {
                    System.Collections.Generic.Dictionary<string, string> d = new System.Collections.Generic.Dictionary<string, string>();
                    foreach (string key in request.QueryString.Keys)
                    {
                        if (d.ContainsKey(key)) { d.Add(key, key); }
                    }
                    if (d.ContainsKey(param))
                    {
                        StoreOverrideState(context);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Reads the state of the current users override setting.
        /// </summary>
        /// <returns><c>true</c>, if the user has a cookie used to override mobile redirect; Otherwise, <c>false</c>.</returns>
        private static bool ReadOverrideState(HttpContextBase context)
        {
            if (context == null || context.Request == null || context.Request.Cookies == null) return false;
            HttpCookie cookie = context.Request.Cookies[StoreName];
            return (cookie != null);
        }

        /// <summary>
        /// Stores the state of the current users override setting.
        /// </summary>
        private static void StoreOverrideState(HttpContextBase context)
        {
            if (context == null || context.Request == null || context.Request.Cookies == null) return;

            HttpCookie cookie = context.Request.Cookies[StoreName];
            if (cookie == null)
            {
                cookie = new HttpCookie(StoreName);
            }

            cookie[StoreName] = "true";
            context.Response.Cookies.Add(cookie);
        }
    }
}