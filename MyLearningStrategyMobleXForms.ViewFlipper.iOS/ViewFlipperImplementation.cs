using MyLearningStrategyMobleXForms.ViewFlipper.Abstractions;
using System;
using Xamarin.Forms;
using MyLearningStrategyMobleXForms.ViewFlipper.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyLearningStrategyMobleXForms.ViewFlipper.Abstractions.ViewFlipper), typeof(ViewFlipperRenderer))]
namespace MyLearningStrategyMobleXForms.ViewFlipper.iOS
{
    /// <summary>
    /// ViewFlipper Renderer
    /// </summary>
    public class ViewFlipperRenderer //: TRender (replace with renderer type
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init() { }
    }
}
