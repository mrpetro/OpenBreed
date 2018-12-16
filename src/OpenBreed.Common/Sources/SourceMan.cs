using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Formats;
using EPF;
using OpenBreed.Common.Logging;
using System.ComponentModel;
using System.Globalization;
using OpenBreed.Common.Database.Items.Sources;

namespace OpenBreed.Common.Sources
{
    public delegate string ExpandVariablesDelegate(string text);

    public class SourceMan
    {
        #region Public Fields

        public ExpandVariablesDelegate ExpandVariables;

        #endregion Public Fields

        #region Private Fields

        private readonly  Dictionary<string, BaseSource> _openedSources = new Dictionary<string, BaseSource> ();
        private Dictionary<string, EPFArchive> _openedArchives = new Dictionary<string, EPFArchive>();

        #endregion Private Fields

        #region Public Constructors

        public SourceMan()
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


    }
}
