using System;
using System.Collections.Generic;
using System.Text;

namespace MyLearningStrategyMobleXForms.Models
{
    public class QuestionsStatsDTO: QuestionsDTO
    {
        public int ResponseCount { get; set; }
        public int AnsweredCorrectly { get; set; }
        public int AnsweredIncorrectly { get; set; }
    }
}
