using Dapper;
using Estrutura.Repository.Common;
using Estrutura.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Estrutura.Implementation.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        #region Attributes

        private readonly string _databaseConfigName = "OneSTConnectionString";

        private const string CREATE_FOREIGN_KEY_SQL_LAYOUT = @"IF NOT EXISTS (SELECT *
                                                                                 FROM sys.foreign_keys 
                                                                                WHERE object_id = OBJECT_ID('{1}')
                                                                                  AND parent_object_id = OBJECT_ID('{0}')
                                                                              )
                                                             ALTER TABLE [{0}] ADD CONSTRAINT [{1}] FOREIGN KEY([{2}]) REFERENCES [{3}] ([{4}])";
        private const string DROP_FOREIGN_KEY_SQL_LAYOUT = @"IF EXISTS (SELECT *
                                                                          FROM sys.foreign_keys 
                                                                         WHERE object_id = OBJECT_ID('{0}')
                                                                           AND parent_object_id = OBJECT_ID('{1}')
                                                                       )
                                                             ALTER TABLE [{1}] DROP CONSTRAINT [{0}]";
        #endregion

        #region Constructors

        public Repository() { }

        public Repository(string databaseConfigName)
        {
            _databaseConfigName = databaseConfigName;
        }

        #endregion

        #region Private Members

        private string BuildParam(string paramName)
        {
            return "@" + paramName;
        }

        private IDbConnection GetSqlConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        /// <summary>
        /// Build a SQL filter statement (e.g.: ColumA = param0).
        /// </summary>
        /// <param name="sqlFilterOperator"></param>
        /// <param name="filter"></param>
        /// <param name="index"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        private string BuildParam(string sqlFilterOperator, FilterClause filter, int index, DynamicParameters paramters)
        {
            string parameterName = string.Empty;
            string filterClause = string.Empty;

            parameterName = "@param" + index.ToString();
            filterClause = sqlFilterOperator + " " + filter.BuildFilterClause + " " + parameterName;
            paramters.Add(parameterName, filter.ColumnValue);

            return filterClause;
        }

        #endregion

        #region Public Members

        public int ExecuteCommand(Func<IDbConnection, int> command, bool readUncommitted = false)
        {
            int id = 0;

            using (IDbConnection connection = GetSqlConnection())
            {
                connection.Open();

                if (readUncommitted)
                {
                    SqlCommand cmd = new SqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;", connection as SqlConnection);
                    cmd.ExecuteNonQuery();
                }

                id = command(connection);
            }

            return id;
        }

        public K ExecuteCommand<K>(Func<IDbConnection, K> command, bool readUncommitted = false)
        {
            K entity;

            using (IDbConnection connection = GetSqlConnection())
            {
                connection.Open();

                if (readUncommitted)
                {
                    SqlCommand cmd = new SqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;", connection as SqlConnection);
                    cmd.ExecuteNonQuery();
                }

                entity = command(connection);
            }

            return entity;
        }

        public T ExecuteCommand(Func<IDbConnection, T> command, bool readUncommitted = false)
        {
            T entity;

            using (IDbConnection connection = GetSqlConnection())
            {
                connection.Open();

                if (readUncommitted)
                {
                    SqlCommand cmd = new SqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;", connection as SqlConnection);
                    cmd.ExecuteNonQuery();
                }

                entity = command(connection);
            }

            return entity;
        }

        public string BuildWhereStatement(List<FilterClause> filters, DynamicParameters paramters)
        {
            StringBuilder whereStatement = new StringBuilder();
            int index = 0;

            whereStatement.AppendLine(BuildParam("WHERE", filters[index], index, paramters));

            for (index = 1; index < filters.Count; index++)
            {
                whereStatement.AppendLine(BuildParam("AND", filters[index], index, paramters));
            }

            return whereStatement.ToString();
        }

        public virtual IEnumerable<T> FindByFilters(string tableName, FilterClause filter)
        {
            return FindByFilters(tableName, new List<FilterClause>() { filter });
        }

        public virtual IEnumerable<T> FindByFilters(string tableName, List<FilterClause> filters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            string query = $"SELECT * FROM {tableName} " + BuildWhereStatement(filters, queryParameters);

            Func<IDbConnection, IEnumerable<T>> command = new Func<IDbConnection, IEnumerable<T>>(connection =>
            {
                return connection.Query<T>(query, queryParameters);
            });

            return this.ExecuteCommand(command).ToList();
        }

        #endregion

        #region Protected Members

        protected string GetConnectionStringName()
        {
            return this._databaseConfigName;
        }

        protected string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[_databaseConfigName].ConnectionString;
        }

        protected string ReplaceQueryParameters(string query, DynamicParameters parameters)
        {
            foreach (var param in parameters.ParameterNames)
            {
                var value = ((SqlMapper.IParameterLookup)parameters)[param];
                query = query.Replace(BuildParam(param), value.ToString());
            }

            return query;
        }

        protected IEnumerable<T> ExecuteCommand(Func<IDbConnection, IEnumerable<T>> command, bool readUncommitted = false)
        {
            IEnumerable<T> entities;

            using (IDbConnection connection = GetSqlConnection())
            {
                connection.Open();

                if (readUncommitted)
                {
                    SqlCommand cmd = new SqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;", connection as SqlConnection);
                    cmd.ExecuteNonQuery();
                }

                entities = command(connection);
            }

            return entities;
        }

        protected void DisableAllConstraints(string table)
        {
            string sql = "ALTER TABLE " + table + " NOCHECK CONSTRAINT ALL";

            Func<IDbConnection, int> command = new Func<IDbConnection, int>(connection =>
            {
                return connection.Execute(sql);
            });

            this.ExecuteCommand(command);
        }

        protected void EnableAllConstraints(string table)
        {
            string sql = "ALTER TABLE " + table + " WITH CHECK CHECK CONSTRAINT ALL;";

            Func<IDbConnection, int> command = new Func<IDbConnection, int>(connection =>
            {
                return connection.Execute(sql);
            });

            this.ExecuteCommand(command);
        }

        protected void DropForeignKeyIfExists(string table, string constraintName)
        {
            string sql = String.Format(DROP_FOREIGN_KEY_SQL_LAYOUT, constraintName, table);

            Func<IDbConnection, int> command = new Func<IDbConnection, int>(connection =>
            {
                return connection.Execute(sql);
            });

            this.ExecuteCommand(command);
        }

        protected void DropConstraint(string table, string constraintName)
        {
            string sql = "ALTER TABLE " + table + " DROP CONSTRAINT " + constraintName;

            Func<IDbConnection, int> command = new Func<IDbConnection, int>(connection =>
            {
                return connection.Execute(sql);
            });

            this.ExecuteCommand(command);
        }

        protected void CreateForeignKeyConstraintIfExists(string table, string foreignKeyName, string foreignKeyColumn, string referenceTable, string referenceColumn)
        {
            string sql = String.Format(CREATE_FOREIGN_KEY_SQL_LAYOUT, table, foreignKeyName, foreignKeyColumn, referenceTable, referenceColumn);

            Func<IDbConnection, int> command = new Func<IDbConnection, int>(connection =>
            {
                return connection.Execute(sql);
            });

            this.ExecuteCommand(command);
        }

        #endregion
    }
}
