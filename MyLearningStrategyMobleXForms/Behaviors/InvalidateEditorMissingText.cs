using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyLearningStrategyMobleXForms.Behaviors
{
    public class InvalidateEditorMissingText : Behavior<Editor>
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        protected override void OnAttachedTo(Editor bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(bindable);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            Editor entry = sender as Editor;
            entry.BackgroundColor = e.NewTextValue.Length == 0 ? Color.Red : Color.Default;


        }
    }
}
