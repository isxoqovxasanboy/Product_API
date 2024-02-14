using System.Data;
using System.Data.Common;
using Npgsql;
using Product_API.Models;

namespace Product_API.Repositories
{
    public class ProductPostgressRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductPostgressRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DBInfo:ConnectionString")!;
            InitDatabase();
            InitTable();
        }

        public Product? Add(Product? product)
        {
            int result = default;
            using (NpgsqlConnection dbConnection = Connection())
            {
                dbConnection.Open();
                string addCommandQuary =
                    "insert into products (Id, Name, Description, PhotoPath) values (@Id,@Name,@Description,@PhotoPath)";
                using (NpgsqlCommand command = new NpgsqlCommand(addCommandQuary, dbConnection))
                {
                    command.Parameters.AddWithValue("Id", product.Id);
                    command.Parameters.AddWithValue("Name", product.Name);
                    command.Parameters.AddWithValue("Description", product.Description);
                    command.Parameters.AddWithValue("PhotoPath", product.PhotoPath);

                    try
                    {
                        result = command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = -1;
                    }
                }
            }

            return result > 0 ? product : null;
        }

        public List<Product> GetAll()
        {
            using (NpgsqlConnection dbConnection = Connection())
            {
                dbConnection.Open();
                string selecCommand =
                    "select * from products";


                using (NpgsqlCommand command = new NpgsqlCommand(selecCommand, dbConnection))
                {
                    var products = new List<Product>();
                    var product = new Product();
                    try
                    {
                        NpgsqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            product.Name = reader[0].ToString()!;
                            product.Description = reader[1].ToString()!;
                            product.PhotoPath = reader[2].ToString()!;
                            products.Add(product);
                        }

                        return products;
                    }
                    catch
                    {
                        return new List<Product>();
                    }
                }
            }

            return new List<Product?>();
        }

        private NpgsqlConnection Connection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        private void InitDatabase()
        {
            using (NpgsqlConnection dbConnection = Connection())
            {
                var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = 'Lesson';";
                using (var command = new NpgsqlCommand(sqlDbCount, dbConnection))
                {
                    var result = (int)command.ExecuteScalar()!;
                    if (result == 0)
                    {
                        var sql = $"CREATE DATABASE \"Lesson\"";
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void InitTable()
        {
            using (NpgsqlConnection dbConnection = Connection())
            {
                var sql = """
                              CREATE TABLE IF NOT EXISTS products (
                                  Id int,
                                  Name varchar,
                                  Description varchar,
                                  PhotoPath varchar
                              );
                          """;
                using (var command = new NpgsqlCommand(sql, dbConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}