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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult resposta = MessageBox.Show("Deseja realmente sair?",
                                                    "Confirmação",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question,
                                                    MessageBoxDefaultButton.Button2);
            if (resposta == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void form1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConfigSerial frm = new frmConfigSerial();
            frm.MdiParent = this;
            frm.Show();
        }

        private void ambienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCadAmbiente frm = new frmCadAmbiente();
            frm.MdiParent = this;
            frm.Show();
        }

        private void equipamentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCadEquipamentos frm = new frmCadEquipamentos();
            frm.MdiParent = this;
            frm.Show();
        }

        private void agendaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCadAgenda frm = new frmCadAgenda();
            frm.MdiParent = this;
            frm.Show();

        }

        private void ambienteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmConAmbiente frm = new frmConAmbiente();
            frm.MdiParent = this;
            frm.Show();
        }

        private void agendaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmConAgenda frm = new frmConAgenda();
            frm.MdiParent = this;
            frm.Show();
        }

        private void equipamentoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmConEquipamento frm = new frmConEquipamento();
            frm.MdiParent = this;
            frm.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString();

            clsConexao conexao = new clsConexao();

            SQLiteCommand comando = new SQLiteCommand();
            comando.CommandText = "SELECT * FROM agenda WHERE inicio = date('now') ";
            comando.Connection = conexao.obterConexao();

            SQLiteDataReader dr = comando.ExecuteReader();

            if( dr.HasRows)
            {

                toolStripStatusLabel2.Text = "trabalhando..";

                while (dr.Read())
                {


                    int acao = Convert.ToInt32(dr["acao"]);
                    int equipamento_id = Convert.ToInt32(dr["equipamento_id"]);


                    if (acao == 1)
                    {

                        SQLiteCommand comando3 = new SQLiteCommand();
                        comando3.CommandText = "SELECT * FROM faturamento WHERE equipamento_id = @equipamento_id";
                        comando3.Parameters.AddWithValue("@equipamento_id", equipamento_id);
                        comando3.Connection = conexao.obterConexao();
                        SQLiteDataReader dr2 = comando.ExecuteReader();
                        if (!dr2.HasRows)
                        {
                            SQLiteCommand comando2 = new SQLiteCommand();
                            comando2.CommandText = "INSERT INTO faturamento (equipamento_id, inicio) VALUES (@equipamento_id, @datahora)";
                            comando2.Parameters.AddWithValue("@datahora", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                            comando2.Parameters.AddWithValue("@equipamento_id", equipamento_id);
                            comando2.Connection = conexao.obterConexao();
                            comando2.ExecuteNonQuery();

                        }
                    }
                    else
                    {
                        SQLiteCommand comando2 = new SQLiteCommand();
                        comando2.CommandText = "UPDATE faturamento SET termino = @datahora WHERE equipamento_id = @equipamento_id";
                        comando2.Parameters.AddWithValue("@datahora", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        comando2.Parameters.AddWithValue("@equipamento_id", equipamento_id);
                        comando2.Connection = conexao.obterConexao();
                        comando2.ExecuteNonQuery();
                    }


                    
                }

               //  toolStripStatusLabel2.Text = "Ocioso";
            }
            

        }
    }
}
