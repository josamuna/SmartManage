using ManageUtilities;
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
using Microsoft.Reporting.WinForms;

namespace smartManage.Desktop
{
    public partial class A : Form
    {
        IDbConnection conn = null;
        private delegate void LoadSomeData(string ThreadName);
        private Thread tLoad = null;
        private ResourceManager stringManager = null;

        public A()
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
            cboCategorieMat.DataSource = clsMetier.GetInstance().getAllClscategorie_materiel1();
            this.setMembersallcbo(cboCategorieMat, "Designation", "Id");
            cboPortee.DataSource = clsMetier.GetInstance().getAllClsportee(); 
            this.setMembersallcbo(cboPortee, "Valeur", "Valeur");
            cboFrequence.DataSource = clsMetier.GetInstance().getAllClsfrequence();
            this.setMembersallcbo(cboFrequence, "Designation", "Designation");
            cboNetete.DataSource = clsMetier.GetInstance().getAllClsnetette();
            this.setMembersallcbo(cboNetete, "Designation", "Designation");
            cboPPM.DataSource = clsMetier.GetInstance().getAllClspage_par_minute();
            this.setMembersallcbo(cboPPM, "Valeur", "Valeur");
            cboMACAdresse.DataSource = clsMetier.GetInstance().getAllClsmateriel_MAC1();
            this.setMembersallcbo(cboMACAdresse, "Mac_adresse1", "Mac_adresse1");

            List<ComboBox> lstCombo = new List<ComboBox>() { cboIdentifiant, cboEtat, cboDelais, cboMacWifi, cboMacLAN,
                cboCategorieMat, cboPortee, cboFrequence, cboNetete, cboPPM, cboMACAdresse };

            SetSelectedIndexComboBox(lstCombo);
        }

