using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public interface ISaveFileQuery
    {
        string Title { get; set; }
        string Filter { get; set; }
        string FileName { get; set; }
        string[] FileNames { get; }
        string InitialDirectory { get; set; }
    }
}
