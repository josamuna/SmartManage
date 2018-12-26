using ManageUtilities;
using ResourcesData;
using smartManage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmReportOrdinateur : Form
    {
        IDbConnection conn = null;
        private delegate void LoadSomeData(string ThreadName);
        private Thread tLoad = null;
        private ResourceManager stringManager = null;

        public frmReportOrdinateur()
        {
            InitializeComponent();
            //Initialisation du ressourceManager
            Assembly _assembly = Assembly.Load("ResourcesData");
            stringManager = new ResourceManager("ResourcesData.Resource", _assembly);
        }

        #region Methods For THREAD
        private void UnloadThreadRessource(Thread thread)
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }
        private void SetSelectedIndexComboBox(List<ComboBox> cbo)
        {
            foreach (ComboBox cmb in cbo)
                if (cmb.Items.Count > 0)
                    cmb.SelectedIndex = 0;
        }

        private void DoExecuteLoadData(string threadName)
        {
            cboIdentifiant.DataSource = clsMetier.GetInstance().getAllClsmateriel();
            this.setMembersallcbo(cboIdentifiant, "Code_str", "Code_str");
            cboEtat.DataSource = clsMetier.GetInstance().getAllClsetat_materiel();
            this.setMembersallcbo(cboEtat, "Designation", "Designation");
            cboDelais.DataSource = clsMetier.GetInstance().getAllClsgarantie();
            this.setMembersallcbo(cboDelais, "Valeur", "Valeur");
            cboMacWifi.DataSource = clsMetier.GetInstance().getAllClsmateriel_MAC1();
            this.setMembersallcbo(cboMacWifi, "Mac_adresse1", "Mac_adresse1");
            cboMacLAN.DataSource = clsMetier.GetInstance().getAllClsmateriel_MAC2();
            this.setMembersallcbo(cboMacLAN, "Mac_adresse2", "Mac_adresse2");

            List<ComboBox> lstCombo = new List<ComboBox>() { cboIdentifiant, cboEtat, cboDelais, cboMacWifi, cboMacLAN };

            SetSelectedIndexComboBox(lstCombo);
        }

        private void ExecuteLoadData()
        {
            LoadSomeData loadDt = new LoadSomeData(DoExecuteLoadData);

            try
            {
                this.Invoke(loadDt, "tLoad");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement des listes déroulantes, " + ex.Message, "Chargement listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
        #endregion

        private void LoadReport(int cboIndex)
        {
            //Initialisation de la chaine de connexion
            conn = new SqlConnection(Model.Properties.Settings.Default.strChaineConnexion);

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (IDbCommand cmd = conn.CreateCommand())
            {
                SqlDataAdapter adapter = null;
                DataSet dataset = null;
                Reports.LstOrdinateur rpt1 = null;
                Reports.LstOrdinateur rpt2 = null;
                Reports.LstOrdinateur rpt3 = null;
                Reports.LstOrdinateur rpt4 = null;
                Reports.LstOrdinateur rpt5 = null;
                Reports.LstOrdinateur rpt6 = null;
                Reports.LstOrdinateur rpt7 = null;

                try
                {
                    switch (cboIndex)
                    {
                        case 0:
                            if (rdLstIdentifiant.Checked)
                            {
                                //Ordinateur par identifiant equipement
                                rpt1 = new Reports.LstOrdinateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
                                left outer join garantie on garantie.id=materiel.id_garantie
                                left outer join categorie_materiel on categorie_materiel.id=materiel.id_categorie_materiel
                                inner join compte on compte.id=materiel.id_compte
                                inner join marque on marque.id=materiel.id_marque
                                inner join modele on modele.id=materiel.id_modele
                                inner join couleur on couleur.id=materiel.id_couleur
                                inner join poids on poids.id=materiel.id_poids
                                inner join etat_materiel on etat_materiel.id=materiel.id_etat_materiel
                                left outer join type_ordinateur on type_ordinateur.id=materiel.id_type_ordinateur
                                left outer join type_clavier on type_clavier.id=materiel.id_type_clavier
                                left outer join OS on OS.id=materiel.id_OS
                                left outer join ram on ram.id=materiel.id_ram
                                left outer join processeur on processeur.id=materiel.id_processeur
                                left outer join nombre_coeur_processeur on nombre_coeur_processeur.id=materiel.id_nombre_coeur_processeur
                                left outer join type_hdd on type_hdd.id=materiel.id_type_hdd
                                left outer join nombre_hdd on nombre_hdd.id=materiel.id_nombre_hdd
                                left outer join capacite_hdd on capacite_hdd.id=materiel.id_capacite_hdd
                                left outer join taille_ecran on taille_ecran.id=materiel.id_taille_ecran
                                left outer join usb2 on usb2.id=materiel.id_usb2
                                left outer join usb3 on usb3.id=materiel.id_usb3
                                left outer join hdmi on hdmi.id=materiel.id_hdmi
                                left outer join vga on vga.id=materiel.id_vga
                                left outer join tension_batterie on tension_batterie.id=materiel.id_tension_batterie
                                left outer join tension_adaptateur on tension_adaptateur.id=materiel.id_tension_adaptateur
                                left outer join puissance_adaptateur on puissance_adaptateur.id=materiel.id_puissance_adaptateur
                                left outer join intensite_adaptateur on intensite_adaptateur.id=materiel.id_intensite_adaptateur
                                where  categorie_materiel.designation='Ordinateur' and materiel.code_str=@code_str and materiel.archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt1.SetDataSource(dataset.Tables["lstTable"]);
                                crvReport.ReportSource = rpt1;
                                crvReport.Refresh();
                            }
                            else if (rdLstEtat.Checked)
                            {
                                //Ordinateur par etat de l'equipement
                                rpt2 = new Reports.LstOrdinateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
                                left outer join garantie on garantie.id=materiel.id_garantie
                                left outer join categorie_materiel on categorie_materiel.id=materiel.id_categorie_materiel
                                inner join compte on compte.id=materiel.id_compte
                                inner join marque on marque.id=materiel.id_marque
                                inner join modele on modele.id=materiel.id_modele
                                inner join couleur on couleur.id=materiel.id_couleur
                                inner join poids on poids.id=materiel.id_poids
                                inner join etat_materiel on etat_materiel.id=materiel.id_etat_materiel
                                left outer join type_ordinateur on type_ordinateur.id=materiel.id_type_ordinateur
                                left outer join type_clavier on type_clavier.id=materiel.id_type_clavier
                                left outer join OS on OS.id=materiel.id_OS
                                left outer join ram on ram.id=materiel.id_ram
                                left outer join processeur on processeur.id=materiel.id_processeur
                                left outer join nombre_coeur_processeur on nombre_coeur_processeur.id=materiel.id_nombre_coeur_processeur
                                left outer join type_hdd on type_hdd.id=materiel.id_type_hdd
                                left outer join nombre_hdd on nombre_hdd.id=materiel.id_nombre_hdd
                                left outer join capacite_hdd on capacite_hdd.id=materiel.id_capacite_hdd
                                left outer join taille_ecran on taille_ecran.id=materiel.id_taille_ecran
                                left outer join usb2 on usb2.id=materiel.id_usb2
                                left outer join usb3 on usb3.id=materiel.id_usb3
                                left outer join hdmi on hdmi.id=materiel.id_hdmi
                                left outer join vga on vga.id=materiel.id_vga
                                left outer join tension_batterie on tension_batterie.id=materiel.id_tension_batterie
                                left outer join tension_adaptateur on tension_adaptateur.id=materiel.id_tension_adaptateur
                                left outer join puissance_adaptateur on puissance_adaptateur.id=materiel.id_puissance_adaptateur
                                left outer join intensite_adaptateur on intensite_adaptateur.id=materiel.id_intensite_adaptateur
                                where categorie_materiel.designation='Ordinateur' and etat_materiel.designation=@designation and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt2.SetDataSource(dataset.Tables["lstTable"]);
                                crvReport.ReportSource = rpt2;
                                crvReport.Refresh();
                            }
                            else if (rdLstEndGarantie.Checked)
                            {
                                //Ordinateur par délais de garantie de l'equipement
                                rpt3 = new Reports.LstOrdinateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
                                left outer join garantie on garantie.id=materiel.id_garantie
                                left outer join categorie_materiel on categorie_materiel.id=materiel.id_categorie_materiel
                                inner join compte on compte.id=materiel.id_compte
                                inner join marque on marque.id=materiel.id_marque
                                inner join modele on modele.id=materiel.id_modele
                                inner join couleur on couleur.id=materiel.id_couleur
                                inner join poids on poids.id=materiel.id_poids
                                inner join etat_materiel on etat_materiel.id=materiel.id_etat_materiel
                                left outer join type_ordinateur on type_ordinateur.id=materiel.id_type_ordinateur
                                left outer join type_clavier on type_clavier.id=materiel.id_type_clavier
                                left outer join OS on OS.id=materiel.id_OS
                                left outer join ram on ram.id=materiel.id_ram
                                left outer join processeur on processeur.id=materiel.id_processeur
                                left outer join nombre_coeur_processeur on nombre_coeur_processeur.id=materiel.id_nombre_coeur_processeur
                                left outer join type_hdd on type_hdd.id=materiel.id_type_hdd
                                left outer join nombre_hdd on nombre_hdd.id=materiel.id_nombre_hdd
                                left outer join capacite_hdd on capacite_hdd.id=materiel.id_capacite_hdd
                                left outer join taille_ecran on taille_ecran.id=materiel.id_taille_ecran
                                left outer join usb2 on usb2.id=materiel.id_usb2
                                left outer join usb3 on usb3.id=materiel.id_usb3
                                left outer join hdmi on hdmi.id=materiel.id_hdmi
                                left outer join vga on vga.id=materiel.id_vga
                                left outer join tension_batterie on tension_batterie.id=materiel.id_tension_batterie
                                left outer join tension_adaptateur on tension_adaptateur.id=materiel.id_tension_adaptateur
                                left outer join puissance_adaptateur on puissance_adaptateur.id=materiel.id_puissance_adaptateur
                                left outer join intensite_adaptateur on intensite_adaptateur.id=materiel.id_intensite_adaptateur
                                where categorie_materiel.designation='Ordinateur' and garantie.valeur=@valeur and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt3.SetDataSource(dataset.Tables["lstTable"]);
                                crvReport.ReportSource = rpt3;
                                crvReport.Refresh();
                            }
                            else if (rdLstMAC.Checked)
                            {
                                //Ordinateur par MAC Wifi de l'equipement
                                rpt4 = new Reports.LstOrdinateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
                                left outer join garantie on garantie.id=materiel.id_garantie
                                left outer join categorie_materiel on categorie_materiel.id=materiel.id_categorie_materiel
                                inner join compte on compte.id=materiel.id_compte
                                inner join marque on marque.id=materiel.id_marque
                                inner join modele on modele.id=materiel.id_modele
                                inner join couleur on couleur.id=materiel.id_couleur
                                inner join poids on poids.id=materiel.id_poids
                                inner join etat_materiel on etat_materiel.id=materiel.id_etat_materiel
                                left outer join type_ordinateur on type_ordinateur.id=materiel.id_type_ordinateur
                                left outer join type_clavier on type_clavier.id=materiel.id_type_clavier
                                left outer join OS on OS.id=materiel.id_OS
                                left outer join ram on ram.id=materiel.id_ram
                                left outer join processeur on processeur.id=materiel.id_processeur
                                left outer join nombre_coeur_processeur on nombre_coeur_processeur.id=materiel.id_nombre_coeur_processeur
                                left outer join type_hdd on type_hdd.id=materiel.id_type_hdd
                                left outer join nombre_hdd on nombre_hdd.id=materiel.id_nombre_hdd
                                left outer join capacite_hdd on capacite_hdd.id=materiel.id_capacite_hdd
                                left outer join taille_ecran on taille_ecran.id=materiel.id_taille_ecran
                                left outer join usb2 on usb2.id=materiel.id_usb2
                                left outer join usb3 on usb3.id=materiel.id_usb3
                                left outer join hdmi on hdmi.id=materiel.id_hdmi
                                left outer join vga on vga.id=materiel.id_vga
                                left outer join tension_batterie on tension_batterie.id=materiel.id_tension_batterie
                                left outer join tension_adaptateur on tension_adaptateur.id=materiel.id_tension_adaptateur
                                left outer join puissance_adaptateur on puissance_adaptateur.id=materiel.id_puissance_adaptateur
                                left outer join intensite_adaptateur on intensite_adaptateur.id=materiel.id_intensite_adaptateur
                                where categorie_materiel.designation='Ordinateur' and materiel.mac_adresse1 LIKE @mac_adresse1 and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse1", DbType.String, 20, cboMacWifi.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt4.SetDataSource(dataset.Tables["lstTable"]);
                                crvReport.ReportSource = rpt4;
                                crvReport.Refresh();
                            }
                            else if (rdLstDateAcquisition.Checked)
                            {
                                //Ordinateur par date d'acquisition de l'equipement
                                rpt5 = new Reports.LstOrdinateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',convert(varchar(30),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
                                left outer join garantie on garantie.id=materiel.id_garantie
                                left outer join categorie_materiel on categorie_materiel.id=materiel.id_categorie_materiel
                                inner join compte on compte.id=materiel.id_compte
                                inner join marque on marque.id=materiel.id_marque
                                inner join modele on modele.id=materiel.id_modele
                                inner join couleur on couleur.id=materiel.id_couleur
                                inner join poids on poids.id=materiel.id_poids
                                inner join etat_materiel on etat_materiel.id=materiel.id_etat_materiel
                                left outer join type_ordinateur on type_ordinateur.id=materiel.id_type_ordinateur
                                left outer join type_clavier on type_clavier.id=materiel.id_type_clavier
                                left outer join OS on OS.id=materiel.id_OS
                                left outer join ram on ram.id=materiel.id_ram
                                left outer join processeur on processeur.id=materiel.id_processeur
                                left outer join nombre_coeur_processeur on nombre_coeur_processeur.id=materiel.id_nombre_coeur_processeur
                                left outer join type_hdd on type_hdd.id=materiel.id_type_hdd
                                left outer join nombre_hdd on nombre_hdd.id=materiel.id_nombre_hdd
                                left outer join capacite_hdd on capacite_hdd.id=materiel.id_capacite_hdd
                                left outer join taille_ecran on taille_ecran.id=materiel.id_taille_ecran
                                left outer join usb2 on usb2.id=materiel.id_usb2
                                left outer join usb3 on usb3.id=materiel.id_usb3
                                left outer join hdmi on hdmi.id=materiel.id_hdmi
                                left outer join vga on vga.id=materiel.id_vga
                                left outer join tension_batterie on tension_batterie.id=materiel.id_tension_batterie
                                left outer join tension_adaptateur on tension_adaptateur.id=materiel.id_tension_adaptateur
                                left outer join puissance_adaptateur on puissance_adaptateur.id=materiel.id_puissance_adaptateur
                                left outer join intensite_adaptateur on intensite_adaptateur.id=materiel.id_intensite_adaptateur
                                where  categorie_materiel.designation='Ordinateur' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);
                                //Console.WriteLine(txtDateAcquisitionDebut.Text);
                                //Console.WriteLine(txtDateAcquisitionFin.Text);
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, txtDateAcquisitionDebut.Text));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, txtDateAcquisitionFin.Text));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt5.SetDataSource(dataset.Tables["lstTable"]);
                                crvReport.ReportSource = rpt5;
                                crvReport.Refresh();
                            }
                            
                            break;
                        case 1:
                            if (rdLstMAC.Checked)
                            {
                                //par MAC LAN de l'equipement
                                rpt6 = new Reports.LstOrdinateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
                                left outer join garantie on garantie.id=materiel.id_garantie
                                left outer join categorie_materiel on categorie_materiel.id=materiel.id_categorie_materiel
                                inner join compte on compte.id=materiel.id_compte
                                inner join marque on marque.id=materiel.id_marque
                                inner join modele on modele.id=materiel.id_modele
                                inner join couleur on couleur.id=materiel.id_couleur
                                inner join poids on poids.id=materiel.id_poids
                                inner join etat_materiel on etat_materiel.id=materiel.id_etat_materiel
                                left outer join type_ordinateur on type_ordinateur.id=materiel.id_type_ordinateur
                                left outer join type_clavier on type_clavier.id=materiel.id_type_clavier
                                left outer join OS on OS.id=materiel.id_OS
                                left outer join ram on ram.id=materiel.id_ram
                                left outer join processeur on processeur.id=materiel.id_processeur
                                left outer join nombre_coeur_processeur on nombre_coeur_processeur.id=materiel.id_nombre_coeur_processeur
                                left outer join type_hdd on type_hdd.id=materiel.id_type_hdd
                                left outer join nombre_hdd on nombre_hdd.id=materiel.id_nombre_hdd
                                left outer join capacite_hdd on capacite_hdd.id=materiel.id_capacite_hdd
                                left outer join taille_ecran on taille_ecran.id=materiel.id_taille_ecran
                                left outer join usb2 on usb2.id=materiel.id_usb2
                                left outer join usb3 on usb3.id=materiel.id_usb3
                                left outer join hdmi on hdmi.id=materiel.id_hdmi
                                left outer join vga on vga.id=materiel.id_vga
                                left outer join tension_batterie on tension_batterie.id=materiel.id_tension_batterie
                                left outer join tension_adaptateur on tension_adaptateur.id=materiel.id_tension_adaptateur
                                left outer join puissance_adaptateur on puissance_adaptateur.id=materiel.id_puissance_adaptateur
                                left outer join intensite_adaptateur on intensite_adaptateur.id=materiel.id_intensite_adaptateur
                                where categorie_materiel.designation='Ordinateur' and materiel.mac_adresse2 LIKE @mac_adresse2 and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse2", DbType.String, 20, cboMacLAN.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt6.SetDataSource(dataset.Tables["lstTable"]);
                                crvReport.ReportSource = rpt6;
                                crvReport.Refresh();
                            }

                            break;

                        case 2:
                            if (rdLstMAC.Checked)
                            {
                                //par MAC Wifi et LAN de l'equipement 
                                rpt7 = new Reports.LstOrdinateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
                                left outer join garantie on garantie.id=materiel.id_garantie
                                left outer join categorie_materiel on categorie_materiel.id=materiel.id_categorie_materiel
                                inner join compte on compte.id=materiel.id_compte
                                inner join marque on marque.id=materiel.id_marque
                                inner join modele on modele.id=materiel.id_modele
                                inner join couleur on couleur.id=materiel.id_couleur
                                inner join poids on poids.id=materiel.id_poids
                                inner join etat_materiel on etat_materiel.id=materiel.id_etat_materiel
                                left outer join type_ordinateur on type_ordinateur.id=materiel.id_type_ordinateur
                                left outer join type_clavier on type_clavier.id=materiel.id_type_clavier
                                left outer join OS on OS.id=materiel.id_OS
                                left outer join ram on ram.id=materiel.id_ram
                                left outer join processeur on processeur.id=materiel.id_processeur
                                left outer join nombre_coeur_processeur on nombre_coeur_processeur.id=materiel.id_nombre_coeur_processeur
                                left outer join type_hdd on type_hdd.id=materiel.id_type_hdd
                                left outer join nombre_hdd on nombre_hdd.id=materiel.id_nombre_hdd
                                left outer join capacite_hdd on capacite_hdd.id=materiel.id_capacite_hdd
                                left outer join taille_ecran on taille_ecran.id=materiel.id_taille_ecran
                                left outer join usb2 on usb2.id=materiel.id_usb2
                                left outer join usb3 on usb3.id=materiel.id_usb3
                                left outer join hdmi on hdmi.id=materiel.id_hdmi
                                left outer join vga on vga.id=materiel.id_vga
                                left outer join tension_batterie on tension_batterie.id=materiel.id_tension_batterie
                                left outer join tension_adaptateur on tension_adaptateur.id=materiel.id_tension_adaptateur
                                left outer join puissance_adaptateur on puissance_adaptateur.id=materiel.id_puissance_adaptateur
                                left outer join intensite_adaptateur on intensite_adaptateur.id=materiel.id_intensite_adaptateur
                                where categorie_materiel.designation='Ordinateur' and (materiel.mac_adresse1 LIKE @mac_adresse1 or materiel.mac_adresse2 LIKE @mac_adresse2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse1", DbType.String, 20, cboMacWifi.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse2", DbType.String, 20, cboMacLAN.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt7.SetDataSource(dataset.Tables["lstTable"]);
                                crvReport.ReportSource = rpt7;
                                crvReport.Refresh();
                            }

                            break;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                }
                finally
                {
                    if (rpt1 != null)
                        rpt1.Dispose();
                    if (rpt2 != null)
                        rpt2.Dispose();
                    if (rpt3 != null)
                        rpt3.Dispose();
                    if (rpt4 != null)
                        rpt4.Dispose();
                    if (rpt5 != null)
                        rpt5.Dispose();
                    if (rpt6 != null)
                        rpt6.Dispose();
                    if (rpt7 != null)
                        rpt7.Dispose();

                    dataset.Dispose();
                    adapter.Dispose();
                    conn.Close();
                }
            }
        }

        private void cmdView_Click(object sender, EventArgs e)
        {
            try
            {
                int index = cboItems.SelectedIndex;
                if (!string.IsNullOrEmpty(cboItems.SelectedItem.ToString()) && !string.IsNullOrEmpty(cboIdentifiant.SelectedItem.ToString()) && !string.IsNullOrEmpty(cboEtat.Text) &&
                    !string.IsNullOrEmpty(cboDelais.Text) && !string.IsNullOrEmpty(cboMacWifi.Text) && !string.IsNullOrEmpty(cboMacLAN.Text))
                {
                    if (index >= 0)
                    {
                        LoadReport(index);
                    }
                    else
                        throw new CustomException("La sélection de l'élément du rapport est invalide !!");
                }
                else
                    throw new CustomException("Veuillez vérifier que toutes les listes déroulantes ne sont pas vides");
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void setMembersallcbo(ComboBox cbo, string displayMember, string valueMember)
        {
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
        }

        private void rdLstEtat_CheckedChanged(object sender, EventArgs e)
        {
            cboItems.Items.Clear();
            cboItems.Items.Add("Par etat de l'équipement");
            cboItems.Sorted = false;
            cboItems.SelectedIndex = 0;

            cboEtat.Enabled = true;
            cboIdentifiant.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;

            chkArchiver.Checked = false;
        }

        private void rdLstEndGarantie_CheckedChanged(object sender, EventArgs e)
        {
            cboItems.Items.Clear();
            cboItems.Items.Add("Par délais de garantie de l'équipement");
            cboItems.Sorted = false;
            cboItems.SelectedIndex = 0;

            cboIdentifiant.Enabled = false;
            cboEtat.Enabled = false;
            cboDelais.Enabled = true;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;

            chkArchiver.Checked = false;
        }

        private void rdLstMAC_CheckedChanged(object sender, EventArgs e)
        {
            cboItems.Items.Clear();
            cboItems.Items.Add("Par MAC Adresse Wifi");
            cboItems.Items.Add("Par MAC Adresse LAN");
            cboItems.Items.Add("Par MAC Adresse Wifi ou LAN");
            cboItems.Sorted = false;
            cboItems.SelectedIndex = 0;

            cboIdentifiant.Enabled = false;
            cboEtat.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = true;
            cboMacLAN.Enabled = true;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;

            chkArchiver.Checked = false;
        }

        private void frmReportOrdinateur_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.UnloadThreadRessource(tLoad);
            }
            catch { }
        }

        private void frmReportOrdinateur_Load(object sender, EventArgs e)
        {
            try
            {
                if (tLoad == null)
                {
                    tLoad = new Thread(new ThreadStart(ExecuteLoadData));
                    tLoad.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lor du chargement des listes déroulantes, {0}", ex.Message), "Chargement listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

            try
            {
                SafeNativeMethods.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)(-1), (UIntPtr)(-1));
            }
            catch (DllNotFoundException ex)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void rdLstIdentifiant_CheckedChanged(object sender, EventArgs e)
        {
            cboItems.Items.Clear();
            cboItems.Items.Add("Par identifiant équipement");
            cboItems.Sorted = false;
            cboItems.SelectedIndex = 0;

            cboIdentifiant.Enabled = true;
            cboEtat.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;

            chkArchiver.Checked = false;
        }

        private void rdLstDateAcquisition_CheckedChanged(object sender, EventArgs e)
        {
            cboItems.Items.Clear();
            cboItems.Items.Add("Par date aquisition de l'équipement");
            cboItems.Sorted = false;
            cboItems.SelectedIndex = 0;

            cboIdentifiant.Enabled = false;
            cboEtat.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = true;
            txtDateAcquisitionFin.Enabled = true;

            chkArchiver.Checked = false;
        }
    }
}
