using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MyLearningStrategyMobleXForms.Converters
{
    public static class ObservableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> items)
        {
            ObservableCollection<T> collection = new ObservableCollection<T>();
            items.ForEach(s => collection.Add(s));
            return collection;
        }
    }

    public static class BindingList
    {
        public static BindingList<T> ToBindingList<T>(this List<T> items)
        {
            BindingList<T> collection = new BindingList<T>();

            foreach (var item in items)
            {
                collection.Add(item);
            }

            return collection;
        }
    }
}
