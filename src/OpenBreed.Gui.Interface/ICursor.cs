using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface
{
    public enum CursorKey
    {
        Right,
        Middle,
        Left
    }

    /// <summary>
    /// Cursor that is a tool for interacting with elements.
    /// </summary>
    public interface ICursor
    {
        float X { get; }
        float Y { get; }


    }
}
