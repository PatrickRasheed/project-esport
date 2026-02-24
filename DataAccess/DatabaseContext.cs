using Npgsql;
using Microsoft.Extensions.Configuration;

namespace TournamentManager.DataAccess
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext()
        {
            // Lire la configuration depuis appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = configuration.GetConnectionString("PostgreSQL")
                ?? throw new InvalidOperationException("ConnectionString 'PostgreSQL' non trouvée");
        }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}