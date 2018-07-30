using Surgicalogic.Common.CustomAttributes;
using Surgicalogic.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Query
{
    public static class DependentDataService
    {
        internal static async Task<bool> CheckDependentAttributes(DataContext _context, Type type, int id)
        {
            var result = false;
            var properties = type.GetProperties();
            foreach (var item in properties)
            {
                if (item.GetCustomAttributes(typeof(DependentAttribute), false).Any())
                {
                    var attr = item.GetCustomAttribute<DependentAttribute>();
                    var table = _context.GetType().GetProperty(item.Name).GetValue(_context) as IQueryable;
                    var query = await table.Where(attr.ForeignKey + "==" + id + " and IsActive").ToDynamicListAsync();

                    return query.Count > 0;
                }
            }

            return result;
        }
    }
}
