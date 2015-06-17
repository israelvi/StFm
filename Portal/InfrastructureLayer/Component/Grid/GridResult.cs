using System.Collections.Generic;

namespace InfrastructureLayer.Component.Grid
{
    public class GridResult<T>
    {
        public long total { get; set; }
        public List<T> rows { get; set; }

    }
}
