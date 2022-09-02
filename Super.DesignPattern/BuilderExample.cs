namespace Super.DesignPattern
{
    public class BuilderExample : IDesignPatternExample
    {
        public void Run()
        {
            Console.Write("Type the word to translate to portuguese: ");
            string wordToTranslate = Console.ReadLine() ?? "empty";

            ParametersRequestBuilder parametersBuilder = new ParametersRequestBuilder();
            BuildParametersSearchGoogle(parametersBuilder, wordToTranslate);

            string url = "https://translate.google.com/" + parametersBuilder.GetRow();
            Console.WriteLine(url);
        }

        public void BuildParametersSearchGoogle(IRequestBuilder requestBuilder, string word)
        {
            requestBuilder.Add("sl", "en");
            requestBuilder.Add("tl", "pt");
            requestBuilder.Add("op", "translate");
            requestBuilder.Add("text", word);
        }
    }

    public interface IRequestBuilder
    {
        void Add(string key, object value);
    }

    public class ParametersRequestBuilder : IRequestBuilder
    {
        private Dictionary<string, object> _data;
        public ParametersRequestBuilder()
        {
            _data = new Dictionary<string, object>();
        }
        public void Add(string key, object value)
        {
            if (_data.ContainsKey(key))
            {
                _data[key] = value;
            }
            else
            {
                _data.Add(key, value);
            }
        }

        public string GetRow()
        {
            return $"?{string.Join("&", _data.Select(a => BuildKey(a.Key, a.Value)))}";
        }

        private string BuildKey(string key, object value)
        {
            return $"{key}={value.ToString()}";
        }
    }

    public class BodyRequestBuilder : IRequestBuilder
    {
        private Dictionary<string, string> _data;
        public BodyRequestBuilder()
        {
            _data = new Dictionary<string, string>();
        }

        public void Add(string key, object value)
        {
            string valueString = value?.ToString() ?? "";
            if (!_data.ContainsKey(key))
            {
                _data.Add(key, valueString);
            }
            else
            {
                _data[key] = valueString;
            }
        }

        public FormUrlEncodedContent GetForm()
        {
            return new FormUrlEncodedContent(_data);
        }
    }
}
