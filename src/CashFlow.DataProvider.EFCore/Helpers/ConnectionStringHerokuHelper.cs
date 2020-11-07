using System.Linq;

namespace CashFlow.DataProvider.EFCore.Helpers
{
    public class ConnectionStringHerokuHelper
    {
        private readonly string connectionString;

        public ConnectionStringHerokuHelper(string connectionString)
        {
            this.connectionString = connectionString;
            SetConnectionString();
        }

        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }

        public string ConnectionString => string.IsNullOrEmpty(connectionString) ? string.Empty : GetConnectionString();

        private void SetConnectionString()
        {
            if (string.IsNullOrEmpty(connectionString))
                return;

            connectionString.Replace("//", "");

            char[] delimiterChars = { '/', ':', '@', '?' };
            string[] strConn = connectionString.Split(delimiterChars);
            strConn = strConn.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            this.User = strConn[1];
            this.Password = strConn[2];
            this.Server = strConn[3];
            this.Database = strConn[5];
            this.Port = strConn[4];
        }

        private string GetConnectionString() => $"host={Server};port={Port};database={Database};uid={User};pwd={Password};sslmode=Require;Trust Server Certificate=true;Timeout=1000";
    }
}
