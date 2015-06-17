using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using Newtonsoft.Json;

namespace InfrastructureLayer.Component.Grid
{

    public static class GridExtensions
    {
        public static GridResult<TSource> ToGrid<TSource>(this IQueryable<TSource> source)
        {
            try
            {
                var result = new GridResult<TSource>();
                var request = HttpContext.Current.Request;

                result.total = source.Count();

                var pLimit = request.QueryString["limit"];
                var pOffset = request.QueryString["offset"];
                var sort = request.QueryString["sort"];
                var order = request.QueryString["order"];
                var search = request.QueryString["search"];
                var filter = request.QueryString["filter"];

                int offset, limit;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    var dicData = JsonConvert.DeserializeObject<Dictionary<string,string>>(filter);
                    var properties = source.ElementType.GetProperties().ToArray();

                    var conditions = new List<string>();
                    var values = new List<object>();
                    var iPos = 0;
                    foreach (var keyVal in dicData)
                    {
                        var property = properties.FirstOrDefault(e => e.Name == keyVal.Key);
                        if (property != null)
                        {
                            conditions.Add(String.Format("{0}.ToString().Contains(@{1})", keyVal.Key, iPos++));
                            values.Add(keyVal.Value);
                        }
                    }

                    if(conditions.Count > 0)
                        source = source.Where(String.Join(" and ", conditions), values.ToArray());

                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var properties = source.ElementType.GetProperties().ToArray();

                    if (properties.Any())
                    {
                        var condition = string.Join(" or ", properties.Select(p => string.Format("{0}.ToString().Contains(@0)", p.Name)).ToArray());
                        source = source.Where(condition, search);
                    }
                }

                if (!string.IsNullOrWhiteSpace(sort))
                    source = source.OrderBy(sort + (string.IsNullOrWhiteSpace(order) ? string.Empty : " " + order));

                if (int.TryParse(pOffset, out offset) && int.TryParse(pLimit, out limit))
                {
                    source = source.Skip(offset).Take(limit);
                }

                result.rows = source.ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
