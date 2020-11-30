using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optativa
{
    public class clsConexao
    {
        public SQLiteConnection conexao_;
        public string banco = ".\\banco.db";


        public clsConexao()
        {
            SqlConnectionStringBuilder string_conexao = new SqlConnectionStringBuilder();
            string_conexao.DataSource = banco;

            conexao_ = new SQLiteConnection();
            conexao_.ConnectionString = string_conexao.ConnectionString;

            conexao_.Open();
        }
        public SQLiteConnection obterConexao()
        {
            return conexao_;
        }
        ~clsConexao()
        {
        }
    }
}
