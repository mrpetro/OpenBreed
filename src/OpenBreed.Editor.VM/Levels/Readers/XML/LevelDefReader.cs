using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Levels.Builders;
using System.IO;
using OpenBreed.Editor.VM.Database.Sources;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Common;

namespace OpenBreed.Editor.VM.Levels.Readers.XML
{
    public class LevelDefReader
    {
        private readonly LevelBuilder m_Builder;

        public LevelDefReader(LevelBuilder builder)
        {
            m_Builder = builder;
        }

        private void LoadMap(LevelBuilder levelBuilder, string mapRef)
        {
            //var mapSourceDef = m_Database.GetSourceDef(mapRef);
            //if (mapSourceDef != null)
            //    levelBuilder.Map = EditorModel.Instance.Sources.Load(mapSourceDef) as MapModel;
        }

        private void LoadTileSet(LevelBuilder levelBuilder, string tileSetRef)
        {
            //var tileSetSourceDef = m_Database.GetSourceDef(tileSetRef);
            //if (tileSetSourceDef != null)
            //    levelBuilder.TileSet = EditorModel.Instance.Sources.Load(tileSetSourceDef) as TileSetModel;
        }

        private void LoadSpriteSet(LevelBuilder levelBuilder, string spriteSetRef)
        {
            //var spriteSetSourceDef = m_Database.GetSourceDef(spriteSetRef);
            //if (spriteSetSourceDef != null)
            //{
            //    var spriteSet = EditorModel.Instance.Sources.Load(spriteSetSourceDef) as SpriteSetModel;
            //    levelBuilder.AddSpriteSet(spriteSet);
            //}
        }

        private void LoadPropertySet(LevelBuilder levelBuilder, string propertySetRef)
        {
            //SourceDef propertySetSourceDef = null;
            //if (string.IsNullOrWhiteSpace(propertySetRef))
            //{
            //    LogMan.Instance.LogWarning("Property Set source not set. Getting default: DefaultPropertySetDef.xml");
            //    propertySetSourceDef = m_Database.GetSourceDef("DefaultPropertySetDef.xml");
            //}
            //else
            //    propertySetSourceDef = m_Database.GetSourceDef(propertySetRef);

            //if (propertySetSourceDef != null)
            //    levelBuilder.PropertySet = EditorModel.Instance.Sources.Load(propertySetSourceDef) as PropertySetModel;
        }

        public ProjectDef Read(LevelDef levelDef)
        {
            var levelBuilder = LevelBuilder.NewLevel();

            levelBuilder.SetId(levelDef.Id);
            levelBuilder.SetName(levelDef.Name);

            LoadMap(levelBuilder, levelDef.MapResourceRef);
            LoadTileSet(levelBuilder, levelDef.TileSetResourceRef);
            LoadPropertySet(levelBuilder, levelDef.PropertySetResourceRef);
            foreach (var spriteSetSourceRef in levelDef.SpriteSetResourceRefs)
                LoadSpriteSet(levelBuilder, spriteSetSourceRef);

            return levelBuilder.Build();
        }

        public ProjectDef Read(Stream stream)
        {
            var levelDef = Tools.RestoreFromXml<LevelDef>(stream);

            return Read(levelDef);
        }
    }
}
