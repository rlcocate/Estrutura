using Dapper;
using Estrutura.Implementation.Repository;
using Estrutura.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;

namespace Estrutura.Repository.Implementation
{
    public abstract class RepositoryModel<T> : Repository<T>, IRepositoryModel<T> where T : class
    {
        #region Attributes

        #endregion

        #region Constructor

        public RepositoryModel() : base() { }

        public RepositoryModel(string databaseConfigName) : base(databaseConfigName) { }

        #endregion

        #region Private Methods

        #endregion

        #region Protected Methods

        protected virtual void DeleteAll(string tableName)
        {
            string sql = "DELETE FROM " + tableName + ";";

            Func<IDbConnection, int> command = new Func<IDbConnection, int>(connection =>
            {
                return connection.Execute(sql);
            });

            this.ExecuteCommand(command);
        }

        #endregion

        #region Public Methods

        public virtual T FindById(String id)
        {
            Func<IDbConnection, T> command = new Func<IDbConnection, T>(connection =>
            {
                return connection.Get<T>(id);
            });
            return this.ExecuteCommand(command);
        }

        public virtual IEnumerable<T> GetAll()
        {
            Func<IDbConnection, IEnumerable<T>> command = new Func<IDbConnection, IEnumerable<T>>(connection =>
            {
                return connection.GetList<T>(new { });
            });

            return this.ExecuteCommand(command);
        }

        //public virtual int Create(T entity)
        //{
        //    Func<IDbConnection, int> command = new Func<IDbConnection, int>(connection =>
        //    {
        //        return connection.Insert<int>(entity);
        //    });
        //    return this.ExecuteCommand(command);
        //}

        //public virtual T Create<R>(T entity)
        //{
        //    Func<IDbConnection, T> command = new Func<IDbConnection, T>(connection =>
        //    {
        //        connection.Insert<R>(entity);
        //        return entity;
        //    });
        //    return this.ExecuteCommand(command);
        //}

        public virtual T Create(T entity)
        {
            Func<IDbConnection, T> command = new Func<IDbConnection, T>(connection =>
            {
                connection.Insert(entity);
                return entity;
            });
            return this.ExecuteCommand(command);
        }

        public virtual T Update(T entity)
        {
            Func<IDbConnection, T> command = new Func<IDbConnection, T>(connection =>
            {
                connection.Update(entity);
                return entity;
            });
            return this.ExecuteCommand(command);
        }

        public virtual void Delete(T entity)
        {
            Func<IDbConnection, T> command = new Func<IDbConnection, T>(connection =>
            {
                connection.Delete(entity);
                return entity;
            });
            this.ExecuteCommand(command);
        }

        #endregion
    }
}
