using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using Xamarin.Forms;

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class ResponcesDetailModel : BaseViewModel
    {
        private ResponsesDataStore dataStore => DependencyService.Get<ResponsesDataStore>();
        public ResponsesDTO Item { get; set; }
        public Command SaveCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public ResponcesDetailModel(ResponsesDTO response, IUserDialogs dialogs) : base(dialogs)
        {
            Item = response;

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
                    Item = await dataStore.Post( Item);
                }
                else
                {
                    Item = await dataStore.Put( Item);
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
                    await dataStore.Delete( Item.QuestionId, Item.ResponseId);
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
