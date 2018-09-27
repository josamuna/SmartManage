using System;
using System.Data;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsAC
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string code_str;
        private string designation;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clsAC> listes()
        {
            return clsMetier.GetInstance().getAllClsAC();
        }
        public List<clsAC> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsAC(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsAC(this);
        }
        public int update(clsAC varscls)
        {
            return clsMetier.GetInstance().updateClsAC(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsAC(this);
        }
        public int delete(clsAC varscls)
        {
            return clsMetier.GetInstance().deleteClsAC(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsAC(this);
        }
        //***Le constructeur par defaut***
        public clsAC()
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