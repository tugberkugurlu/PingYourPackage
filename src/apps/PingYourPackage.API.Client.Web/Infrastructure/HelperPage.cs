﻿using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace PingYourPackage.API.Client.Web.Infrastructure {

    //http://stackoverflow.com/questions/4710853/using-mvc-htmlhelper-extensions-from-razor-declarative-views
    public class HelperPage : System.Web.WebPages.HelperPage {

        // Workaround - exposes the MVC HtmlHelper instead of the normal helper
        public static new HtmlHelper Html {

            get { return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Html; }
        }

        public static UrlHelper Url {

            get { return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Url; }
        }

        public static RouteData RouteData {

            get { return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Url.RequestContext.RouteData; }
        }
    }
}