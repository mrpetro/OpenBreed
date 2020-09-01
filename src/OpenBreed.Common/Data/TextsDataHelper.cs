using OpenBreed.Common.Model.Maps;
using OpenBreed.Common.Model.Maps.Blocks;
using OpenBreed.Common.Model.Texts;
using OpenBreed.Common.Builders.Texts;
using OpenBreed.Database.Interface.Items.Texts;
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
            var builder = TextBuilder.NewTextModel();
            builder.SetName(textBlock.Name);
            builder.Text = textBlock.Value;
            return builder.Build();
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

        public static TextModel FromBinary(DataProvider provider, ITextEmbeddedEntry textEntry)
        {
            var builder = TextBuilder.NewTextModel();
            builder.SetText(textEntry.Text);
            return builder.Build();
        }
    }
}
