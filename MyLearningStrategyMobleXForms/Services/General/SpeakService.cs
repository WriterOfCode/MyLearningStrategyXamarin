using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyLearningStrategyMobleXForms.Services
{
    public class SpeakService : ISpeakService
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        private bool isSpeaking;
        public bool IsSpeaking
        {
            get { return isSpeaking; }
            set { isSpeaking = value; }
        }
        private bool canSpeak;
        public bool CanSpeak
        {
            get {return (QueuedTextToSpeek.Count > 0 && !isSpeaking);}
            set { canSpeak = value; }
        }

        public ConcurrentQueue<string> QueuedTextToSpeek { get; set; } = new ConcurrentQueue<string>();

        public SpeakService()
        {
            cts = new CancellationTokenSource();
        }
        public void CancelSpeech()
        {
            if (cts?.IsCancellationRequested ?? true)
                return;

            cts.Cancel();
        }
        public async Task SpeakQueued()
        {
            try
            {
                if (isSpeaking) return;
                if (QueuedTextToSpeek == null) return;
                isSpeaking = true;
  
                var locales = await TextToSpeech.GetLocalesAsync();
                // Grab the first locale
                var locale = locales.FirstOrDefault();

                var settings = new SpeechOptions()
                {
                    Volume = .75f,
                    Pitch = 1.0f,
                    Locale = locale
                };

                ConcurrentDictionary<string, Task> Speaker = new ConcurrentDictionary<string, Task>();
                string theText;
                while (QueuedTextToSpeek.TryDequeue(out theText))
                {
                    Speaker.TryAdd(theText, TextToSpeech.SpeakAsync(theText, cancelToken: cts.Token));
                }

                await Task.WhenAll(
                    Speaker.Values.ToArray<Task>())
                     .ContinueWith((t) =>
                     {
                         IsSpeaking = false;
                     },
                     TaskScheduler.FromCurrentSynchronizationContext());

            }
            catch (OperationCanceledException)
            {
                //log.Info(oe);
            }
            catch (AggregateException)
            {
                //log.Fatal(TaskLoggingHelper.FormatAggergateException(ae, 100));
                cts.Cancel();
            }
            catch (Exception)
            {
                //log.Fatal(e);
                cts.Cancel(true);
            }
            finally
            {
                isSpeaking = false;
            }
        }
    }
}
