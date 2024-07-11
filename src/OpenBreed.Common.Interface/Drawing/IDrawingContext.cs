using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public interface IDrawingContext : IDisposable
    {
        #region Public Properties

        IMatrix Transform { get; set; }

        #endregion Public Properties

        #region Public Methods

        void Clear(MyColor color);

        void DrawImage(IImage bitmap, int x, int y, MyRectangle srcRect);

        void DrawImage(IImage bitmap, float x, float y, MyRectangle srcRect);

        void DrawImage(IImage bitmap, float x, float y, float width, float height);

        void DrawRectangle(IPen pen, MyRectangle rectangle);

        void FillRectangle(IBrush brush, MyRectangle rectangle);

        void DrawString(string text, IFont font, IBrush brush, float x, float y);

        void Flush();

        void Setup();

        #endregion Public Methods
    }
}