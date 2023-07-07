using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MyLearningStrategyMobleXForms.Services
{
    public class ImagesDataStore
    {
        public string GetImagePath()
        {
            return System.IO.Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory);

        }
    }
}
