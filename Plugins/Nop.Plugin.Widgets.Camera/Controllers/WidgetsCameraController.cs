using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.Camera.Infrastructure.Cache;
using Nop.Plugin.Widgets.Camera.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.Camera.Controllers
{
    public class WidgetsCameraController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;

        public WidgetsCameraController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService, 
            IPictureService pictureService,
            ISettingService settingService,
            ICacheManager cacheManager,
            ILocalizationService localizationService)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._localizationService = localizationService;
        }

        protected string GetPictureUrl(int pictureId)
        {
            string cacheKey = string.Format(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, pictureId);
            return _cacheManager.Get(cacheKey, () =>
            {
                var url = _pictureService.GetPictureUrl(pictureId, showDefaultPicture: false);
                //little hack here. nulls aren't cacheable so set it to ""
                if (url == null)
                    url = "";

                return url;
            });
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var cameraSettings = _settingService.LoadSetting<CameraSettings>(storeScope);
            var model = new ConfigurationModel();
            model.Picture1Id = cameraSettings.Picture1Id;
            model.Text1 = cameraSettings.Text1;
            model.Link1 = cameraSettings.Link1;
            model.Picture2Id = cameraSettings.Picture2Id;
            model.Text2 = cameraSettings.Text2;
            model.Link2 = cameraSettings.Link2;
            model.Picture3Id = cameraSettings.Picture3Id;
            model.Text3 = cameraSettings.Text3;
            model.Link3 = cameraSettings.Link3;
            model.Picture4Id = cameraSettings.Picture4Id;
            model.Text4 = cameraSettings.Text4;
            model.Link4 = cameraSettings.Link4;
            model.Picture5Id = cameraSettings.Picture5Id;
            model.Text5 = cameraSettings.Text5;
            model.Link5 = cameraSettings.Link5;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Link2, storeScope);
                model.Picture3Id_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Link3, storeScope);
                model.Picture4Id_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Picture4Id, storeScope);
                model.Text4_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Text4, storeScope);
                model.Link4_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Link4, storeScope);
                model.Picture5Id_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Picture5Id, storeScope);
                model.Text5_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Text5, storeScope);
                model.Link5_OverrideForStore = _settingService.SettingExists(cameraSettings, x => x.Link5, storeScope);
            }

            return View("~/Plugins/Widgets.Camera/Views/WidgetsCamera/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var cameraSettings = _settingService.LoadSetting<CameraSettings>(storeScope);
            cameraSettings.Picture1Id = model.Picture1Id;
            cameraSettings.Text1 = model.Text1;
            cameraSettings.Link1 = model.Link1;
            cameraSettings.Picture2Id = model.Picture2Id;
            cameraSettings.Text2 = model.Text2;
            cameraSettings.Link2 = model.Link2;
            cameraSettings.Picture3Id = model.Picture3Id;
            cameraSettings.Text3 = model.Text3;
            cameraSettings.Link3 = model.Link3;
            cameraSettings.Picture4Id = model.Picture4Id;
            cameraSettings.Text4 = model.Text4;
            cameraSettings.Link4 = model.Link4;
            cameraSettings.Picture5Id = model.Picture5Id;
            cameraSettings.Text5 = model.Text5;
            cameraSettings.Link5 = model.Link5;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Picture1Id, storeScope);
            
            if (model.Text1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Text1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Text1, storeScope);
            
            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Link1, storeScope);
            
            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Picture2Id, storeScope);
            
            if (model.Text2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Text2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Text2, storeScope);
            
            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Link2, storeScope);
            
            if (model.Picture3Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Picture3Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Picture3Id, storeScope);
            
            if (model.Text3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Text3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Text3, storeScope);
            
            if (model.Link3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Link3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Link3, storeScope);
            
            if (model.Picture4Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Picture4Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Picture4Id, storeScope);
            
            if (model.Text4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Text4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Text4, storeScope);

            if (model.Link4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Link4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Link4, storeScope);

            if (model.Picture5Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Picture5Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Picture5Id, storeScope);

            if (model.Text5_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Text5, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Text5, storeScope);

            if (model.Link5_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(cameraSettings, x => x.Link5, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(cameraSettings, x => x.Link5, storeScope);
            
            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone, object additionalData = null)
        {
            var cameraSettings = _settingService.LoadSetting<CameraSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel();
            model.Picture1Url = GetPictureUrl(cameraSettings.Picture1Id);
            model.Text1 = cameraSettings.Text1;
            model.Link1 = cameraSettings.Link1;

            model.Picture2Url = GetPictureUrl(cameraSettings.Picture2Id);
            model.Text2 = cameraSettings.Text2;
            model.Link2 = cameraSettings.Link2;

            model.Picture3Url = GetPictureUrl(cameraSettings.Picture3Id);
            model.Text3 = cameraSettings.Text3;
            model.Link3 = cameraSettings.Link3;

            model.Picture4Url = GetPictureUrl(cameraSettings.Picture4Id);
            model.Text4 = cameraSettings.Text4;
            model.Link4 = cameraSettings.Link4;

            model.Picture5Url = GetPictureUrl(cameraSettings.Picture5Id);
            model.Text5 = cameraSettings.Text5;
            model.Link5 = cameraSettings.Link5;

            if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
                string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
                string.IsNullOrEmpty(model.Picture5Url))
                //no pictures uploaded
                return Content("");


            return View("~/Plugins/Widgets.Camera/Views/WidgetsCamera/PublicInfo.cshtml", model);
        }
    }
}