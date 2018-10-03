using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clssignataire
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private int id_personne;
        private string code_ac;
        private string signature_specimen;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clssignataire> listes()
        {
            return clsMetier.GetInstance().getAllClssignataire();
        }
        public List<clssignataire> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClssignataire(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClssignataire(this);
        }
        public int update(clssignataire varscls)
        {
            return clsMetier.GetInstance().updateClssignataire(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClssignataire(this);
        }
        public int delete(clssignataire varscls)
        {
            return clsMetier.GetInstance().deleteClssignataire(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClssignataire(this);
        }
        //***Le constructeur par defaut***
        public clssignataire()
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
        }  //***Accesseur de signature_specimen***
        public string Signature_specimen
        {
            get { return signature_specimen; }
            set { signature_specimen = value; }
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
} //***fin namespace