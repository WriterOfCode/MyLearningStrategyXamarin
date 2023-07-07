using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MyLearningStrategyMobleXForms.Models;
using MyLearningStrategyMobleXForms.Services;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Collections.Generic;

namespace MyLearningStrategyMobleXForms.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        UserProfilesDataStore userData = new UserProfilesDataStore();
        public UserProfileViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            LoadDataCommand = new Command(async () => await LoadData());
        }
        public string ExternalId { get; set; }
        private Guid _originator;
        public Guid Originator
        {
            get { return _originator; }
            set { SetProperty(ref _originator, value, nameof(Originator)); }
        }
        public Command LoadDataCommand { get; private set; }
        private UserProfilesDTO _UserProfile;
        public UserProfilesDTO UserProfile
        {
            get { return _UserProfile; }
            set { SetProperty(ref _UserProfile, value, nameof(UserProfile)); }
        }
        private async Task LoadData()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var userInfoTask = await userData.Get(ExternalId);
       
                UserProfile = userInfoTask;
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
