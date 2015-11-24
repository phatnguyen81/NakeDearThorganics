using System;
using System.Collections.Generic;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.CustomBestSeller.Models
{
    public partial class ProductOverviewModel : BaseNopEntityModel
    {
        public ProductOverviewModel()
        {
            DefaultPictureModel = new PictureModel();
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string SeName { get; set; }

        //picture
        public PictureModel DefaultPictureModel { get; set; }

    }
}