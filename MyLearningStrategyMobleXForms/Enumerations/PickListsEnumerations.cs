using MyLearningStrategyMobleXForms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLearningStrategyMobleXForms.Services
{
    public static class  PickListsEnum
    {
        public enum SortRules : int
        {
            QuestionAsc = 0,
            QuestionDesc = 1,
            Random = 2,
            Category = 3,
        }
        public enum QuestionSelection : int
        {
            All = 0,
            Random = 1,
            Category =2
        }
        public enum ResponseSelection : int
        {
            All = 0,
            Random = 1,
            Category = 2,
            OnlyCorrect = 3
        }

    }
}
