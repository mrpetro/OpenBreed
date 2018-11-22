using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Editor.VM.Database.Sources;

namespace OpenBreed.Editor.VM.Sources
{
    public class DirectoryFileSource : BaseSource
    {
        private string m_DirectoryPath;

        public string DirectoryPath { get { return m_DirectoryPath; } }

        public DirectoryFileSource(SourcesHandler manager, DirectoryFileSourceDef sourceDef) :
            base(manager, sourceDef)
        {
            m_DirectoryPath = manager.Editor.Settings.ExpandVariables(sourceDef.DirectoryPath);
        }

        protected override Stream Open()
        {
            string filePath = Path.Combine(DirectoryPath, Name);
            return File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
        }

        protected override void Close()
        {
            Stream.Close();

            base.Close();
        }
    }
}
