using Acr.UserDialogs;
using Akavache;
using MyLearningStrategyMobleXForms.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Xamarin.Forms.Xaml;

namespace MyLearningStrategyMobleXForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPageView : ContentPage
    {
        public SettingsPageView()
        {
            InitializeComponent();
            clearCache.Clicked += async (sender, args) =>
            {
                BlobCache.LocalMachine.InvalidateAll();
            };
        }

    }
}