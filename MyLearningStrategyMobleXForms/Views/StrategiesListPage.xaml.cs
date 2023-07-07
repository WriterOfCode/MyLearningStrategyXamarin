using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyLearningStrategyMobleXForms.ViewModels;
using MyLearningStrategyMobleXForms.Models;
using System.Diagnostics;
using Acr.UserDialogs;

namespace MyLearningStrategyMobleXForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StrategiesListPage : ContentPage
    {
        StrategiesListViewModel viewModel;
        public StrategiesListPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new StrategiesListViewModel(App.CurrenUserProfile.Originator, UserDialogs.Instance);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.List.Count == 0)
                viewModel.LoadDataCommand.Execute(null);
        }
        async void OnAdd(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Navigation.PushModalAsync(new NavigationPage(new StrategyDetailPage(App.CurrenUserProfile.Originator)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void OnEdit(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                MenuItem obj = sender as MenuItem;
                // Find the single item here
                StrategyDTO dto = obj.BindingContext as StrategyDTO;
                if (dto == null)
                    return;

                await Navigation.PushModalAsync(new NavigationPage(new StrategyDetailPage( dto))); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }


        }

    }
}
