using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsretrait_materiel
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private int id_personne;
        private string code_ac;
        private int? id_optio;
        private int? id_promotion;
        private int? id_section;
        private DateTime date_retrait;
        private bool retirer;
        private bool? deposer;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clsretrait_materiel> listes()
        {
            return clsMetier.GetInstance().getAllClsretrait_materiel();
        }
        public List<clsretrait_materiel> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsretrait_materiel(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsretrait_materiel(this);
        }
        public int update(clsretrait_materiel varscls)
        {
            return clsMetier.GetInstance().updateClsretrait_materiel(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsretrait_materiel(this);
        }
        public int delete(clsretrait_materiel varscls)
        {
            return clsMetier.GetInstance().deleteClsretrait_materiel(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsretrait_materiel(this);
        }
        //***Le constructeur par defaut***
        public clsretrait_materiel()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de id_personne***
        public int Id_personne
        {
            get { return id_personne; }
            set { id_personne = value; }
        }  //***Accesseur de code_ac***
        public string Code_ac
        {
            get { return code_ac; }
            set { code_ac = value; }
        }  //***Accesseur de id_optio***
        public int? Id_optio
        {
            get { return id_optio; }
            set { id_optio = value; }
        }  //***Accesseur de id_promotion***
        public int? Id_promotion
        {
            get { return id_promotion; }
            set { id_promotion = value; }
        }  //***Accesseur de id_section***
        public int? Id_section
        {
            get { return id_section; }
            set { id_section = value; }
        }  //***Accesseur de date_retrait***
        public DateTime Date_retrait
        {
            get { return date_retrait; }
            set { date_retrait = value; }
        }  //***Accesseur de retirer***
        public bool Retirer
        {
            get { return retirer; }
            set { retirer = value; }
        }  //***Accesseur de deposer***
        public bool? Deposer
        {
            get { return deposer; }
            set { deposer = value; }
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
        }
    } //***fin class
}//***fin namespace