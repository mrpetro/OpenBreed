using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorToolVM : BaseViewModel
    {
        //public virtual void DrawSelection(Graphics gfx)
        //{
        //    Pen selectedPen = new Pen(Color.LightGreen);
        //    Pen selectPen = new Pen(Color.LightBlue);
        //    Pen deselectPen = new Pen(Color.Red);

        //    for (int index = 0; index < SelectedIndexes.Count; index++)
        //    {
        //        Rectangle rectangle = CurrentTileSet.Items[SelectedIndexes[index]].Rectangle;
        //        gfx.DrawRectangle(selectedPen, rectangle);
        //    }

        //    if (SelectMode == SelectModeEnum.Select)
        //        gfx.DrawRectangle(selectPen, SelectionRectangle.GetRectangle(CurrentTileSet.TileSize));
        //    else if (SelectMode == SelectModeEnum.Deselect)
        //        gfx.DrawRectangle(deselectPen, SelectionRectangle.GetRectangle(CurrentTileSet.TileSize));
        //}
        internal virtual void OnCursor(MapViewCursorVM cursor)
        {
        }
    }
}
