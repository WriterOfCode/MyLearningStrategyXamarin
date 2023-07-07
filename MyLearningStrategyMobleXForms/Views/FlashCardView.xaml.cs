using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLearningStrategyMobleXForms.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyLearningStrategyMobleXForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlashCardView : ContentView
    {
        public FlashCardView()
        {
            InitializeComponent();
        }
        ObservableCollection<ImageMediaUrls> _imageMediaURLsSouce = new ObservableCollection<ImageMediaUrls>();
        public ObservableCollection<ImageMediaUrls> FlashCardsImageMediaSource
        {
            set
            {
                _imageMediaURLsSouce = value;
                OnPropertyChanged("FlashCardsImageMediaSource");
            }
            get
            {
                return _imageMediaURLsSouce;
            }
        }

        public static readonly BindableProperty FlashCardQuestionProperty =
                               BindableProperty.Create(propertyName: nameof(FlashCardQuestion),
                                                       returnType: typeof(FlashCardsQuestionsDTO),
                                                       declaringType: typeof(FlashCardsQuestionsDTO),
                                                       defaultValue: null);

        public FlashCardsQuestionsDTO FlashCardQuestion
        {
            get => (FlashCardsQuestionsDTO)GetValue(FlashCardQuestionProperty);
            set => SetValue(FlashCardQuestionProperty, value);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);    // Be sure to do all the "normal" activities of the base class

            switch (propertyName)
            {
                case nameof(FlashCardQuestion):
                    UpdateResponses();
                    break;                    // Note the method naming convention: "Update" + property name.
            }
        }

        private void UpdateResponses()
        {
            // Write the property value to a XAML label. Note that the property is simply referenced by its name.
            // Note that if you don't explicitly update the XAML label, the change won't be visible.
            //ParameterNameLabel.Text = ParameterName;.
            if (FlashCardQuestion != null & FlashCardQuestion.ImageMediaUrls != null)
            {
                if (FlashCardQuestion.ImageMediaUrls.Count > 0)
                {
                    FlashCardsImageMediaSource = new ObservableCollection<ImageMediaUrls>(FlashCardQuestion.ImageMediaUrls);
                }
            }
        }
    
    
    
    }
}