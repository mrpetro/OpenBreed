using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class PosAndTexCoordArrayBuilder : IPosTexCoordArrayBuilder
    {
        #region Private Fields

        private readonly List<Vertex> vertices = new List<Vertex>();

        private readonly PrimitiveRenderer primitiveRenderer;

        private readonly List<int[]> loopIndices = new List<int[]>();

        #endregion Private Fields

        #region Public Constructors

        public PosAndTexCoordArrayBuilder(PrimitiveRenderer primitiveRenderer)
        {
            this.primitiveRenderer = primitiveRenderer;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddVertex(Vector2 pos, Vector2 texCoord)
        {
            vertices.Add(new Vertex(pos, texCoord, Color4.White));
        }

        public void AddTriangleIndices(int v1, int v2, int v3)
        {
            loopIndices.Add(new int[] { v1, v2, v3 });
        }

        public void AddLoopIndices(int v1, int v2, int v3, int v4)
        {
            loopIndices.Add(new int[] { v1, v2, v3, v4 });
        }

        public int CreateTexturedVao() => primitiveRenderer.CreateTexturedVao(GetFloats());

        public int CreateVao() => primitiveRenderer.CreateVao(GetFloats());

        #endregion Public Methods

        #region Private Methods

        private static void Append(List<float> list, Vertex vertex)
        {
            list.Add(vertex.position.X);
            list.Add(vertex.position.Y);
            list.Add(0.0f);
            list.Add(vertex.texCoord.X);
            list.Add(vertex.texCoord.Y);
        }

        private float[] GetFloats()
        {
            var floats = new List<float>();

            foreach (var loop in loopIndices)
            {
                for (int i = 0; i < loop.Length; i++)
                    Append(floats, vertices[loop[i]]);
            }

            return floats.ToArray();
        }

        #endregion Private Methods
    }


    internal class VertexArrayBuilder : IPosArrayBuilder
    {
        #region Private Fields

        private readonly List<Vertex> vertices = new List<Vertex>();

        private readonly PrimitiveRenderer primitiveRenderer;

        private readonly List<int[]> loopIndices = new List<int[]>();

        #endregion Private Fields

        #region Public Constructors

        public VertexArrayBuilder(PrimitiveRenderer primitiveRenderer)
        {
            this.primitiveRenderer = primitiveRenderer;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddVertex(Vector2 pos)
        {
            vertices.Add(new Vertex(pos.X, pos.Y));
        }

        public void AddVertex(float x, float y, float z)
        {
            vertices.Add(new Vertex(x, y));
        }

        public void AddTriangleIndices(int v1, int v2, int v3)
        {
            loopIndices.Add(new int[] { v1, v2, v3 });
        }

        public void AddLoopIndices(int v1, int v2, int v3, int v4)
        {
            loopIndices.Add(new int[] { v1, v2, v3, v4 });
        }

        public int CreateTexturedVao() => primitiveRenderer.CreateTexturedVao(GetFloats());

        public int CreateVao() => primitiveRenderer.CreateVao(GetFloats());

        #endregion Public Methods

        #region Private Methods

        private static void Append(List<float> list, Vertex vertex)
        {
            list.Add(vertex.position.X);
            list.Add(vertex.position.Y);
            list.Add(0.0f);
        }

        private float[] GetFloats()
        {
            var floats = new List<float>();

            foreach (var loop in loopIndices)
            {
                for (int i = 0; i < loop.Length; i++)
                    Append(floats, vertices[loop[i]]);
            }

            return floats.ToArray();
        }

        #endregion Private Methods
    }
}