namespace LanguageInterpreter
{
    public class Environment
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();

        public void Define(string name, object value)
        {
            _values[name] = value;
        }

        public object Get(string name)
        {
            if (_values.ContainsKey(name))
                return _values[name];

            throw new RuntimeException($"Undefined variable '{name}'.");
        }

        public void Assign(string name, object value)
        {
            if (_values.ContainsKey(name))
            {
                _values[name] = value;
                return;
            }

            throw new RuntimeException($"Undefined variable '{name}'.");
        }
    }
}