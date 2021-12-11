using OpenBreed.Database.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Database.Xml.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupXmlDatabase(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<XmlDatabaseMan>(() => new XmlDatabaseMan(managerCollection.GetManager<IVariableMan>()));
        }

        public static void SetupXmlReadonlyDatabase(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<XmlReadonlyDatabaseMan>(() => new XmlReadonlyDatabaseMan(managerCollection.GetManager<IVariableMan>()));
        }
    }
}
