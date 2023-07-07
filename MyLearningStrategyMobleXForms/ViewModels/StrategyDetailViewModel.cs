using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using Xamarin.Forms;

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class StrategyDetailViewModel : BaseViewModel
    {
        public StrategyDetailViewModel( StrategyDTO strategy, IUserDialogs dialogs) : base(dialogs)
        {

            PopulatePickLists();
            Item = strategy;
                       
            SaveCommand = new Command(async () => await ExecuteSave());
            DeleteCommand = new Command(async () => await ExecuteDelete());

            try
            {
                PickedSortRule = SortRulesList.First<PickListDTO>(q => q.Id == Item.ResponseSelection);
                PickedQuestion = QuestionSelectionList.First<PickListDTO>(q => q.Id == Item.QuestionSelection);
                PickedResponse = ResponseSelectionList.First<PickListDTO>(q => q.Id == Item.SortRuleId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private StrategiesDataStore dataStore => DependencyService.Get<StrategiesDataStore>();
        private StrategyDTO _item;
        public StrategyDTO Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value, nameof(Item)); }
        }
        PickListDTO pickedQuestion;
        public PickListDTO PickedQuestion
        {
            get { return pickedQuestion; }

            set
            {
                if (pickedQuestion != value)
                {
                    pickedQuestion = value;
                    Item.QuestionSelection = pickedQuestion.Id;
                    SetProperty(ref pickedQuestion, value, nameof(PickedQuestion));
                }
            }
        }

        PickListDTO pickedSortRule;
        public PickListDTO PickedSortRule
        {
            get { return pickedSortRule; }

            set
            {
                if (pickedSortRule != value)
                {
                    pickedSortRule = value;
                    Item.SortRuleId = pickedSortRule.Id;
                    SetProperty(ref pickedSortRule, value, nameof(PickedSortRule));
                }
            }
        }

        PickListDTO pickedResponse;
        public PickListDTO PickedResponse
        {
            get { return pickedResponse; }

            set
            {
                if (pickedResponse != value)
                {
                    pickedResponse = value;
                    Item.ResponseSelection = pickedResponse.Id;
                    SetProperty(ref pickedResponse, value, nameof(PickedResponse));
                }
            }
        }


        private PickListDTO _selectedItem;
        public PickListDTO SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        #region PickLists

        public ObservableCollection<PickListDTO> SortRulesList { get; set; } = new ObservableCollection<PickListDTO>();
        public ObservableCollection<PickListDTO> QuestionSelectionList { get; set; } = new ObservableCollection<PickListDTO>();
        public ObservableCollection<PickListDTO> ResponseSelectionList { get; set; } = new ObservableCollection<PickListDTO>();

        void PopulatePickLists()
        {
            SortRulesList.Add(new PickListDTO { Definition = "Question Ascending", Id = 0 });
            SortRulesList.Add(new PickListDTO { Definition = "Question Decending", Id = 1 });
            SortRulesList.Add(new PickListDTO { Definition = "Category", Id = 2 });
            SortRulesList.Add(new PickListDTO { Definition = "Random", Id = 3 });
           
            QuestionSelectionList.Add(new PickListDTO { Definition = "All", Id = 0 });
            QuestionSelectionList.Add(new PickListDTO { Definition = "Random", Id = 1 });
            QuestionSelectionList.Add(new PickListDTO { Definition = "Category", Id = 2 });

            ResponseSelectionList.Add(new PickListDTO { Definition = "All", Id = 0 });
            ResponseSelectionList.Add(new PickListDTO { Definition = "Category", Id = 1 });
            ResponseSelectionList.Add(new PickListDTO { Definition = "Random", Id = 2 });
            ResponseSelectionList.Add(new PickListDTO { Definition = "Only Correct", Id = 3 });
        }
        #endregion

        public Command SaveCommand { get; set; }
        public Command DeleteCommand { get; set; }

        async Task ExecuteSave()
        {

            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                if (Item.CloudRowId == null || Item.CloudRowId == Guid.Empty)
                {
                    Item = await dataStore.Post( Item);
                    MessagingCenter.Send(this, MessagingConstants.StrategyAdded, Item);
                }
                else
                {
                    Item = await dataStore.Put( Item);
                    MessagingCenter.Send(this, MessagingConstants.StrategyUpdated, Item);
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
                    await dataStore.Delete( Item.StrategyId);
                    MessagingCenter.Send(this, MessagingConstants.StrategyDeleted, Item);
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
