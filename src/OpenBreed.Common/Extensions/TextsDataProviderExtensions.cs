using OpenBreed.Common.Data;

namespace OpenBreed.Common.Extensions
{
    public static class TextsDataProviderExtensions
    {
        #region Public Methods

        public static string GetTextString(this TextsDataProvider textsDataProvider, string textId)
        {
            var text = textsDataProvider.GetText(textId).Text;
            var terminationIndex = text.IndexOf('\0');
            if(terminationIndex >= 0)
                text = text.Substring(0, terminationIndex);

            return text;
        }

        #endregion Public Methods
    }
}