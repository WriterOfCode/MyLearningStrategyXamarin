using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyLearningStrategyMobleXForms.DataTemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlashCardResponseTemplate : ViewCell
    {
        public static readonly BindableProperty BaseContextProperty =
    BindableProperty.Create("BaseContext", typeof(object), typeof(FlashCardResponseTemplate), null, propertyChanged: OnParentContextPropertyChanged);

        public object BaseContext
        {
            get { return GetValue(BaseContextProperty); }
            set { SetValue(BaseContextProperty, value); }
        }

        public FlashCardResponseTemplate()
        {
            InitializeComponent();
        }

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as FlashCardResponseTemplate).BaseContext = newValue;
            }
        }
    }
}