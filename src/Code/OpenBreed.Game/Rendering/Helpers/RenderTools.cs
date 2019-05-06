using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Game.Physics.Components;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenBreed.Game.Rendering.Helpers
{
    public static class RenderTools
    {

        public static void CreateIndicesArray(uint[] indices, out int ibo)
        {
            ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

#if DEBUG
            int bufferSize;
            //Validate that the buffer is the correct size
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
            if (sizeof(uint) * indices.Length != bufferSize)
                throw new ApplicationException("Indices array not uploaded correctly");
#endif
            // Clear the buffer Binding
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public static void CreateVertexArray(Vertex[] vertices, out int vbo)
        {
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, Vertex.SizeInBytes * vertices.Length, vertices, BufferUsageHint.StaticDraw);

#if DEBUG
            int bufferSize;
            //Validate that the buffer is the correct size
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
            if (Vertex.SizeInBytes * vertices.Length != bufferSize)
                throw new ApplicationException("Vertex array not uploaded correctly");
#endif
            // Clear the buffer Binding
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public static void DrawRectangle(float left, float bottom, float right, float top)
        {
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(Color4.Blue);
            GL.Vertex3(left, top, 0.0f);
            GL.Vertex3(left, bottom, 0.0f);
            GL.Vertex3(right, bottom, 0.0f);
            GL.Vertex3(right, top, 0.0f);
            GL.End();
        }

        public static void DrawBox(Box2 box, Color4 color)
        {
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(color);
            GL.Vertex3(box.Left, box.Top, 0.0f);
            GL.Vertex3(box.Left, box.Bottom, 0.0f);
            GL.Vertex3(box.Right, box.Bottom, 0.0f);
            GL.Vertex3(box.Right, box.Top, 0.0f);
            GL.End();
        }

        public static void Create(Vertex[] vertices, uint[] indices, out int vbo, out int ibo)
        {
            vbo = GL.GenBuffer();
            ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, Vertex.SizeInBytes * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

#if DEBUG
            int bufferSize;
            //Validate that the buffer is the correct size
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
            if (Vertex.SizeInBytes * vertices.Length != bufferSize)
                throw new ApplicationException("Vertex array not uploaded correctly");
#endif
            // Clear the buffer Binding
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public static void Delete(int vbo, int ibo)
        {
            GL.DeleteBuffer(ibo);
            GL.DeleteBuffer(vbo);
        }

        public static void Draw(Viewport viewport, int vbo, int ibo, int indicesCount)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.IndexArray);

            GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, 0);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes);
            GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes * 2);

            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);

            GL.DisableClientState(ArrayCap.IndexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.VertexArray);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

    }
}
