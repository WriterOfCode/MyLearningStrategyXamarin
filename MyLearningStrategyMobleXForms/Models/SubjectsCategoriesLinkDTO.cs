using System;
using System.Collections.Generic;
using System.Text;

namespace MyLearningStrategyMobleXForms.Models
{
    public class SubjectsCategoriesLinkDTO
    {
        public int BodyOfKnowledgeId { get; set; }
        public int CategoryId { get; set; }
        public Guid Originator { get; set; }
    }
}
