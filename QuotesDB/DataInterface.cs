using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace QuotesDB
{
    interface DataInterface
    {
        bool openDatabase(string dbName);
        bool createDatabase();
        bool closeDatabase();
        T Insert<T>(string sql);
        DataTable Get(string sql);
        bool Exists(string sql);
        bool Update(string sql);
        bool Delete(string tbl, string criteria);
    }
}
