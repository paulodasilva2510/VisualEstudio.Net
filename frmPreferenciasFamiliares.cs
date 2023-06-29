using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Projeto_Business;
using Preferencias_VO;

namespace Projeto_Preferencias1_Familiares_10042021
{
    public partial class frmPreferenciasFamiliares : Form
    {
        string strPreferenciasAntiga, strFamiliaresAntigo;
        bool bolPreferencias, bolFamiliares;
        int intIdAntigo, intCodAntigo;

        PreferenciasBLL objPreferenciasBLL;
        PreferenciasVO objPreferenciasVO;
        Familiares_BLL objFamiliaresBLL;
        FamiliaresVO objFamiliaresVO;

        public frmPreferenciasFamiliares()
        {
            InitializeComponent();
        }

        private void btnMensagem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clique ok ou Cancelar", "Desvio", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                MessageBox.Show("Clicou ok");
            }
            else
            {
                MessageBox.Show("Clicou cancelar");
            }
        }

        private void btnImportaTXT_Click(object sender, EventArgs e)
        {
            try
            {
                objPreferenciasBLL = new PreferenciasBLL();
                lstbxPreferencias.Items.Clear();
                lstbxPreferencias.Items.AddRange(objPreferenciasBLL.ImportaTxt().ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deu errado: " + ex.Message);
            }
        }

        private void btnConectado_Click(object sender, EventArgs e)
        {
            try
            {
                objPreferenciasBLL = new PreferenciasBLL();
                lstbxPreferencias.Items.Clear();
                lstbxPreferencias.Items.AddRange(objPreferenciasBLL.ImportaTxtBd().ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deu errado: " + ex.Message);
            }
        }

        private void btnDesconectado_Click(object sender, EventArgs e)
        {
            try
            {
                objPreferenciasBLL = new PreferenciasBLL();
                lstbxPreferencias.Items.Clear();
                lstbxPreferencias.Items.AddRange(objPreferenciasBLL.ImportaTxtBdD().ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deu errado: " + ex.Message);
            }
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            Consulta();
        }

        public void Consulta(int? intId = null, string strPreferencias = null)
        {
            try
            {
                objPreferenciasBLL = new PreferenciasBLL();
                objPreferenciasVO = new PreferenciasVO();

                if (!string.IsNullOrEmpty(intId.ToString()))
                {
                    objPreferenciasVO.setId(Convert.ToInt32(intId));
                }
                 if (!string.IsNullOrEmpty(strPreferencias))
                {
                    objPreferenciasVO.setDescricao(strPreferencias);
                }

                bndsrcPreferencias.DataSource = objPreferenciasBLL.Consulta(objPreferenciasVO);
                dtgvwPreferencias.DataSource = bndsrcPreferencias;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Deu errado: " + ex.Message);
            }
        }

        private void btnInclusao_Click(object sender, EventArgs e)
        {
            Inclusao(dtgvwPreferencias.CurrentCell.EditedFormattedValue.ToString());
            Consulta();
        }

        public void Inclusao(string strPreferencias)
        {
            try
            {
                objPreferenciasBLL = new PreferenciasBLL();
                objPreferenciasVO = new PreferenciasVO(strPreferencias);

                if (objPreferenciasBLL.Inclusao(objPreferenciasVO))
                {
                    MessageBox.Show("Inclusão ok");
                }
                else
                {
                    MessageBox.Show("Falha na Inclusão ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deu errado: " + ex.Message);
            }
        }

        private void btnExclusao_Click(object sender, EventArgs e)
        {
            Exclusao(intIdAntigo);
            Consulta();
        }

        public void Exclusao(int intId)
        {
            try
            {
                objPreferenciasBLL = new PreferenciasBLL();
                objPreferenciasVO = new PreferenciasVO();

                objPreferenciasVO.setId(intId);

                if (objPreferenciasBLL.Exclusao(objPreferenciasVO))
                {
                    MessageBox.Show("Exclusao ok");
                }
                else
                {
                    MessageBox.Show("Falha na Exclusao ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deu errado: " + ex.Message);
            }
        }

        private void btnAlteracao_Click(object sender, EventArgs e)
        {
            Alteracao(intIdAntigo, dtgvwPreferencias.CurrentRow.Cells["Descricao"].EditedFormattedValue.ToString());
            Consulta();
        }

        public void Alteracao(int intId, string strPreferencias)
        {
            try
            {
                objPreferenciasBLL = new PreferenciasBLL();
                objPreferenciasVO = new PreferenciasVO(intId, strPreferencias);

                if (objPreferenciasBLL.Alteracao(objPreferenciasVO))
                {
                    MessageBox.Show("Alteracao ok");
                }
                else
                {
                    MessageBox.Show("Falha na Alteracao ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deu errado: " + ex.Message);
            }
        }
        private void dtgvwPreferencias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            strPreferenciasAntiga = dtgvwPreferencias.CurrentRow.Cells["Descricao"].Value.ToString();

            if (!string.IsNullOrEmpty(dtgvwPreferencias.CurrentRow.Cells["ID"].Value.ToString()))
            {
                intIdAntigo = Convert.ToInt32(dtgvwPreferencias.CurrentRow.Cells["ID"].Value.ToString());
            }
        }

        private void frmPreferenciasFamiliares_Load(object sender, EventArgs e)
        {
            Consulta();
            ConsultaBD_Familiares();
        }

        private void bndnavIncluirPreferencias_Click(object sender, EventArgs e)
        {
            bolPreferencias = true;
        }

        private void bndnavExcluirPreferencias_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show ("Confirma a Exclusão de Preferencias = " + strPreferenciasAntiga, "Exclusão de Preferencias", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Exclusao(intIdAntigo);
            }
            Consulta();
        }


        private void bndnavConsultaPreferencias_Click(object sender, EventArgs e)
        {
            Consulta(null, bndnavTXTPreferencias.Text);
        }

        private void bndnavConfirmaPreferencias_Click(object sender, EventArgs e)
        {
            if (bolPreferencias)
            {
                if (MessageBox.Show("Confirma a Inclusão de Preferencias = " + dtgvwPreferencias.CurrentRow.Cells["Descricao"].EditedFormattedValue.ToString(),
                                    "Inclusão de Preferencias", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Inclusao(dtgvwPreferencias.CurrentRow.Cells["Descricao"].EditedFormattedValue.ToString());
                }

                bolPreferencias = false;
            }
            else
            {
                if (MessageBox.Show("Confirma a Alteração de Preferencias = " + dtgvwPreferencias.CurrentRow.Cells["Descricao"].EditedFormattedValue.ToString(),
                                   "Alteração de Preferencias", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Alteracao(intIdAntigo, dtgvwPreferencias.CurrentRow.Cells["Descricao"].EditedFormattedValue.ToString());
                }
            }
            Consulta();
        }

        private void bndnavConsultaFamiliares_Click(object sender, EventArgs e)
        {
            ConsultaBD_Familiares(null, bndnavTXTFamiliares.Text);
        }

        public void ConsultaBD_Familiares(int? intCod = null, string strNomeFamiliar = null)
        {
            try
            {
                objFamiliaresVO = new FamiliaresVO();
                objFamiliaresVO.Nome = strNomeFamiliar;

                objFamiliaresBLL = new Familiares_BLL();

                bndsrcFamiliares.DataSource = objFamiliaresBLL.Consulta(objFamiliaresVO);

                dtgvwFamiliares.DataSource = bndsrcFamiliares;

                dtgvwFamiliares.AllowUserToAddRows = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Deu Errado: " + ex.Message);
            }
        }

        private void bndnavIncluirFamiliares_Click(object sender, EventArgs e)
        {
            bolFamiliares = true;
        }

        private void bndnavExcluirFamiliares_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirma a Exclusão de Familiares = " + strFamiliaresAntigo,
                                "Exclusão de familiares", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                ExclusaoDeFamiliares(intCodAntigo);   
            }

            ConsultaBD_Familiares();
        }

        public void ExclusaoDeFamiliares(int intCod)
        {
            try
            {
                objFamiliaresBLL = new Familiares_BLL();
                objFamiliaresVO = new FamiliaresVO();
                objFamiliaresVO.setCod(intCod);

                if (objFamiliaresBLL.Exclusao(objFamiliaresVO))
                {
                    MessageBox.Show("Exclusão ok");
                }
                else
                {
                    MessageBox.Show("Falha na Exclusão ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problema na Exclusão de Familiares: " + ex.Message);
            }
        }
              
        public void InclusaoDeFamiliares(string strNome = null, string strSexo = null, int? intIdade = null, double? dbGanhos = null, double? dbGastos = null, string strObs = null)
        {
            try
            {
                objFamiliaresBLL = new Familiares_BLL();
                objFamiliaresVO = new FamiliaresVO();
                objFamiliaresVO.Nome = strNome;
                objFamiliaresVO.Sexo = strSexo;
                              
                if (intIdade != null)
                {
                    objFamiliaresVO.setIdade(Convert.ToInt32(intIdade));
                }

                if (dbGanhos != null)
                {
                    objFamiliaresVO.setGanhos(Convert.ToDouble(dbGanhos));
                }

                if (dbGastos != null)
                {
                    objFamiliaresVO.setGanhos(Convert.ToDouble(dbGastos));
                }
                objFamiliaresVO.Observacao = strObs;

                if (objFamiliaresBLL.Inclusao(objFamiliaresVO))
                {
                    MessageBox.Show("Inclusão ok");
                }
                else
                {
                    MessageBox.Show("Falha na Inclusão ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show ("Falha na Inclusão de familiares!" + ex.Message);
            }
        }

        public void AlteracaoDeFamiliares(int intCod, string strNome, string strSexo, int intIdade, double dbGanhos, double dbGastos, string strOBS)
        {
            try
            {
                objFamiliaresBLL = new Familiares_BLL();
                objFamiliaresVO = new FamiliaresVO(intCod, strNome, strSexo, intIdade, dbGanhos, dbGastos, strOBS);

                if (objFamiliaresBLL.Alteracao(objFamiliaresVO))
                {
                    MessageBox.Show("Alteração de familiares ok");
                }
                else
                {
                    MessageBox.Show("falha na Alteração de familiaresk");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deu errado: " + ex.Message);
            }
        }

        private void bndnavConfirmarFamiliares_Click(object sender, EventArgs e)
        {
            if (bolFamiliares)
            {
                if (MessageBox.Show("Confirma a Inclusão de Familiares = " + dtgvwFamiliares.CurrentRow.Cells["Nome"].EditedFormattedValue.ToString(),
                                    "Inclusão de familiares", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    InclusaoDeFamiliares(
                    dtgvwFamiliares.CurrentRow.Cells["Nome"].EditedFormattedValue.ToString(),
                    dtgvwFamiliares.CurrentRow.Cells["Sexo"].EditedFormattedValue.ToString(),
                    dtgvwFamiliares.CurrentRow.Cells["Idade"].EditedFormattedValue.ToString()
                    == string.Empty ? 0 : Convert.ToInt32(dtgvwFamiliares.CurrentRow.Cells["Idade"].EditedFormattedValue.ToString()),
                    dtgvwFamiliares.CurrentRow.Cells["Ganhos_Total"].EditedFormattedValue.ToString()
                    == string.Empty ? 0 : Convert.ToInt32(dtgvwFamiliares.CurrentRow.Cells["Ganhos_Total"].EditedFormattedValue.ToString()),                
                    dtgvwFamiliares.CurrentRow.Cells["Gastos_Total"].EditedFormattedValue.ToString()
                    == string.Empty ? 0 : Convert.ToInt32(dtgvwFamiliares.CurrentRow.Cells["Gastos_Total"].EditedFormattedValue.ToString()),                                    
                    dtgvwFamiliares.CurrentRow.Cells["Observacao"].EditedFormattedValue.ToString());
                }

                bolFamiliares = false;
            }
            else
            {
                if (MessageBox.Show("Confirma a Alteração de Familiares = " + dtgvwFamiliares.CurrentRow.Cells["Nome"].EditedFormattedValue.ToString(),
                                   "Alteração de familiares", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    AlteracaoDeFamiliares(intCodAntigo,
                    dtgvwFamiliares.CurrentRow.Cells["Nome"].EditedFormattedValue.ToString(),
                    dtgvwFamiliares.CurrentRow.Cells["Sexo"].EditedFormattedValue.ToString(),
                    Convert.ToInt32(dtgvwFamiliares.CurrentRow.Cells["Idade"].EditedFormattedValue.ToString()),
                    Convert.ToDouble(dtgvwFamiliares.CurrentRow.Cells["Ganhos_Total"].EditedFormattedValue.ToString()),
                    Convert.ToDouble(dtgvwFamiliares.CurrentRow.Cells["Gastos_Total"].EditedFormattedValue.ToString()),
                    dtgvwFamiliares.CurrentRow.Cells["Observacao"].EditedFormattedValue.ToString());
                }
            }
            ConsultaBD_Familiares();
        }

        private void dtgvwFamiliares_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            strFamiliaresAntigo = dtgvwFamiliares.CurrentRow.Cells["Nome"].Value.ToString();

            if (!string.IsNullOrEmpty(dtgvwFamiliares.CurrentRow.Cells["COD"].Value.ToString()))
            {
                intCodAntigo = Convert.ToInt32(dtgvwFamiliares.CurrentRow.Cells["COD"].Value.ToString());
            }
        }

        private void lstbxPreferencias_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
