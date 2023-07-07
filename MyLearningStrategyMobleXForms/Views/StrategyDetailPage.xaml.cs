using MyLearningStrategyMobleXForms.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyLearningStrategyMobleXForms.ViewModels;
using Acr.UserDialogs;

namespace MyLearningStrategyMobleXForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StrategyDetailPage : ContentPage
    {
        StrategyDetailViewModel viewModel;
        public StrategyDetailPage(StrategyDTO strategy)
        {
            InitializeComponent();

            if (strategy.CloudRowId != Guid.Empty)
            {
                ToolbarItem toolbarItem = new ToolbarItem
                {
                    Text = "Delete",
                    Order = ToolbarItemOrder.Primary,
                    Priority = 2,
                    IconImageSource = "strategy40delete.png"
                };
                toolbarItem.Clicked += OnDelete;
                this.ToolbarItems.Add(toolbarItem);
            }
            Title = "Strategy";
            BindingContext = viewModel = new StrategyDetailViewModel( strategy, UserDialogs.Instance);
        }
        public StrategyDetailPage(Guid originator)
        {
            InitializeComponent();
            Title = "Strategy";
            BindingContext = this.viewModel = new StrategyDetailViewModel( new StrategyDTO(), UserDialogs.Instance);
        }
        async void OnSave(object sender, EventArgs e)
        {
            viewModel.SaveCommand.Execute(null);
            await Navigation.PopModalAsync(true);
        }
        async void OnCancel(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
        async void OnDelete(object sender, EventArgs e)
        {
            viewModel.DeleteCommand.Execute(null);
            await Navigation.PopModalAsync(true);
        }
    }
}