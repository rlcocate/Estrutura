using Dapper;
using Estrutura.Repository.Common;
using System.Collections.Generic;

namespace Estrutura.Repository.Interface
{
    public interface IRepositoryModel<T>
    {
        /// <summary>
        /// Gets a list of an entity of T type using one filter.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<T> FindByFilters(string tableName, FilterClause filter);

        /// <summary>
        /// Gets a list of an entity of T type using multiples filters.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        IEnumerable<T> FindByFilters(string tableName, List<FilterClause> filters);

        /// <summary>
        /// Build a where statement using a list of filters.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        string BuildWhereStatement(List<FilterClause> filters, DynamicParameters parameters);
    }
}
