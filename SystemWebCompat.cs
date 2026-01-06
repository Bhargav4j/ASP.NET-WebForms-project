using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

// Compatibility shims for System.Web types for .NET 8
namespace System.Web
{
    public class HttpException : Exception
    {
        public HttpException() { }
        public HttpException(string message) : base(message) { }
        public HttpException(int httpCode, string message) : base(message) { }
    }

    public class HttpContext
    {
        public static HttpContext Current { get; set; }
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public HttpServerUtility Server { get; set; }
        public System.Security.Principal.IPrincipal User { get; set; }
    }

    public class HttpRequest
    {
        public string QueryString { get; set; }
        public string UserHostAddress { get; set; }
        public string RawUrl { get; set; }
        public bool IsSecureConnection { get; set; }
        public HttpCookieCollection Cookies { get; set; } = new HttpCookieCollection();
    }

    public class HttpResponse
    {
        public void Redirect(string url) { }
        public HttpCookieCollection Cookies { get; set; } = new HttpCookieCollection();
    }

    public class HttpCookie
    {
        public HttpCookie() { }
        public HttpCookie(string name) { Name = name; }
        public HttpCookie(string name, string value) { Name = name; Value = value; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public DateTime Expires { get; set; }
    }

    public class HttpCookieCollection : System.Collections.Generic.List<HttpCookie>
    {
        public HttpCookie this[string name]
        {
            get => this.FirstOrDefault(c => c.Name == name);
        }

        public void Add(HttpCookie cookie) => base.Add(cookie);
        public void Set(HttpCookie cookie)
        {
            var existing = this.FirstOrDefault(c => c.Name == cookie.Name);
            if (existing != null) base.Remove(existing);
            base.Add(cookie);
        }
        public void Remove(string name)
        {
            var cookie = this.FirstOrDefault(c => c.Name == name);
            if (cookie != null) base.Remove(cookie);
        }
    }

    public class HttpServerUtility
    {
        public string MapPath(string path) => path;
    }

    public class HttpApplication
    {
        protected virtual void Application_Start() { }
        protected virtual void Application_End() { }
        protected virtual void Application_Error() { }
    }

    public class HttpContextWrapper : HttpContextBase
    {
        public HttpContextWrapper(HttpContext httpContext) { }
    }

    public abstract class HttpContextBase
    {
        public virtual HttpRequestBase Request { get; set; }
        public virtual HttpResponseBase Response { get; set; }
    }

    public abstract class HttpRequestBase
    {
        public virtual string RawUrl { get; set; }
    }

    public abstract class HttpResponseBase
    {
    }

    public static class HttpUtility
    {
        public static string UrlEncode(string str) => System.Net.WebUtility.UrlEncode(str);
        public static string UrlDecode(string str) => System.Net.WebUtility.UrlDecode(str);
        public static string HtmlEncode(string str) => System.Net.WebUtility.HtmlEncode(str);
        public static string HtmlDecode(string str) => System.Net.WebUtility.HtmlDecode(str);
    }
}

namespace System.Web.UI
{
    public class Page
    {
        public bool IsPostBack { get; set; }
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public HttpContext Context { get; set; }
        public string ViewStateUserKey { get; set; }
        public StateBag ViewState { get; set; } = new StateBag();
        public event EventHandler PreLoad;
        protected virtual void Page_Load(object sender, EventArgs e) { }
        protected string GetRouteUrl(string routeName, object routeValues) => "";
    }

    public class StateBag : System.Collections.Generic.Dictionary<string, object>
    {
    }

    public class UserControl
    {
        public Page Page { get; set; }
        public HttpContext Context { get; set; }
        public HttpRequest Request { get; set; }
        public bool Visible { get; set; }
        protected string GetRouteUrl(string routeName, object routeValues) => "";
    }

    public class MasterPage
    {
        public Page Page { get; set; }
        public HttpContext Context { get; set; }
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public bool IsPostBack { get; set; }
        public StateBag ViewState { get; set; } = new StateBag();
    }

    public class Control
    {
        public string ID { get; set; }
        public bool Visible { get; set; }
    }

    public class HtmlControl : Control
    {
    }
}

namespace System.Web.UI.WebControls
{
    public class WebControl : System.Web.UI.Control
    {
        public string Text { get; set; }
        public bool Enabled { get; set; }
    }

    public class TextBox : WebControl
    {
    }

    public class Button : WebControl
    {
        public event EventHandler Click;
    }

    public class Label : WebControl
    {
    }

    public class Literal : WebControl
    {
        public string Mode { get; set; }
    }

    public class HyperLink : WebControl
    {
        public string NavigateUrl { get; set; }
    }

