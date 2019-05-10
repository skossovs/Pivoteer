using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.Common
{
    public static class Clonning
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : new()
        {
            return listToClone.Select(item => { return (T)DeepClone(item); }).ToList();
        }

        public static object DeepClone(object obj)
        {
            object objResult = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult;
        }

    }
}
