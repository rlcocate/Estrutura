using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Repository.Common
{
    public sealed class FilterOperator
    {
        private readonly String _sqlOperator;

        public static readonly FilterOperator Equal = new FilterOperator("=");
        public static readonly FilterOperator Grater = new FilterOperator(">");

        private FilterOperator(String sqlOperator)
        {
            this._sqlOperator = sqlOperator;
        }

        public override String ToString()
        {
            return _sqlOperator;
        }
    }
}
