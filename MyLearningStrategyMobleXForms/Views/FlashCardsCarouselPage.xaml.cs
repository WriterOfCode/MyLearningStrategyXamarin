using System.Linq;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyLearningStrategyMobleXForms.ViewModels;
using MyLearningStrategyMobleXForms.Models;
using Xamarin.Essentials;
using Acr.UserDialogs;

namespace MyLearningStrategyMobleXForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlashCardsCarouselPage : CarouselPage
    {
        SensorSpeed speed = SensorSpeed.UI;
        FlashCardsViewModel FlashCardModel;
        public FlashCardsCarouselPage(SubjectsDTO subject)
        {
            InitializeComponent();
            Title = "Flash Cards";

            try
            {
                BindingContext = FlashCardModel = new FlashCardsViewModel(subject, UserDialogs.Instance);
                FlashCardModel.LoadDataCommand.Execute(null);
                // Register for reading changes, be sure to unsubscribe when finished
                Accelerometer.ShakeDetected += ShakeToShuffle;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        private void ShakeToShuffle(object sender, EventArgs e)
        {
            FlashCardModel.ShuffleFlashCardsCommand.Execute(null);
        }
        protected override void OnAppearing()
        {

            base.OnAppearing();
            FlashCardModel.PopulateFlashCardsCommand.Execute(null);
            FlashCardModel.ShuffleFlashCardsCommand.Execute(null);
            if (Accelerometer.IsMonitoring)
                Accelerometer.Stop();
            else
                Accelerometer.Start(speed);
        }
        void OnShuffel(object sender, EventArgs e)
        {
            FlashCardModel.PopulateFlashCardsCommand.Execute(null);
            FlashCardModel.ShuffleFlashCardsCommand.Execute(null);
        }
        async void OnCancel(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
        protected override void OnDisappearing()
        {
            if (Accelerometer.IsMonitoring)
                Accelerometer.Stop();
        }

        private void PickStategy(object sender, EventArgs e)
        {
            FlashCardModel.PickStategyComand.Execute(null);
            FlashCardModel.PopulateFlashCardsCommand.Execute(null);
        }
    }
}