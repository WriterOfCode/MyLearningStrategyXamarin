using System;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.ViewModels;
using Acr.UserDialogs;

namespace MyLearningStrategyMobleXForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectDetailPage : ContentPage, INotifyPropertyChanged
    {
        SubjectDetailModel viewModel;
        public SubjectDetailPage(Guid originator, SubjectsDTO subject)
        {
            InitializeComponent();

            if (subject.CloudRowId != Guid.Empty)
            {
                ToolbarItem toolbarItem = new ToolbarItem
                {
                    Text = "Delete",
                    Order = ToolbarItemOrder.Secondary,
                    Priority = 1,
                    IconImageSource = "strategy40delete.png"
                };
                toolbarItem.Clicked += OnDelete;
                this.ToolbarItems.Add(toolbarItem);
            }

            Title = "Subject";
            BindingContext = this.viewModel = new SubjectDetailModel(originator, subject, UserDialogs.Instance) ;
        }
        public SubjectDetailPage(Guid originator)
        {
            InitializeComponent();
            Title = "Subject";
            BindingContext = this.viewModel = new SubjectDetailModel(originator, new SubjectsDTO(), UserDialogs.Instance);
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
        async void PickPhoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
            });


            if (file == null)
                return;
            viewModel.Item.ImageDevice = file.Path;
        }

        private void ImageDevice_Error(object sender, FFImageLoading.Forms.CachedImageEvents.ErrorEventArgs e)
        {

        }
    }
}