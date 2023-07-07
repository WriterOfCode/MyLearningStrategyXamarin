using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Windows.Input;
using System.Threading;
using System.Collections.Generic;

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class MediaViewModel : BaseViewModel
    {
        public Command Open { get; }

        public MediaViewModel(Guid originator, IUserDialogs dialogs) : base(dialogs)
        {
            Originator = originator;
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            ExecuteLoadDataCommand();
            PickStategyComand = CreateActionSheetCommand(false, null);
            //this.Open = new Command(() =>
            //{
            //    ToastConfig.DefaultBackgroundColor = System.Drawing.Color.SlateGray;
            //    ToastConfig.DefaultActionTextColor = System.Drawing.Color.Maroon;
            //    ToastConfig.DefaultMessageTextColor = System.Drawing.Color.White;

            //    dialogs.Toast(new ToastConfig("This is my toast")
            //        .SetDuration(TimeSpan.FromSeconds(20))
            //        .SetPosition(ToastPosition.Bottom)
            //        .SetAction(x => x
            //            .SetText("bob")
            //        )
            //    );
            //});    
        }

        private StrategiesDataStore dsStrategies => DependencyService.Get<StrategiesDataStore>();
        private Guid _originator;
        public Guid Originator
        {
            get { return _originator; }
            set { SetProperty(ref _originator, value, nameof(Originator)); }
        }

        private void Strategy(StrategyDTO pickedStatagy)
        {
            PickedStrategy = pickedStatagy;
        }
        private StrategyDTO _strategyDTO;

        public StrategyDTO PickedStrategy
        {
            get { return _strategyDTO; }
            set { SetProperty(ref _strategyDTO, value, nameof(PickedStrategy)); }
        }



        public ObservableCollection<StrategyDTO> Strategies { get; set; } = new ObservableCollection<StrategyDTO>();
        public Command LoadDataCommand { get; set; }
        async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var strategies = await dsStrategies.Get();
                Strategies.Clear();

                if (strategies != null)
                {
                    var stortedList = strategies.OrderBy(o => o.Name);
                    foreach (var item in stortedList) Strategies.Add(item);
                   
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
 
        public ICommand PickStategyComand { get; set; }
        ICommand CreateActionSheetCommand(bool useBottomSheet,  string message = null)
        {
            return new Command(() =>
            {
                var cfg = new ActionSheetConfig()
                    .SetTitle("Pick Flash Cards")
                    .SetMessage(message)
                    .SetUseBottomSheet(useBottomSheet);

                foreach (var strategy in Strategies)
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
