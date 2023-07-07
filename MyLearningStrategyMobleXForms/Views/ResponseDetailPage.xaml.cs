using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyLearningStrategyMobleXForms.ViewModels;
using MyLearningStrategyMobleXForms.Models;
using Acr.UserDialogs;

namespace MyLearningStrategyMobleXForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResponseDetailPage : ContentPage
    {
        ResponcesDetailModel viewModel;
        //api/Responses/{QuestionId}/{ResponseId}
        public ResponseDetailPage(ResponsesDTO response)
        {
            InitializeComponent();

            if (response.CloudRowId != Guid.Empty)
            {
                ToolbarItem toolbarItem = new ToolbarItem
                {
                    Text = "Delete",
                    Order = ToolbarItemOrder.Secondary,
                    Priority = 1,
                    IconImageSource = "answerdelete40.png"
                };
                toolbarItem.Clicked += OnDelete;
                this.ToolbarItems.Add(toolbarItem);
            }
            

            Title = "Response";
            BindingContext = viewModel = new ResponcesDetailModel(response, UserDialogs.Instance);
        }

        async void OnSave(object sender, EventArgs e)
        {
            viewModel.SaveCommand.Execute(null);
            await Navigation.PopModalAsync(true);
        }
        async void OnCancel(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
        async void OnDelete(object sender, EventArgs e)
        {
            viewModel.DeleteCommand.Execute(null);
            await Navigation.PopModalAsync(true);
        }

    }
}