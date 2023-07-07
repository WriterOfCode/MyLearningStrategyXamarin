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
    public class CategoriesListViewModel : BaseViewModel
    {
        private CategoriesDataStore dataStore => DependencyService.Get<CategoriesDataStore>();

        public CategoriesListViewModel(Guid originator, IUserDialogs dialogs) : base(dialogs)
        {
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
        }

        private Guid _originator;
        public Guid Originator
        {
            get { return _originator; }
            set { SetProperty(ref _originator, value, nameof(Originator)); }
        }
        public ObservableCollection<CategoriesDTO> List { get; set; } = new ObservableCollection<CategoriesDTO>();
        public Command LoadDataCommand { get; set; }
        async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = await dataStore.Get();
                var stortedList = items.OrderBy(o => o.CategoryName);
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
