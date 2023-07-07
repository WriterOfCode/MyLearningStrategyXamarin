using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using Xamarin.Forms;

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class QuestionsDetailModel : BaseViewModel
    {
        private QuestionsDataStore dataStore => DependencyService.Get<QuestionsDataStore>();

        public QuestionsDTO Item { get; set; }
        public Command SaveCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public QuestionsDetailModel(QuestionsDTO question, IUserDialogs dialogs) : base(dialogs)
        {
            Item = question;
            
            SaveCommand = new Command(async () => await ExecuteSave());
            DeleteCommand = new Command(async () => await ExecuteDelete());
        }

        async Task ExecuteSave()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                if (Item.CloudRowId == null || Item.CloudRowId == Guid.Empty)
                {
                    Item = await dataStore.Post(Item);
                }
                else
                {
                    Item = await dataStore.Put(Item);
                }
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
        async Task ExecuteDelete()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                if (Item.CloudRowId != null & Item.CloudRowId != Guid.Empty)
                {
                    await dataStore.Delete( Item.BodyOfKnowledgeId, Item.QuestionId);
                }
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
