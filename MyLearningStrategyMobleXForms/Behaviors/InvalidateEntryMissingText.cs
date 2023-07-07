using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyLearningStrategyMobleXForms.Behaviors
{
    class InvalidateEntryMissingText : Behavior<Entry>
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(bindable);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            entry.BackgroundColor = e.NewTextValue.Length == 0 ? Color.Red : Color.Default;
        }
    
    }
}
