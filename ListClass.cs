namespace LanguageInterpreter
{
    public class ListClass
    {
        private List<object> elements;

        public ListClass()
        {
            elements = new List<object>();
        }

        public ListClass(List<object> items)
        {
            elements = new List<object>(items);
        }

        public void Add(object item)
        {
            elements.Add(item);
        }

        public object Get(int index)
        {
            if (index < 0 || index >= elements.Count)
            {
                throw new Exception($"Index out of range: {index}");
            }
            return elements[index];
        }

        public void Set(int index, object value)
        {
            if (index < 0 || index >= elements.Count)
            {
                throw new Exception($"Index out of range: {index}");
            }
            elements[index] = value;
        }

        public object Remove(int index)
        {
            if (index < 0 || index >= elements.Count)
            {
                throw new Exception($"Index out of range: {index}");
            }

            object removed = elements[index];
            elements.RemoveAt(index);
            return removed;
        }

        public int Count()
        {
            return elements.Count;
        }

        public override string ToString()
        {
            string result = "[";
            for (int i = 0; i < elements.Count; i++)
            {
                if (i > 0)
                    result += ", ";

                result += elements[i]?.ToString() ?? "null";
            }
            result += "]";
            return result;
        }
    }
}