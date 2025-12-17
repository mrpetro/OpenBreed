using OpenBreed.Wecs.Abstractions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game
{
    internal class LuaEntityEventHandler<TEvent> : NLua.Method.LuaDelegate
    {
        void CallFunction(IEntity entity, TEvent eventArgs)
        {
            object[] args = new object[] { entity, eventArgs };
            object[] inArgs = new object[] { entity, eventArgs };
            int[] outArgs = new int[] { };
            base.CallFunction(args, inArgs, outArgs);
        }
    }

    internal class LuaEventHandler<TEvent> : NLua.Method.LuaDelegate
    {
        void CallFunction(TEvent eventArgs)
        {
            object[] args = new object[] { eventArgs };
            object[] inArgs = new object[] { eventArgs };
            int[] outArgs = new int[] { };
            base.CallFunction(args, inArgs, outArgs);
        }
    }
}
