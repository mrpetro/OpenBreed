using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items.DataSources;
using System;

namespace OpenBreed.Database.Xml.Items.DataSources
{
    [Serializable]
    public abstract class XmlDbDataSource : XmlDbEntry, IDbDataSource
    {
    }
}