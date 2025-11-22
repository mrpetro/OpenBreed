using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Extensions
{
    public static class AssemblyExtensions
    {
        #region Public Methods

        public static IEnumerable<Type> GetInterfaces<TInterface>(this Assembly assembly)
        {
            foreach (var type in assembly
                .DefinedTypes.Where(type => !type.IsAbstract && !type.IsInterface)
                .Where(type => type.ImplementedInterfaces.Any(item => item == typeof(TInterface))))
            {
                yield return type;
            }
        }

        #endregion Public Methods
    }
}