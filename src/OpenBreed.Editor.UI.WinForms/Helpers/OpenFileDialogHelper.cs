using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Helpers
{
    public class OpenFileDialogHelper
    {
        public static void PrepareToSelectABTAGameRunFile(OpenFileDialog dialog, string initFilePath)
        {
            dialog.Reset();
            dialog.Title = "Select Alien Breed: Tower Assault run file...";
            dialog.Filter = "Exe file (*.exe)|*.exe|BAT file (*.bat)|*.bat";
            dialog.FileName = initFilePath;
        }

        public static void PrepareToOpenABDatabase(OpenFileDialog openFileDialog)
        {
            openFileDialog.Title = "Select ABEd Project File to Open...";
            openFileDialog.Filter = "Alien Breed Database file (*.xml)|*.xml|All Files (*.*)|*.*";
        }

        public static void PrepareToOpenABEdProjFile(OpenFileDialog openFileDialog, string initialDirectory = null)
        {
            openFileDialog.Title = "Select ABEd Project File to Open...";
            openFileDialog.Filter = "OpenABEd project files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = initialDirectory;
        }

        public static void PrepareToOpenMAPFile(OpenFileDialog openFileDialog)
        {
            openFileDialog.Title = "Select Alien Breed Map File to Open...";
            openFileDialog.Filter = "Alien Breed Map Files (MAP.*)|MAP.*|All Files (*.*)|*.*";
        }

        public static void PrepareToOpenBLKFile(OpenFileDialog openFileDialog)
        {
            openFileDialog.Title = "Select Alien Breed Tile File to Open...";
            openFileDialog.Filter = "Alien Breed Tile File (*.BLK)|*.BLK|All Files (*.*)|*.*";
        }

        public static void PrepareToOpenSPRFile(OpenFileDialog openFileDialog)
        {
            openFileDialog.Title = "Select Alien Breed Sprite File to Open...";
            openFileDialog.Filter = "Alien Breed Sprite File (*.SPR)|*.SPR|All Files (*.*)|*.*";
        }

        public static void PrepareToOpenEPFArchive(OpenFileDialog openFileDialog)
        {
            openFileDialog.Title = "Select EPF Archive to Open...";
            openFileDialog.Filter = "East Point Software File (*.EPF)|*.EPF|All Files (*.*)|*.*";
        }
    }
}
