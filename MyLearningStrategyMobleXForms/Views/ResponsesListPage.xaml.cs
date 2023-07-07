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
    public partial class ResponsesListPage : ContentPage
    {
        ResponcesListViewModel viewModel;

        public ResponsesListPage(int bodyOfKnowledgeID, int questionId)
        {
            InitializeComponent();
            Title = "Responses";
            BindingContext = viewModel = new ResponcesListViewModel(  bodyOfKnowledgeID, questionId, UserDialogs.Instance);
        }
        async void OnAdd(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Navigation.PushModalAsync(
                new NavigationPage(new ResponseDetailPage(
                new ResponsesDTO {  QuestionId = viewModel.QuestionID }))); 

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
        async void OnEditItem(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                MenuItem obj = sender as MenuItem;
                // Find the single item here
                ResponsesDTO selectedResponse = obj.BindingContext as ResponsesDTO;
                if (selectedResponse == null)
                    return;
                await Navigation.PushModalAsync(new NavigationPage(new ResponseDetailPage( selectedResponse))); ;
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
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.List.Count == 0)
                viewModel.LoadDataCommand.Execute(null);
            MessagingCenter.Subscribe<ResponcesDetailModel>(this, MessagingConstants.ResponseAdded, (arg) =>
            {
                viewModel.LoadDataCommand.Execute(null);
            });
            MessagingCenter.Subscribe<ResponcesDetailModel>(this, MessagingConstants.ResponseDeleted, (arg) =>
            {
                viewModel.LoadDataCommand.Execute(null);
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<ResponcesDetailModel>(this, MessagingConstants.ResponseAdded);
            MessagingCenter.Unsubscribe<ResponcesDetailModel>(this, MessagingConstants.ResponseDeleted);
        }
    }
}
