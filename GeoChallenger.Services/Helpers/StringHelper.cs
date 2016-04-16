namespace GeoChallenger.Services.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Cut text to the specified length, doesn't split words
        /// </summary>
        /// <param name="text">Original text</param>
        /// <param name="maxLength">Maximum length of text.</param>
        /// <param name="tail">Part that is added at the end of result string.</param>
        /// <returns>Resulted text.</returns>
        // TODO: test.
        public static string Cut(string text, int maxLength, string tail = " ...")
        {
            if (string.IsNullOrEmpty(text)) {
                return string.Empty;
            }

            if (text.Length <= maxLength) {
                return text;
            }

            var result = text.Substring(0, maxLength);
            var lastSpace = result.LastIndexOf(' ');
            if (lastSpace != -1) {
                result = result.Substring(0, lastSpace);
            }
            return result + tail;
        }
    }
}