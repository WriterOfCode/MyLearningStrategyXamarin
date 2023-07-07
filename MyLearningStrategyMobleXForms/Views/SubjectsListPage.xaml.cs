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
    public partial class SubjectsListPage : ContentPage
    {
        SubjectsListViewModel viewModel;
        public SubjectsListPage()
        {
            InitializeComponent();
            Title = "Subjects";
            BindingContext = viewModel = new SubjectsListViewModel(UserDialogs.Instance);
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Subjects.Count == 0)
                viewModel.LoadDataCommand.Execute(null);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (viewModel.CancelSpeechCommand!=null)
            viewModel.CancelSpeechCommand.Execute(this);
        }

        private void OnAddItem(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Navigation.PushModalAsync(new NavigationPage(new SubjectDetailPage(App.CurrenUserProfile.Originator)));
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
        private void OnFlashCards(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                // Find the single item here
                SubjectsDTO subject = (sender as MenuItem).BindingContext as SubjectsDTO;
                if (subject == null) return;

                Navigation.PushModalAsync(
                    new NavigationPage(
                        new FlashCardsCarouselPage(subject)));
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
        private void OnEdit(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                MenuItem obj = sender as MenuItem;
                // Find the single item here
                SubjectsDTO bok = obj.BindingContext as SubjectsDTO;
                if (bok == null)
                    return;

                Navigation.PushModalAsync(
                    new NavigationPage(
                        new SubjectDetailPage(
                            App.CurrenUserProfile.Originator, bok)));

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
        private void OnDelete(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                MenuItem obj = sender as MenuItem;
                // Find the single item here
                SubjectsDTO bok = obj.BindingContext as SubjectsDTO;
                if (bok == null)
                    return;

                viewModel.DeleteCommand.Execute(bok.BodyOfKnowledgeId);
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
        private void OnItemTaped(object sender, ItemTappedEventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (e.Item == null)
                    return;

                var bok = e.Item as SubjectsDTO;
                if (bok == null)
                    return;

                 Navigation.PushAsync(new QuestionsListPage(bok));

                ((ListView)sender).SelectedItem = null;

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