using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using Xamarin.Forms;
using System.Collections.Generic;
using Acr.UserDialogs;

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class StrategiesListViewModel : BaseViewModel
    {
        public StrategiesListViewModel(Guid originator, IUserDialogs dialogs) : base(dialogs)
        {
            Originator = originator;
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
        }
        private StrategiesDataStore dataStore => DependencyService.Get<StrategiesDataStore>();
        private Guid _originator;
        public Guid Originator
        {
            get { return _originator; }
            set { SetProperty(ref _originator, value, nameof(Originator)); }
        }
        public ObservableCollection<StrategyDTO> List { get; set; } = new ObservableCollection<StrategyDTO>();
        public Command LoadDataCommand { get; set; }
        async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                PopulatePickLists();
                var items = await dataStore.Get();
                var stortedList = items.OrderBy(o => o.Name);
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

        #region PickLists

        public ObservableCollection<PickListDTO> SortRulesList => new ObservableCollection<PickListDTO>();
        public ObservableCollection<PickListDTO> QuestionSelectionList => new ObservableCollection<PickListDTO>();
        public ObservableCollection<PickListDTO> ResponseSelectionList => new ObservableCollection<PickListDTO>();

        void PopulatePickLists()
        {
            SortRulesList.Clear();
            SortRulesList.Add(new PickListDTO { Definition = "Question - a to z", Id = 0 });
            SortRulesList.Add(new PickListDTO { Definition = "Question in Reverse Order - z to a", Id = 1 });
            SortRulesList.Add(new PickListDTO { Definition = "Category", Id = 2 });
            SortRulesList.Add(new PickListDTO { Definition = "Random", Id = 3 });

            QuestionSelectionList.Clear();
            QuestionSelectionList.Add(new PickListDTO { Definition = "All", Id = 0 });
            QuestionSelectionList.Add(new PickListDTO { Definition = "Random", Id = 1 });
            QuestionSelectionList.Add(new PickListDTO { Definition = "Category", Id = 2 });

            ResponseSelectionList.Clear();
            ResponseSelectionList.Add(new PickListDTO { Definition = "All", Id = 0 });
            ResponseSelectionList.Add(new PickListDTO { Definition = "Category", Id = 1 });
            ResponseSelectionList.Add(new PickListDTO { Definition = "Random", Id = 2 });
            ResponseSelectionList.Add(new PickListDTO { Definition = "Only Correct", Id = 3 });
        }
        #endregion
    }
}
