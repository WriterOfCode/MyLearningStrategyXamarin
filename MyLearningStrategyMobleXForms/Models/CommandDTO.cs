using System.Windows.Input;

namespace MyLearningStrategyMobleXForms.Models
{
    public class CommandDTO
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
        public IStrategyDTO Strategy { get; set; }
        public int BodyOfKnowledgeId { get; set; }
    }
}
