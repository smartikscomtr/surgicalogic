using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using Surgicalogic.Common.Extensions;
using Surgicalogic.Data.Entities.Base;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel.Base;
using Surgicalogic.Services.Extensions;
using Surgicalogic.Services.QueryBuilder;
using Surgicalogic.Services.QueryBuilder.Clauses;
using Surgicalogic.Services.QueryBuilder.Statements;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores.Base
{
    public class StoreService<TEntity, TModel, TSorting, TFilter>
        where TEntity : Entity
        where TModel : EntityModel
        where TSorting : struct
        where TFilter : struct
    {
        protected virtual IEnumerable<FilterModel<TFilter>> DefaultFilters => null;
        protected virtual TSorting DefaultSorting => default(TSorting);

        private readonly string _connectionString;
        private readonly string _schema;

        private IDbConnection _providedConnection;

        protected StoreService(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DataContext"];
            _schema = configuration["AppSettings:Database:SchemaName"];
        }

        public void SetConnection(IDbConnection connection)
        {
            _providedConnection = connection;
        }

        public virtual async Task<ResultModel<TModel>> GetAsync(FilterSortPaginationModel<TSorting, TFilter> filterSortPagination)
        {
            var connection = GetConnection();

            try
            {
                var tableName = GetTableName();

                var selectQueryBuilder = new SelectQueryBuilder();

                selectQueryBuilder.FromClause.Table = new TableStatement(_schema, tableName);

                await SetSearchFilterSortAsync(selectQueryBuilder, filterSortPagination);

                //Get Total Count
                var totalCount = await connection.ExecuteScalarAsync<int>(selectQueryBuilder.BuildCountQuery(), commandType: CommandType.Text);

                var pageSize = filterSortPagination?.PageSize;
                var page = filterSortPagination?.Page;

                SetPagination(selectQueryBuilder, pageSize, page, totalCount);

                var entities = await connection.QueryAsync<TEntity>(selectQueryBuilder.BuildQuery(), commandType: CommandType.Text);

                var models = Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(entities);

                return new ResultModel<TModel>
                {
                    Result = await BeforeSelectResultAsync(connection, models),
                    TotalCount = totalCount,
                    Info = new Info()
                };
            }
            finally
            {
                HandleConnection(connection);
            }
        }

        public virtual async Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>(FilterSortPaginationModel<TSorting, TFilter> filterSortPagination)            
        {
            var result = await GetAsync(filterSortPagination);

            return new ResultModel<TOutputModel>
            {
                TotalCount = result.TotalCount,
                Result = Mapper.Map<IEnumerable<TOutputModel>>(result.Result),
                Info = result.Info
            };
        }

        public virtual async Task<TModel> FirstOrDefaultAsync(FilterSortPaginationModel<TSorting, TFilter> filterSortPagination)
        {
            var connection = GetConnection();

            try
            {
                var tableName = GetTableName();

                var selectQueryBuilder = new SelectQueryBuilder();

                selectQueryBuilder.FromClause.Table = new TableStatement(_schema, tableName);

                await SetSearchFilterSortAsync(selectQueryBuilder, filterSortPagination);

                var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(selectQueryBuilder.BuildQuery(), commandType: CommandType.Text);

                if (entity == null)
                {
                    //return null;
                }

                var model = Mapper.Map<TEntity, TModel>(entity);

                model = await BeforeSelectResultAsync(connection, model);

                return model;
            }
            finally
            {
                HandleConnection(connection);
            }
        }

        public virtual async Task<TModel> FindByIdAsync(int id)
        {
            var connection = GetConnection();

            try
            {
                var query = $"SELECT * FROM \"{_schema}\".\"{GetTableName()}\" WHERE \"Id\" = {id}";

                var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(query, commandType: CommandType.Text);

                if (entity == null)
                {
                    //return null;
                }

                var model = Mapper.Map<TEntity, TModel>(entity);

                model = await BeforeSelectResultAsync(connection, model);

                return model;
            }
            finally
            {
                HandleConnection(connection);
            }
        }

        public virtual async Task<ResultModel<TModel>> InsertAsync(TModel model)
        {
            var connection = GetConnection();

            try
            {
                model = await BeforeInsertAsync(connection, model);

                var entity = Mapper.Map<TModel, TEntity>(model);

                var insertQueryBuilder = new InsertQueryBuilder<TEntity>();

                var tableName = GetTableName();

                insertQueryBuilder.Table = new TableStatement(_schema, tableName);

                var query = insertQueryBuilder.BuildQuery(entity);

                model.Id = await connection.ExecuteScalarAndGetInsertedIdAsync(query);
                return new ResultModel<TModel>
                {
                    Result = model,
                    Info = new Info()
                };
            }
            finally
            {
                HandleConnection(connection);
            }
        }

        public virtual async Task<ResultModel<int>> DeleteByIdAsync(int id)
        {
            var connection = GetConnection();

            try
            {
                var selectQuery = $"SELECT * FROM \"{GetTableName()}\" WHERE \"Id\" = {id}";

                var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(selectQuery, commandType: CommandType.Text);

                await BeforeDeleteAsync(connection, entity);

                var deleteQuery = $"DELETE FROM \"{GetTableName()}\" WHERE \"Id\" = {id}";

                return new ResultModel<int>
                {
                    Result = await connection.ExecuteAsync(deleteQuery, commandType: CommandType.Text),
                    Info = new Info()
                };
            }
            finally
            {
                HandleConnection(connection);
            }
        }

        public virtual async Task<ResultModel<TModel>> UpdateAsync(TModel model)
        {
            var connection = GetConnection();

            try
            {
                var id = model.Id;

                var selectQuery = $"SELECT * FROM \"{GetTableName()}\" WHERE \"Id\" = {id}";

                var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(selectQuery, commandType: CommandType.Text);

                model = await BeforeUpdateAsync(connection, model, entity);

                entity = Mapper.Map(model, entity);

                var updateQueryBuilder = new UpdateQueryBuilder<TEntity>();

                var tableName = GetTableName();

                updateQueryBuilder.Table = new TableStatement(_schema, tableName);

                var query = updateQueryBuilder.BuildQuery(entity);

                await connection.ExecuteAsync(query);

                return new ResultModel<TModel>
                {
                    Result = model,
                    Info = new Info()

                };

            }
            finally
            {
                HandleConnection(connection);
            }
        }

        public virtual async Task UpdateAsync(IEnumerable<(string Column, object Value)> set, IEnumerable<(string Column, object Value)> where)
        {
            var connection = GetConnection();

            try
            {
                var updateQueryBuilder = new UpdateQueryBuilder<TEntity>();

                var tableName = GetTableName();

                updateQueryBuilder.Table = new TableStatement(_schema, tableName);

                var query = updateQueryBuilder.BuildQuery(set, where);

                await connection.ExecuteAsync(query);
            }
            finally
            {
                HandleConnection(connection);
            }
        }

        protected virtual Task SetSearchAsync(SelectQueryBuilder query, string search)
        {
            return Task.CompletedTask;
        }

        protected virtual Task SetFiltersAsync(SelectQueryBuilder query, IEnumerable<FilterModel<TFilter>> filters)
        {
            return Task.CompletedTask;
        }

        protected virtual Task SetSortingAsync(SelectQueryBuilder query, TSorting? sorting)
        {
            return Task.CompletedTask;
        }

        protected virtual async Task<IEnumerable<TModel>> BeforeSelectResultAsync(IDbConnection connection, IEnumerable<TModel> models)
        {
            var list = new List<TModel>();

            foreach (var model in models)
            {
                list.Add(await BeforeSelectResultAsync(connection, model));
            }

            return list;
        }

        protected virtual Task<TModel> BeforeSelectResultAsync(IDbConnection connection, TModel model)
        {
            return Task.FromResult(model);
        }

        protected virtual Task<TModel> BeforeInsertAsync(IDbConnection connection, TModel model)
        {
            return Task.FromResult(model);
        }

        protected virtual Task<TModel> BeforeUpdateAsync(IDbConnection connection, TModel model, TEntity entity)
        {
            return Task.FromResult(model);
        }

        protected virtual Task BeforeDeleteAsync(IDbConnection connection, TEntity entity)
        {
            return Task.CompletedTask;
        }

        #region Helper Methods

        private IDbConnection GetConnection()
        {
            if (_providedConnection != null)
            {
                if (_providedConnection.State == ConnectionState.Closed)
                {
                    _providedConnection.Open();
                }

                return _providedConnection;
            }

            var conn = new SqlConnection(_connectionString);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn;
        }

        private void HandleConnection(IDbConnection connection)
        {
            if (connection != _providedConnection)
            {
                connection?.Close();
                connection?.Dispose();
            }
        }

        protected virtual string GetTableName()
        {
            return typeof(TEntity).TableName();
        }

        private async Task SetSearchFilterSortAsync(SelectQueryBuilder query, FilterSortPaginationModel<TSorting, TFilter> filterSortPagination)
        {
            var search = filterSortPagination?.Search;
            var filters =
                filterSortPagination?.Filters?
                    .Select(x => new FilterModel<TFilter>
                    {
                        Filter = x.Filter,
                        Value = x.Value
                    })
                    .ToArray()
                ?? DefaultFilters;
            var sorting = filterSortPagination?.Sorting ?? DefaultSorting;

            if (!(search?.Trim()).IsNullOrEmpty())
            {
                await SetSearchAsync(query, search);
            }

            if (filters != null)
            {
                await SetFiltersAsync(query, filters);
            }

            await SetSortingAsync(query, sorting);
        }

        private void SetPagination(SelectQueryBuilder query, int? pageSize, int? page, int totalCount)
        {
            if (pageSize.HasValue)
            {
                query.LimitClause = new LimitClause
                {
                    Page = page ?? 1,
                    PageSize = pageSize.Value
                };
            }
        }

        #endregion

    }
}
