using System;

namespace MyLearningStrategyMobleXForms.Models
{
    public interface IStrategyDTO
    {
        string Description { get; set; }
        string Name { get; set; }
        bool OnlyCorrect { get; set; }
        Guid Originator { get; set; }
        int QuestionSelection { get; set; }
        bool RecycleIncorrectlyAnswered { get; set; }
        int ResponseSelection { get; set; }
        int SortRuleId { get; set; }
        int StrategyId { get; set; }
    }
}