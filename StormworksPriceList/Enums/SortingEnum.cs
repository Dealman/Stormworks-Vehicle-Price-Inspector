using System;
using System.Collections.Generic;
using System.Text;

namespace StormworksPriceInspector.Enums
{
    public static class SortingEnum
    {
        public enum SortDirection
        {
            Ascending,
            Descending
        }

        public enum SortOption
        {
            Name,
            Cost,
            Date
        }
    }
}
