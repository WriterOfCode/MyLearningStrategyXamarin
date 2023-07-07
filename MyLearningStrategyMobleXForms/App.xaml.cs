using System;
using Xamarin.Forms;
using Akavache;
using MyLearningStrategyMobleXForms.Services.Data;
using MyLearningStrategyMobleXForms.Services;
using MyLearningStrategyMobleXForms.Models;

namespace MyLearningStrategyMobleXForms
{
    public partial class App : Application
    {
        public static IBlobCache GlobalBlobCash;

        public static UserProfilesDTO CurrenUserProfile { get; set; }
        public App()
        {
            InitializeComponent();
            //CurrenUserProfile = new UserProfilesDTO { Originator = Guid.Parse("4d463812-9173-4a6a-b9e9-fd5d860edf42"), ExternalID= "A321CBA7-8346-4E8F-A962-DA751D81B8F4" };
            //CurrenUserProfile = new UserProfilesDTO { Originator = Guid.Parse("6ad10852-4897-43ed-a7ba-f71a14156a39") };
            //A321CBA7 - 8346 - 4E8F - A962 - DA751D81B8F4  60F0A231-BF12-481E-B82C-B069C5B1EC44
           
            //azure dev
            CurrenUserProfile = new UserProfilesDTO { Originator = 
            Guid.Parse("4d463812-9173-4a6a-b9e9-fd5d860edf42"), 
            ExternalID= "A321CBA7-8346-4E8F-A962-DA751D81B8F4"};


            DependencyService.Register<CategoriesDataStore>();
            DependencyService.Register<LearningHistoryDataStore>();
            DependencyService.Register<LearningHistoryProgressDataStore>();
            DependencyService.Register<QuestionsCategoriesDataStore>();
            DependencyService.Register<QuestionsDataStore>();
            DependencyService.Register<ResponsesDataStore>();
            DependencyService.Register<SpeakService>();
            DependencyService.Register<StrategiesDataStore>();
            DependencyService.Register<SubjectsCategoriesDataStore>();
            DependencyService.Register<SubjectsDataStore>();
            DependencyService.Register<StatsDataStore>();
            DependencyService.Register<UserPermissionsDataStore>();
            DependencyService.Register<UserProfilesDataStore>();


            BlobCache.ApplicationName = DataServiceConfig.ApplicationName;

            MainPage = new AppMasterDetail();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //AppCenter.Start("android=89d8bdef-cd8b-4380-bfc6-209fcc593430;" +
            //      "ios={Your iOS App secret here}",
            //      typeof(Analytics), typeof(Crashes));


        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
          
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


    }
}
