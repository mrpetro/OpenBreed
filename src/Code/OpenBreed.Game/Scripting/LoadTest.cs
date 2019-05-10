using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Scripting
{
    public class LoadTest
    {
        public void Example()
        {
            using (Lua lua = new Lua())
            {
                lua.DoString("luanet.load_assembly('mscorlib')");
                lua.DoString("luanet.load_assembly('NLuaTest', 'NLuaTest.TestTypes')");
                lua.DoString("TestClass = luanet.import_type('NLuaTest.TestTypes.TestClass')");
                lua.DoString("test = TestClass()");
                lua.DoString("err,errMsg = pcall(test.exceptionMethod,test)");
                bool err = (bool)lua["err"];

                var errMsg = (Exception)lua["errMsg"];
                //Assert.AreEqual(false, err);
                //Assert.AreNotEqual(null, errMsg.InnerException);
                //Assert.AreEqual("exception test", errMsg.InnerException.Message);
            }
        }
    }
}
