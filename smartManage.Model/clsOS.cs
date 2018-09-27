using System;
using System.Data;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsOS
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private int id_type_os;
        private int id_architecture_os;
        private string designation;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clsOS> listes()
        {
            return clsMetier.GetInstance().getAllClsOS();
        }
        public List<clsOS> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsOS(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsOS(this);
        }
        public int update(clsOS varscls)
        {
            return clsMetier.GetInstance().updateClsOS(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsOS(this);
        }
        public int delete(clsOS varscls)
        {
            return clsMetier.GetInstance().deleteClsOS(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsOS(this);
        }
        //***Le constructeur par defaut***
        public clsOS()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de id_type_os***
        public int Id_type_os
        {
            get { return id_type_os; }
            set { id_type_os = value; }
        }  //***Accesseur de id_architecture_os***
        public int Id_architecture_os
        {
            get { return id_architecture_os; }
            set { id_architecture_os = value; }
        }  //***Accesseur de designation***
        public string Designation
        {
            get { return designation; }
            set { designation = value; }
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