        private void ExecuteLoadData()
        {
            LoadSomeData loadDt = new LoadSomeData(DoExecuteLoadData);

            try
            {
                this.Invoke(loadDt, "tLoad");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche dans le thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche dans le thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }
        #endregion

        private void LoadReport(int cboIndex, int categorie, Control radiobutton)
        {
            //Initialisation de la chaine de connexion
            conn = new SqlConnection(Model.Properties.Settings.Default.strChaineConnexion);

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            switch(categorie)
            {
                case 0:
                    //*****Choix pour Ordinateur
                    #region Choix pour Ordinateur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Ordinateur par identifiant equipement
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where  categorie_materiel.designation='Ordinateur' and materiel.code_str=@code_str and materiel.archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Ordinateur par etat de l'equipement

                                cmd.CommandText = @"select materiel.id,materiel.code_str as 'Code',categorie_materiel.designation as 'CatMat',compte.numero as 'Numero',CONVERT(varchar(10),date_acquisition,3) as 'DateAcq',garantie.valeur as 'Garantie',marque.designation as 'Marque',
                                modele.designation as 'Modele',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MacWifi', materiel.mac_adresse2 as 'MacLAN',type_ordinateur.designation as 'TypeOrdi',type_clavier.designation as 'Clavier',OS.designation 'SystemeExpl',ram.id as 'Memoire',processeur.valeur as 'Processeur',
                                nombre_coeur_processeur.valeur as 'CoeurProcesseur',type_hdd.designation as 'TypeHDD',nombre_hdd.valeur as 'NbrHDD',capacite_hdd.valeur as 'CapaciteHDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB20',usb3.valeur as 'USB30',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'UBat',tension_adaptateur.valeur as 'UAdapt',puissance_adaptateur.valeur as 'PAdapt',materiel.numero_cle as 'Numerocle', intensite_adaptateur.valeur as 'IAdapt', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'TypeSwitch',

                                puissance.valeur as 'PImp',intensite.valeur as 'IImp',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'TypeImprimante',

                                tension_alimentation.valeur as 'UAlim',usb.valeur as 'NbrUSB',memoire.valeur as 'NbrMemoire',sorties_audio.valeur as 'NbrSortiesAud',microphone.valeur as 'NbrMicro',gain.valeur as 'Gain',type_amplificateur.designation as 'TypeAmplificateur',

                                gbe.valeur as 'NbrGbe',fe.valeur as 'NbrFe',fo.valeur as 'NbrFo',serial.valeur as 'NbrSerial',default_pwd.designation as 'DefaultPwd',default_ip.designation as 'DefaultIP',console.valeur as 'NbrConsole',auxiliaire.valeur as 'NbrAux',materiel.capable_usb as 'SupportUSB', type_routeur_AP.designation as 'TyperouteurAP', version_ios.designation as 'VersionIOS',

                                portee.valeur as 'Portee',type_AP.designation as 'TypeAP',

                                frequence.designation as 'Frequence',antenne.valeur as 'NbrAnt',

                                netette.designation as 'Netete',materiel.compatible_wifi as 'SupportWifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Ordinateur' and etat_materiel.designation=@designation and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadReportWithSubReport(adapter, "DataSet1", "smartManage.Desktop.Reports.RptOrdinateur.rdlc");
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Ordinateur par délais de garantie de l'equipement
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Ordinateur' and garantie.valeur=@valeur and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                //rpvReport.ReportSource = rpt3;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstMAC.Name))
                            {
                                #region Ordinateur par MAC Wifi de l'equipement
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Ordinateur' and materiel.mac_adresse1 LIKE @mac_adresse1 and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse1", DbType.String, 20, cboMacWifi.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                //rpvReport.ReportSource = rpt4;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Ordinateur par date d'acquisition de l'equipement
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Ordinateur' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);
                                //Console.WriteLine(txtDateAcquisitionDebut.Text);
                                //Console.WriteLine(txtDateAcquisitionFin.Text);
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                //rpvReport.ReportSource = rpt5;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 1 && rd.Name.Equals(rdLstMAC.Name))
                            {
                                #region par MAC LAN de l'equipement
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Ordinateur' and materiel.mac_adresse2 LIKE @mac_adresse2 and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse2", DbType.String, 20, cboMacLAN.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                //rpvReport.ReportSource = rpt6;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 2 && rd.Name.Equals(rdLstMAC.Name))
                            {
                                #region par MAC Wifi et LAN de l'equipement 
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Ordinateur' and (materiel.mac_adresse1 LIKE @mac_adresse1 or materiel.mac_adresse2 LIKE @mac_adresse2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse1", DbType.String, 20, cboMacWifi.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse2", DbType.String, 20, cboMacLAN.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpvReport.Refresh();
                                #endregion
                            }

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 1:
                    //*****Choix pour Switch
                    #region Choix pour Switch
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;
                            Reports.LstSwitch rpt1 = null;
                            Reports.LstSwitch rpt2 = null;
                            Reports.LstSwitch rpt3 = null;
                            Reports.LstSwitch rpt4 = null;
                            Reports.LstSwitch rpt5 = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Switch par identifiant equipement
                                rpt1 = new Reports.LstSwitch();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where  categorie_materiel.designation='Switch' and materiel.code_str=@code_str and materiel.archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt1.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt1;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Switch par etat de l'equipement
                                rpt2 = new Reports.LstSwitch();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Switch' and etat_materiel.designation=@designation and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt2.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt2;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Switch par délais de garantie de l'equipement
                                rpt3 = new Reports.LstSwitch();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Switch' and garantie.valeur=@valeur and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt3.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt3;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstMAC_Adresse.Name))
                            {
                                #region Switch par MAC l'equipement
                                rpt4 = new Reports.LstSwitch();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Switch' and materiel.mac_adresse1 LIKE @mac_adresse1 and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse1", DbType.String, 20, cboMACAdresse.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt4.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt4;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Switch par date d'acquisition de l'equipement
                                rpt5 = new Reports.LstSwitch();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Switch' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt5.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt5;
                                rpvReport.Refresh();
                                #endregion
                            }

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

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 2:
                    //*****Choix pour Imprimante
                    #region Choix pour Imprimante
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;
                            Reports.LstImprimante rpt1 = null;
                            Reports.LstImprimante rpt2 = null;
                            Reports.LstImprimante rpt3 = null;
                            Reports.LstImprimante rpt4 = null;
                            Reports.LstImprimante rpt5 = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Imprimante par identifiant equipement
                                rpt1 = new Reports.LstImprimante();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where  categorie_materiel.designation='Imprimante' and materiel.code_str=@code_str and materiel.archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt1.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt1;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Imprimante par etat de l'equipement
                                rpt2 = new Reports.LstImprimante();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Imprimante' and etat_materiel.designation=@designation and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt2.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt2;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Imprimante par délais de garantie de l'equipement
                                rpt3 = new Reports.LstImprimante();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Imprimante' and garantie.valeur=@valeur and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt3.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt3;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstPPM.Name))
                            {
                                #region Imprimante par PPM l'equipement
                                rpt4 = new Reports.LstImprimante();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Imprimante' and page_par_minute.valeur=@page_par_minute and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@page_par_minute", DbType.Int32, 4, Convert.ToInt32(cboPPM.SelectedValue, CultureInfo.InvariantCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt4.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt4;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Imprimante par date d'acquisition de l'equipement
                                rpt5 = new Reports.LstImprimante();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                    from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Imprimante' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt5.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt5;
                                rpvReport.Refresh();
                                #endregion
                            }

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

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 3:
                    //*****Choix pour Emetteur
                    #region Choix pour Emetteur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;
                            Reports.LstEmetteur rpt1 = null;
                            Reports.LstEmetteur rpt2 = null;
                            Reports.LstEmetteur rpt3 = null;
                            Reports.LstEmetteur rpt4 = null;
                            Reports.LstEmetteur rpt5 = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Emetteur par identifiant equipement
                                rpt1 = new Reports.LstEmetteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where  categorie_materiel.designation='Emetteur' and materiel.code_str=@code_str and materiel.archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt1.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt1;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Emetteur par etat de l'equipement
                                rpt2 = new Reports.LstEmetteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Emetteur' and etat_materiel.designation=@designation and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt2.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt2;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Emetteur par délais de garantie de l'equipement
                                rpt3 = new Reports.LstEmetteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Emetteur' and garantie.valeur=@valeur and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt3.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt3;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstFrequence.Name))
                            {
                                #region Emetteur par frequence de fonctionnement l'equipement
                                rpt4 = new Reports.LstEmetteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                    from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Emetteur' and frequence.designation=@frequence and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@frequence", DbType.String, 20, cboFrequence.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt4.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt4;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Emetteur par date d'acquisition de l'equipement
                                rpt5 = new Reports.LstEmetteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Emetteur' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt5.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt5;
                                rpvReport.Refresh();
                                #endregion
                            }

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

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 4:
                    //*****Choix pour Amplificateur
                    #region Choix pour Amplificateur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;
                            Reports.LstAmplificateur rpt1 = null;
                            Reports.LstAmplificateur rpt2 = null;
                            Reports.LstAmplificateur rpt3 = null;
                            Reports.LstAmplificateur rpt4 = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Amplificateur par identifiant equipement
                                rpt1 = new Reports.LstAmplificateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where  categorie_materiel.designation='Emetteur' and materiel.code_str=@code_str and materiel.archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt1.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt1;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Amplificateur par etat de l'equipement
                                rpt2 = new Reports.LstAmplificateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Amplificateur' and etat_materiel.designation=@designation and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt2.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt2;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Amplificateur par délais de garantie de l'equipement
                                rpt3 = new Reports.LstAmplificateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Amplificateur' and garantie.valeur=@valeur and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt3.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt3;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Amplificateur par date d'acquisition de l'equipement
                                rpt4 = new Reports.LstAmplificateur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Amplificateur' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt4.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt4;
                                rpvReport.Refresh();
                                #endregion
                            }

                            if (rpt1 != null)
                                rpt1.Dispose();
                            if (rpt2 != null)
                                rpt2.Dispose();
                            if (rpt3 != null)
                                rpt3.Dispose();
                            if (rpt4 != null)
                                rpt4.Dispose();

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 5:
                    //*****Choix pour Retroprojecteur
                    #region Choix pour Retroprojecteur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;
                            Reports.LstRetroprojecteur rpt1 = null;
                            Reports.LstRetroprojecteur rpt2 = null;
                            Reports.LstRetroprojecteur rpt3 = null;
                            Reports.LstRetroprojecteur rpt4 = null;
                            Reports.LstRetroprojecteur rpt5 = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Retroprojecteur par identifiant equipement
                                rpt1 = new Reports.LstRetroprojecteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where  categorie_materiel.designation='Retroprojecteur' and materiel.code_str=@code_str and materiel.archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt1.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt1;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Retroprojecteur par etat de l'equipement
                                rpt2 = new Reports.LstRetroprojecteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Retroprojecteur' and etat_materiel.designation=@designation and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt2.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt2;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Retroprojecteur par délais de garantie de l'equipement
                                rpt3 = new Reports.LstRetroprojecteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Retroprojecteur' and garantie.valeur=@valeur and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt3.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt3;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstNetete.Name))
                            {
                                #region Retroprojecteur par netete de l'equipement
                                rpt4 = new Reports.LstRetroprojecteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Retroprojecteur' and netette.designation=@netette and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@netette", DbType.String, 20, cboNetete.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt4.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt4;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Retroprojecteur par date d'acquisition de l'equipement
                                rpt5 = new Reports.LstRetroprojecteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Retroprojecteur' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt5.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt5;
                                rpvReport.Refresh();
                                #endregion
                            }

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

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 6:
                    //*****Choix pour Routeur_Access Point
                    #region Choix pour Routeur_Access Point
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;
                            Reports.LstRouteurAP rpt1 = null;
                            Reports.LstRouteurAP rpt2 = null;
                            Reports.LstRouteurAP rpt3 = null;
                            Reports.LstRouteurAP rpt4 = null;
                            Reports.LstRouteurAP rpt5 = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Routeur_Access Point par identifiant equipement
                                rpt1 = new Reports.LstRouteurAP();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where  categorie_materiel.designation='Routeur_Access Point' and materiel.code_str=@code_str and materiel.archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt1.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt1;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Routeur_Access Point par etat de l'equipement
                                rpt2 = new Reports.LstRouteurAP();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Routeur_Access Point' and etat_materiel.designation=@designation and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt2.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt2;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Routeur_Access Point par délais de garantie de l'equipement
                                rpt3 = new Reports.LstRouteurAP();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Routeur_Access Point' and garantie.valeur=@valeur and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt3.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt3;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstMAC_Adresse.Name))
                            {
                                #region Routeur_Access Point par MAC l'equipement
                                rpt4 = new Reports.LstRouteurAP();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Routeur_Access Point' and materiel.mac_adresse1 LIKE @mac_adresse1 and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse1", DbType.String, 20, cboMACAdresse.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt4.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt4;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Routeur_Access Point par date d'acquisition de l'equipement
                                rpt5 = new Reports.LstRouteurAP();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Routeur_Access Point' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt5.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt5;
                                rpvReport.Refresh();
                                #endregion
                            }

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

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 7:
                    //*****Choix pour Access Point
                    #region Choix pour Emetteur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;
                            Reports.LstEmetteur rpt1 = null;
                            Reports.LstEmetteur rpt2 = null;
                            Reports.LstEmetteur rpt3 = null;
                            Reports.LstEmetteur rpt4 = null;
                            Reports.LstEmetteur rpt5 = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Access Point par identifiant equipement
                                rpt1 = new Reports.LstEmetteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where  categorie_materiel.designation='Access Point' and materiel.code_str=@code_str and materiel.archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt1.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt1;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Access Point par etat de l'equipement
                                rpt2 = new Reports.LstEmetteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Access Point' and etat_materiel.designation=@designation and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt2.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt2;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Access Point par délais de garantie de l'equipement
                                rpt3 = new Reports.LstEmetteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Access Point' and garantie.valeur=@valeur and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt3.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt3;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstPortee.Name))
                            {
                                #region Access Point par portee l'equipement
                                rpt4 = new Reports.LstEmetteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Access Point' and portee.valeur=@portee and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@portee", DbType.Int32, 4, Convert.ToInt32(cboPortee.SelectedValue, CultureInfo.InvariantCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt4.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt4;
                                rpvReport.Refresh();
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Access Point par date d'acquisition de l'equipement
                                rpt5 = new Reports.LstEmetteur();
                                cmd.CommandText = @"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
                                modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
                                materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
                                nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
                                vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
                                materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

                                type_switch.designation as 'Type Switch',

                                puissance.valeur as 'P.Imp.(W)',intensite.valeur as 'I.Imp.(A)',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'Type Imprimante',

                                tension_alimentation.valeur as 'U.Alim.(V)',usb.valeur as 'Nbr.USB',memoire.valeur as 'Nbr. Mémoire',sorties_audio.valeur as 'Nbr. Sorties Aud.',microphone.valeur as 'Nbr. Micro.',gain.valeur as 'Gain(dB)',type_amplificateur.designation as 'Type Amplificateur',

                                gbe.valeur as 'Nbr.Gbe',fe.valeur as 'Nbr.Fe',fo.valeur as 'Nbr.Fo',serial.valeur as 'Nbr.Serial',default_pwd.designation as 'Default Pwd',default_ip.designation as 'Default IP',console.valeur as 'Nbr.Console',auxiliaire.valeur as 'Nbr.Aux.',materiel.capable_usb as 'Support USB', type_routeur_AP.designation as 'Type routeur_AP', version_ios.designation as 'Version IOS',

                                portee.valeur as 'Portée(m)',type_AP.designation as 'Type AP',

                                frequence.designation as 'Fréquence(Hz)',antenne.valeur as 'Nbr. Ant.',

                                netette.designation as 'Netété',materiel.compatible_wifi as 'Support Wifi'

                                 from materiel 
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

                                --Printer
                                left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
                                left outer join puissance on puissance.id=materiel.id_puissance
                                left outer join intensite on intensite.id=materiel.id_intensite
                                left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

                                --Amplificateur
                                left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
                                left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
                                left outer join usb on usb.id=materiel.id_usb
                                left outer join memoire on memoire.id=materiel.id_memoire
                                left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
                                left outer join microphone on microphone.id=materiel.id_microphone
                                left outer join gain on gain.id=materiel.id_gain

                                --Routeur_AP
                                left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
                                left outer join gbe on gbe.id=materiel.id_gbe
                                left outer join fe on fe.id=materiel.id_fe
                                left outer join fo on fo.id=materiel.id_fo
                                left outer join serial on serial.id=materiel.id_serial
                                left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
                                left outer join default_ip on default_ip.id=materiel.id_default_ip
                                left outer join console on console.id=materiel.id_console
                                left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
                                left outer join version_ios on version_ios.id=materiel.id_version_ios
                                --capable_usb

                                --AccessPoint
                                left outer join type_AP on type_AP.id=materiel.id_type_AP
                                left outer join portee on portee.id=materiel.id_portee

                                --Switch
                                left outer join type_switch on type_switch.id=materiel.id_type_switch

                                --Emetteur
                                left outer join frequence on frequence.id=materiel.id_frequence
                                left outer join antenne on antenne.id=materiel.id_antenne

                                --Retroprojecteur
                                left outer join netette on netette.id=materiel.id_netette
                                where categorie_materiel.designation='Access Point' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver";

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = new DataSet();
                                dataset.Locale = CultureInfo.InvariantCulture;
                                adapter.Fill(dataset, "lstTable");

                                rpt5.SetDataSource(dataset.Tables["lstTable"]);
                                //rpvReport.ReportSource = rpt5;
                                rpvReport.Refresh();
                                #endregion
                            }

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

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
            }
        }

        private DataSet LoadReportWithSubReport(SqlDataAdapter adapter, string dataSetName, string embeddedRessource)
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

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //Initialisation de la chaine de connexion
            conn = new SqlConnection(Model.Properties.Settings.Default.strChaineConnexion);

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


        //public void setData(DataTable dtsrc, string reportpath1)
        //{

        //    //dt = dtsrc;
        //    this.reportViewer1.LocalReport.DataSources.Clear();
        //    this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtsrc));
        //    this.reportViewer1.LocalReport.ReportPath = reportpath1;
        //    //this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ParametreNom", "Parametrevaleur"));
        //    this.reportViewer1.RefreshReport();
        //}

        private void cmdView_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboCategorieMat.Items.Count > 0 && cboItems.Items.Count > 0)
                {
                    int categorie = cboCategorieMat.SelectedIndex;
                    int index = cboItems.SelectedIndex;

                    switch(categorie)
                    {
                        case 0:
                            //*****Choix pour Ordinateur
                            ValidateReport(categorie, index);
                            break;
                        case 1:
                            //*****Choix pour Switch
                            ValidateReport(categorie, index);
                            break;
                        case 2:
                            //*****Choix pour Imprimante
                            ValidateReport(categorie, index);
                            break;
                        case 3:
                            //*****Choix pour Emetteur
                            ValidateReport(categorie, index);
                            break;
                        case 4:
                            //*****Choix pour Amplificateur
                            ValidateReport(categorie, index);
                            break;
                        case 5:
                            //*****Choix pour Retroprojecteur
                            ValidateReport(categorie, index);
                            break;
                        case 6:
                            //*****Choix pour Routeur_Access Point
                            ValidateReport(categorie, index);
                            break;
                        case 7:
                            //*****Choix pour Access Point
                            ValidateReport(categorie, index);
                            break;
                    }
                }
                else
                    throw new CustomException("Impossible de charger les catégories de matériel ou la liste des items pour le rapport, veuillez réactualiser le formulaire");
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

        private void ValidateReport(int categorie, int index)
        {
            if (rdLstIdentifiant.Checked)
            {
                if (!string.IsNullOrEmpty(cboIdentifiant.SelectedItem.ToString()))
                {
                    LoadReport(index, categorie, rdLstIdentifiant);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des codes des matériels n'est pas vide");
            }
            else if (rdLstEtat.Checked)
            {
                if (!string.IsNullOrEmpty(cboEtat.Text))
                {
                    LoadReport(index, categorie, rdLstEtat);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des état des matériels n'est pas vide");
            }
            else if (rdLstEndGarantie.Checked)
            {
                if (!string.IsNullOrEmpty(cboDelais.Text))
                {
                    LoadReport(index, categorie, rdLstEndGarantie);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des délais n'est pas vide");
            }
            else if (rdLstMAC.Checked)
            {
                if (!string.IsNullOrEmpty(cboMacWifi.Text) || !string.IsNullOrEmpty(cboMacLAN.Text))
                {
                    LoadReport(index, categorie, rdLstMAC);
                }
                else
                    throw new CustomException("Veuillez vérifier que toutes les listes déroulantes des adresses MAC ne sont pas vides suivant la catégorie choisie");
            }
            else if (rdLstDateAcquisition.Checked)
            {
                if (!string.IsNullOrEmpty(txtDateAcquisitionDebut.Text) && !string.IsNullOrEmpty(txtDateAcquisitionFin.Text))
                {
                    LoadReport(index, categorie, rdLstDateAcquisition);
                }
                else
                    throw new CustomException("Veuillez vérifier que toutes les listes déroulantes ne sont pas vides");
            }
            else if (rdLstPortee.Checked)   
            {
                if (!string.IsNullOrEmpty(cboPortee.Text))
                {
                    LoadReport(index, categorie, rdLstPortee);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des portées n'est pas vide");
            }
            else if (rdLstMAC_Adresse.Checked)
            {
                if (!string.IsNullOrEmpty(cboMACAdresse.Text))
                {
                    LoadReport(index, categorie, rdLstMAC_Adresse);
                }
                else
                    throw new CustomException("Veuillez vérifier que toutes la liste déroulante des adresses MAC n'est pas vide");
            }
            else if (rdLstFrequence.Checked)
            {
                if (!string.IsNullOrEmpty(cboFrequence.Text))
                {
                    LoadReport(index, categorie, rdLstFrequence);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des fréquences n'est pas vide");
            }
            else if (rdLstNetete.Checked)
            {
                if (!string.IsNullOrEmpty(cboNetete.Text))
                {
                    LoadReport(index, categorie, rdLstNetete);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des netété n'est pas vide");
            }
            else if (rdLstPPM.Checked)
            {
                if (!string.IsNullOrEmpty(cboPPM.Text))
                {
                    LoadReport(index, categorie, rdLstPPM);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des pages par minute n'est pas vide");
            }
        }

        private void setMembersallcbo(ComboBox cbo, string displayMember, string valueMember)
        {
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
        }

        private void rdLstEtat_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstEtat);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstEndGarantie_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstEndGarantie);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }            
        }

        private void rdLstMAC_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstMAC);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void frmReportOrdinateur_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void DesableItem()
        {
            cboIdentifiant.Enabled = false;
            cboEtat.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;

            cboPortee.Enabled = false;
            cboFrequence.Enabled = false;
            cboNetete.Enabled = false;
            cboPPM.Enabled = false;
            cboMACAdresse.Enabled = false;

            chkArchiver.Checked = false;

            rdLstIdentifiant.Checked = false;
            rdLstEtat.Checked = false;
            rdLstEndGarantie.Checked = false;
            rdLstMAC.Checked = false;
            rdLstDateAcquisition.Checked = false;

            rdLstPortee.Checked = false;
            rdLstMAC_Adresse.Checked = false;
            rdLstFrequence.Checked = false;
            rdLstNetete.Checked = false;
            rdLstPPM.Checked = false;
        }

        private void frmReportOrdinateur_Load(object sender, EventArgs e)
        {
            
        }

        private void ActivateControls(int categorie, Control control)
        {
            //Comon Desable
            cboIdentifiant.Enabled = false;
            cboEtat.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;

            cboPortee.Enabled = false;
            cboFrequence.Enabled = false;
            cboNetete.Enabled = false;
            cboPPM.Enabled = false;
            cboMACAdresse.Enabled = false;

            chkArchiver.Checked = false;

            switch (categorie)
            {
                case 0:
                    //*****Choix pour Ordinateur
                    ActivateControlOrdinateur(control);
                    break;
                case 1:
                    //*****Choix pour Switch
                    ActivateControlSwitch(control);
                    break;
                case 2:
                    //*****Choix pour Imprimante
                    ActivateControlImprimante(control);
                    break;
                case 3:
                    //*****Choix pour Emetteur
                    ActivateControlEmetteur(control);
                    break;
                case 4:
                    //*****Choix pour Amplificateur
                    ActivateControlAmplificateur(control);
                    break;
                case 5:
                    //*****Choix pour Retroprojecteur
                    ActivateControlRetroprojecteur(control);
                    break;
                case 6:
                    //*****Choix pour Routeur_Access Point
                    ActivateControlRouteurAP(control);
                    break;
                case 7:
                    //*****Choix pour Access Point
                    ActivateControlAP(control);
                    break;
            }
        }

        private void ActivateControlOrdinateur(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstMAC.Name))
            {
                //Activation pour MAC adresse, Wifi ou LAN
                cboItems.Items.Clear();
                cboItems.Items.Add("Par MAC Adresse Wifi");
                cboItems.Items.Add("Par MAC Adresse LAN");
                cboItems.Items.Add("Par MAC Adresse Wifi ou LAN");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboMacWifi.Enabled = true;
                cboMacLAN.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlSwitch(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstMAC_Adresse.Name))
            {
                //Activation pour MAC adresse
                cboItems.Items.Clear();
                cboItems.Items.Add("Par MAC Adresse");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboMACAdresse.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlImprimante(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstPPM.Name))
            {
                //Activation pour Nombre des page par minute
                cboItems.Items.Clear();
                cboItems.Items.Add("Par nombre des pages par minute");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboPPM.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlEmetteur(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstFrequence.Name))
            {
                //Activation pour Frequence
                cboItems.Items.Clear();
                cboItems.Items.Add("Par Fréquence de fonctionnement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboFrequence.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlAmplificateur(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlRetroprojecteur(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstNetete.Name))
            {
                //Activation pour Netete
                cboItems.Items.Clear();
                cboItems.Items.Add("Par Netété de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboNetete.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlRouteurAP(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstMAC_Adresse.Name))
            {
                //Activation pour Mac Adresse
                cboItems.Items.Clear();
                cboItems.Items.Add("Par MAC Adresse de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboMACAdresse.Enabled = true;
            }
            //else if (rd.Name.Equals(rdLstPortee.Name))
            //{
            //    //Activation pour Portee
            //    cboItems.Items.Clear();
            //    cboItems.Items.Add("Par portée de l'équipement");
            //    cboItems.Sorted = false;
            //    cboItems.SelectedIndex = 0;

            //    cboPortee.Enabled = true;
            //}
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlAP(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstMAC_Adresse.Name))
            {
                //Activation pour Mac Adresse
                cboItems.Items.Clear();
                cboItems.Items.Add("Par MAC Adresse de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboMACAdresse.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstPortee.Name))
            {
                //Activation pour Portee
                cboItems.Items.Clear();
                cboItems.Items.Add("Par portée de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboPortee.Enabled = true;
            }

            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void rdLstIdentifiant_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstIdentifiant);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstDateAcquisition_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstDateAcquisition);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstPortee_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstPortee);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstMAC_Adresse_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstMAC_Adresse);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstFrequence_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstFrequence);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstNetete_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstNetete);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstPPM_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstPPM);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void frmReportMateriel_Load(object sender, EventArgs e)
        {
            //Deseable allt controls to allow make choice
            DesableItem();

            try
            {
                if (tLoad == null)
                {
                    tLoad = new Thread(new ThreadStart(ExecuteLoadData));
                    tLoad.Start();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
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
            this.rpvReport.RefreshReport();
        }

        private void frmReportMateriel_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.UnloadThreadRessource(tLoad);
            }
            catch { }
        }
    }
}
