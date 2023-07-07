using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MyLearningStrategyMobleXForms.Services
{
    public interface ISpeakService
    {
        bool CanSpeak { get; set; }
        bool IsSpeaking { get; set; }
        void CancelSpeech();
        ConcurrentQueue<string> QueuedTextToSpeek { get; set; }
        Task SpeakQueued();
    }
}