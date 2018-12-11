using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Sources.Formats;
using EPF;
using OpenBreed.Common.Logging;
using System.ComponentModel;
using System.Globalization;
using OpenBreed.Common.Database.Items.Sources;

namespace OpenBreed.Common.Sources
{
    public delegate string ExpandVariablesDelegate(string text);

    public class SourcesHandler
    {
        #region Public Fields

        public ExpandVariablesDelegate ExpandVariables;

        #endregion Public Fields

        #region Private Fields

        private readonly Dictionary<string, ISourceFormat> _formats = new Dictionary<string, ISourceFormat>();
        private readonly  Dictionary<string, BaseSource> _openedSources = new Dictionary<string, BaseSource> ();
        private Dictionary<string, EPFArchive> _openedArchives = new Dictionary<string, EPFArchive>();

        #endregion Private Fields

        #region Public Constructors

        public void RegisterFormat(string formatAlias, ISourceFormat format)
        {
            if (_formats.ContainsKey(formatAlias))
                throw new InvalidOperationException($"Format alias '{formatAlias}' already registered.");

            _formats.Add(formatAlias, format);
        }

        public SourcesHandler()
        {
            ExpandVariables = ExpandVariablesDefault;      
        }

        #endregion Public Constructors

        #region Public Methods

        public BaseSource Create(SourceDef sourceDef)
        {
            if (sourceDef is DirectoryFileSourceDef)
                return new DirectoryFileSource(this, (DirectoryFileSourceDef)sourceDef);
            else if (sourceDef is EPFArchiveFileSourceDef)
                return new EPFArchiveFileSource(this, (EPFArchiveFileSourceDef)sourceDef);
            else
                throw new NotImplementedException("Unknown sourceDef");
        }

        public BaseSource GetSource(SourceDef sourceDef)
        {
            try
            {
                //Check if same source is already opened
                BaseSource source = null;
                if (_openedSources.TryGetValue(sourceDef.Name, out source))
                    return source;

                source = Create(sourceDef);
                _openedSources.Add(source.Name, source);
                return source;
            }
            catch (Exception ex)
            {
                LogMan.Instance.LogError(string.Format("Unable to get source using definition {0}. Reason: {1}", sourceDef.Name, ex.Message));
            }

            return null;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void CloseAll()
        {
            foreach (var openedArchive in _openedArchives)
                openedArchive.Value.Dispose();

            _openedArchives.Clear();
        }

        internal EPFArchive GetArchive(string archivePath)
        {
            string normalizedPath = OpenBreed.Common.Tools.GetNormalizedPath(archivePath);

            EPFArchive archive = null;
            if (!_openedArchives.TryGetValue(normalizedPath, out archive))
            {
                File.Copy(normalizedPath, normalizedPath + ".bkp", true);
                archive = EPFArchive.ToExtract(File.Open(normalizedPath, FileMode.Open), true);
                _openedArchives.Add(normalizedPath, archive);
            }

            return archive;
        }

        internal ISourceFormat GetFormatMan(string formatType)
        {
            ISourceFormat sourceMan = null;
            if (_formats.TryGetValue(formatType, out sourceMan))
                return sourceMan;
            else
                throw new InvalidOperationException("Unknown format: " + formatType);
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

        internal void ReleaseSource(BaseSource source)
        {
            _openedSources.Remove(source.Name);
        }

        #endregion Internal Methods

        #region Private Methods

        private string ExpandVariablesDefault(string value)
        {
            return value;
        }

        #endregion Private Methods

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
    }
}
