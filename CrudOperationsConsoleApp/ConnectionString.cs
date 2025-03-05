using System;
using System.Data.SqlClient;

namespace CrudOperationProject
{
    public class Connection_String
    {
        public static string cs = "Server=DELL\\SQLEXPRESS; Database=StudentDetails; Trusted_Connection=True; Encrypt=True; TrustServerCertificate=True";

        public static string dbcs { get => cs; }
    }
}
