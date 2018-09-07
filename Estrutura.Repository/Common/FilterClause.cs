using System;

namespace Estrutura.Repository.Common
{
    /// <summary>
    /// Sql clause used to filter data.
    /// </summary>
    public class FilterClause
    {
        public String ColumnName { get; set; }

        public String ColumnValue { get; set; }

        public FilterOperator Operator { get; set; }

        public string BuildFilterClause
        {
            get
            {
                return ColumnName + " " + Operator.ToString();
            }
        }
    }
}
