using EtlConexao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETLApplication.Controller
{
    class VisualizaAndamentoController
    {
        Conexao conexao;
        public VisualizaAndamentoController()
        {
            conexao = new Conexao();
        }
        public void MostrarNaGrid(int pIdPacote,int pIdArquivo,DateTime pDataInicial, DateTime pDataFinal, DataGridView dgv)
        {
            string DataIni = $"convert(datetime,'{pDataInicial.ToString("yyyy-MM-dd")} 00:00:00', 101)";
            string DataFin = $"convert(datetime,'{pDataFinal.ToString("yyyy-MM-dd")} 23:59:59',101)";
            string Periodo= $"{DataIni} and {DataFin}";
            string select = "";
            select += "select distinct src.id_pacote 'ID Pacote',src.nm_pacote 'Pacote', src.id_arquivo 'ID',src.nm_arquivo 'Nome Arquivo', src.data as 'Data Início', ";
            select += " src.ultima_data as 'Data Término', convert(time, src.ultima_data - src.data) 'Tempo',";
            select += " case when move_mensagem like '%True%' then 'SUCESSO' else 'ERRO' end as Status";
            select += " from(select p.id_pacote, p.nm_pacote, a.id_arquivo, a.nm_arquivo, l.data, l.mensagem,";
            select += " lead(l.data, 1) over(partition by l.id_arquivo order by l.data) as ultima_data,";
            select += " lead(l.mensagem, 1) over(partition by l.id_arquivo order by l.data) as ultima_mensagem,";
            select += " lead(l.data, 2) over(partition by l.id_arquivo order by l.data) as move_data,";
            select += " lead(l.mensagem, 2) over(partition by l.id_arquivo order by l.data) as move_mensagem";
            select += " from tb_logs l, tb_arquivos a, tb_pacotes p";
            select += " where l.id_arquivo = a.id_arquivo";
            select += " and a.id_pacote = p.id_pacote";
            select += " and( mensagem like 'INICIO DO ARQUIVO%'";
            select += "     or mensagem like 'TÉRMINO DO ARQUIVO%'";
            select += "     or mensagem like 'Move Para Processados(%)'";
            select += "   )";
                if (pIdPacote > 0)
                {
                    select += $" and a.id_pacote = {pIdPacote.ToString()}";
                }
                if (pIdArquivo > 0)
                {
                    select += $" and l.id_arquivo = {pIdArquivo.ToString()}";
                }
                select += $" and convert(datetime, Data,101) between {Periodo}";
            select += " ) as src";
            select += " where(src.mensagem like 'INICIO DO ARQUIVO%'";
            select += "  and src.ultima_mensagem like 'TÉRMINO DO ARQUIVO%'";
            select += "  and src.move_mensagem like 'Move Para Processados(%)')  or(src.mensagem like 'INICIO DO ARQUIVO%' and src.ultima_mensagem is null)";
            select += " order by src.ultima_data desc";

            try
            {
                conexao.GetDadosTodos(select, dgv);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao tentar apresentar os dados de visualização do andamento. Motivo: "+ex.Message);
            }
        }


        public void MostrarLogArqNaGrid(int pIdArquivo, DataGridView dgv)
        {
            string select = "";
            select = "select distinct";
            select += " a.id_arquivo 'ID',";
            select += " a.nm_arquivo 'Arquivo',";
            select += " l.Etapa,";
            select += " l.data,";
            select += " l.Mensagem,";
            select += " l.ComandoSql";
            select += " from tb_arquivos a, tb_logs l";
            select += " where l.id_arquivo = a.id_arquivo";
            select += $"  and l.id_arquivo = {pIdArquivo.ToString()}";
            select += $" Order by l.Data desc";
            try
            {
                conexao.GetDadosTodos(select, dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao tentar apresentar os dados de Log do Arquivo{pIdArquivo.ToString()}. Motivo: " + ex.Message);
            }
        }
    }

}

