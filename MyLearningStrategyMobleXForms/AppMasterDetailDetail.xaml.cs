using Acr.UserDialogs;
using MyLearningStrategyMobleXForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyLearningStrategyMobleXForms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppMasterDetailDetail : ContentPage
    {
        UserProfileViewModel viewModel;
        public AppMasterDetailDetail()
        {
            InitializeComponent();
            BindingContext = viewModel = new UserProfileViewModel(UserDialogs.Instance);
            viewModel.ExternalId = "A321CBA7-8346-4E8F-A962-DA751D81B8F4";
            viewModel.LoadDataCommand.Execute(null);
        }
    }
}