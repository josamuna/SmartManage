using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsmateriel
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string code_str;
        private int id_categorie_materiel;
        private int id_compte;
        private string qrcode;
        private DateTime? date_acquisition;
        private int? id_garantie;
        private int id_marque;
        private int id_modele;
        private int id_couleur;
        private int id_poids;
        private int id_etat_materiel;
        private string photo1;
        private string photo2;
        private string photo3;
        private string label;
        private string mac_adresse1;
        private string mac_adresse2;
        private string commentaire;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        private int? id_type_ordinateur;
        private int? id_type_clavier;
        private int? id_os;
        private int? id_ram;
        private int? id_processeur;
        private int? id_nombre_coeur_processeur;
        private int? id_type_hdd;
        private int? id_nombre_hdd;
        private int? id_capacite_hdd;
        private int? id_taille_ecran;
        private int? id_usb2;
        private int? id_usb3;
        private int? id_hdmi;
        private int? id_vga;
        private int? id_tension_batterie;
        private int? id_tension_adaptateur;
        private int? id_puissance_adaptateur;
        private int? id_intensite_adaptateur;
        private int? numero_cle;
        private int? id_type_imprimante;
        private int? id_puissance;
        private int? id_intensite;
        private int? id_page_par_minute;
        private int? id_type_amplificateur;
        private int? id_tension_alimentation;
        private int? id_usb;
        private int? id_memoire;
        private int? id_sorties_audio;
        private int? id_microphone;
        private int? id_gain;
        private int? id_type_routeur_ap;
        private int? id_version_ios;
        private int? id_gbe;
        private int? id_fe;
        private int? id_fo;
        private int? id_serial;
        private bool? capable_usb;
        private int? id_default_pwd;
        private int? id_default_ip;
        private int? id_console;
        private int? id_auxiliaire;
        private int? id_type_ap;
        private int? id_type_switch;
        private string frequence;
        private int? id_antenne;
        private int? id_netette;
        private bool? compatible_wifi;
        //***Listes***
        public List<clsmateriel> listes()
        {
            return clsMetier.GetInstance().getAllClsmateriel();
        }
        public List<clsmateriel> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsmateriel(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsmateriel(this);
        }
        public int update(clsmateriel varscls)
        {
            return clsMetier.GetInstance().updateClsmateriel(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsmateriel(this);
        }
        public int delete(clsmateriel varscls)
        {
            return clsMetier.GetInstance().deleteClsmateriel(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsmateriel(this);
        }
        //***Le constructeur par defaut***
        public clsmateriel()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de code_str***
        public string Code_str
        {
            get { return code_str; }
            set { code_str = value; }
        }  //***Accesseur de id_categorie_materiel***
        public int Id_categorie_materiel
        {

            get { return id_categorie_materiel; }
            set { id_categorie_materiel = value; }
        }  //***Accesseur de id_compte***
        public int Id_compte
        {
            get { return id_compte; }
            set { id_compte = value; }
        }  //***Accesseur de qrcode***
        public string Qrcode
        {
            get { return qrcode; }
            set { qrcode = value; }
        }  //***Accesseur de date_acquisition***
        public DateTime? Date_acquisition
        {
            get { return date_acquisition; }
            set { date_acquisition = value; }
        }  //***Accesseur de id_garantie***
        public int? Id_garantie
        {
            get { return id_garantie; }
            set { id_garantie = value; }
        }  //***Accesseur de id_marque***
        public int Id_marque
        {
            get { return id_marque; }
            set { id_marque = value; }
        }  //***Accesseur de id_modele***
        public int Id_modele
        {
            get { return id_modele; }
            set { id_modele = value; }
        }  //***Accesseur de id_couleur***
        public int Id_couleur
        {
            get { return id_couleur; }
            set { id_couleur = value; }
        }  //***Accesseur de id_poids***
        public int Id_poids
        {
            get { return id_poids; }
            set { id_poids = value; }
        }  //***Accesseur de id_etat_materiel***
        public int Id_etat_materiel
        {
            get { return id_etat_materiel; }
            set { id_etat_materiel = value; }
        }  //***Accesseur de photo1***
        public string Photo1
        {
            get { return photo1; }
            set { photo1 = value; }
        }  //***Accesseur de photo2***
        public string Photo2
        {
            get { return photo2; }
            set { photo2 = value; }
        }  //***Accesseur de photo3***
        public string Photo3
        {
            get { return photo3; }
            set { photo3 = value; }
        }  //***Accesseur de label***
        public string Label
        {
            get { return label; }
            set { label = value; }
        }  //***Accesseur de mac_adresse1***
        public string Mac_adresse1
        {
            get { return mac_adresse1; }
            set { mac_adresse1 = value; }
        }  //***Accesseur de mac_adresse2***
        public string Mac_adresse2
        {

            get { return mac_adresse2; }
            set { mac_adresse2 = value; }
        }  //***Accesseur de commentaire***
        public string Commentaire
        {
            get { return commentaire; }
            set { commentaire = value; }
        }  //***Accesseur de user_created***
        public string User_created
        {

            get { return user_created; }
            set { user_created = value; }
        }  //***Accesseur de date_created***
        public DateTime? Date_created
        {
            get { return date_created; }
            set { date_created = value; }
        }  //***Accesseur de user_modified***
        public string User_modified
        {

            get { return user_modified; }
            set { user_modified = value; }
        }  //***Accesseur de date_modified***
        public DateTime? Date_modified
        {
            get { return date_modified; }
            set { date_modified = value; }
        }  //***Accesseur de id_type_ordinateur***
        public int? Id_type_ordinateur
        {
            get { return id_type_ordinateur; }
            set { id_type_ordinateur = value; }
        }  //***Accesseur de id_type_clavier***
        public int? Id_type_clavier
        {
            get { return id_type_clavier; }
            set { id_type_clavier = value; }
        }  //***Accesseur de id_os***
        public int? Id_os
        {
            get { return id_os; }
            set { id_os = value; }
        }  //***Accesseur de id_ram***
        public int? Id_ram
        {
            get { return id_ram; }
            set { id_ram = value; }
        }  //***Accesseur de id_processeur***
        public int? Id_processeur
        {
            get { return id_processeur; }
            set { id_processeur = value; }
        }  //***Accesseur de id_nombre_coeur_processeur***
        public int? Id_nombre_coeur_processeur
        {
            get { return id_nombre_coeur_processeur; }
            set { id_nombre_coeur_processeur = value; }
        }  //***Accesseur de id_type_hdd***
        public int? Id_type_hdd
        {
            get { return id_type_hdd; }
            set { id_type_hdd = value; }
        }  //***Accesseur de id_nombre_hdd***
        public int? Id_nombre_hdd
        {
            get { return id_nombre_hdd; }
            set { id_nombre_hdd = value; }
        }  //***Accesseur de id_capacite_hdd***
        public int? Id_capacite_hdd
        {
            get { return id_capacite_hdd; }
            set { id_capacite_hdd = value; }
        }  //***Accesseur de id_taille_ecran***
        public int? Id_taille_ecran
        {
            get { return id_taille_ecran; }
            set { id_taille_ecran = value; }
        }  //***Accesseur de id_usb2***
        public int? Id_usb2
        {
            get { return id_usb2; }
            set { id_usb2 = value; }
        }  //***Accesseur de id_usb3***
        public int? Id_usb3
        {
            get { return id_usb3; }
            set { id_usb3 = value; }
        }  //***Accesseur de id_hdmi***
        public int? Id_hdmi
        {
            get { return id_hdmi; }
            set { id_hdmi = value; }
        }  //***Accesseur de id_vga***
        public int? Id_vga
        {
            get { return id_vga; }
            set { id_vga = value; }
        }  //***Accesseur de id_tension_batterie***
        public int? Id_tension_batterie
        {
            get { return id_tension_batterie; }
            set { id_tension_batterie = value; }
        }  //***Accesseur de id_tension_adaptateur***
        public int? Id_tension_adaptateur
        {
            get { return id_tension_adaptateur; }
            set { id_tension_adaptateur = value; }
        }  //***Accesseur de id_puissance_adaptateur***
        public int? Id_puissance_adaptateur
        {
            get { return id_puissance_adaptateur; }
            set { id_puissance_adaptateur = value; }
        }  //***Accesseur de id_intensite_adaptateur***
        public int? Id_intensite_adaptateur
        {
            get { return id_intensite_adaptateur; }
            set { id_intensite_adaptateur = value; }
        }  //***Accesseur de numero_cle***
        public int? Numero_cle
        {
            get { return numero_cle; }
            set { numero_cle = value; }
        }  //***Accesseur de id_type_imprimante***
        public int? Id_type_imprimante
        {
            get { return id_type_imprimante; }
            set { id_type_imprimante = value; }
        }  //***Accesseur de id_puissance***
        public int? Id_puissance
        {
            get { return id_puissance; }
            set { id_puissance = value; }
        }  //***Accesseur de id_intensite***
        public int? Id_intensite
        {
            get { return id_intensite; }
            set { id_intensite = value; }
        }  //***Accesseur de id_page_par_minute***
        public int? Id_page_par_minute
        {
            get { return id_page_par_minute; }
            set { id_page_par_minute = value; }
        }  //***Accesseur de id_type_amplificateur***
        public int? Id_type_amplificateur
        {
            get { return id_type_amplificateur; }
            set { id_type_amplificateur = value; }
        }  //***Accesseur de id_tension_alimentation***
        public int? Id_tension_alimentation
        {
            get { return id_tension_alimentation; }
            set { id_tension_alimentation = value; }
        }  //***Accesseur de id_usb***
        public int? Id_usb
        {
            get { return id_usb; }
            set { id_usb = value; }
        }  //***Accesseur de id_memoire***
        public int? Id_memoire
        {
            get { return id_memoire; }
            set { id_memoire = value; }
        }  //***Accesseur de id_sorties_audio***
        public int? Id_sorties_audio
        {
            get { return id_sorties_audio; }
            set { id_sorties_audio = value; }
        }  //***Accesseur de id_microphone***
        public int? Id_microphone
        {
            get { return id_microphone; }
            set { id_microphone = value; }
        }  //***Accesseur de id_gain***
        public int? Id_gain
        {
            get { return id_gain; }
            set { id_gain = value; }
        }  //***Accesseur de id_type_routeur_ap***
        public int? Id_type_routeur_ap
        {
            get { return id_type_routeur_ap; }
            set { id_type_routeur_ap = value; }
        }  //***Accesseur de id_version_ios***
        public int? Id_version_ios
        {
            get { return id_version_ios; }
            set { id_version_ios = value; }
        }  //***Accesseur de id_gbe***
        public int? Id_gbe
        {
            get { return id_gbe; }
            set { id_gbe = value; }
        }  //***Accesseur de id_fe***
        public int? Id_fe
        {
            get { return id_fe; }
            set { id_fe = value; }
        }  //***Accesseur de id_fo***
        public int? Id_fo
        {
            get { return id_fo; }
            set { id_fo = value; }
        }  //***Accesseur de id_serial***
        public int? Id_serial
        {
            get { return id_serial; }
            set { id_serial = value; }
        }  //***Accesseur de capable_usb***
        public bool? Capable_usb
        {
            get { return capable_usb; }
            set { capable_usb = value; }
        }  //***Accesseur de id_default_pwd***
        public int? Id_default_pwd
        {
            get { return id_default_pwd; }
            set { id_default_pwd = value; }
        }  //***Accesseur de id_default_ip***
        public int? Id_default_ip
        {
            get { return id_default_ip; }
            set { id_default_ip = value; }
        }  //***Accesseur de id_console***
        public int? Id_console
        {
            get { return id_console; }
            set { id_console = value; }
        }  //***Accesseur de id_auxiliaire***
        public int? Id_auxiliaire
        {
            get { return id_auxiliaire; }
            set { id_auxiliaire = value; }
        }  //***Accesseur de id_type_ap***
        public int? Id_type_ap
        {
            get { return id_type_ap; }
            set { id_type_ap = value; }
        }  //***Accesseur de id_type_switch***
        public int? Id_type_switch
        {
            get { return id_type_switch; }
            set { id_type_switch = value; }
        }  //***Accesseur de frequence***
        public string Frequence
        {
            get { return frequence; }
            set { frequence = value; }
        }  //***Accesseur de id_antenne***
        public int? Id_antenne
        {
            get { return id_antenne; }
            set { id_antenne = value; }
        }  //***Accesseur de id_netette***
        public int? Id_netette
        {
            get { return id_netette; }
            set { id_netette = value; }
        }  //***Accesseur de compatible_wifi***
        public bool? Compatible_wifi
        {
            get { return compatible_wifi; }
            set { compatible_wifi = value; }
        }
    } //***fin class
} //***fin namespace