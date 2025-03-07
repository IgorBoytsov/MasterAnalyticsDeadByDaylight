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
    }
}