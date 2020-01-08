﻿using System;
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
    public class GruposModel:GenericModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_grupo { get; set; }
        public string nm_grupo { get; set; }
        public string descr_grupos { get; set; }
        public string status { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime data_criacao { get; set; }
        public DateTime data_atualizacao { get; set; }
        public GruposModel()
        {
            NomeTabela = "tb_grupos";
        }
        public void SetDados(SqlDataReader dr)
        {
            try
            {
                id_grupo = Convert.ToInt32(isNullResultVazio(dr["id_grupo"]));
                nm_grupo = isNullResultVazio(dr["nm_grupo"]);
                descr_grupos = isNullResultVazio(dr["descr_grupos"]);
                status = isNullResultVazio(dr["Status"]);
                data_criacao = Convert.ToDateTime(dr["Data_criacao"].ToString());
                data_atualizacao = Convert.ToDateTime(dr["Data_atualizacao"].ToString());
            }
            finally
            {
                dr.Close();
            }

        }
        public List<GruposModel> GetListaPermissoes(SqlDataReader dr)
        {
            List<GruposModel> grupos = new List<GruposModel>();
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        grupos.Add
                        (
                            new GruposModel()
                            {
                                id_grupo = Convert.ToInt32(isNullResultVazio(dr["id_grupo"])),
                                nm_grupo = isNullResultVazio(dr["nm_grupo"]),
                                descr_grupos = isNullResultVazio(dr["descr_grupos"]),
                                status = isNullResultVazio(dr["Status"]),
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
            return grupos;
        }

        public override void SetDados(DataGridView dgv)
        {
            if (dgv.CurrentRow != null)
            {
                if (dgv.CurrentRow.Cells["ID"].Value.ToString() != "")
                {
                    id_grupo = Convert.ToInt32(isNullResultVazio(dgv.CurrentRow.Cells["ID"].Value));
                    nm_grupo = isNullResultVazio(dgv.CurrentRow.Cells["Empresa"].Value);
                    descr_grupos = isNullResultVazio(dgv.CurrentRow.Cells["Descrição"].Value);
                    status = isNullResultVazio(dgv.CurrentRow.Cells["status"].Value);
                    data_criacao = Convert.ToDateTime(dgv.CurrentRow.Cells["Data Criação"].Value.ToString());
                    data_atualizacao = Convert.ToDateTime(dgv.CurrentRow.Cells["Data Atualização"].Value.ToString());
                }
            }
        }

        public override void Clear()
        {
            id_grupo = 0;
            nm_grupo = "";
            descr_grupos = "";
            status = "";
            data_criacao = DateTime.Now;
            data_atualizacao = DateTime.Now;

        }

        public override void GetListaCampos()
        {
            base.GetListaCampos();
            ListaCamposView.Clear();
            ListaCamposView.Add("ID");
            ListaCamposView.Add("Empresa");
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
                case "ID": Result = "e.id_grupo"; break;
                case "Empresa": Result = "e.nm_grupo"; break;
                case "Descrição": Result = "e.descr_grupos"; break;
                case "Status": Result = "e.status"; break;
                case "Data Criação": Result = "e.data_criacao"; break;
                case "Data Atualização": Result = "e.data_atualizacao"; break;
            }
            return Result;
        }

        public override string GetTypeFieldParaBusca(string pCampoView)
        {
            string Result = "";
            switch (pCampoView)
            {
                case "ID": Result = "integer"; break;
                case "Empresa": Result = "String"; break;
                case "Descrição": Result = "String"; break;
                case "Status": Result = "String"; break;
                case "Data Criação": Result = "DateTime"; break;
                case "Data Atualização": Result = "DateTime"; break;
            }
            return Result;
        }
    }
}
