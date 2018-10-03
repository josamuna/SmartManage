using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clssection
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string designation1;
        private string designation2;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clssection> listes()
        {
            return clsMetier.GetInstance().getAllClssection();
        }
        public List<clssection> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClssection(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClssection(this);
        }
        public int update(clssection varscls)
        {
            return clsMetier.GetInstance().updateClssection(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClssection(this);
        }
        public int delete(clssection varscls)
        {
            return clsMetier.GetInstance().deleteClssection(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClssection(this);
        }
        //***Le constructeur par defaut***
        public clssection()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de designation1***
        public string Designation1
        {
            get { return designation1; }
            set { designation1 = value; }
        }  //***Accesseur de designation2***
        public string Designation2
        {
            get { return designation2; }
            set { designation2 = value; }
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