namespace SQLHelper.Entities.Structs
{
    internal ref struct ColumnValue
    {
        private readonly object? _value;
        public ColumnValue(object? value)
        {
            _value = value;
        }
        public override string ToString()
        {
            var type = _value?.GetType();

            if (type is null) return string.Empty;

            if (type == typeof(string) || type == typeof(char))
            {
                return string.Format("'{0}'", _value);
            }
            return string.Format("{0}", _value).Replace(",", "");
        }
    }
}