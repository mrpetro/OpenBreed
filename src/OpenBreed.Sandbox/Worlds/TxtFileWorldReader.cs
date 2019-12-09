using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core;
using OpenBreed.Core.Common;

namespace OpenBreed.Sandbox.Worlds
{
    internal class TxtFileWorldReader : IWorldReader, IDisposable
    {
        public ICore Core { get; }
        private StreamReader txtReader;

        internal TxtFileWorldReader(ICore core, string filePath)
        {
            Core = core;
            txtReader = File.OpenText(filePath);
        }

        private void ReadBody(WorldBuilder worldBuilder)
        {
            ReadSize(worldBuilder);

            worldBuilder.Add(WorldBuilderHelper.SETUP_WORLD, new object[] { worldBuilder.Width, worldBuilder.Height });

            for (int y = 0; y < worldBuilder.Height; y++)
                ReadMapLine(worldBuilder, y);
        }

        private void ReadExits(WorldBuilder worldBuilder, WorldBuilderHelper helper)
        {
            var split = txtReader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var exitsNoTxt = split[1];
            var exitsNo = int.Parse(exitsNoTxt);

            for (int i = 0; i < exitsNo; i++)
            {
                split = txtReader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var exitNoTxt = split[0][0];
                var mapName = split[1];
                var entryNoTxt = split[2][0];

                helper.RegisterExit(exitNoTxt, mapName, entryNoTxt);
            }
        }

        public World GetWorld()
        {
            var worldBuilder = Core.Worlds.GetBuilder();
            var helper = new WorldBuilderHelper(worldBuilder);
            helper.RegisterHandlers();

            ReadName(worldBuilder);
            ReadBody(worldBuilder);
            ReadExits(worldBuilder, helper);

            return worldBuilder.Build(); 
        }

        private void ReadName(WorldBuilder builder)
        {
            var name = txtReader.ReadLine().Split(new char[] { ' '}, StringSplitOptions.RemoveEmptyEntries)[1];
            builder.SetName(name);
        }

        private void ReadSize(WorldBuilder builder)
        {
            var split = txtReader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var widthTxt = split[1];
            var heightTxt = split[2];
            var width = int.Parse(widthTxt);
            var height = int.Parse(heightTxt);

            builder.SetSize(width, height);
        }

        private void ReadMapLine(WorldBuilder builder, int y)
        {
            var mapLine = txtReader.ReadLine();

            for (int x = 0; x < builder.Width; x++)
                ReadCell(builder, x, builder.Height - y - 1, mapLine[x*2], mapLine[x*2 + 1]);
        }

        private void ReadCell(WorldBuilder builder, int x, int y, int actionCode, int gfxCode)
        {
            builder.Add(actionCode, new object[] { x, y, gfxCode } );
        }
        
        public void Dispose()
        {
            txtReader.Close();
        }
    }
}
