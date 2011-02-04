using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuotesDB
{
    interface DataInterface
    {
        bool openDatabase(String dbName);
        bool closeDatabase();
    }
}
