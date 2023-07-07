using MyLearningStrategyMobleXForms.Models;
using System.Collections.Generic;

public enum SortOrderRules
{
    None = 0,
    QuestionAsc = 1,
    QuestionDesc =2,
    Random = 3,
    RandomFavorIncorrect = 4
}

public static class MessagingConstants
{
    public const string SubjectDeleted  = "SubjectDeleted";
    public const string SubjectAdded    = "SubjectAdded";
    public const string SubjectUpdated  = "SubjectUpdated";
    public const string ResponseDeleted = "ResponseDeleted";
    public const string ResponseAdded   = "ResponseAdded";
    public const string ResponseUpdated = "ResponseUpdated";
    public const string QuestionAdded   = "QuestionAdded";
    public const string QuestionDeleted = "QuestionDeleted";
    public const string QuestionUpdated = "QuestionUpdated";
    public const string StrategyAdded   = "StrategyAdded";
    public const string StrategyDeleted = "StrategyDeleted";
    public const string StrategyUpdated = "StrategyUpdated";
}

