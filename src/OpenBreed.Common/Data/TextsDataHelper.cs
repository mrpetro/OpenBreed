using OpenBreed.Common.Maps;
using OpenBreed.Common.Maps.Blocks;
using OpenBreed.Common.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    internal class TextsDataHelper
    {
        public static TextModel Create(MapTextBlock textBlock)
        {
            var textModel = new TextModel();
            textModel.Name = textBlock.Name;
            textModel.Text = textBlock.Value;
            return textModel;
        }

        public static TextModel FromMapModel(DataProvider provider, ITextFromMapEntry textData)
        {
            var mapModel = provider.GetData(textData.DataRef) as MapModel;

            if (mapModel == null)
                return null;

            var textBlock = mapModel.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == textData.BlockName);

            if (textBlock == null)
                return null;

            return Create(textBlock);
        }

        public static TextModel FromBinary(DataProvider provider, ITextEmbeddedEntry textData)
        {
            return null;
        }
    }
}
