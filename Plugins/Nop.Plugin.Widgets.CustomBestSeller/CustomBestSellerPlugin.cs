using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.CustomBestSeller
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class CustomBestSellerPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public CustomBestSellerPlugin(IPictureService pictureService, 
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
            return new List<string> { "home_page_custom_best_seller" };
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
            controllerName = "WidgetsCustomBestSeller";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Widgets.CustomBestSeller.Controllers" }, { "area", null } };
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
            controllerName = "WidgetsCustomBestSeller";
            routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.Widgets.CustomBestSeller.Controllers"},
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


            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture1", "Picture 1");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture2", "Picture 2");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture3", "Picture 3");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture4", "Picture 4");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture5", "Picture 5");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture", "Picture");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture.Hint", "Upload picture.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Text", "Comment");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Text.Hint", "Enter comment for picture. Leave empty if you don't want to display any text.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Link", "URL");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Link.Hint", "Enter URL. Leave empty if you don't want this picture to be clickable.");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<CustomBestSellerSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture1");
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture2");
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture3");
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture4");
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture5");
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture");
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Picture.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Text");
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Text.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Link");
            this.DeletePluginLocaleResource("Plugins.Widgets.CustomBestSeller.Link.Hint");
            
            base.Uninstall();
        }
    }
}