    public class CheckBox : WebControl
    {
        public bool Checked { get; set; }
    }

    public class Panel : WebControl
    {
    }

    public class PlaceHolder : WebControl
    {
    }

    public class ContentPlaceHolder : WebControl
    {
    }

    public class Image : WebControl
    {
        public string ImageUrl { get; set; }
        public string AlternateText { get; set; }
    }

    public class RequiredFieldValidator : WebControl
    {
        public string ControlToValidate { get; set; }
        public string ErrorMessage { get; set; }
        public string Display { get; set; }
    }

    public class DropDownList : WebControl
    {
        public string SelectedValue { get; set; }
    }

    public class ListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class RegularExpressionValidator : WebControl
    {
        public string ControlToValidate { get; set; }
        public string ErrorMessage { get; set; }
        public string ValidationExpression { get; set; }
        public string Display { get; set; }
    }

    public class Calendar : WebControl
    {
        public DateTime SelectedDate { get; set; }
        public DateTime VisibleDate { get; set; }
    }
}

namespace System.Web.UI.HtmlControls
{
    public class HtmlControl : System.Web.UI.Control
    {
    }

    public class HtmlGenericControl : HtmlControl
    {
        public HtmlGenericControl() { }
        public HtmlGenericControl(string tag) { }
    }

    public class HtmlForm : HtmlControl
    {
    }

    public class HtmlInputText : HtmlControl
    {
        public string Value { get; set; }
    }

    public class HtmlInputButton : HtmlControl
    {
        public string Value { get; set; }
    }

    public class HtmlAnchor : HtmlControl
    {
        public string HRef { get; set; }
    }
}

namespace System.Web.Optimization
{
    public class BundleCollection
    {
        public void Add(Bundle bundle) { }
    }

    public class Bundle
    {
        public Bundle(string virtualPath) { }
        public Bundle(string virtualPath, IBundleTransform transform) { }
    }

    public class ScriptBundle : Bundle
    {
        public ScriptBundle(string virtualPath) : base(virtualPath) { }
    }

    public class StyleBundle : Bundle
    {
        public StyleBundle(string virtualPath) : base(virtualPath) { }
    }

    public interface IBundleTransform { }

    public static class BundleTable
    {
        public static BundleCollection Bundles { get; } = new BundleCollection();
    }
}

namespace System.Web.Routing
{
    public class Route
    {
        public string Url { get; set; }
    }

    public class RouteCollection
    {
        private System.Collections.Generic.Dictionary<string, Route> _routes = new System.Collections.Generic.Dictionary<string, Route>();

        public Route this[string name]
        {
            get => _routes.ContainsKey(name) ? _routes[name] : null;
            set => _routes[name] = value;
        }

        public void MapPageRoute(string routeName, string routeUrl, string physicalFile) { }
        public void MapPageRoute(string routeName, string routeUrl, string physicalFile, bool checkPhysicalUrlAccess) { }
        public void MapPageRoute(string routeName, string routeUrl, string physicalFile, bool checkPhysicalUrlAccess, RouteValueDictionary defaults) { }
    }

    public class RouteValueDictionary : System.Collections.Generic.Dictionary<string, object>
    {
        public RouteValueDictionary() { }
        public RouteValueDictionary(object values) { }
    }

    public static class RouteTable
    {
        public static RouteCollection Routes { get; } = new RouteCollection();
    }
}

namespace System.Web.Security
{
    public static class AntiXssEncoder
    {
        public static string HtmlEncode(string value, bool useNamedEntities) => value;
    }

    public class FormsAuthentication
    {
        public static bool RequireSSL { get; set; }
        public static void SignOut() { }
        public static void SetAuthCookie(string userName, bool createPersistentCookie) { }
        public static void RedirectFromLoginPage(string userName, bool createPersistentCookie) { }
    }

    public class FormsAuthenticationTicket
    {
        public FormsAuthenticationTicket(int version, string name, DateTime issueDate, DateTime expiration, bool isPersistent, string userData) { }
        public string Name { get; set; }
    }

    public class Membership
    {
        public static MembershipUser CreateUser(string username, string password) => null;
        public static MembershipUser CreateUser(string username, string password, string email) => null;
        public static bool ValidateUser(string username, string password) => false;
    }

    public class MembershipUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}

namespace Microsoft.AspNet.FriendlyUrls
{
    public static class FriendlyUrlExtensions
    {
        public static void EnableFriendlyUrls(this System.Web.Routing.RouteCollection routes) { }
    }
}

namespace Microsoft.AspNet.FriendlyUrls.Resolvers
{
    public static class WebFormsFriendlyUrlResolver
    {
        public static bool IsMobileView(System.Web.HttpContextBase context) => false;
    }
}
