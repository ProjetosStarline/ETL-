using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETLApplication.Model
{
    public class CategoriasModel : GenericModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_categoria { get; set; }
        public int id_grupo { get; set; }
        public string nm_categoria { get; set; }
        public string descr_categorias { get; set; }
        public string status { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime data_criacao { get; set; }
        public DateTime data_atualizacao { get; set; }
        public CategoriasModel()
        {
            NomeTabela = "tb_categorias";
        }

        public void SetDados(SqlDataReader dr)
        {
            try
            {
                id_categoria = Convert.ToInt32(isNullResultVazio(dr["id_categoria"]));
                id_grupo = Convert.ToInt32(isNullResultVazio(dr["id_grupo"]));
                nm_categoria = isNullResultVazio(dr["nm_categoria"]);
                descr_categorias = isNullResultVazio(dr["descr_categorias"]);
                status = isNullResultVazio(dr["status"]);
                data_criacao = Convert.ToDateTime(dr["Data_criacao"].ToString());
                data_atualizacao = Convert.ToDateTime(dr["Data_atualizacao"].ToString());
            }
            finally
            {
                dr.Close();
            }

        }
        public List<CategoriasModel> GetListaPermissoes(SqlDataReader dr)
        {
            List<CategoriasModel> categorias = new List<CategoriasModel>();
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        categorias.Add
                        (
                            new CategoriasModel()
                            {
                                id_categoria = Convert.ToInt32(isNullResultVazio(dr["id_categoria"])),
                                id_grupo = Convert.ToInt32(isNullResultVazio(dr["id_grupo"])),
                                nm_categoria = isNullResultVazio(dr["nm_categoria"]),
                                descr_categorias = isNullResultVazio(dr["descr_categorias"]),
                                status = isNullResultVazio(dr["status"]),
                                data_criacao = Convert.ToDateTime(dr["Data_criacao"].ToString()),
                                data_atualizacao = Convert.ToDateTime(dr["Data_atualizacao"].ToString())
                            }
                        );
                    }
                }
            }
            finally
            {
                dr.Close();
            }
            return categorias;
        }

        public override void SetDados(DataGridView dgv)
        {
            if (dgv.CurrentRow != null)
            {
                if (dgv.CurrentRow.Cells["ID"].Value.ToString() != "")
                {
                    id_categoria = Convert.ToInt32(isNullResultZero(dgv.CurrentRow.Cells["ID"].Value.ToString()));
                    id_grupo = Convert.ToInt32(isNullResultZero(dgv.CurrentRow.Cells["ID Empresa"].Value.ToString()));
                    nm_categoria = isNullResultVazio(dgv.CurrentRow.Cells["Filial"].Value);
                    descr_categorias = isNullResultVazio(dgv.CurrentRow.Cells["Descrição"].Value);
                    status = isNullResultVazio(dgv.CurrentRow.Cells["status"].Value);
                    data_criacao = Convert.ToDateTime(dgv.CurrentRow.Cells["Data Criação"].Value.ToString());
                    data_atualizacao = Convert.ToDateTime(dgv.CurrentRow.Cells["Data Atualização"].Value.ToString());
                }
            }
        }
        public override void Clear()
        {
            id_categoria = 0;
            id_grupo = 0;
            nm_categoria = "";
            descr_categorias = "";
            status = "";
            data_criacao = DateTime.Now;
            data_atualizacao = DateTime.Now;

        }

        public override void GetListaCampos()
        {
            base.GetListaCampos();
            ListaCamposView.Clear();
            ListaCamposView.Add("ID Empresa");
            ListaCamposView.Add("Empresa");
            ListaCamposView.Add("ID");
            ListaCamposView.Add("Filial");
            ListaCamposView.Add("Descrição");
            ListaCamposView.Add("Status");
            ListaCamposView.Add("Data Criação");
            ListaCamposView.Add("Data Atualização");
        }

        public override string GetFieldParaBusca(string pCampoView)
        {
            string Result = "";
            switch (pCampoView)
            {
                case "ID Empresa": Result = "e.id_grupo"; break;
                case "Empresa": Result = "e.nm_grupo"; break;
                case "ID": Result = "c.id_categoria"; break;
                case "Filial": Result = "c.nm_categoria"; break;
                case "Descrição": Result = "c.descr_categorias"; break;
                case "Status": Result = "c.status"; break;
                case "Data Criação": Result = "c.data_criacao"; break;
                case "Data Atualização": Result = "c.data_atualizacao"; break;
            }
            return Result;
        }

        public override string GetTypeFieldParaBusca(string pCampoView)
        {
            string Result = "";
            switch (pCampoView)
            {
                case "ID Empresa": Result = "integer"; break;
                case "Empresa": Result = "String"; break;
                case "ID": Result = "integer"; break;
                case "Filial": Result = "String"; break;
                case "Descrição": Result = "String"; break;
                case "Status": Result = "String"; break;
                case "Data Criação": Result = "String"; break;
                case "Data Atualização": Result = "String"; break;
            }
            return Result;
        }
    }
}
