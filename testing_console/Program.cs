using domain;
using objekty;

namespace testing_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "../../../../objekty/bin/Debug/net6.0/objekty.dll";
            string db_source = "Data Source=mydb.db";

            var mapper = new orm.mapper1(path, db_source);

            logic_domain1 logic_Domain = new logic_domain1(path, db_source);

            logic_Domain.print_all();

            court court1 = new court("kur03", "Kurt na floorbal", 100, 8, 20);
            logic_Domain.add_court(court1);
            logic_Domain.print_all();
            court1.Closes = 10;
            logic_Domain.update_court(court1);
            logic_Domain.print_all();

            //orm_test.test();
        }
    }
}