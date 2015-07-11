using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.Camera
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class CameraPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public CameraPlugin(IPictureService pictureService, 
            ISettingService settingService, IWebHelper webHelper)
        {
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._webHelper = webHelper;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string> { "home_page_top" };
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "WidgetsCamera";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Widgets.Camera.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsCamera";
            routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.Widgets.Camera.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }
        
        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //pictures
            //var sampleImagesPath = _webHelper.MapPath("~/Plugins/Widgets.Camera/Content/nivoslider/sample-images/");


            ////settings
            //var settings = new CameraSettings
            //{
            //    Picture1Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner1.jpg"), "image/pjpeg", "banner_1").Id,
            //    Text1 = "",
            //    Link1 = _webHelper.GetStoreLocation(false),
            //    Picture2Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner2.jpg"), "image/pjpeg", "banner_2").Id,
            //    Text2 = "",
            //    Link2 = _webHelper.GetStoreLocation(false),
            //    //Picture3Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner3.jpg"), "image/pjpeg", "banner_3").Id,
            //    //Text3 = "",
            //    //Link3 = _webHelper.GetStoreLocation(false),
            //};
            //_settingService.SaveSetting(settings);


            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Picture1", "Picture 1");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Picture2", "Picture 2");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Picture3", "Picture 3");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Picture4", "Picture 4");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Picture5", "Picture 5");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Picture", "Picture");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Picture.Hint", "Upload picture.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Text", "Comment");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Text.Hint", "Enter comment for picture. Leave empty if you don't want to display any text.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Link", "URL");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Camera.Link.Hint", "Enter URL. Leave empty if you don't want this picture to be clickable.");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<CameraSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Picture1");
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Picture2");
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Picture3");
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Picture4");
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Picture5");
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Picture");
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Picture.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Text");
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Text.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Link");
            this.DeletePluginLocaleResource("Plugins.Widgets.Camera.Link.Hint");
            
            base.Uninstall();
        }
    }
}
