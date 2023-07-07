using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MyLearningStrategyMobleXForms.Services
{
    public class FlashCardShakeAndShuffle
    {
        // Set speed delay for monitoring changes.
        SensorSpeed speed = SensorSpeed.UI;

        public FlashCardShakeAndShuffle()
        {
            // Register for reading changes, be sure to unsubscribe when finished
            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
        }

        void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            // Process shake event
        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                    Accelerometer.Start(speed);
            }
            catch
            {
                // Other error has occurred.
            }
        }
    }
}
