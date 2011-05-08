// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Application.MVCExtensions.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Application.MVCExtensions.Mobile;

    public class MockMobileCapableRazorViewEngine : MobileCapableRazorViewEngine
    {
        private IView view;
        private IViewEngine viewEngine;
        private string viewPathExpected;

        public MockMobileCapableRazorViewEngine(IView view, IViewEngine viewEngine, string viewPathExpected)
        {
            this.view = view;
            this.viewEngine = viewEngine;
            this.viewPathExpected = viewPathExpected;
        }

        public override ViewEngineResult ResolveView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (this.viewPathExpected.Equals(viewName, StringComparison.OrdinalIgnoreCase))
            {
                return new ViewEngineResult(this.view, this.viewEngine);
            }

            return new ViewEngineResult(new List<string>(0));
        }
    }
}