using System;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.ViewModels;
using Acr.UserDialogs;

namespace MyLearningStrategyMobleXForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionDetailPage : ContentPage
    {
        public QuestionsDetailModel ViewModel { get; set; }
        public QuestionDetailPage(QuestionsDTO question)
        {
            InitializeComponent();

            if (question.CloudRowId != Guid.Empty)
            {
                ToolbarItem toolbarItem = new ToolbarItem
                {
                    Text = "Delete",
                    Order = ToolbarItemOrder.Secondary,
                    Priority = 1
                };
                toolbarItem.Clicked += OnDelete;
                this.ToolbarItems.Add(toolbarItem);
            }

            Title = "Question";
            BindingContext = ViewModel = new QuestionsDetailModel( question, UserDialogs.Instance);
        }

        async void OnSave(object sender, EventArgs e)
        {
            ViewModel.SaveCommand.Execute(null);
            await Navigation.PopModalAsync(true);
        }
        async void OnDelete(object sender, EventArgs e)
        {
            ViewModel.DeleteCommand.Execute(null);
            await Navigation.PopModalAsync(true);
        }
        async void OnCancel(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        async void PickPhoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full
            });


            if (file == null)
                return;
            ViewModel.Item.Image = file.Path;
        }
    }
}