//=============================================================
//🔷🔷🔷🔷🔷=== Author: janluksoft@interia.pl ===🔷🔷🔷🔷🔷
//─────────────────────────────────────────────────────────────

//File: 'CorrectPersonList.cs'

using System.Text.RegularExpressions;

namespace pN9_CorrectPersonList;

public class CorrectPersonList : IPrintPerson
{
    IPrintPerson printer; // Use interface reference
    List<RecPerson>? _listPersonRec;
    List<Person>? _ListPerson;
    public int _iPersonsCount { get => field; set => field = value; }


    public CorrectPersonList()
    {
        printer = this;
        _iPersonsCount = 0;
    }

    public (bool ok, List<Person> l) GetPersonList(bool xbInit = false)
    {
        if (xbInit) Start();

        bool _ok = false;
        if (_iPersonsCount > 0) _ok = true;

        return (_ok, _ListPerson);
    }

    public void Start()
    {
        var rec = GetExampleListPersonR();

        if (!rec.ok) Console.WriteLine(rec.sInfo);
        else
        {
            _listPersonRec = rec.listPersonR;
            _ListPerson = GetListPerson(rec.listPersonR);
            _iPersonsCount = _ListPerson.Count;
        }
    }

    (bool ok, string sInfo, List<RecPerson> listPersonR) GetExampleListPersonR()
    {
        try
        {   // ✅ C# 9: Records with init-only properties
            var ListRecord = new List<RecPerson>
            {
                new("Irena ", "Szewinska","F", "Poland" ,"00-432", 60, "1965-12-11"), //
                new(" Ewa"  , "  Swoboda","F", "Poland" ,"15-432", 25, "2000-07-08"), //
                new(" Iga"  , "  Swiatek","F", "Poland" ,"00-432", 31, "1994-48-01"),
                new("Serena", "  Wiliams","F", "USA"    ,"21-432", 44, "1981-01-14"), //
                new("  Mark", "    Twain","M", "USA"    ,"00-432",120, "1905-03-24"),
                new("   Tom", "   Pid7ck","M", "GB"     ,"22-432y",27, "2000-12-01"),
                new("Marita", "     Koch","F", "Germany","32-471", 45, "20FR-07-09"),
                new("Thomas", "   Ceccon","M", "Italy"  ,"WN-432", 29, "1996-05-05"),
                new("Javier", "Sotomayor","M", "Cuba"   ,"74-832", 42, "1983-07-04"), //
                new("Zhang ", "   Lin   ","M", "China"  ,"32-471", 33, "1992-11-15"), //
                new("Michael","Schumacher","M","Germany","63-556", 56, "1969-01-03"),
                new("Martin", "Schmitt"  ,"M", "Germany","32-471", 47, "1978-07-09"),
                new("Steffi", "Graf"     ,"F", "Germany","32-471", 56, "1969-06-14"),
                new("Boris" , " Becker"  ,"M", "Germany","32-471", 58, "1967-11-22"),
                new("Erik " , " Zabel"   ,"M", "Germany","32-471", 55, "1970-07-07"),
                new("Jan"   , "Ullrich"  ,"M", "Germany","32-471", 52, "1973-12-02"),
                new("Magda" , " Neuner"  ,"F", "Germany","32-471", 38, "1987-02-09"),
                new("Thibaut"," Pinot"   ,"M", "France" ,"05-332", 35, "1990-05-29"),
                new("Julian","Alaphilippe","M","France" ,"05-332", 32, "1992-06-11"),
                new("Romain", "Bardet"   ,"M", "France" ,"05-332", 35, "1990-11-09"),
                new("Brigitte", "Bardot" ,"F", "France" ,"05-332", 91, "1934-09-28"),
                new("Raquel", "Welch"    ,"F", "USA"    ,"05-332", 85, "1940-09-05"),
                new("Monica", "Belucci"  ,"F", "Italy"  ,"05-332", 61, "1964-09-30"),
            };
            return (true, "Ok", ListRecord);
        }
        catch (Exception e)
        {
            string sInfo = "Error in the Record: " + e.Message;
            return (false, "Ok", new List<RecPerson> { });
        }
    }

    /// <summary>
    /// the method GetListPerson rewrites rows from the list of records (List<RecPerson> xlistPersonR) 
    /// to the list (List<Person> ListPerson). The Person class checks the correctness of the data 
    /// with {set} accessors and if it is incorrect, the row in the ListPerson list is not created at all.
    /// </summary>
    /// <param name="xlistPersonR">Input list of records</param>
    /// <returns></returns>
    List<Person> GetListPerson(List<RecPerson> xlistPersonR)
    {

        var ListPerson = new List<Person>();

        foreach (var item in xlistPersonR)
        {
            try
            {
                printer.PrintPersonObj("Proposal: ", item); //Print data in from Records. Works!

                Person tempPerson = new Person();
                tempPerson.SetFields(item.FirstName, item.SurName, ((item.Gend == "F") ? Gender.F : Gender.M),
                                     item.Country, item.PostCode, item.Age, item.DateBr);
                ListPerson.Add(tempPerson);

                printer.PrintPersonObj("   Entry: ", ListPerson[^1]); //Print data out from class. Works!
            }
            catch (Exception e)
            {
                Console.WriteLine("           ERROR:    " + e.Message);
            }
        }

        return (ListPerson);
    }


