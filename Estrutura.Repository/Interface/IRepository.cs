using Dapper;
using Estrutura.Repository.Common;
using System;
using System.Collections.Generic;

namespace Estrutura.Repository.Interface
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Get list of entity T using one filter.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<T> FindByFilters(String tableName, FilterClause filter);

        /// <summary>
        /// Get list of entity T using multiple filters.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        IEnumerable<T> FindByFilters(String tableName, List<FilterClause> filters);

        /// <summary>
        /// Build a where statement using a list of filters.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        String BuildWhereStatement(List<FilterClause> filters, DynamicParameters paramters);
    }
}
