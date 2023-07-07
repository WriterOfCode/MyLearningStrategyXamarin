using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using MyLearningStrategyMobleXForms.Converters;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Collections.Generic;

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class SubjectsListViewModel : BaseViewModel
    {
        public SubjectsListViewModel(IUserDialogs dialogs):base(dialogs)
        {
            speakService = new SpeakService();
            LoadDataCommand = new Command(async () => await LoadData());
            SpeakCommand = new Command(async () => await SpeakQueued());
            CancelSpeechCommand = new Command(() => CancelSpeech());
            DeleteCommand = new Command<int>(async bok => await DeleteSubject(bok));
        }

        private ISpeakService speakService = new SpeakService();
        private StrategiesDataStore dataStoreStrategies => DependencyService.Get<StrategiesDataStore>();
        private SubjectsDataStore dataStore => DependencyService.Get<SubjectsDataStore>();
        public ObservableCollection<SubjectsDTO> Subjects { get; set; } = new ObservableCollection<SubjectsDTO>();
        private IEnumerable<StrategyDTO> Strategies { get; set; }
        public Command LoadDataCommand { get; private set; }
        public Command SpeakCommand { get; private set; }
        public Command CancelSpeechCommand { get; private set; }
        public Command DeleteCommand { get; private set; }
        public Command PickStategyComand { get; private set; }
        private async Task LoadData()
        {
            if (IsBusy)
                return;

            IsBusy = true;
           
            try
            {
                Strategies = await dataStoreStrategies.Get();
                var subjects = await dataStore.Get();

                Subjects.Clear();
                subjects.OrderBy(o => o.Name).ToList().ForEach(s => Subjects.Add(s));

                PickStategyComand = CreateActionSheetCommand(false, null);
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

  
        private async Task<bool> DeleteSubject(int BodyOfKnowledgeId)
        {
           return await dataStore.Delete( BodyOfKnowledgeId);
        }      
        private void CancelSpeech()
        {
            speakService.CancelSpeech();
        }
        private Task SpeakQueued()
        {
            if  (speakService.QueuedTextToSpeek.IsEmpty)
            {
                foreach (var item in Subjects)
                {
                    speakService.QueuedTextToSpeek.Enqueue(item.ToString());
                }
            }
            return  speakService.SpeakQueued();
        }

        private Command CreateActionSheetCommand(bool useBottomSheet, string message = null)
        {
            return new Command(() =>
            {
                var cfg = new ActionSheetConfig()
                    .SetTitle("Pick Flash Cards")
                    .SetMessage(message)
                    .SetUseBottomSheet(useBottomSheet);

                foreach (var strategy in Strategies)
                {
                    cfg.Add(strategy.Name, () => this.SetPickedStrategy(strategy), null);
                }
                cfg.SetCancel("Cancel", () => this.SetPickedStrategy(null), null);

                var disp = this.Dialogs.ActionSheet(cfg);
            });
        }

        public StrategyDTO PickedStrategy { get; private set; }
        private void SetPickedStrategy(StrategyDTO pickedStatagy)
        {
            PickedStrategy = pickedStatagy;
        }
    }
}
