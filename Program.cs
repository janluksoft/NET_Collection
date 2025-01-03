//=============================================================
//🔷🔷🔷🔷🔷=== Author: janluksoft@interia.pl ===🔷🔷🔷🔷🔷
//─────────────────────────────────────────────────────────────

// File: 'Program.cs'

using pN9_CorrectPersonList;

namespace pN9_Yield; // ✅ C# 10: File-scoped namespace

internal class Program : IPrintPerson
{

    static void Main(string[] args)
    {
        IPrintPerson print = new Program();

        Console.WriteLine("Hello, program is starting ... \n\r");
        Console.WriteLine("Checking the correctness of the input data by the set accessors:\n\r");

        //--- Create listPerson --------------
        CorrectPersonList cCPersonL = new CorrectPersonList();
        var _list = cCPersonL.GetPersonList(true);
                                                        // --- End --------------
        if (!_list.ok) { Console.WriteLine("PersonList is empty. End of work ..."); return; }

        // 0) LINQ                 (lambda x-parameter => code)
        IOrderedEnumerable<Person> listPerLinq = (_list.l)
                                    .OrderBy(x => x.Age)//Sort field1| above Filter
                                    .ThenBy(x => x.Country);    //sort field2| 2 conditions
        List<Person> listPerson = listPerLinq.ToList();

        //--- All list print -------------
        print.PrintListPerson("  ", listPerson, true, "Correct list (List<Person> listPerson):");



        // --- Yield technique -------------------
        Console.WriteLine($"\n\r1) YIELD-1 series: item.Age > 32 and Country != 'France'");
        foreach (var item in ListPersonWithYield1(listPerson))
            print.PrintPersonObj("  ", item, true);

        Console.WriteLine($"\n\r2) YIELD-2 series: item.Age > 32 and Country != 'France'");
        foreach (var item in ListPersonWithYield2(listPerson))
            print.PrintPersonObj("  ", item, true);

        Console.WriteLine($"\n\r3) SelectMany: item.Age > 32 and Country != 'France'");
        //IEnumerable<Person> rr = ListPersonWithSelectMany(listPerson);
        foreach (var item in ListPersonWithSelectMany(listPerson))
            print.PrintPersonObj("  ", item, true);

        //==========================
        Console.WriteLine($"\n\r4) Yield with Transformation (Mapping)");
        //IEnumerable<Person> rr = ListPersonWithSelectMany(listPerson);
        foreach (var item in ListPersonNamesWithYield(listPerson))
            Console.WriteLine($"  {item}");


        Console.WriteLine("\n5) Grouping with Yield:");
        foreach (var group in GroupPersonsByCountry(listPerson))
        {
            Console.WriteLine($"\n\rCountry: {group.Country}:");
            foreach (var e in group.People)
                print.PrintPersonObj("  ", e, true);
        }

        int pages = 4;
        int i = 1;
        Console.WriteLine($"\n6) Pagination with Yield (Lazy Loading) ({pages} items):");
        foreach (var group in PaginatePersons(listPerson, pages))
        {
            Console.WriteLine($"\n\r Group {i}:");
            i++;
            foreach (var e in group)
                print.PrintPersonObj("    ", e, true);
        }

        return; //For clarity

        //=========== END Main() ========================

        //                           ---- YIELD -------
        // ✅ C# 7: Local functions bellow
        // This is the way with [yield return] - (a control-flow statement)
        // M1) Method: 'if' statement generated condition. Use 'yeld return' statement
        IEnumerable<Person> ListPersonWithYield1(List<Person> xList)
        {   
            foreach (var item in xList)
                // the control is passed to the caller if the condition is met
                if ((item.Age > 32) && (item.Country != "France"))
                    yield return item;
        }

        //                           ---- YIELD -------
        // M2) Method: LINQ generated condition.  Use 'yeld return' statement
        IEnumerable<Person> ListPersonWithYield2(List<Person> xList)
        {
            foreach (var item in xList.Where(item => 
                       ((item.Age > 32) && (item.Country != "France")) ) //end Where
                    ) // end foreach
                    // the control is passed to the caller if the condition is met
                    yield return item;
        }

        //                       -------- SELECTMANY -------------
        // This is the way with LINQ lambda expression and SelectMany method.
        // We can't yield return be used directly inside a lambda because yield return
        // is a control-flow statement, and lambdas are expressions, not statements. 
        // M3) Method: LINQ generated condition.  Use 'SelectMany' method
        IEnumerable<Person> ListPersonWithSelectMany(List<Person> xList) =>
            xList.Where(item => (item.Age > 32) && (item.Country != "France"))
                .SelectMany(item => Enumerable.Repeat(item, 1));

        //====== Next additional yield use cases ========


        // 41) Yield with Transformation (Mapping)
        IEnumerable<string> ListPersonNamesWithYield(List<Person> xList)
        {
            foreach (var item in xList)
                yield return $"{item.FirstName.Trim()+ " "+ item.SurName,-19} ({item.Age} years)";
        }

        // 42 Grouping with Yield
        IEnumerable<(string Country, List<Person> People)> GroupPersonsByCountry(List<Person> xList)
        {
            foreach (var group in xList.GroupBy(p => p.Country))
                yield return (group.Key, group.ToList());
        }

        // 43 Pagination with Yield (Lazy Loading)
        IEnumerable<List<Person>> PaginatePersons(List<Person> xList, int pageSize)
        {
            for (int i = 0; i < xList.Count; i += pageSize)
                yield return xList.Skip(i).Take(pageSize).ToList();
        }
    }
}
