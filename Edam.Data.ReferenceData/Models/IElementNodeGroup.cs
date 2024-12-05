using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.DataObjects.Models;

public interface IElementNodeGroup
{
    Task<MapInfo> GetMapAsync(ElementNodeInfo node, string name);
}
