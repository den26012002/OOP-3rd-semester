namespace Backups.Client.Entities
{
    public class JsonStringBuilder
    {
        private string _resultJsonString;
        public JsonStringBuilder()
        {
            Clear();
        }

        public void Clear()
        {
            _resultJsonString = string.Empty;
        }

        public void AddProperty(string propertyName, string propertyValue)
        {
            if (_resultJsonString.Length != 0)
            {
                _resultJsonString += ",\n";
            }

            _resultJsonString += $"\"{propertyName}\" : \"{propertyValue}\"";
        }

        public string GetResult()
        {
            return "{ " + _resultJsonString + " }";
        }
    }
}
