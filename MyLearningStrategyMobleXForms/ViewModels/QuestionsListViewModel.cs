using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class QuestionsListViewModel : BaseViewModel
    {
        private QuestionsDataStore dataStore => DependencyService.Get<QuestionsDataStore>();
        private int _bodyOfKnowledgeId;
        public int BodyOfKnowledgeId
        {
            get { return _bodyOfKnowledgeId; }
            set { SetProperty(ref _bodyOfKnowledgeId, value, nameof(BodyOfKnowledgeId)); }
        }
        public ObservableCollection<QuestionsDTO> List { get; set; } = new ObservableCollection<QuestionsDTO>();
        public Command LoadDataCommand { get; set; }
        public QuestionsListViewModel(int bodyOfKnowledgeId, IUserDialogs dialogs) : base(dialogs)
        {
            BodyOfKnowledgeId = bodyOfKnowledgeId;
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
        }

        private async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var items = await dataStore.GetCached();
                var stortedList = items.Where(q => q.BodyOfKnowledgeId == BodyOfKnowledgeId)
                          .Select(s => s)
                          .OrderBy(o => o.Question);
                List.Clear();

                if (stortedList != null)
                    foreach (var item in stortedList) List.Add(item);
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
