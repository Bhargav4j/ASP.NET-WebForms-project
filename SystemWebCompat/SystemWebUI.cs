// Comprehensive compatibility stubs for System.Web.UI namespace
namespace System.Web.UI
{
    public class Page
    {
        public static string ViewStateUserKey { get; set; }
        public static event EventHandler PreLoad;
        public bool IsPostBack { get; set; }
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public HttpContext Context { get; set; }
        public string GetRouteUrl(object routeParameters) => "";
    }

    public class UserControl
    {
        public bool Visible { get; set; }
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public HttpContext Context { get; set; }
        public bool IsPostBack { get; set; }
        public string GetRouteUrl(object routeParameters) => "";
        public string GetRouteUrl(string routeName, object routeValues) => "";
    }

    public class MasterPage
    {
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public HttpContext Context { get; set; }
        public bool IsPostBack { get; set; }
        public System.Web.UI.StateBag ViewState { get; set; }
    }

    public class StateBag
    {
        public object this[string key]
        {
            get => null;
            set { }
        }
    }

    public class Control
    {
        public bool Visible { get; set; }
    }

    public class LiteralControl : Control { }
    public class HtmlTextWriter { }
    public interface IPostBackEventHandler { }
    public interface ICallbackEventHandler { }

    namespace HtmlControls
    {
        public class HtmlControl
        {
            public bool Visible { get; set; }
        }
        public class HtmlForm : HtmlControl { }
        public class HtmlGenericControl : HtmlControl { }
        public class HtmlInputControl : HtmlControl { }
        public class HtmlInputText : HtmlInputControl
        {
            public string Value { get; set; }
        }
        public class HtmlInputButton : HtmlInputControl { }
        public class HtmlInputCheckBox : HtmlInputControl
        {
            public bool Checked { get; set; }
        }
        public class HtmlInputRadioButton : HtmlInputControl
        {
            public bool Checked { get; set; }
        }
        public class HtmlInputHidden : HtmlInputControl
        {
            public string Value { get; set; }
        }
        public class HtmlInputFile : HtmlInputControl { }
        public class HtmlInputImage : HtmlInputControl { }
        public class HtmlTextArea : HtmlControl
        {
            public string Value { get; set; }
        }
        public class HtmlSelect : HtmlControl { }
        public class HtmlTable : HtmlControl { }
        public class HtmlTableRow : HtmlControl { }
        public class HtmlTableCell : HtmlControl { }
        public class HtmlImage : HtmlControl { }
        public class HtmlAnchor : HtmlControl { }
        public class HtmlButton : HtmlControl { }
    }

    namespace WebControls
    {
        public class WebControl : Control
        {
            public bool Visible { get; set; }
        }
        public class Button : WebControl { }
        public class TextBox : WebControl
        {
            public string Text { get; set; }
            public new bool Visible { get; set; }
        }
        public class Label : WebControl
        {
            public string Text { get; set; }
            public new bool Visible { get; set; }
        }
        public class HyperLink : WebControl { }
        public class LinkButton : WebControl { }
        public class ImageButton : WebControl { }
        public class CheckBox : WebControl
        {
            public bool Checked { get; set; }
            public new bool Visible { get; set; }
        }
        public class RadioButton : WebControl
        {
            public bool Checked { get; set; }
        }
        public class DropDownList : WebControl { }
        public class ListBox : WebControl { }
        public class GridView : WebControl { }
        public class DetailsView : WebControl { }
        public class FormView : WebControl { }
        public class Repeater : WebControl { }
        public class DataList : WebControl { }
        public class ListView : WebControl { }
        public class SqlDataSource : WebControl { }
        public class ObjectDataSource : WebControl { }
        public class EntityDataSource : WebControl { }
        public class Literal : WebControl { }
        public class PlaceHolder : WebControl { }
        public class Panel : WebControl
        {
            public new bool Visible { get; set; }
        }
        public class Table : WebControl { }
        public class TableRow : WebControl { }
        public class TableCell : WebControl { }
        public class Image : WebControl { }
        public class HiddenField : WebControl { }
        public class ValidationSummary : WebControl { }
        public class RequiredFieldValidator : WebControl { }
        public class CompareValidator : WebControl { }
        public class RangeValidator : WebControl { }
        public class RegularExpressionValidator : WebControl { }
        public class CustomValidator : WebControl { }
        public class LoginView : WebControl { }
        public class LoginStatus : WebControl { }
        public class LoginName : WebControl { }
        public class CreateUserWizard : WebControl { }
        public class PasswordRecovery : WebControl { }
        public class ChangePassword : WebControl { }
        public class Menu : WebControl { }
        public class TreeView : WebControl { }
        public class SiteMapPath : WebControl { }
        public class ContentPlaceHolder : WebControl { }
        public class ScriptManager : WebControl { }
        public class UpdatePanel : WebControl { }
        public class UpdateProgress : WebControl { }
        public class Timer : WebControl { }
        public class Calendar : WebControl { }
    }
}

