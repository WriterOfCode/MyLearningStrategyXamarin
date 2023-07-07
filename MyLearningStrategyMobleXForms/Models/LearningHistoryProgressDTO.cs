using System;
using System.Collections.Generic;
using System.Text;

namespace MyLearningStrategyMobleXForms.Models
{
    public class LearningHistoryProgressDTO : BaseDTO
    {
        public int LearningHistoryProgressId { get; set; }
        public int StrategyHistoryId { get; set; }
        public int QuestionId { get; set; }
        public int AnsweredIncorrectlyCount { get; set; }
        public int AnsweredCorrectlyCount { get; set; }
    }
}
