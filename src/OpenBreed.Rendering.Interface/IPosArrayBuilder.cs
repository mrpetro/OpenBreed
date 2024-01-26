using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface
{
    public interface IPosTexCoordArrayBuilder
    {
        void AddVertex(Vector2 pos, Vector2 texCoord);
        void AddTriangleIndices(int v1, int v2, int v3);
        void AddLoopIndices(int v1, int v2, int v3, int v4);

        int CreateTexturedVao();
        int CreateVao();
    }

    public interface IPosArrayBuilder
    {
        void AddVertex(Vector2 pos);
        void AddVertex(float x, float y, float z);
        void AddTriangleIndices(int v1, int v2, int v3);
        void AddLoopIndices(int v1, int v2, int v3, int v4);
        void AddLoopIndices(int[] vArray);
        void ClearLoopIndices();

        int CreateTexturedVao();
        int CreateVao();
    }
}
