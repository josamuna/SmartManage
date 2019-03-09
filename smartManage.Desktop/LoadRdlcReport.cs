using Microsoft.Reporting.WinForms;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace smartManage.Desktop
{
    class LoadRdlcReport
    {
        private static LoadRdlcReport fact;
        private LoadRdlcReport()
        {
        }

        internal static LoadRdlcReport GetInstance()
        {
            if (fact == null)
                fact = new LoadRdlcReport();
            return fact;
        }

        internal DataSet LoadReportWithSubReportSignataire(SqlDataAdapter adapter, string dataSetName, string embeddedRessource, ReportViewer rpvReport)
        {
            DataSet dataset = new DataSet();
            dataset.Locale = CultureInfo.InvariantCulture;
            adapter.Fill(dataset, dataSetName);

            rpvReport.LocalReport.DataSources.Clear();
            rpvReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
            rpvReport.LocalReport.DataSources.Add(new ReportDataSource(dataSetName, dataset.Tables[0]));
            rpvReport.LocalReport.ReportEmbeddedResource = embeddedRessource;
            rpvReport.RefreshReport();

            return dataset;
        }

        internal void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //Initialisation de la chaine de connexion
            IDbConnection conn = new SqlConnection(Model.Properties.Settings.Default.strChaineConnexion);

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (IDbCommand cmd = conn.CreateCommand())
            {
                SqlDataAdapter adapter = null;
                DataSet dataset = null;

                try
                {
                    cmd.CommandText = @"select grade.designation + ' ' + ISNULL(personne.nom,'') + ' ' + ISNULL(personne.postnom,'') + ' ' + ISNULL(personne.prenom,'') as 'Signataire', signataire.signature_specimen as 'Signature' 
                    from signataire 
                    inner join AC on AC.code_str=signataire.code_AC
                    inner join personne on personne.id=signataire.id_personne 
                    inner join grade on grade.id=personne.id_grade
                    where signataire.code_AC=(select code_str from current_AC)";

                    SqlCommand sqlCmd = cmd as SqlCommand;
                    adapter = new SqlDataAdapter(sqlCmd);
                    dataset = new DataSet();
                    dataset.Locale = CultureInfo.InvariantCulture;
                    string dataSourceName = e.DataSourceNames[0];
                    adapter.Fill(dataset, dataSourceName);
                    e.DataSources.Add(new ReportDataSource(dataSourceName, dataset.Tables[0]));
                }
                finally
                {
                    if (dataset != null)
                        dataset.Dispose();
                    if (adapter != null)
                        adapter.Dispose();
                    if (conn != null)
                        conn.Close();
                }
            }
        }
    }
}
