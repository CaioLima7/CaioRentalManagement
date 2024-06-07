using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Settings;

namespace Catalog.API.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly string _connectionString;

        public MotorcycleRepository(IOptions<CatalogDatabaseSettings> settings)
        {
            _connectionString = settings.Value.ConnectionString;
        }

        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Motorcycle>> GetAllAsync()
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<Motorcycle>("SELECT * FROM Motorcycles");
                return result;
            }
        }

        public async Task<Motorcycle> GetByIdAsync(int id)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Int32);
                var result = await dbConnection.QueryFirstOrDefaultAsync<Motorcycle>(
                    "SELECT * FROM Motorcycles WHERE Id = @Id", parameters);
                return result;
            }
        }

        public async Task AddAsync(Motorcycle motorcycle)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();
                var sql = "INSERT INTO Motorcycles (Model, Plate, Year) VALUES (@Model, @Plate, @Year)";
                var parameters = new DynamicParameters();
                parameters.Add("Model", motorcycle.Model, DbType.String);
                parameters.Add("Plate", motorcycle.Plate, DbType.String);
                parameters.Add("Year", motorcycle.Year, DbType.Int32);
                parameters.Add("Created_at", motorcycle.Created_at, DbType.Date);
                await dbConnection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task UpdateAsync(Motorcycle motorcycle)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();
                var sql = "UPDATE Motorcycles SET Model = @Model, Plate = @Plate, Year = @Year WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("Model", motorcycle.Model, DbType.String);
                parameters.Add("Plate", motorcycle.Plate, DbType.String);
                parameters.Add("Year", motorcycle.Year, DbType.Int32);
                parameters.Add("Id", motorcycle.Id, DbType.Int32);
                await dbConnection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();
                var sql = "DELETE FROM Motorcycles WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Int32);
                await dbConnection.ExecuteAsync(sql, parameters);
            }
        }
    }

}
