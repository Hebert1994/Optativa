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
    public partial class frmCadEquipamentos : Form
    {
        public int id = -1;
        public frmCadEquipamentos()
        {
            InitializeComponent();
        }

        private void txtConsumo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            try
            {
                clsConexao conexao = new clsConexao();

                SQLiteCommand comando = new SQLiteCommand();


                if (id != -1)
                {
                    comando.CommandText = "UPDATE equipamentos SET nome = @nome, consumo = @consumo, porta = @porta, ambiente_id = @ambiente_id ";
                    comando.Parameters.AddWithValue("@id", id);
                }
                else
                { 
                    comando.CommandText = "INSERT INTO equipamentos (nome, consumo, porta, ambiente_id) VALUES (@nome, @consumo, @porta, @ambiente_id)";
                }

                    
                comando.Parameters.AddWithValue("@nome", txtNomeEquipamento.Text);
                comando.Parameters.AddWithValue("@consumo", txtConsumo.Text);
                comando.Parameters.AddWithValue("@porta", cmbPorta.Text);
                comando.Parameters.AddWithValue("@ambiente_id", cmdAmbiente.SelectedValue);
                comando.Connection = conexao.obterConexao();
                comando.ExecuteNonQuery();

                if (id != -1)
                {
                    MessageBox.Show("Equipamento atualizado com sucesso!",
                                "Sucesso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Equipamento cadastrado com sucesso!",
                                "Sucesso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                }
                this.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ops! Ocorreu um erro!",
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void frmCadEquipamentos_Load(object sender, EventArgs e)
        {

            if (id != -1)
            {
                clsConexao conexao = new clsConexao();

                SQLiteCommand comando = new SQLiteCommand();
                comando.CommandText = "SELECT * FROM equipamentos WHERE id = @id";
                comando.Parameters.AddWithValue("@id", id);
                comando.Connection = conexao.obterConexao();
                SQLiteDataReader dr = comando.ExecuteReader();
                dr.Read();
                txtNomeEquipamento.Text = dr["nome"].ToString();
                txtConsumo.Text = dr["consumo"].ToString();
                cmbPorta.Text = dr["porta"].ToString();
                cmdAmbiente.Text = dr["ambiente_id"].ToString();

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
            comando.CommandText = "DELETE  FROM equipamentos WHERE id = @id";
            comando.Parameters.AddWithValue("@id", id);
            comando.Connection = conexao.obterConexao();
            comando.ExecuteNonQuery();

            this.Close();
        }
    }
}
