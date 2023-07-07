using System;
using System.Collections.Generic;
using System.Text;

namespace MyLearningStrategyMobleXForms.Models
{
    public class LearningHistoryStatsDTO: LearningHistoryDTO
    {
        public int AnsweredCorrectly { get; set; }
        public int AnsweredIncorrectly { get; set; }
        public int Answered { get; set; }
    }
}
