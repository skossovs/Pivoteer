using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.Data
{
    class SheetData
    {
        public string SheetName;
        public string ClassName;
    }
    class ExcelFileData
    {
        public string          ExcelFileFullPath;
        public List<SheetData> Sheets;
    }

    // TODO: rename this project representation
    class Project
    {
        public string              ProjectDescription;
        public List<ExcelFileData> ExcelFiles;
    }
}
