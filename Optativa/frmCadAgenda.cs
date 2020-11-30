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
    public partial class frmCadAgenda : Form
    {
        public int id = -1;
        public frmCadAgenda()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            try
            {
                clsConexao conexao = new clsConexao();

                SQLiteCommand comando = new SQLiteCommand();

                if (id != -1)
                {

                    comando.CommandText = "UPDATE agenda SET nome = @nome WHERE id = @id";
                    comando.Parameters.AddWithValue("@id", id);
                }
                else
                {
                    comando.CommandText = "INSERT INTO agenda (inicio, acao, equipamento_id) VALUES (@inicio, @acao, @equipamento_id)";
                }

                comando.Parameters.AddWithValue("@equipamento_id", cmdEquipamento.SelectedValue);
                comando.Parameters.AddWithValue("@inicio", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                comando.Parameters.AddWithValue("@acao", cmbAcao.SelectedItem);
                comando.Connection = conexao.obterConexao();
                comando.ExecuteNonQuery();

                if (id != -1)
                {
                    MessageBox.Show("Agendamento atualizado com sucesso!",
                                "Sucesso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Agendamento  com sucesso!",
                                "Sucesso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                }

                
                this.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ops! Ocorreu um erro!" + err.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void frmCadAgenda_Load(object sender, EventArgs e)
        {
            if (id != -1)
            {
                clsConexao conexao = new clsConexao();

                SQLiteCommand comando = new SQLiteCommand();
                comando.CommandText = "SELECT * FROM agenda WHERE id = @id";
                comando.Parameters.AddWithValue("@id", id);
                comando.Connection = conexao.obterConexao();

                SQLiteDataReader dr = comando.ExecuteReader();
                dr.Read();
                dateTimePicker1.Text = dr["inicio"].ToString();
                cmbAcao.Text = dr["acao"].ToString();

                dr.Close();
                btnExcluir.Visible = true;

            }
            else 
            {
               clsConexao conexao = new clsConexao();

                SQLiteCommand comando = new SQLiteCommand();
                comando.CommandText = "SELECT id, nome FROM ambientes ORDER BY nome";
                comando.Connection = conexao.obterConexao();
                SQLiteDataReader dr = comando.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);

                cmdAmbiente.Items.Clear();

                cmdAmbiente.DataSource = dt;
                cmdAmbiente.DisplayMember = "nome";
                cmdAmbiente.ValueMember = "id";

            }
                
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult resposta = MessageBox.Show("Deseja realmente EXCLUIR?",
                                                   "Confirmação",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Warning,
                                                   MessageBoxDefaultButton.Button2);
            if (resposta == DialogResult.No)
            {
                return;
            }

            clsConexao conexao = new clsConexao();

            SQLiteCommand comando = new SQLiteCommand();
            comando.CommandText = "DELETE  FROM agenda WHERE id = @id";
            comando.Parameters.AddWithValue("@id", id);
            comando.Connection = conexao.obterConexao();
            comando.ExecuteNonQuery();

            this.Close();
        }


        private void cmdAmbiente_SelectedValueChanged(object sender, EventArgs e)
        {         
            clsConexao conexao = new clsConexao();

            SQLiteCommand comando = new SQLiteCommand();
            comando.CommandText = "SELECT id, nome FROM equipamentos WHERE ambiente_id = @id ORDER BY nome";
            comando.Parameters.AddWithValue("@id", cmdAmbiente.SelectedValue);
            comando.Connection = conexao.obterConexao();
            SQLiteDataReader dr = comando.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);

            cmdEquipamento.DataSource = dt;
            cmdEquipamento.DisplayMember = "nome";
            cmdEquipamento.ValueMember = "id";

        }
    }
}