namespace System.Web
{
    public class HttpApplication
    {
        public HttpApplication Application { get; set; }
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public HttpServerUtility Server { get; set; }
        public HttpSessionState Session { get; set; }
        protected virtual void Application_Start() { }
    }

    public class HttpContext
    {
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public HttpServerUtility Server { get; set; }
        public HttpSessionState Session { get; set; }
        public System.Security.Claims.ClaimsPrincipal User { get; set; }
    }

    public class HttpContextWrapper
    {
        public HttpContextWrapper(HttpContext context) { }
    }

    public class HttpRequest
    {
        public bool IsSecureConnection { get; set; }
        public Uri Url { get; set; }
        public string RawUrl { get; set; }
        public HttpCookieCollection Cookies { get; set; }
    }

    public class HttpResponse
    {
        public HttpCookieCollection Cookies { get; set; }
        public void Redirect(string url) { }
    }

    public class HttpServerUtility { }

    public class HttpSessionState { }

    public class HttpApplicationState { }

    public class HttpCookie
    {
        public HttpCookie(string name) { }
        public bool HttpOnly { get; set; }
        public string Value { get; set; }
        public bool Secure { get; set; }
    }

    public class HttpCookieCollection
    {
        public void Add(HttpCookie cookie) { }
        public void Set(HttpCookie cookie) { }
        public HttpCookie this[string name] => null;
    }

    public class HttpPostedFile { }

    public class HttpBrowserCapabilities { }

    namespace Security
    {
        public class FormsAuthentication
        {
            public static bool RequireSSL { get; set; }
        }
        public class FormsAuthenticationTicket { }
        public class MembershipProvider { }
        public class RoleProvider { }
    }

    namespace Optimization
    {
        public class ScriptBundle
        {
            public ScriptBundle(string virtualPath) { }
        }
        public class StyleBundle
        {
            public StyleBundle(string virtualPath) { }
        }
        public class BundleTable
        {
            public static BundleCollection Bundles { get; set; }
        }
        public class BundleCollection { }
    }

    namespace Routing
    {
        public class RouteCollection
        {
            public bool RouteExistingFiles { get; set; }
            public void MapPageRoute(string routeName, string routeUrl, string physicalFile) { }
            public Route this[string name] => null;
        }
        public class RouteTable
        {
            public static RouteCollection Routes { get; set; }
        }
        public class Route { }
        public class PageRouteHandler { }
    }
}

namespace Microsoft.AspNet.FriendlyUrls
{
    public class FriendlyUrlSettings
    {
        public bool AutoRedirectMode { get; set; }
    }

    namespace Resolvers
    {
        public class MobileViewSelectorProvider { }
    }
}

namespace DotNetOpenAuth.AspNet
{
    public class OpenAuthSecurityManager
    {
        public static void RegisterProviders() { }
    }
}

namespace Microsoft.AspNet.Membership.OpenAuth
{
    public class OpenAuthAccountData { }
}

// Stub classes for missing types
public class WebFormsFriendlyUrlResolver
{
    public WebFormsFriendlyUrlResolver(System.Web.HttpContextWrapper context) { }
    public static bool IsMobileView(System.Web.HttpContextWrapper context) => false;
}

public class BundleConfig
{
    public static void RegisterBundles(System.Web.Optimization.BundleCollection bundles) { }
}

public class AuthConfig
{
    public static void RegisterOpenAuth() { }
}

public class RouteConfig
{
    public static void RegisterRoutes(System.Web.Routing.RouteCollection routes) { }
}
