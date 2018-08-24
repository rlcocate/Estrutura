using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Estrutura.Repository
{
    public abstract class SqlBaseRepository<T>: Configuration
    {
        public virtual IEnumerable<T> GetAll()
        {
            using (IDbConnection db = new SqlConnection(this.connectionString))
            {
                var query = "SELECT Id, Name FROM States";
                var q = db.Query<T>(query, commandType: CommandType.Text);
                return q;
            }
        }

        public T GetByName(string name)
        {
            using (IDbConnection db = new SqlConnection(this.connectionString))
            {
                var query = "SELECT Id, Name FROM States WHERE Name = @name";
                var q = db.Query<T>(query, new { name = name }, commandType: CommandType.Text);
                return (T)q.AsList()[0];
            }
        }

        public void Insert(T entity)
        {
            dynamic obj = entity;

            using (IDbConnection db = new SqlConnection(this.connectionString))
            {
                var query = "INSERT INTO States VALUES (@Name)";
                var q = db.Execute(query, new { name = obj.Name }, commandType: CommandType.Text);
            }
        }

        //public void Edit(States states)
        //{

        //}

        //public void Delete(int id)
        //{

        //}
    }
}
