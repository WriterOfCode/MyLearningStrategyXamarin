using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyLearningStrategyMobleXForms.Models
{
    public class FlashCardsQuestionsDTO : QuestionsDTO
    {
        public int AnsweredCorrectly { get; set; }
        public int AnsweredIncorrectly { get; set; }
        public List<ResponsesDTO> Responses { get; set; }
        public List<ImageMediaUrls> ImageMediaUrls { get; set; }
        public LearningHistoryProgressDTO Progress { get; set; }
    }
}
