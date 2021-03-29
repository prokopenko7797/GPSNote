using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : IEntityModel, new();

        Task<T> FindWithQueryAsync<T>(string query) where T : IEntityModel, new();

        Task<IEnumerable<T>> QueryAsync<T>(string query) where T : IEntityModel, new();
        Task<T> GetByIdAsync<T>(int id) where T : IEntityModel, new();
        Task<int> DeleteAsync<T>(int id) where T : IEntityModel, new();
        Task<int> InsertAsync<T>(T item) where T : IEntityModel, new();
        Task<int> UpdateAsync<T>(T item) where T : IEntityModel, new();
    }
}
