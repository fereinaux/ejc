using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Utils.Services
{
    public class DatatableService : IDatatableService
    {
        // Reflection used to identify DateTime fields for formatting
        private readonly Type _typeNullDatetime = typeof(DateTime?);
        private readonly Type _typeDateTime = typeof(DateTime);


        public Func<T, T> CreateNewStatement<T>(string fields)
        {
            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(T), "o");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(T));

            // create initializers
            var bindings = fields.Split(',').Select(o => o.Trim())
                .Select(o => {

            // property "Field1"
            var mi = typeof(T).GetProperty(o);

            // original value "o.Field1"
            var xOriginal = Expression.Property(xParameter, mi);

            // set value "Field1 = o.Field1"
            return Expression.Bind(mi, xOriginal);
                }
            );

            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, T>>(xInit, xParameter);

            // compile to Func<Data, Data>
            return lambda.Compile();
        }

        public MemoryStream GenerateExcel<T>(IEnumerable<T> source, string fields = "")
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Planilha");


               
                if (source.Count() > 0)
                {

                    //Do NOT include Col1
                    var mi = typeof(T)
                        .GetProperties()
                        .Where(pi => pi.Name != "Id" && (string.IsNullOrEmpty(fields) || fields.Split(',').ToList().Contains(pi.Name)))
                        .Select(pi => (MemberInfo)pi)
                        .ToArray();

                    ws.Cells.LoadFromCollection(source
                                                , true,
                                                OfficeOpenXml.Table.TableStyles.Light1
                                                , BindingFlags.Public | BindingFlags.Instance
                                                , mi);
                }

                PropertyInfo[] _modelProperties = typeof(T).GetProperties().Where(pi => pi.Name != "Id" && (string.IsNullOrEmpty(fields) || fields.Split(',').ToList().Contains(pi.Name))).ToArray();

                FormatDateTimeAsDate(ws, _modelProperties);

                MemoryStream result = new MemoryStream();
                pck.SaveAs(result);
                result.Position = 0;
                return result;
            }
        }

        private void FormatDateTimeAsDate(ExcelWorksheet worksheet, PropertyInfo[] _modelProperties)
        {
            for (var j = 0; j < _modelProperties.Length; j++)
            {
                if (_modelProperties[j].PropertyType == _typeNullDatetime
                        && DateTimeFormatInfo.CurrentInfo != null)
                    worksheet.Column(j + 1).Style.Numberformat.Format =
                        DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

                if (_modelProperties[j].PropertyType != _typeDateTime) continue;
                if (DateTimeFormatInfo.CurrentInfo != null)
                    worksheet.Column(j + 1).Style.Numberformat.Format =
                        DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            }
        }

    }
}