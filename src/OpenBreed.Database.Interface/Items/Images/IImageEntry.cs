using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Images
{
    public interface IImageEntry : IEntry
    {
        string DataRef { get; set; }
    }
}
