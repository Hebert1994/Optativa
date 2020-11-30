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
    public partial class frmCadAmbiente : Form
    {
        public int id = -1;
        public frmCadAmbiente()
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
                    comando.CommandText = "UPDATE ambientes SET nome = @nome WHERE  id = @id  ";
                    comando.Parameters.AddWithValue("@id", id);
                }
                else
                {
                    comando.CommandText = "INSERT INTO ambientes (nome) VALUES (@nome)";
                }
                    
                    comando.Parameters.AddWithValue("@nome", txtnomeambiente.Text);
                    comando.Connection = conexao.obterConexao();
                    comando.ExecuteNonQuery();

                if (id != -1)
                {
                    MessageBox.Show("Ambiente atualizado com sucesso!",
                                "Sucesso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ambiente cadastrado com sucesso!",
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

        private void frmCadAmbiente_Load(object sender, EventArgs e)
        {
            if ( id != -1)
            {
                clsConexao conexao = new clsConexao();

                SQLiteCommand comando = new SQLiteCommand();
                comando.CommandText = "SELECT * FROM ambientes WHERE id = @id";
                comando.Parameters.AddWithValue("@id", id);
                comando.Connection = conexao.obterConexao();

                SQLiteDataReader dr = comando.ExecuteReader();
                dr.Read();
                txtnomeambiente.Text = dr["nome"].ToString();

                dr.Close();
                btnExcluir.Visible = true;
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
            comando.CommandText = "DELETE  FROM ambientes WHERE id = @id";
            comando.Parameters.AddWithValue("@id", id);
            comando.Connection = conexao.obterConexao();
            comando.ExecuteNonQuery();

            this.Close();
        }
    }
}
