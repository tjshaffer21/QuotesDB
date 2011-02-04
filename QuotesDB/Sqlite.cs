using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuotesDB
{
    class Sqlite : DataInterface
    {
        bool openDatabase(String name)
        {

            return true;
        }

        bool closeDatabase()
        {
            return true;
        }
    }
}
