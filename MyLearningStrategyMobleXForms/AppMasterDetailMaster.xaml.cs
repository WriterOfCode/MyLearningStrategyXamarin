using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyLearningStrategyMobleXForms.Views;

namespace MyLearningStrategyMobleXForms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppMasterDetailMaster : ContentPage
    {
        public ListView ListView;

        public AppMasterDetailMaster()
        {
            InitializeComponent();

            BindingContext = new AppMasterDetailMasterViewModel();
            ListView = MenuItemsListView;
        }

        class AppMasterDetailMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<AppMasterDetailMasterMenuItem> MenuItems { get; set; }

            public AppMasterDetailMasterViewModel()
            {
                MenuItems = new ObservableCollection<AppMasterDetailMasterMenuItem>(new[]
                {
                    new AppMasterDetailMasterMenuItem { Id = 0,
                        Title = "Subjects",
                        Image="bookshelf40.png",
                        TargetType = typeof(SubjectsListPage),
                        PageOptions= DetailPageOptions.CreateInstance},
                    new AppMasterDetailMasterMenuItem { Id = 1,
                        Title = "Strategies",
                        Image="strategy40.png",
                        TargetType = typeof(StrategiesListPage),
                        PageOptions= DetailPageOptions.CreateInstance},
                    new AppMasterDetailMasterMenuItem { Id = 2,
                        Title = "Categories",
                        Image="openedfolder40.png",
                        TargetType = typeof(CategoriesListPage),
                        PageOptions= DetailPageOptions.CreateInstance},
                    new AppMasterDetailMasterMenuItem { Id = 3,
                        Title = "Setting",
                        Image="settingsgears40.png",
                        TargetType = typeof(SettingsPageView),
                        PageOptions= DetailPageOptions.CreateInstance},
                    new AppMasterDetailMasterMenuItem { Id = 4,
                        Title = "Media",
                        Image="image40gallery.png",
                        TargetType =typeof(MediaPage),
                        PageOptions = DetailPageOptions.CreateInstance },
                }); 
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}