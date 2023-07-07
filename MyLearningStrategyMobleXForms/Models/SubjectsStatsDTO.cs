using System;
using System.Collections.Generic;
using System.Text;

namespace MyLearningStrategyMobleXForms.Models
{
    public class SubjectsStatsDTO: SubjectsDTO
    {
        public int QuestionsCount { get; set; }
        public int LearningStrategiesCount { get; set; }
        public int LearningHistoryCount { get; set; }
        public int AnsweredCorrectlyCount { get; set; }
        public string Image
        {
            get
            {
                string image = base.ImageDevice ?? base.ImageCloud ?? string.Empty;
                return image.Length == 0 ? string.Empty : image;
            }
            set {
                base.ImageDevice = value;
                OnPropertyChanged("ImageDevice");
            }
        }
        public IEnumerable<CategoriesDTO> Categories { get; set; }

        public override string ToString()
        {
            return "Subject, " + this.Name + ", Description, " + this.Description + ", " + this.AnsweredCorrectlyCount + " Answered Correctly of " + this.QuestionsCount + " Questions. ";
        }
    }
}