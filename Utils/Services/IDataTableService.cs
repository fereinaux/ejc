using System;
using System.Collections.Generic;
using System.IO;

namespace Utils.Services
{
    public interface IDatatableService
    {
        MemoryStream GenerateExcel<T>(IEnumerable<T> source, string fields = "");
        Func<T, T> CreateNewStatement<T>(string fields);
    }
}