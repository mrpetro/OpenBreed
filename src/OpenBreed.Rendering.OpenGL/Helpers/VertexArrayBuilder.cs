using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal abstract class VertexArrayBuilder
    {
        #region Protected Fields

        protected readonly List<Vertex> vertices = new List<Vertex>();

        #endregion Protected Fields

        #region Private Fields

        private readonly PrimitiveRenderer primitiveRenderer;

        private readonly List<int[]> loopIndices = new List<int[]>();

        #endregion Private Fields

        #region Protected Constructors

        protected VertexArrayBuilder(PrimitiveRenderer primitiveRenderer)
        {
            this.primitiveRenderer = primitiveRenderer;
        }

        #endregion Protected Constructors

        #region Public Methods

        public void AddTriangleIndices(int v1, int v2, int v3)
        {
            loopIndices.Add(new int[] { v1, v2, v3 });
        }

        public void AddLoopIndices(int v1, int v2, int v3, int v4)
        {
            loopIndices.Add(new int[] { v1, v2, v3, v4 });
        }

        public void AddLoopIndices(int[] vArray)
        {
            loopIndices.Add(vArray);
        }

        public void ClearLoopIndices()
        {
            loopIndices.Clear();
        }

        public int CreateTexturedVao() => primitiveRenderer.CreateTexturedVao(GetFloats());

        public int CreateVao() => primitiveRenderer.CreateVao(GetFloats());

        #endregion Public Methods

        #region Protected Methods

        protected abstract void Append(List<float> list, Vertex vertex);

        #endregion Protected Methods

        #region Private Methods

        private float[] GetFloats()
        {
            var floats = new List<float>();

            for (int j = 0; j < loopIndices.Count; j++)
            {
                var loop = loopIndices[j];

                for (int i = 0; i < loop.Length; i++)
                    Append(floats, vertices[loop[i]]);
            }

            return floats.ToArray();
        }

        #endregion Private Methods
    }

    internal class PosTexCoordArrayBuilder : VertexArrayBuilder, IPosTexCoordArrayBuilder
    {
        #region Public Constructors

        public PosTexCoordArrayBuilder(PrimitiveRenderer primitiveRenderer) : base(primitiveRenderer)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddVertex(Vector2 pos, Vector2 texCoord)
        {
            vertices.Add(new Vertex(pos, texCoord, Color4.White));
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Append(List<float> list, Vertex vertex)
        {
            list.Add(vertex.position.X);
            list.Add(vertex.position.Y);
            list.Add(0.0f);
            list.Add(vertex.texCoord.X);
            list.Add(vertex.texCoord.Y);
        }

        #endregion Protected Methods
    }

    internal class PosArrayBuilder : VertexArrayBuilder, IPosArrayBuilder
    {
        #region Public Constructors

        public PosArrayBuilder(PrimitiveRenderer primitiveRenderer) : base(primitiveRenderer)
        {
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

        #endregion Public Methods

        #region Protected Methods

        protected override void Append(List<float> list, Vertex vertex)
        {
            list.Add(vertex.position.X);
            list.Add(vertex.position.Y);
            list.Add(0.0f);
        }

        #endregion Protected Methods
    }
}