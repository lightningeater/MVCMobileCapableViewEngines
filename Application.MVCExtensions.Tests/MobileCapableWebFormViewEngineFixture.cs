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

namespace Application.MVCExtensions.Tests
{
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Application.MVCExtensions.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MobileCapableWebFormViewEngineFixture
    {
        [TestMethod]
        public void FindViewReturnsSpecificMobileViewPath()
        {
            var supportedBrowserName = "Pocket IE";
            var testControllerContext = RetrieveTestControllerContext(true, supportedBrowserName);
            var viewName = "Index";
            var viewPathExpected = string.Format(CultureInfo.InvariantCulture, "Mobile/WindowsMobile/{0}", viewName);

            ExecuteTest(testControllerContext, viewName, viewPathExpected);
        }

        [TestMethod]
        public void FindViewReturnsSharedMobileViewPath()
        {
            var supportedBrowserName = "Pocket IE";
            var testControllerContext = RetrieveTestControllerContext(true, supportedBrowserName);
            var viewName = "SharedMobileView";
            var viewPathExpected = string.Format(CultureInfo.InvariantCulture, "Mobile/{0}", viewName);

            ExecuteTest(testControllerContext, viewName, viewPathExpected);
        }

        [TestMethod]
        public void FindViewReturnsDesktopViewPath()
        {
            var supportedBrowserName = "IE";
            var testControllerContext = RetrieveTestControllerContext(false, supportedBrowserName);
            var viewName = "Index";
            var viewPathExpected = viewName;

            ExecuteTest(testControllerContext, viewName, viewPathExpected);
        }

        [TestMethod]
        public void FindViewReturnsSharedMobileViewPathWhenSpecificMobileFolderNotExists()
        {
            var notSupportedBrowserName = "Not Supported Browser";
            var testControllerContext = RetrieveTestControllerContext(true, notSupportedBrowserName);
            var viewName = "Index";
            var viewPathExpected = string.Format(CultureInfo.InvariantCulture, "Mobile/{0}", viewName);

            ExecuteTest(testControllerContext, viewName, viewPathExpected);
        }

        [TestMethod]
        public void FindViewReturnsDesktopViewPathWhenSpecificMobileFolderAndSharedMobileFolderNotExists()
        {
            var notSupportedBrowserName = "Not Supported Browser";
            var testControllerContext = RetrieveTestControllerContext(true, notSupportedBrowserName);
            var viewName = "UndefinedView";
            var viewPathExpected = string.Format(CultureInfo.InvariantCulture, "{0}", viewName);

            ExecuteTest(testControllerContext, viewName, viewPathExpected);
        }

        private static void ExecuteTest(ControllerContext testControllerContext, string viewName, string viewPathExpected)
        {
            var testViewFormView = new WebFormView(testControllerContext, viewPathExpected);
            var fakeViewEngine = new Mock<IViewEngine>();
            var webFormViewEngine = new MockMobileCapableWebFormViewEngine(
                                            testViewFormView,
                                            fakeViewEngine.Object,
                                            viewPathExpected);

            var viewEngineResult = webFormViewEngine.FindView(testControllerContext, viewName, string.Empty, false);

            Assert.IsNotNull(viewEngineResult);
            Assert.IsNotNull(viewEngineResult.View);
            Assert.IsNotNull(viewEngineResult.ViewEngine);
            Assert.AreEqual<string>(viewPathExpected, ((WebFormView)viewEngineResult.View).ViewPath);
        }

        private static ControllerContext RetrieveTestControllerContext(bool isMobileDevice, string browser)
        {
            var fakeHttpContext = MvcMockHelpers.FakeHttpContext();

            var httpBrowserCapabilities = new Mock<HttpBrowserCapabilitiesBase>();
            httpBrowserCapabilities.Setup(p => p.IsMobileDevice)
                                   .Returns(isMobileDevice);
            httpBrowserCapabilities.Setup(p => p.Browser)
                                   .Returns(browser);
            MvcMockHelpers.SetHttpBrowserCapabilities(fakeHttpContext.Request, httpBrowserCapabilities.Object);

            var fakeController = new Mock<ControllerBase>();
            var testRouteData = new RouteData();
            testRouteData.Values.Add("controller", "TestController");

            return new ControllerContext(fakeHttpContext, testRouteData, fakeController.Object);
        }
    }
}