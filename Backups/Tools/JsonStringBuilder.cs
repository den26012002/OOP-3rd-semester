namespace Backups.Tools
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

        public void AddProperty(string propertyName, string[] propertyValues)
        {
            if (_resultJsonString.Length != 0)
            {
                _resultJsonString += ",\n";
            }

            _resultJsonString += $"\"{propertyName}\" : [";
            foreach (string propertyValue in propertyValues)
            {
                if (_resultJsonString[_resultJsonString.Length - 1] != '[')
                {
                    _resultJsonString += ",";
                }

                _resultJsonString += $"\n\"{propertyValue}\"";
            }

            _resultJsonString += $"\n]";
        }

        public void AddPropertyObject(string propertyName, string propertyJsonObject)
        {
            if (_resultJsonString.Length != 0)
            {
                _resultJsonString += ",\n";
            }

            _resultJsonString += $"\"{propertyName}\" : {propertyJsonObject}";
        }

        public void AddPropertyObject(string propertyName, string[] propertyJsonOjbects)
        {
            if (_resultJsonString.Length != 0)
            {
                _resultJsonString += ",\n";
            }

            _resultJsonString += $"{propertyName} : [";
            foreach (string propertyValue in propertyJsonOjbects)
            {
                if (_resultJsonString[_resultJsonString.Length - 1] != '[')
                {
                    _resultJsonString += ",";
                }

                _resultJsonString += $"\n{propertyValue}";
            }

            _resultJsonString += $"\n]";
        }

        public string GetResult()
        {
            return "{ " + _resultJsonString + " }";
        }
    }
}
