using OpenBreed.Rendering.Abstractions;
using System;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class RenderContextItem<TData>
    {
        private readonly Dictionary<IRenderContext, TData> contextLookup = new Dictionary<IRenderContext, TData>();

        public TData Add(IRenderContext renderContext, TData data)
        {
            return contextLookup[renderContext] = data;
        }

        public TData Get(IRenderContext renderContext)
        {
            return contextLookup[renderContext];
        }

        public TData GetOrAdd(IRenderContext renderContext, Func<IRenderContext, TData> initializer)
        {
            if (contextLookup.TryGetValue(renderContext, out TData data))
            {
                return data;
            }

            data = initializer.Invoke(renderContext);
            Add(renderContext, data);
            return data;
        }
    }
}