using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.Attributes
{
    using Pivot.Accessories.Common;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple =false)]
    public class IsValues : Attribute
    {

    }
}
