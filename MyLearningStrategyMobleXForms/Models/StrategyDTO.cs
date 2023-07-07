using MyLearningStrategyMobleXForms.Services;
using System;

namespace MyLearningStrategyMobleXForms.Models
{
    public class StrategyDTO : BaseDTO, IStrategyDTO
    {
        public Guid Originator { get; set; }
        public int StrategyId { get; set; }
        public int UserProfileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortRuleId { get; set; }
        public int QuestionSelection { get; set; }
        public int ResponseSelection { get; set; }
        public bool OnlyCorrect { get; set; }
        public bool RecycleIncorrectlyAnswered { get; set; }
        public string Summary
        {
            get
            {
                string summary = "";
                switch (SortRuleId)
                {
                    case (int)PickListsEnum.SortRules.QuestionAsc:
                        summary = "Sort cards by Questions ascending.";
                        break;

                    case (int)PickListsEnum.SortRules.QuestionDesc:
                        summary = "Sort cards by Questions decending.";
                        break;

                    case (int)PickListsEnum.SortRules.Category:
                        summary = "Sort cards by Category.";
                        break;

                    case (int)PickListsEnum.SortRules.Random:
                        summary = "Sort cards by randomly.";
                        break;
                    default:
                        summary = summary + "Sort cards by randomly.";
                        break;

                }
                switch (QuestionSelection)
                {

                    case (int)PickListsEnum.QuestionSelection.All:
                        summary = summary + " Select all questions.";
                        break;
                    case (int)PickListsEnum.QuestionSelection.Category:
                        summary = summary + " Select questions by category.";
                        break;
                    case (int)PickListsEnum.QuestionSelection.Random:
                        summary = summary + " Select questions randomly.";
                        break;
                    default:
                        summary = summary + " Select all questions.";
                        break;
                }

                switch (ResponseSelection)
                {

                    case (int)PickListsEnum.ResponseSelection.All:
                        summary = summary + " Select all responses.";
                        break;
                    case (int)PickListsEnum.ResponseSelection.Category:
                        summary = summary + " Select responses by category.";
                        break;
                    case (int)PickListsEnum.ResponseSelection.OnlyCorrect:
                        summary = summary + " Select only correct responses.";
                        break;
                    case (int)PickListsEnum.ResponseSelection.Random:
                        summary = summary + " Select responses randomy.";
                        break;
                    default:
                        summary = summary + " Select all responses.";
                        break;
                }
                if (RecycleIncorrectlyAnswered) { summary = summary + " Recycle incorrectly answered."; }

                return summary;
            }
        }
    }
}
