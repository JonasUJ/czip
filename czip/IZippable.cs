using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czip
{
    public interface IZippable
    {
        string Name { get; set; }
        SerializedData Serialize();
    }
}
