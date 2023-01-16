using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Input.Interface
{
    public interface IActionBinder
    {
        void Bind<TKey, TAction>(TAction action, TKey key) where TAction : Enum where TKey : Enum;
    }
}
