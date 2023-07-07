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
    public partial class QuestionsListPage : ContentPage
    {
        SubjectsDTO BodyOfKnowledge;
        QuestionsListViewModel viewModel;
        public QuestionsListPage( SubjectsDTO bodyOfKnowledge)
        {
            InitializeComponent();
            Title = "Questions";
            BodyOfKnowledge = bodyOfKnowledge;
            BindingContext = viewModel = new QuestionsListViewModel( BodyOfKnowledge.BodyOfKnowledgeId, UserDialogs.Instance);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.List.Count == 0)
                viewModel.LoadDataCommand.Execute(null);
        }
        async void OnAddItem(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Navigation.PushModalAsync(new NavigationPage(new QuestionDetailPage(
                    new QuestionsDTO { BodyOfKnowledgeId = viewModel.BodyOfKnowledgeId }))); 

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
                QuestionsDTO dto = obj.BindingContext as QuestionsDTO;
                if (dto == null)
                    return;

                await Navigation.PushModalAsync(new NavigationPage(
                    new QuestionDetailPage(dto)));
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
        async void OnItemTaped(object sender, ItemTappedEventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (e.Item == null)
                    return;

                var selectedQuestion = e.Item as QuestionsDTO;
                if (selectedQuestion == null)
                    return;

                await Navigation.PushAsync(new ResponsesListPage(selectedQuestion.BodyOfKnowledgeId , selectedQuestion.QuestionId));

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
