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
    public class CategoryDetailViewModel : BaseViewModel
    {

        private CategoriesDataStore dataStore => DependencyService.Get<CategoriesDataStore>();
        private Guid _originator;
        public Guid Originator
        {
            get { return _originator; }
            set { SetProperty(ref _originator, value, nameof(Originator)); }
        }
        private CategoriesDTO _Category;
        public CategoriesDTO Category
        {
            get { return _Category; }
            set { SetProperty(ref _Category, value, nameof(Category)); }
        }
        public Command SaveCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public CategoryDetailViewModel(Guid originator, CategoriesDTO dto, IUserDialogs dialogs) : base(dialogs)
        {
            Originator = originator;
            Category = dto;

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
                if (Category.CloudRowId == null || Category.CloudRowId == Guid.Empty)
                {
                    Category = await dataStore.Post(Category);
                    MessagingCenter.Send(this, MessagingConstants.SubjectAdded, Category);
                }
                else
                {
                    Category = await dataStore.Put(Category);
                    MessagingCenter.Send(this, MessagingConstants.SubjectUpdated, Category);
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
                if (Category.CloudRowId != null & Category.CloudRowId != Guid.Empty)
                {
                    await dataStore.Delete(Category.CategoryId);
                    MessagingCenter.Send(this, MessagingConstants.SubjectDeleted, Category);
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
