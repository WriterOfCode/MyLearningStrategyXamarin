using System;
using System.Collections.Generic;
using System.Text;
using MyLearningStrategyMobleXForms.Models;

namespace MyLearningStrategyMobleXForms.Models
{
    public class ImageMediaUrls: BaseDTO
    {
        public string ImageDevice { get; set; }
        public string ImageCloud { get; set; }
        public string Image
        {
            get
            {
               string newImage = ImageDevice.Length > 0 ? ImageDevice : ImageCloud.Length>0 ? ImageCloud : string.Empty;
                return newImage;
            }
        }
        public int ImageHash { get; set; }
    }
}
