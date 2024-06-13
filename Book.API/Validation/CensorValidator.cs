using System.Text.RegularExpressions;

namespace Book.API.Validation
{
    public class CensorValidator
    {
        public IList<string> CensoredWords { get; private set; }

        public CensorValidator(IEnumerable<string> censoredWords)
        {
            if (censoredWords == null)
                throw new ArgumentNullException(nameof(censoredWords));

            CensoredWords = new List<string>(censoredWords);
        }

        public static async Task<CensorValidator> CreateFromFileAsync()
        {
            string filePath = "Validation\\swears.txt";
            var censoredWords = await ReadCensoredWordsFromFileAsync(filePath);
            return new CensorValidator(censoredWords);
        }

        private static async Task<IEnumerable<string>> ReadCensoredWordsFromFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("The specified file does not exist", filePath);

            var censoredWords = await File.ReadAllLinesAsync(filePath);
            return censoredWords;
        }

        public string CensorText(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            string censoredText = text;

            foreach (string censoredWord in CensoredWords)
            {
                string regularExpression = ToRegexPattern(censoredWord);

                censoredText = Regex.Replace(censoredText, regularExpression, StarCensoredMatch,
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }

            return censoredText;
        }

        private static string StarCensoredMatch(Match m)
        {
            string word = m.Captures[0].Value;
            return new string('*', word.Length);
        }

        private string ToRegexPattern(string wildcardSearch)
        {
            string regexPattern = Regex.Escape(wildcardSearch);
            regexPattern = regexPattern.Replace(@"\*", ".*?");
            regexPattern = regexPattern.Replace(@"\?", ".");

            if (regexPattern.StartsWith(".*?"))
            {
                regexPattern = regexPattern.Substring(3);
                regexPattern = @"(^\b)*?" + regexPattern;
            }

            regexPattern = @"\b" + regexPattern + @"\b";

            return regexPattern;
        }
    }
}