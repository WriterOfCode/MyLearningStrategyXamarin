using System;
using System.Collections.Generic;
using System.Text;
using MyLearningStrategyMobleXForms.Models;
using Xamarin.Forms;

namespace MyLearningStrategyMobleXForms
{
    public class SubjectDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
           return ((SubjectsStatsDTO)item).ImageCloud.Length==0? SubjectWithStatsTemplate:SubjectStatsWithImage;
        }
        public DataTemplate SubjectTemplate { get; set; }
        public DataTemplate SubjectWithStatsTemplate { get; set; }
        public DataTemplate SubjectStatsWithImage { get; set; }

    }
}
