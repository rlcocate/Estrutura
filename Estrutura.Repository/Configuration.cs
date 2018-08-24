using System.Configuration;

namespace Estrutura.Repository
{
    public abstract class Configuration
    {
        protected readonly string connectionString = ConfigurationManager.ConnectionStrings["Estrutura"].ConnectionString;
    }
}
