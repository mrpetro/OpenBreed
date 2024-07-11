using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Dialog
{
    public interface IOpenFileQuery
    {
        string InitialDirectory { get; set; }
        string Title { get; set; }
        string Filter { get; set; }
        bool Multiselect { get; set; }
        string[] FileNames { get; }
        string FileName { get; set; }

        DialogAnswer Show();
    }
}
