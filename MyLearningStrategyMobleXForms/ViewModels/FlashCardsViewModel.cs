using System;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using static MyLearningStrategyMobleXForms.Services.PickListsEnum;
using Acr.UserDialogs;
using System.ComponentModel;
using System.Windows.Input;

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class FlashCardsViewModel : BaseViewModel
    {
        private StrategiesDataStore dsStrategies => DependencyService.Get<StrategiesDataStore>();
        private LearningHistoryDataStore dsLearningHistory => DependencyService.Get<LearningHistoryDataStore>();
        private LearningHistoryProgressDataStore dsLearningHistProgress => DependencyService.Get<LearningHistoryProgressDataStore>();
        private QuestionsDataStore dsQuestions => DependencyService.Get<QuestionsDataStore>();
        private ResponsesDataStore dsResponses => DependencyService.Get<ResponsesDataStore>();
               
        private IEnumerable<StrategyDTO> strategiesToPickFrom;
        private IEnumerable<LearningHistoryDTO> learningHistoryToPickFrom;
        private IEnumerable<LearningHistoryProgressDTO> learningProgressToPickFrom;
        private IEnumerable<QuestionsDTO> questionsToPickfrom;
        private IEnumerable<ResponsesDTO> responsesToPickfrom;
        
        private SubjectsDTO _subject;
        public SubjectsDTO Subject
        {
            get { return _subject; }
            set { SetProperty(ref _subject, value, nameof(Subject)); }
        }

        private void Strategy(StrategyDTO pickedStatagy)
        {
            Title = pickedStatagy.Name;
            PickedStrategy = pickedStatagy;
        }
        private StrategyDTO _strategyDTO;

        public StrategyDTO PickedStrategy
        {
            get { return _strategyDTO; }
            set { SetProperty(ref _strategyDTO, value, nameof(PickedStrategy)); }
        }



        private LearningHistoryDTO PickedLearningHistory;
        private List<FlashCardsQuestionsDTO> pickedFlashCards = new List<FlashCardsQuestionsDTO>();
        ObservableCollection<FlashCardsQuestionsDTO> _flashCardsSource = new ObservableCollection<FlashCardsQuestionsDTO>();
        public ObservableCollection<FlashCardsQuestionsDTO> FlashCardsSource
        {
            set
            {
                _flashCardsSource = value;
                OnPropertyChanged("FlashCardsSource");
            }
            get
            {
                return _flashCardsSource;
            }
        }

        public FlashCardsViewModel(SubjectsDTO subject, IUserDialogs dialogs) : base(dialogs)
        {
            Subject = subject;
            LoadDataCommand = new Command(async () => await LoadDataAsync());
            ShuffleFlashCardsCommand = new Command(async () => await ShuffleFlashCards());
            PopulateFlashCardsCommand = new Command(async () => await PopulateFlashCards());
            PickStategyComand = CreateActionSheetCommand(false, null);
        }

        public Command LoadDataCommand { get; set; }
        private async Task LoadDataAsync()
        {
            if (!IsBusy)
            { 
                IsBusy = true;
                try
                {
                    strategiesToPickFrom = await dsStrategies.Get();
                    learningHistoryToPickFrom = await dsLearningHistory.Get();
                    learningProgressToPickFrom = await dsLearningHistProgress.Get();
                    questionsToPickfrom = await dsQuestions.Get();
                    responsesToPickfrom = await dsResponses.Get();
                    PickStrategyOrDefaultStrategy();
                    PickQuestionsForFlashCards();
                    PickResponsesForFlashCards();
                    ShufflePopulateFlashCardList();
                }
                catch (AggregateException ae)
                {
                    Debug.WriteLine(ae);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            };
        }
        public Command ShuffleFlashCardsCommand { get; set; }
        private async Task ShuffleFlashCards()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                await Task.Run(() =>
                {
                    ShufflePopulateFlashCardList();
                });
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
        public Command PopulateFlashCardsCommand { get; set; }

        private async Task PopulateFlashCards()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                await Task.Run(() =>
                {
                    PickStrategyOrDefaultStrategy();
                    PickQuestionsForFlashCards();
                    PickResponsesForFlashCards();
                    //PickLearningHistoryProgress();
                    ShufflePopulateFlashCardList();
                });

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

        private void PickStrategyOrDefaultStrategy()
        {
            if (PickedStrategy == null)
            { PickedStrategy = strategiesToPickFrom.First(); }

            if (PickedLearningHistory == null || 
                PickedLearningHistory.StrategyId!=PickedStrategy.StrategyId)
            {
                PickedLearningHistory = learningHistoryToPickFrom
                .Where(s => s.BodyOfKnowledgeId == Subject.BodyOfKnowledgeId
                & s.StrategyId == PickedStrategy.StrategyId)
                .DefaultIfEmpty(new LearningHistoryDTO
                {
                    StrategyId = PickedStrategy.StrategyId,
                    BodyOfKnowledgeId = Subject.BodyOfKnowledgeId,
                    Name = PickedStrategy.Name,
                    Description = PickedStrategy.Description,
                    SortRuleId = PickedStrategy.SortRuleId,
                    QuestionSelection = PickedStrategy.QuestionSelection,
                    ResponseSelection = PickedStrategy.ResponseSelection,
                    OnlyCorrect = PickedStrategy.OnlyCorrect,
                    RecycleIncorrectlyAnswered = PickedStrategy.RecycleIncorrectlyAnswered,
                    FirstLearningRunDate = System.DateTime.Now
                })
                .First();
            }

        }
        private void PickQuestionsForFlashCards()
        {
            IEnumerable<QuestionsDTO> pickList = questionsToPickfrom;
            
            if (PickedLearningHistory.QuestionSelection == (int)QuestionSelection.Random)
            {
                Random randomPicQty = new Random();
                int qtyQuestionsToPickMin = decimal.ToInt32((questionsToPickfrom.Count() * (4 / 100)));
                int qtyQuestionsToPick = randomPicQty.Next(qtyQuestionsToPickMin, questionsToPickfrom.Count());

                Random randq = new Random();
                foreach (QuestionsDTO question in questionsToPickfrom) { question.OrderBy = randq.Next(); }

                pickList = questionsToPickfrom
                    .Where(q => q.BodyOfKnowledgeId == PickedLearningHistory.BodyOfKnowledgeId)
                    .OrderBy(q => q.OrderBy)
                    .Take(qtyQuestionsToPick);
            }
            else if (PickedLearningHistory.QuestionSelection == (int)QuestionSelection.Category)
            {
                pickList = questionsToPickfrom
                    .Where(q => q.BodyOfKnowledgeId == PickedLearningHistory.BodyOfKnowledgeId)
                    .OrderBy(q => q.OrderBy);
            }
            else
            {
                pickList = questionsToPickfrom
                    .Where(q => q.BodyOfKnowledgeId == PickedLearningHistory.BodyOfKnowledgeId)
                    .OrderBy(q => q.OrderBy);
            }
            pickedFlashCards.Clear();
            pickedFlashCards.AddRange(pickList.Select(fc => new FlashCardsQuestionsDTO
            {
                OrderBy = fc.OrderBy,
                BodyOfKnowledgeId = fc.BodyOfKnowledgeId,
                QuestionId = fc.QuestionId,
                Question = fc.Question,
                Mnemonic = fc.Mnemonic,
                Image_1_Cloud = fc.Image_1_Cloud,
                Image_2_Cloud = fc.Image_2_Cloud,
                Image_3_Cloud = fc.Image_3_Cloud,
                Image_1_Device = fc.Image_1_Device,
                Image_2_Device = fc.Image_2_Device,
                Image_3_Device = fc.Image_3_Device,
                Image_1_Hash = fc.Image_1_Hash,
                Image_2_Hash = fc.Image_2_Hash,
                Image_3_Hash = fc.Image_3_Hash,
                Hyperlink_1 = fc.Hyperlink_1,
                Hyperlink_2 = fc.Hyperlink_2,
                Hyperlink_3 = fc.Hyperlink_3,
                LastModifiedOffset = fc.LastModifiedOffset,
                CloudRowId = fc.CloudRowId
            }));
        }
        private void PickResponsesForFlashCards()
        {
          
            //Pick Responses
            switch (PickedLearningHistory.ResponseSelection)
            {
                case (int)ResponseSelection.Random:

                    Random randomPicQty = new Random();
                    int qtyResponsesToPickMin = decimal.ToInt32((responsesToPickfrom.Count() * (4 / 100)));
                    int qtyResponsesToPick = randomPicQty.Next(qtyResponsesToPickMin, responsesToPickfrom.Count());

                    Random randr = new Random();
                    foreach (ResponsesDTO responses in responsesToPickfrom)
                    { responses.OrderBy = randr.Next(); }

                    foreach (var question in pickedFlashCards)
                    {
                        //question.ImageMediaUrls = new List<ImageMediaUrls>();
                        //if (question.Image_1_Cloud.Length > 0 || question.Image_1_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_1_Cloud, ImageDevice = question.Image_1_Device, ImageHash = question.Image_1_Hash, CloudRowId = question.CloudRowId }); }
                        //if (question.Image_2_Cloud.Length > 0 || question.Image_2_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_2_Cloud, ImageDevice = question.Image_2_Device, ImageHash = question.Image_2_Hash, CloudRowId = question.CloudRowId }); }
                        //if (question.Image_3_Cloud.Length > 0 || question.Image_3_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_3_Cloud, ImageDevice = question.Image_3_Device, ImageHash = question.Image_3_Hash, CloudRowId = question.CloudRowId }); }

                        question.Responses = new List<ResponsesDTO>();
                        question.Responses.AddRange(responsesToPickfrom
                                .Where(r => r.QuestionId == question.QuestionId)
                                .OrderBy(r => r.IsCorrect)
                                .ThenBy(r => r.OrderBy)
                                .Take(qtyResponsesToPick)
                                .ToList<ResponsesDTO>());
                    }
                    break;
                case (int)ResponseSelection.All:
                    foreach (var question in pickedFlashCards)
                    {
                        //question.ImageMediaUrls = new List<ImageMediaUrls>();
                        //if (question.Image_1_Cloud.Length > 0 || question.Image_1_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_1_Cloud, ImageDevice = question.Image_1_Device, ImageHash = question.Image_1_Hash, CloudRowId = question.CloudRowId }); }
                        //if (question.Image_2_Cloud.Length > 0 || question.Image_2_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_2_Cloud, ImageDevice = question.Image_2_Device, ImageHash = question.Image_2_Hash, CloudRowId = question.CloudRowId }); }
                        //if (question.Image_3_Cloud.Length > 0 || question.Image_3_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_3_Cloud, ImageDevice = question.Image_3_Device, ImageHash = question.Image_3_Hash, CloudRowId = question.CloudRowId }); }

                        question.Responses = new List<ResponsesDTO>();
                        question.Responses.AddRange(responsesToPickfrom
                        .Where(r => r.QuestionId == question.QuestionId)
                        .OrderBy(q => q.Response)
                        .ToList<ResponsesDTO>());
                    }
                    break;
                case (int)ResponseSelection.Category:

                    break;
                case (int)ResponseSelection.OnlyCorrect:
                    foreach (var question in pickedFlashCards)
                    {
                        question.ImageMediaUrls = new List<ImageMediaUrls>();

                        //if (question.Image_1_Cloud.Length > 0 || question.Image_1_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_1_Cloud, ImageDevice = question.Image_1_Device, ImageHash = question.Image_1_Hash, CloudRowId = question.CloudRowId }); }
                        //if (question.Image_2_Cloud.Length > 0 || question.Image_2_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_2_Cloud, ImageDevice = question.Image_2_Device, ImageHash = question.Image_2_Hash, CloudRowId = question.CloudRowId }); }
                        //if (question.Image_3_Cloud.Length > 0 || question.Image_3_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_3_Cloud, ImageDevice = question.Image_3_Device, ImageHash = question.Image_3_Hash, CloudRowId = question.CloudRowId }); }


                        question.Responses = new List<ResponsesDTO>();
                        question.Responses.AddRange(responsesToPickfrom
                        .Where(r => r.QuestionId == question.QuestionId & r.IsCorrect == true)
                        .ToList<ResponsesDTO>());
                    }
                    break;
                default:
                    foreach (var question in pickedFlashCards)
                    {
                        //question.ImageMediaUrls = new List<ImageMediaUrls>();
                        //if (question.Image_1_Cloud.Length > 0 || question.Image_1_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_1_Cloud, ImageDevice = question.Image_1_Device, ImageHash = question.Image_1_Hash, CloudRowId = question.CloudRowId }); }
                        //if (question.Image_2_Cloud.Length > 0 || question.Image_2_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_2_Cloud, ImageDevice = question.Image_2_Device, ImageHash = question.Image_2_Hash, CloudRowId = question.CloudRowId }); }
                        //if (question.Image_3_Cloud.Length > 0 || question.Image_3_Device.Length > 0)
                        //{ question.ImageMediaUrls.Add(new ImageMediaUrls { ImageCloud = question.Image_3_Cloud, ImageDevice = question.Image_3_Device, ImageHash = question.Image_3_Hash, CloudRowId = question.CloudRowId }); }

                        question.Responses = new List<ResponsesDTO>();
                        question.Responses.AddRange(responsesToPickfrom
                        .Where(r => r.QuestionId == question.QuestionId)
                        .OrderBy(q => q.Response)
                        .ToList<ResponsesDTO>());
                    }
                    break;
            }
        }
        private void PickLearningHistoryProgress()
        {
            foreach (var question in pickedFlashCards)
            {
                question.Progress = new LearningHistoryProgressDTO
                    {QuestionId = question.QuestionId ,
                    StrategyHistoryId = PickedLearningHistory.StrategyHistoryId};


                question.Progress = learningProgressToPickFrom
                .Where(r => r.QuestionId == question.QuestionId & r.StrategyHistoryId == PickedLearningHistory.StrategyHistoryId)
                .FirstOrDefault();
            }
        }
        private void ShufflePopulateFlashCardList()
        {
            System.Random randq = new Random();
            //List<FlashCardsQuestionsDTO> sortedFlashCards;
            //Shuffel Flash Cards
            //sortedFlashCards.Clear();
            List<FlashCardsQuestionsDTO> sortedFlashCards;
            switch (PickedLearningHistory.SortRuleId)
            {
                case (int)SortOrderRules.Random:

                    pickedFlashCards.ForEach(q => q.OrderBy = randq.Next());
                    sortedFlashCards = pickedFlashCards
                        .OrderBy(q => q.OrderBy)
                        .ToList();
                    break;
                case (int)SortOrderRules.RandomFavorIncorrect:
                    pickedFlashCards.ForEach(q => q.OrderBy = randq.Next());
                    sortedFlashCards = pickedFlashCards
                        .OrderByDescending(q => q.AnsweredIncorrectly)
                        .ThenByDescending(q => q.OrderBy)
                        .ToList();
                    break;
                case (int)SortOrderRules.QuestionAsc:
                    sortedFlashCards = pickedFlashCards
                        .OrderBy(q => q.Question)
                        .ToList();

                    break;
                case (int)SortOrderRules.QuestionDesc:
                    sortedFlashCards = pickedFlashCards
                        .OrderByDescending(q => q.Question)
                        .ToList();
                    break;
                default:
                    sortedFlashCards = pickedFlashCards
                        .OrderBy(q => q.OrderBy)
                        .ToList();
                    break;
            }
            FlashCardsSource =  new ObservableCollection<FlashCardsQuestionsDTO>(sortedFlashCards);
        }

        public ICommand PickStategyComand { get; set; }
        ICommand CreateActionSheetCommand(bool useBottomSheet, string message = null)
        {
            return new Command(() =>
            {
                var cfg = new ActionSheetConfig()
                    .SetTitle("Pick Flash Cards")
                    .SetMessage(message)
                    .SetUseBottomSheet(useBottomSheet);

                foreach (var strategy in strategiesToPickFrom)
                {
                    cfg.Add(strategy.Name,
                        () => this.Strategy(strategy), null);
                }
                cfg.SetCancel("Cancel", () => this.Strategy(null), null);

                var disp = this.Dialogs.ActionSheet(cfg);
            });
        }
    }
}
