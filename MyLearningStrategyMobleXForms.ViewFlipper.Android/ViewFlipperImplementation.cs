using MyLearningStrategyMobleXForms.ViewFlipper.Abstractions;
using System;
using Xamarin.Forms;
using MyLearningStrategyMobleXForms.ViewFlipper.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyLearningStrategyMobleXForms.ViewFlipper.Abstractions.ViewFlipper), typeof(ViewFlipperRenderer))]
namespace MyLearningStrategyMobleXForms.ViewFlipper.Android
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