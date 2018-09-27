using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smartManage.Desktop
{
    interface ICRUDGeneral
    {
        void New();
        void Search(string criteria);
        void Save();
        void UpdateRec();
        void Delete();
        void RefreshRec();
        void Preview();
    }
}
