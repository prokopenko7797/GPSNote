using GPSNote.Constants;
using GPSNote.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.Repository
{
    public class Repository : IRepository
    {

        private readonly SQLiteAsyncConnection _database;

        public Repository()
        {
            _database = new SQLiteAsyncConnection(
                Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), Constant.DB_Name));
        }

        #region ______Public Methods______
        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : IEntityModel, new()
        {
            await _database.CreateTableAsync<T>();
            return await _database.Table<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query) where T : IEntityModel, new()
        {
            await _database.CreateTableAsync<T>();
            return await _database.QueryAsync<T>(query);
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : IEntityModel, new()
        {
            await _database.CreateTableAsync<T>();
            return await _database.GetAsync<T>(id);
        }

        public async Task<int> DeleteAsync<T>(int id) where T : IEntityModel, new()
        {
            await _database.CreateTableAsync<T>();
            return await _database.DeleteAsync<T>(id);
        }

        public async Task<int> InsertAsync<T>(T item) where T : IEntityModel, new()
        {
            await _database.CreateTableAsync<T>();
            return await _database.InsertAsync(item);
        }

        public async Task<int> UpdateAsync<T>(T item) where T : IEntityModel, new()
        {
            await _database.CreateTableAsync<T>();
            return await _database.UpdateAsync(item);
        }

        public async Task<T> FindWithQueryAsync<T>(string query) where T : IEntityModel, new()
        {
            await _database.CreateTableAsync<T>();
            return await _database.FindWithQueryAsync<T>(query);
        }
        #endregion
    }
}
