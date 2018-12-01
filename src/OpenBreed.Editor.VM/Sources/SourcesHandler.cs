using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Database.Sources;
using System.IO;
using OpenBreed.Editor.VM.Database.Resources;
using OpenBreed.Editor.VM.Sources.Formats;
using EPF;
using OpenBreed.Common.Logging;
using System.ComponentModel;
using System.Globalization;

namespace OpenBreed.Editor.VM.Sources
{
    public class SourcesHandler
    {
        private readonly EditorVM m_Editor;
        private readonly Dictionary<string, ISourceFormat> m_Formats = new Dictionary<string, ISourceFormat>();

        private readonly  Dictionary<string, BaseSource> m_OpenedSources = new Dictionary<string, BaseSource> ();

        private Dictionary<string,EPFArchive> m_OpenedArchives;

        public EditorVM Editor { get { return m_Editor; } }

        public SourcesHandler(EditorVM editor)
        {
            if (editor == null)
                throw new ArgumentNullException("Editor");

            m_Editor = editor;

            m_OpenedArchives = new Dictionary<string, EPFArchive>();
            m_Formats.Add("ABSE_MAP", new ABSEMAPFormat());
            m_Formats.Add("ABHC_MAP", new ABHCMAPFormat());
            m_Formats.Add("ABTA_MAP", new ABTAMAPFormat());
            m_Formats.Add("ABTABLK", new ABTABLKFormat());
            m_Formats.Add("ABTASPR", new ABTASPRFormat());
            m_Formats.Add("ACBM_TILE_SET", new ACBMTileSetFormat());
            m_Formats.Add("LevelXML", new LevelXMLFormat());
            m_Formats.Add("PropertySetXML", new PropertySetXMLFormat());
        }

        internal Dictionary<string, object> GetParameters(List<SourceParameterDef> parameterDefs)
        {
            var parameters = new Dictionary<string, object>();

            foreach (var parameterDef in parameterDefs)
            {
                if (string.IsNullOrWhiteSpace(parameterDef.Name) ||
                    string.IsNullOrWhiteSpace(parameterDef.Type))
                    continue;

                if (parameters.ContainsKey(parameterDef.Name))
                    continue;

                var type = Type.GetType(parameterDef.Type);
                var tc = TypeDescriptor.GetConverter(type);
                var value = tc.ConvertFromString(null, CultureInfo.InvariantCulture, parameterDef.Value);
                parameters.Add(parameterDef.Name, value);
            }

            return parameters;
        }

        internal EPFArchive GetArchive(string archivePath)
        {
            string normalizedPath = OpenBreed.Common.Tools.GetNormalizedPath(archivePath);

            EPFArchive archive = null;
            if (!m_OpenedArchives.TryGetValue(normalizedPath, out archive))
            {
                File.Copy(normalizedPath, normalizedPath + ".bkp", true);
                archive = EPFArchive.ToExtract(File.Open(normalizedPath, FileMode.Open), true);
                m_OpenedArchives.Add(normalizedPath, archive);
            }

            return archive;
        }

        internal ISourceFormat GetFormatMan(string formatType)
        {
            ISourceFormat sourceMan = null;
            if (m_Formats.TryGetValue(formatType, out sourceMan))
                return sourceMan;
            else
                throw new InvalidOperationException("Unknown format: " + formatType);
        }

        public BaseSource Create(SourceDef sourceDef)
        {
            if (sourceDef is DirectoryFileSourceDef)
                return new DirectoryFileSource(this, (DirectoryFileSourceDef)sourceDef);
            else if (sourceDef is EPFArchiveFileSourceDef)
                return new EPFArchiveFileSource(this, (EPFArchiveFileSourceDef)sourceDef);
            else
                throw new NotImplementedException("Unknown sourceDef");
        }

        internal void ReleaseSource(BaseSource source)
        {
            m_OpenedSources.Remove(source.Name);
        }

        public BaseSource GetSource(SourceDef sourceDef)
        {
            try
            {
                //Check if same source is already opened
                BaseSource source = null;
                if (m_OpenedSources.TryGetValue(sourceDef.Name, out source))
                    return source;

                source = Create(sourceDef);
                m_OpenedSources.Add(source.Name, source);
                return source;
            }
            catch (Exception ex)
            {
                LogMan.Instance.LogError(string.Format("Unable to get source using definition {0}. Reason: {1}", sourceDef.Name, ex.Message));
            }

            return null;
        }

        //public object Load(SourceDef sourceDef)
        //{
        //    try
        //    {
        //        //Check if same source is already opened
        //        BaseSource source = null;
        //        if(m_OpenedSources.TryGetValue(sourceDef.Name, out source))
        //            return source.Load();

        //        source = Create(sourceDef);
        //        var model = source.Load();
        //        m_OpenedSources.Add(source.Name, source);
        //        return model;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogMan.Instance.LogError(string.Format("Loading file '{0}' from '{1}' failed. Reason: {1}", sourceDef.Name, sourceDef, ex.Message));
        //    }

        //    return null;
        //}

        internal void CloseAll()
        {
            foreach (var openedArchive in m_OpenedArchives)
                openedArchive.Value.Dispose();

            m_OpenedArchives.Clear();
        }
    }
}
