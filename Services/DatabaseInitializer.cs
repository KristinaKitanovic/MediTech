using BookListRazor.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

public interface IDatabaseInitializer
{
    Task InitializeAsync(); // Asinkroni metod
}

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public DatabaseInitializer(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task InitializeAsync()
    {
        var embeddedResourceName = "BookListRazor.Scripts.MediTechDB.sql"; // Putanja do vaše SQL skripte (embeddovan resurs)

        // Učitaj embedded resurs
        var assembly = Assembly.GetExecutingAssembly();
        using (var stream = assembly.GetManifestResourceStream(embeddedResourceName))
        using (var reader = new StreamReader(stream))
        {
            var sql = reader.ReadToEnd();

            // Asinkrono izvršenje SQL komande
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync(); // Otvorimo konekciju asinkrono
                using (var command = new SqlCommand(sql, connection))
                {
                    await command.ExecuteNonQueryAsync(); // Asinkrono izvršavanje SQL komande
                }
            }
        }
    }
}
