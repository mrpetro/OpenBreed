//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using OpenBreed.Editor.VM.Tiles;

//namespace OpenBreed.Editor.VM.Tiles.Helpers
//{
//    public enum SelectModeEnum
//    {
//        Nothing,
//        Select,
//        Deselect
//    }

//    public class TilesSelector
//    {

//        private SelectionRectangle m_SelectionRectangle = null;
//        private Point m_MinCoord;
//        private Point m_MaxCoord;
//        private Point m_CenterCoord;
//        private List<int> m_Selected = null;
//        private SelectModeEnum m_SelectMode;

//        public SelectionRectangle SelectionRectangle { get { return m_SelectionRectangle; } }
//        public Point MinCoord { get { return m_MinCoord; } }
//        public Point MaxCoord { get { return m_MaxCoord; } }
//        public Point CenterCoord { get { return m_CenterCoord; } }
//        public bool MultiSelect { get; set; }
//        public List<int> Selected { get { return m_Selected; } }
//        public SelectModeEnum Mode { get { return m_SelectMode; } }
//        public TileSetVM VM { get; private set; }
//        public bool IsEmpty { get { return m_Selected.Count == 0; } }

//        public TilesSelector(TileSetVM vm)
//        {
//            VM = vm;

//            m_Selected = new List<int>();
//            m_SelectionRectangle = new SelectionRectangle();
//            m_SelectMode = SelectModeEnum.Nothing;
//            MultiSelect = false;
//        }

//        internal Point GetIndexCoords(Point point)
//        {
//            return VM.GetIndexCoords(point);
//        }

//        public void AddSelection(List<int> tileIdList)
//        {
//            foreach (int tileId in tileIdList)
//                AddSelection(tileId);
//        }

//        public void AddSelection(int tileId)
//        {
//            if (!m_Selected.Contains(tileId))
//                m_Selected.Add(tileId);
//        }

//        public void RemoveSelection(List<int> tileIdList)
//        {
//            foreach (int tileId in tileIdList)
//                RemoveSelection(tileId);
//        }

//        public void RemoveSelection(int tileId)
//        {
//            m_Selected.Remove(tileId);
//        }

//        public void ClearSelection()
//        {
//            m_Selected.Clear();
//        }

//        private void CalculateSelectionCenter()
//        {
//            Rectangle rectangle = VM.Items[m_Selected[0]].Rectangle;

//            m_MinCoord.X = rectangle.Left;
//            m_MaxCoord.X = rectangle.Right;
//            m_MinCoord.Y = rectangle.Bottom;
//            m_MaxCoord.Y = rectangle.Top;

//            for (int i = 1; i < m_Selected.Count; i++)
//            {
//                rectangle = VM.Items[m_Selected[i]].Rectangle;

//                m_MinCoord.X = Math.Min(m_MinCoord.X, rectangle.Left);
//                m_MaxCoord.X = Math.Max(m_MaxCoord.X, rectangle.Right);
//                m_MinCoord.Y = Math.Min(m_MinCoord.Y, rectangle.Bottom);
//                m_MaxCoord.Y = Math.Max(m_MaxCoord.Y, rectangle.Top);
//            }

//            m_CenterCoord = VM.GetSnapCoords(new Point((m_MinCoord.X + m_MaxCoord.X) / 2, (m_MinCoord.Y + m_MaxCoord.Y) / 2));
//        }

//        public void DrawSelection(Graphics gfx)
//        {
//            Pen selectedPen = new Pen(Color.LightGreen);
//            Pen selectPen = new Pen(Color.LightBlue);
//            Pen deselectPen = new Pen(Color.Red);

//            for (int index = 0; index < Selected.Count; index++)
//            {
//                Rectangle rectangle = VM.Items[m_Selected[index]].Rectangle;
//                gfx.DrawRectangle(selectedPen, rectangle);
//            }

//            if (Mode == SelectModeEnum.Select)
//                gfx.DrawRectangle(selectPen, SelectionRectangle.GetRectangle(VM.TileSize));
//            else if (Mode == SelectModeEnum.Deselect)
//                gfx.DrawRectangle(deselectPen, SelectionRectangle.GetRectangle(VM.TileSize));
//        }

//        public void StartSelection(SelectModeEnum selectMode, Point point)
//        {
//            m_SelectMode = selectMode;
//            SelectionRectangle.SetStart(GetIndexCoords(point));

//            if (!MultiSelect && selectMode == SelectModeEnum.Select)
//                ClearSelection();
//        }

//        public void UpdateSelection(Point point)
//        {
//            SelectionRectangle.Update(GetIndexCoords(point));
//        }

//        public void FinishSelection(Point point)
//        {
//            SelectionRectangle.SetFinish(GetIndexCoords(point));

//            Rectangle selectionRectangle = SelectionRectangle.GetRectangle(VM.TileSize);
//            List<int> selectionList = VM.GetTileIdList(selectionRectangle);

//            if (Mode == SelectModeEnum.Select)
//            {
//                if (!MultiSelect)
//                    ClearSelection();

//                AddSelection(selectionList);
//            }
//            else if (Mode == SelectModeEnum.Deselect)
//            {
//                RemoveSelection(selectionList);
//            }

//            if (!IsEmpty)
//                CalculateSelectionCenter();

//            m_SelectMode = SelectModeEnum.Nothing;
//        }
//    }
//}
