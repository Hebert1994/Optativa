using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Optativa
{
    public partial class frmConAmbiente : Form
    {
        public frmConAmbiente()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            clsConexao conexao = new clsConexao();

            SQLiteCommand comando = new SQLiteCommand();
            comando.CommandText = "SELECT * FROM ambientes WHERE nome LIKE @busca ORDER BY nome";
            comando.Parameters.AddWithValue("@busca", '%' + textnome.Text + '%');
            comando.Connection = conexao.obterConexao();
            comando.ExecuteNonQuery();

            SQLiteDataReader dr = comando.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);

            dataGridView1.DataSource = dt;

            dataGridView1.AutoGenerateColumns = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            frmCadAmbiente frm = new frmCadAmbiente();
            frm.MdiParent = this.ParentForm;
            frm.id = id;
            frm.Show();
            this.Close();
         }
    }
}
