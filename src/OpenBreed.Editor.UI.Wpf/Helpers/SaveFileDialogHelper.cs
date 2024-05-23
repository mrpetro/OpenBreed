using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Editor.UI.Wpf.Helpers
{
    public class SaveFileDialogHelper
    {
        public static void PrepareToSaveMAPFile(SaveFileDialog saveFileDialog, string initFileName = null)
        {
            saveFileDialog.Title = "Input Alien Breed Map File to Save...";
            saveFileDialog.Filter = "Alien Breed Map Files (MAP.*)|MAP.*|All Files (*.*)|*.*";
            saveFileDialog.FileName = initFileName;
            saveFileDialog.CheckFileExists = true;
        }

        public static void PrepareToSaveBLKFile(SaveFileDialog saveFileDialog)
        {
            saveFileDialog.Title = "Input Alien Breed Tile File to Save...";
            saveFileDialog.Filter = "Alien Breed Tile File (*.BLK)|*.BLK|All Files (*.*)|*.*";
        }

        public static void PrepareToSaveSPRFile(SaveFileDialog saveFileDialog)
        {
            saveFileDialog.Title = "Input Alien Breed Sprite File to Save...";
            saveFileDialog.Filter = "Alien Breed Sprite File (*.SPR)|*.SPR|All Files (*.*)|*.*";
        }

        public static void PrepareToSaveEPFArchive(SaveFileDialog saveFileDialog)
        {
            saveFileDialog.Title = "Input EPF Archive to Save...";
            saveFileDialog.Filter = "East Point Software File (*.EPF)|*.EPF|All Files (*.*)|*.*";
        }
    }
}
