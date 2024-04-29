using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Dialog
{
    public interface IFolderBrowserQuery
    {
        string Description { get; set; }
        string SelectedPath { get; set; }
    }
}
