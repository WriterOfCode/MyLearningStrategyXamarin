using System;
//using System.ComponentModel.DataAnnotations;

namespace MyLearningStrategyMobleXForms.Models
{
    public class LearningHistoryDTO : BaseDTO
    {

        public int StrategyHistoryId { get; set; }
        public int StrategyId { get; set; }
        public int BodyOfKnowledgeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortRuleId { get; set; }
        public int QuestionSelection { get; set; }
        public int ResponseSelection { get; set; }
        public bool OnlyCorrect { get; set; }
        public bool RecycleIncorrectlyAnswered { get; set; }
        public DateTime FirstLearningRunDate { get; set; }
        public int NumberOfTimesTried { get; set; }
        public int LastQuestionId { get; set; }
    }
}