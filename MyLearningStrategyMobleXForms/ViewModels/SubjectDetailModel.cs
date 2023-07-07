using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using Xamarin.Forms;

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class SubjectDetailModel : BaseViewModel
    {
        private SubjectsDataStore dataStore => DependencyService.Get<SubjectsDataStore>();
        private Guid _originator;
        public Guid Originator
        {
            get { return _originator; }
            set { SetProperty(ref _originator, value, nameof(Originator)); }
        }
        private SubjectsDTO _item;
        public SubjectsDTO Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value, nameof(Item)); }
        }
        public Command SaveCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public SubjectDetailModel(Guid originator, SubjectsDTO dto, IUserDialogs dialogs) : base(dialogs)
        {
            Originator = originator;
            Item = dto;

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
                    Item = await dataStore.Post(Item);
                    MessagingCenter.Send(this, MessagingConstants.SubjectAdded, Item);
                }
                else
                {
                    Item = await dataStore.Put( Item);
                    MessagingCenter.Send(this, MessagingConstants.SubjectUpdated, Item);
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
                    await dataStore.Delete(Item.BodyOfKnowledgeId);
                    MessagingCenter.Send(this, MessagingConstants.SubjectDeleted , Item);
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