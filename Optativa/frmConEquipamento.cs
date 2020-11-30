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
    public partial class frmConEquipamento : Form
    {
        public frmConEquipamento()
        {
            InitializeComponent();
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            clsConexao conexao = new clsConexao();

            SQLiteCommand comando = new SQLiteCommand();
            comando.CommandText = "SELECT equipamentos.*, ambientes.nome AS ambiente FROM equipamentos JOIN ambientes ON ambiente_id = ambientes.id WHERE equipamentos.nome LIKE @busca ORDER BY equipamentos.nome";
            comando.Parameters.AddWithValue("@busca", '%' + textBox1.Text + '%');
            comando.Connection = conexao.obterConexao();
            comando.ExecuteNonQuery();

            SQLiteDataReader dr = comando.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = dt;

            
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            frmCadEquipamentos frm = new frmCadEquipamentos();
            frm.MdiParent = this.ParentForm;
            frm.id = id;
            frm.Show();
            this.Close();
        }
    }
}
