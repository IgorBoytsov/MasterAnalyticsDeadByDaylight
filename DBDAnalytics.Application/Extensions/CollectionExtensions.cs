using System.Collections.ObjectModel;

namespace DBDAnalytics.Application.Extensions
{
    public static class CollectionExtensions
    {
        public static void ReplaceItem<T>(this IList<T> collection, T selectedItem, T newItem)
        {
            if (collection == null || selectedItem == null || newItem == null)
                throw new ArgumentNullException("Collection, SelectedItem или new item не должны быть null");

            int index = collection.IndexOf(selectedItem);
            if (index != -1)
            {
                collection[index] = newItem;
            }
        }

        public static void ReverseInPlace<T>(this ObservableCollection<T> collection)
        {
            if (collection == null || collection.Count < 2)
                return;

            int count = collection.Count;

            for (int i = 0; i < count - 1; i++)
            {
                collection.Move(count - 1, i);
            }
        }
    }
}