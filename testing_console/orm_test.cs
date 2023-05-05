using objekty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_console
{
    internal class orm_test
    {
        public static void test()
        {
            string path = "../../../../objekty/bin/Debug/net6.0/objekty.dll";
            string db_source = "Data Source=mydb.db";

            var mapper = new orm.mapper1(path, db_source);

            mapper.create_tables();
            //mapper.test();

            user jenda = new user("jen01", "jenheslo", "Jenda", "Nejen", "nejenda@centrum.cz");
            mapper.create(jenda);

            court kurt = new court("kur01",  "Kurt na hokej", 100, 8, 20);
            mapper.create(kurt);

            reservation rezervace = new reservation("kur01", DateTime.Now, 100, "jen01");
            mapper.create(rezervace);
            reservation rezervace2 = new reservation( "kur01", DateTime.Now.AddSeconds(2), 100,"jen01");
            mapper.create(rezervace2);

            mapper.readall("reservation");

            rezervace.Price = 200;

            mapper.update(rezervace);

            //mapper.delete(rezervace2);

            return;
        }
    }
}
