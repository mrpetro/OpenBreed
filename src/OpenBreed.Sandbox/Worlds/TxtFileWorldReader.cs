﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Worlds
{
    internal class TxtFileWorldReader : IWorldReader, IDisposable
    {
        public ICore Core { get; }
        private StreamReader txtReader;
        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;
        private readonly ViewportCreator viewportCreator;

        internal TxtFileWorldReader(ICore core, IWorldMan worldMan , IEntityMan entityMan, ViewportCreator viewportCreator, string filePath)
        {
            Core = core;
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.viewportCreator = viewportCreator;
            txtReader = File.OpenText(filePath);
        }

        private void ReadBody(WorldBuilder worldBuilder)
        {
            ReadSize(worldBuilder);

            for (int y = 0; y < worldBuilder.height; y++)
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

        private void ReadViewports(WorldBuilder worldBuilder, WorldBuilderHelper helper)
        {
            var split = txtReader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var viewportsNoTxt = split[1];
            var viewportsNo = int.Parse(viewportsNoTxt);

            for (int i = 0; i < viewportsNo; i++)
            {
                split = txtReader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var viewportNoTxt = split[0][0];
                var width = int.Parse(split[1]);
                var height = int.Parse(split[2]);
                var cameraName = split[3];

                helper.RegisterViewport(viewportNoTxt, width, height, cameraName);
            }
        }

        public World GetWorld()
        {
            var worldBuilder = worldMan.Create();
            var gameWorldHelper = Core.GetManager<GameWorldHelper>();

            var helper = new WorldBuilderHelper(worldBuilder, entityMan, viewportCreator);

            helper.RegisterHandlers();

            ReadName(worldBuilder);
            ReadBody(worldBuilder);
            ReadExits(worldBuilder, helper);
            ReadViewports(worldBuilder, helper);
            gameWorldHelper.AddSystems(worldBuilder);
            return worldBuilder.Build(Core); 
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

            for (int x = 0; x < builder.width; x++)
                ReadCell(builder, x, builder.height - y - 1, mapLine[x*2], mapLine[x*2 + 1]);
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
