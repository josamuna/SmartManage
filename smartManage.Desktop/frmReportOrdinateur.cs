using smartManage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmReportOrdinateur : Form
    {
        IDbConnection conn = null;
        private delegate void LoadSomeData(string ThreadName);
        private Thread tLoad = null;

        public frmReportOrdinateur()
        {
            InitializeComponent();
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
                MessageBox.Show("Echec de chargement des listes déroulantes, " + ex.Message, "Chargement listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion

        private string SetQueryExecute(ComboBox cboItems)
        {
            string query = null;

            switch (cboItems.SelectedIndex)
            {
                case 0:
                    if (rdLstIdentifiant.Checked)
                    {
                        //par identifiant equipement
                        query = string.Format(@"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
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
                        where materiel.code_str='{0}' and materiel.archiver={1}", cboIdentifiant.SelectedValue, chkArchiver.Checked ? 1 : 0);
                    }
                    else if (rdLstEtat.Checked)
                    {
                        //par etat de l'equipement
                        query = string.Format(@"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
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
                        where etat_materiel.designation='{0}' and materiel.archiver={1}", cboEtat.SelectedValue, chkArchiver.Checked ? 1 : 0);
                    }
                    else if (rdLstEndGarantie.Checked)
                    {
                        //par délais de garantie de l'equipement
                        query = string.Format(@"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
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
                        where garantie.valeur={0} and materiel.archiver={1}", cboDelais.SelectedValue, chkArchiver.Checked ? 1 : 0);
                    }
                    else if (rdLstMAC.Checked)
                    {
                        //par MAC Wifi de l'equipement
                        query = string.Format(@"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
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
                        where materiel.mac_adresse1='{0}' and materiel.archiver={1}", cboMacWifi.SelectedValue, chkArchiver.Checked ? 1 : 0);
                    }
                    else if (rdLstDateAcquisition.Checked)
                    {
                        //par date acquisition de l'equipement
                        query = string.Format(@"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
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
                        where (materiel.date_acquisition between '{0}' and '{1}') and materiel.archiver={2}", txtDateAcquisitionDebut.Text, txtDateAcquisitionFin.Text, chkArchiver.Checked ? 1 : 0);
                    }

                    break;
                case 1:
                    if (rdLstMAC.Checked)
                    {
                        //par MAC LAN de l'equipement
                        query = string.Format(@"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
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
                        where materiel.mac_adresse2='{0}' and materiel.archiver={1}", cboMacLAN.SelectedValue, chkArchiver.Checked ? 1 : 0);
                    }

                    break;
                case 2:
                    if (rdLstMAC.Checked)
                    {
                        //par MAC Wifi ou LAN de l'equipement
                        query = string.Format(@"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
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
                        where (materiel.mac_adresse1='{0}' or materiel.mac_adresse2='{1}') and materiel.archiver={2}", cboMacWifi.SelectedValue, cboMacLAN.SelectedValue, chkArchiver.Checked ? 1 : 0);
                    }

                    break;
            }
            return query;
        }

        private void LoadReport(string query, int cboIndex)
        {
            //Initialisation de la chaine de connexion
            conn = new SqlConnection(smartManage.Model.Properties.Settings.Default.strChaineConnexion);

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                IDbDataAdapter adapter = new SqlDataAdapter((SqlCommand)cmd);
                DataSet dataset = new DataSet();
                adapter.Fill(dataset);

                switch (cboIndex)
                {
                    case 0:
                        if (rdLstIdentifiant.Checked)
                        {
                            //par identifiant equipement
                            Reports.rpt1 rpt = new Reports.rpt1();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose();
                        }
                        else if (rdLstEtat.Checked)
                        {
                            //par etat de l'equipement
                            Reports.rpt1 rpt = new Reports.rpt1();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose();
                        }
                        else if (rdLstEndGarantie.Checked)
                        {
                            //par délais de garantie de l'equipement
                            Reports.rpt1 rpt = new Reports.rpt1();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose(); ;
                        }
                        else if (rdLstMAC.Checked)
                        {
                            //par MAC Wifi de l'equipement
                            Reports.rpt1 rpt = new Reports.rpt1();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose();
                        }
                        else if (rdLstDateAcquisition.Checked)
                        {
                            //par date d'acuisition de l'equipement
                            Reports.rpt1 rpt = new Reports.rpt1();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose(); ;
                        }

                        break;

                    case 1:
                        if (rdLstMAC.Checked)
                        {
                            //par MAC LAN de l'equipement
                            Reports.rpt1 rpt = new Reports.rpt1();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose();
                        }

                        break;

                    case 2:
                        if (rdLstMAC.Checked)
                        {
                            //par MAC Wifi ou LAN de l'equipement
                            Reports.rpt1 rpt = new Reports.rpt1();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose();
                        }

                        break; 
                }
            }
        }

        private void cmdView_Click(object sender, EventArgs e)
        {
            try
            {
                //LoadReport(SetQueryExecute(cboItems), cboItems.SelectedIndex);
                conn = new SqlConnection(smartManage.Model.Properties.Settings.Default.strChaineConnexion);

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(@"select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
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
                    where materiel.code_str='{0}' and materiel.archiver={1}", cboIdentifiant.SelectedValue, chkArchiver.Checked ? 1 : 0); ;
                    IDbDataAdapter adapter = new SqlDataAdapter((SqlCommand)cmd);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);

                    Reports.rpt1 rpt = new Reports.rpt1();
                    rpt.SetDataSource(dataset.Tables["lstTable"]);
                    crvReport.ReportSource = rpt;
                    crvReport.Refresh();
                    dataset.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement du rapport, " + ex.Message, "Chargement rapport", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                conn.Close();
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
                MessageBox.Show(string.Format("Erreur lor du chargement des listes déroulantes, {0}", ex.Message), "Chargement listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            try
            {
                smartManage.Tools.clsTools.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
            catch { }
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