    List<Person> GetExampleListPerson()
    {
        try
        {   // ✅ C# 9: Records with init-only properties
            var ListP = new List<Person>
            {
                new("Irena ", "Szewinska",Gender.F,"Poland" ,"00-432", 54, "1965-12-11"), //
                new("   Ewa", "  Swoboda",Gender.F, "Poland" ,"15-432", 25, "2000-07-08"), //
                new("   Iga", "  Swiatek",Gender.F, "Poland" ,"00-432", 31, "1994-48-01")
            };
            return (ListP);
        }
        catch (Exception e)
        {
            string sInfo = "Error in the Record: " + e.Message;
            return (new List<Person> { });
        }
    }

} // End class CorrectPersonList


// ✅ C# 9: Records with `init;` properties
public record RecPerson(string FirstName, string SurName, string Gend,
                        string Country, string PostCode, int Age, string DateBr)
{
    public string FirstName { get; init; } = ValidateName(FirstName);
    public string SurName { get; init; } = SurName;
    public string Gend { get; init; } = Gend;
    public string Country { get; init; } = Country;
    public string PostCode { get; init; } = PostCode;
    public int Age { get; init; } = Age;
    public string DateBr { get; init; } = DateBr;

    private static string ValidateName(string name)
    {
        if (Regex.IsMatch(name, @"\d")) throw new Exception($"The name [{name}] contains numbers!");
        return name;
    }
}

public enum Gender { M, F }


public class Person
{
    // ✅ C# 11: word 'field'
    public string? FirstName
    { get => field; set => field = ValidateName(value); }

    public string? SurName
    { get => field; set => field = ValidateSurName(value); }

    public Gender? Gend
    { get => field; set => field = value; }

    public string? Country
    { get => field; set => field = value; }

    public string? PostCode
    {
        get => field;
        set
        {
            if (field == value) return;
            if (!Regex.IsMatch(value, @"^\d{2}-\d{3}$"))
                throw new Exception($"Invalid PostCode format [{value}]. Expected: '00-000'.");
            field = value;
        }
    }

    public int Age
    {
        get => field;
        set
        {
            if (value > 110) throw new Exception($"The Age [{value}] is too big.");
            field = value;
        }
    }

    public string? DateBr
    {
        get => field;
        set => field = ValidateDate(value);
    }

    public Person(string? firstName, string? surName, Gender? gend,
                          string? country, string? postCode, int age, string dateBr)
    {
        SetFields(firstName, surName, gend, country, postCode, age, dateBr);
    }

    public Person()
    {
    }

    public void SetFields(string? firstName, string? surName, Gender? gend,
                          string? country, string? postCode, int age, string dateBr)
    {
        FirstName = firstName;
        SurName = surName;
        Gend = gend;
        Country = country;
        PostCode = postCode;
        Age = age;
        DateBr = dateBr;
    }

    private static string ValidateSurName(string? name)
    {
        return ValidateName(name).Trim();
    }

    private static string ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new Exception("Name cannot be empty.");
        if (Regex.IsMatch(name, @"\d")) throw new Exception($"The name [{name}] contains numbers!");
        return name;
    }

    private static string ValidateDate(string? date)
    {
        if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
            throw new Exception($"Invalid date format [{date}]. Use YYYY-MM-DD.");
        return date;
    }

}

// ✅ IPrintPerson interface
public interface IPrintPerson
{
    // ✅ C# 8: Body of method in interface

    void PrintPersonObj(string xInfo, RecPerson xR) =>
    PrintPerson(xInfo, xR.FirstName, xR.SurName, xR.Gend, xR.Country, xR.PostCode, xR.Age, xR.DateBr);

    public void PrintPersonObj(string xInfo, Person xR, bool xPrintLast = true) =>
        PrintPerson(xInfo, xR.FirstName, xR.SurName, xR.Gend.ToString(), xR.Country,
            xR.PostCode, xR.Age, xR.DateBr, xPrintLast);

    public void PrintListPerson(string xInfo, List<Person> xLP, bool xPrintLast = true, string xsHead = "The New List:")
    {
        Console.WriteLine($"\n\r{xsHead}   (Rows count:{xLP.Count} )");
        foreach (var xR in xLP)
        {
            PrintPersonObj(xInfo, xR, xPrintLast);
        }
    }

    void PrintPerson(string? xInfo, string? xFname, string? xSname, string? xGend,
                        string? xCountry, string? xCode, int xAge, string? xDate,
                        bool xPrintLast = true)
    {
        string mess = $"Person: {FixedString(xFname + " " + xSname, 16)}" +
                      $" [{xGend}], {FixedString(xCountry, 8)}" +
                      $" Age: {xAge,3}";
        if (xPrintLast)
            mess += $", Code: {xCode}, BirthDate: {xDate}.";

        Console.WriteLine(xInfo + " " + mess);
    }

    string FixedString(string? xmessage, int xLen)
    {
        return (xmessage.PadRight(xLen).Substring(0, xLen));
    }

    void PrintHead(string xInfo, int xCount)
    {
        Console.WriteLine($"\n\r{xInfo} (count: {xCount})");
    }
}
