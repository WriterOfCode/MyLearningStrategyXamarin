using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyLearningStrategyMobleXForms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppMasterDetail : MasterDetailPage
    {
        public AppMasterDetail()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as AppMasterDetailMasterMenuItem;
            if (item == null)
                return;
            switch (item.PageOptions)
            {
                case DetailPageOptions.CreateInstance:
                    var page = (Page)Activator.CreateInstance(item.TargetType);
                    page.Title = item.Title;
                    Detail = new NavigationPage(page);
                    IsPresented = false;
                    break;
                case DetailPageOptions.TabbedPage:
                    break;
                default:
                    break;
            }

            MasterPage.ListView.SelectedItem = null;
        }
    }
}