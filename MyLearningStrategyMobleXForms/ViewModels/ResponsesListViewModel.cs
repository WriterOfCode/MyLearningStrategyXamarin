using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using Xamarin.Forms;
using Acr.UserDialogs;
//response

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class ResponcesListViewModel : BaseViewModel
    {
        private ResponsesDataStore dataStore => DependencyService.Get<ResponsesDataStore>();

        private int _questionId;
        public int QuestionID
        {
            get { return _questionId; }
            set { SetProperty(ref _questionId, value, nameof(QuestionID)); }
        }
        private int _bodyOfKnowledgeId;
        public int BodyOfKnowledgeID
        {
            get { return _bodyOfKnowledgeId; }
            set { SetProperty(ref _bodyOfKnowledgeId, value, nameof(BodyOfKnowledgeID)); }
        }
        public ObservableCollection<ResponsesDTO> List { get; set; } = new ObservableCollection<ResponsesDTO>();
        public Command LoadDataCommand { get; set; }
        public ResponcesListViewModel(int bodyOfKnowledgeId, int questionId, IUserDialogs dialogs) : base(dialogs)
        {
            QuestionID = questionId;
            BodyOfKnowledgeID = bodyOfKnowledgeId;
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
        }
        async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = await dataStore.GetCached();
                List.Clear();
                if (items != null)
                {
                    var stortedList = items.Where(q => q.QuestionId == QuestionID)
                          .Select(s => s)
                          .OrderBy(o => o.Response);

                    foreach (var item in stortedList) List.Add(item);
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
