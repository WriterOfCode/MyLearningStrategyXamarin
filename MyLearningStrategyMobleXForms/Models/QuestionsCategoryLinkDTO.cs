using System;
using System.Collections.Generic;
using System.Text;

namespace MyLearningStrategyMobleXForms.Models
{
    public class QuestionsCategoryLinkDTO:BaseDTO
    {
        public int QuestionId { get; set; }
        public int CategoryId { get; set; }
        public Guid Originator { get; set; }
    }
}
