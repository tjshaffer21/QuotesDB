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
        DataTable Get(string sql);
    }
}
