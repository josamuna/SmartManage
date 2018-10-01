using System;
using System.Data;
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
        private int? guarantie;
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
        private double? ram;
        private double? processeur;
        private int? nombre_coeur_processeur;
        private int? nombre_hdd;
        private int? capacite_hdd;
        private double? indice_performance;
        private int? pouce;
        private int? nombre_usb2;
        private int? nombre_usb3;
        private int? nombre_hdmi;
        private int? nombre_vga;
        private double? tension_batterie;
        private double? tension_adaptateur;
        private double? puissance_adaptateur;
        private double? intensite_adaptateur;
        private int? numero_cle;
        private int? id_type_imprimante;
        private double? puissance;
        private double? intensite;
        private double? nombre_page_par_minute;
        private int? id_type_amplificateur;
        private int? tension_alimentation;
        private int? nombre_usb;
        private int? nombre_memoire;
        private int? nombre_sorties_audio;
        private int? nombre_microphone;
        private double? gain;
        private int? id_type_routeur_ap;
        private int? id_version_ios;
        private int? nombre_gbe;
        private int? nombre_fe;
        private int? nombre_fo;
        private int? nombre_serial;
        private bool? capable_usb;
        private string motpasse_defaut;
        private string default_ip;
        private int? nombre_console;
        private int? nombre_auxiliaire;
        private int? id_type_ap;
        private int? id_type_switch;
        private double? frequence;
        private string alimentation;
        private int? nombre_antenne;
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
        }  //***Accesseur de guarantie***
        public int? Guarantie
        {
            get { return guarantie; }
            set { guarantie = value; }
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
        }  //***Accesseur de ram***
        public double? Ram
        {
            get { return ram; }
            set { ram = value; }
        }  //***Accesseur de processeur***
        public double? Processeur
        {
            get { return processeur; }
            set { processeur = value; }
        }  //***Accesseur de nombre_coeur_processeur***
        public int? Nombre_coeur_processeur
        {
            get { return nombre_coeur_processeur; }
            set { nombre_coeur_processeur = value; }
        }  //***Accesseur de nombre_hdd***
        public int? Nombre_hdd
        {
            get { return nombre_hdd; }
            set { nombre_hdd = value; }
        }  //***Accesseur de capacite_hdd***
        public int? Capacite_hdd
        {
            get { return capacite_hdd; }
            set { capacite_hdd = value; }
        }  //***Accesseur de indice_performance***
        public double? Indice_performance
        {
            get { return indice_performance; }
            set { indice_performance = value; }
        }  //***Accesseur de pouce***
        public int? Pouce
        {
            get { return pouce; }
            set { pouce = value; }
        }  //***Accesseur de nombre_usb2***
        public int? Nombre_usb2
        {
            get { return nombre_usb2; }
            set { nombre_usb2 = value; }
        }  //***Accesseur de nombre_usb3***
        public int? Nombre_usb3
        {
            get { return nombre_usb3; }
            set { nombre_usb3 = value; }
        }  //***Accesseur de nombre_hdmi***
        public int? Nombre_hdmi
        {
            get { return nombre_hdmi; }
            set { nombre_hdmi = value; }
        }  //***Accesseur de nombre_vga***
        public int? Nombre_vga
        {
            get { return nombre_vga; }
            set { nombre_vga = value; }
        }  //***Accesseur de tension_batterie***
        public double? Tension_batterie
        {
            get { return tension_batterie; }
            set { tension_batterie = value; }
        }  //***Accesseur de tension_adaptateur***
        public double? Tension_adaptateur
        {
            get { return tension_adaptateur; }
            set { tension_adaptateur = value; }
        }  //***Accesseur de puissance_adaptateur***
        public double? Puissance_adaptateur
        {
            get { return puissance_adaptateur; }
            set { puissance_adaptateur = value; }
        }  //***Accesseur de intensite_adaptateur***
        public double? Intensite_adaptateur
        {
            get { return intensite_adaptateur; }
            set { intensite_adaptateur = value; }
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
        }  //***Accesseur de puissance***
        public double? Puissance
        {
            get { return puissance; }
            set { puissance = value; }
        }  //***Accesseur de intensite***
        public double? Intensite
        {
            get { return intensite; }
            set { intensite = value; }
        }  //***Accesseur de nombre_page_par_minute***
        public double? Nombre_page_par_minute
        {
            get { return nombre_page_par_minute; }
            set { nombre_page_par_minute = value; }
        }  //***Accesseur de id_type_amplificateur***
        public int? Id_type_amplificateur
        {
            get { return id_type_amplificateur; }
            set { id_type_amplificateur = value; }
        }  //***Accesseur de tension_alimentation***
        public int? Tension_alimentation
        {
            get { return tension_alimentation; }
            set { tension_alimentation = value; }
        }  //***Accesseur de nombre_usb***
        public int? Nombre_usb
        {
            get { return nombre_usb; }
            set { nombre_usb = value; }
        }  //***Accesseur de nombre_memoire***
        public int? Nombre_memoire
        {
            get { return nombre_memoire; }
            set { nombre_memoire = value; }
        }  //***Accesseur de nombre_sorties_audio***
        public int? Nombre_sorties_audio
        {
            get { return nombre_sorties_audio; }
            set { nombre_sorties_audio = value; }
        }  //***Accesseur de nombre_microphone***
        public int? Nombre_microphone
        {
            get { return nombre_microphone; }
            set { nombre_microphone = value; }
        }  //***Accesseur de gain***
        public double? Gain
        {
            get { return gain; }
            set { gain = value; }
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
        }  //***Accesseur de nombre_gbe***
        public int? Nombre_gbe
        {
            get { return nombre_gbe; }
            set { nombre_gbe = value; }
        }  //***Accesseur de nombre_fe***
        public int? Nombre_fe
        {
            get { return nombre_fe; }
            set { nombre_fe = value; }
        }  //***Accesseur de nombre_fo***
        public int? Nombre_fo
        {
            get { return nombre_fo; }
            set { nombre_fo = value; }
        }  //***Accesseur de nombre_serial***
        public int? Nombre_serial
        {
            get { return nombre_serial; }
            set { nombre_serial = value; }
        }  //***Accesseur de capable_usb***
        public bool? Capable_usb
        {
            get { return capable_usb; }
            set { capable_usb = value; }
        }  //***Accesseur de motpasse_defaut***
        public string Motpasse_defaut
        {
            get { return motpasse_defaut; }
            set { motpasse_defaut = value; }
        }  //***Accesseur de default_ip***
        public string Default_ip
        {
            get { return default_ip; }
            set { default_ip = value; }
        }  //***Accesseur de nombre_console***
        public int? Nombre_console
        {
            get { return nombre_console; }
            set { nombre_console = value; }
        }  //***Accesseur de nombre_auxiliaire***
        public int? Nombre_auxiliaire
        {
            get { return nombre_auxiliaire; }
            set { nombre_auxiliaire = value; }
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
        public double? Frequence
        {
            get { return frequence; }
            set { frequence = value; }
        }  //***Accesseur de alimentation***
        public string Alimentation
        {
            get { return alimentation; }
            set { alimentation = value; }
        }  //***Accesseur de nombre_antenne***
        public int? Nombre_antenne
        {
            get { return nombre_antenne; }
            set { nombre_antenne = value; }
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