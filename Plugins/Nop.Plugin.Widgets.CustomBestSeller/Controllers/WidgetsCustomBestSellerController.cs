using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Plugin.Widgets.CustomBestSeller.Infrastructure.Cache;
using Nop.Plugin.Widgets.CustomBestSeller.Models;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.CustomBestSeller.Controllers
{
    public class WidgetsCustomBestSellerController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private readonly CatalogSettings _catalogSettings;
        private readonly IProductService _productService;
        private readonly IOrderReportService _orderReportService;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IWebHelper _webHelper;
        private readonly MediaSettings _mediaSettings;

        public WidgetsCustomBestSellerController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService, 
            IPictureService pictureService,
            ISettingService settingService,
            ICacheManager cacheManager,
            ILocalizationService localizationService, CatalogSettings catalogSettings, IProductService productService, IOrderReportService orderReportService, IAclService aclService, IStoreMappingService storeMappingService, IWebHelper webHelper, MediaSettings mediaSettings)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._localizationService = localizationService;
            _catalogSettings = catalogSettings;
            _productService = productService;
            _orderReportService = orderReportService;
            _aclService = aclService;
            _storeMappingService = storeMappingService;
            _webHelper = webHelper;
            _mediaSettings = mediaSettings;
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
            var cameraSettings = _settingService.LoadSetting<CustomBestSellerSettings>(storeScope);
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

            return View("~/Plugins/Widgets.CustomBestSeller/Views/WidgetsCustomBestSeller/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var cameraSettings = _settingService.LoadSetting<CustomBestSellerSettings>(storeScope);
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
        [NonAction]
        protected virtual IEnumerable<ProductOverviewModel> PrepareProductOverviewModels(IEnumerable<Product> products,
            int? productThumbPictureSize = null)
        {
            return this.PrepareProductOverviewModels(_workContext,
                _storeContext, 
                _productService,
                _localizationService, 
                _pictureService, _webHelper, _cacheManager,
                _catalogSettings, _mediaSettings, products,
                productThumbPictureSize);
        }


        public IEnumerable<ProductOverviewModel> PrepareProductOverviewModels(
            IWorkContext workContext,
            IStoreContext storeContext,
            IProductService productService,
            ILocalizationService localizationService,
            IPictureService pictureService,
            IWebHelper webHelper,
            ICacheManager cacheManager,
            CatalogSettings catalogSettings,
            MediaSettings mediaSettings,
            IEnumerable<Product> products,
            int? productThumbPictureSize = null)
        {
            if (products == null)
                throw new ArgumentNullException("products");

            var models = new List<ProductOverviewModel>();
            foreach (var product in products)
            {
                var model = new ProductOverviewModel
                {
                    Id = product.Id,
                    Name = product.GetLocalized(x => x.Name),
                    ShortDescription = product.GetLocalized(x => x.ShortDescription),
                    FullDescription = product.GetLocalized(x => x.FullDescription),
                    SeName = product.GetSeName(),
                };
           
                //picture
                #region Prepare product picture

                //If a size has been set in the view, we use it in priority
                int pictureSize = productThumbPictureSize.HasValue ? productThumbPictureSize.Value : mediaSettings.ProductThumbPictureSize;
                //prepare picture model
                var defaultProductPictureCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_DEFAULTPICTURE_MODEL_KEY, product.Id, pictureSize, true, workContext.WorkingLanguage.Id, webHelper.IsCurrentConnectionSecured(), storeContext.CurrentStore.Id);
                model.DefaultPictureModel = cacheManager.Get(defaultProductPictureCacheKey, () =>
                {
                    var picture = pictureService.GetPicturesByProductId(product.Id, 1).FirstOrDefault();
                    var pictureModel = new PictureModel
                    {
                        ImageUrl = pictureService.GetPictureUrl(picture, pictureSize),
                        FullSizeImageUrl = pictureService.GetPictureUrl(picture)
                    };
                    //"title" attribute
                    pictureModel.Title = (picture != null && !string.IsNullOrEmpty(picture.TitleAttribute)) ?
                        picture.TitleAttribute :
                        string.Format(localizationService.GetResource("Media.Product.ImageLinkTitleFormat"), model.Name);
                    //"alt" attribute
                    pictureModel.AlternateText = (picture != null && !string.IsNullOrEmpty(picture.AltAttribute)) ?
                        picture.AltAttribute :
                        string.Format(localizationService.GetResource("Media.Product.ImageAlternateTextFormat"), model.Name);

                    return pictureModel;
                });

                #endregion

          
                models.Add(model);
            }
            return models;
        }
        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone, object additionalData = null)
        {
            if (!_catalogSettings.ShowBestsellersOnHomepage || _catalogSettings.NumberOfBestsellersOnHomepage == 0)
                return Content("");

            //load and cache report
            var report = _cacheManager.Get(string.Format(ModelCacheEventConsumer.HOMEPAGE_BESTSELLERS_IDS_KEY, _storeContext.CurrentStore.Id),
                () =>
                    _orderReportService.BestSellersReport(storeId: _storeContext.CurrentStore.Id,
                    pageSize: _catalogSettings.NumberOfBestsellersOnHomepage));


            //load products
            var products = _productService.GetProductsByIds(report.Select(x => x.ProductId).ToArray());
            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();
            //availability dates
            products = products.Where(p => p.IsAvailable()).ToList();

            if (products.Count == 0)
                products = _productService.SearchProducts(
                storeId: _storeContext.CurrentStore.Id,
                visibleIndividuallyOnly: true,
                orderBy: ProductSortingEnum.CreatedOn,
                pageSize: _catalogSettings.NumberOfBestsellersOnHomepage);
              

            //prepare model
            var model = PrepareProductOverviewModels(products,  300).ToList();
            return View("~/Plugins/Widgets.CustomBestSeller/Views/WidgetsCustomBestSeller/PublicInfo.cshtml", model);
        }
    }
}