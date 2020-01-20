using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    static class ProjectExtensions
    {
        public static Group ExcelFileDataToGroup(this Data.ExcelFileData excelFileData, string parentPath)
        {
            var group = new Group() {
                Key = (parentPath + (char)0x00 + excelFileData.ExcelFileFullPath).GetHashCode(),
                Path = parentPath + (char)0x00 + excelFileData.ExcelFileFullPath,
                Name = excelFileData.ExcelFileFullPath,
                SubGroups = new List<Group>(),
                Entries = new List<Entry>()
            };

            return group;
        }

        public static Group ExcelSheetDataToGroup(this Data.SheetData excelSheetData)
        {
            var group = new Group() { Key = 0, Name = excelSheetData.SheetName };

            return group;
        }
    }
}
