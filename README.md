# C# Collections: using Yield and SelectMany

.NET9 C# app: "Yield" queries and modern techniques to C# 11.

## Description

This "Yield" program is written in C# .NET 9 which allows you to use C# 12 language syntax.
Here the program mainly demonstrates collection management techniques.
The C# program processes a list of `Person` objects using various techniques, 
including LINQ queries, `yield return`, and `SelectMany`. The program:

1. Validates and filters input data.
2. Sorts the list using LINQ.
3. Implements filtering using `yield return` and `SelectMany`.
4. Demonstrates `yield` in mapping, grouping, and pagination scenarios.
5. Prints the processed data in a structured format.

## Core Features

### 1. Data Validation and Sorting
The program starts by validating the input data. If any data is incorrect (e.g., invalid date formats or names with numbers), the program reports errors and excludes such entries.

After validation, the program:
- Sorts the list by `Age` (ascending order) and then by `Country` using LINQ (`OrderBy` and `ThenBy`).
- Converts the sorted `IOrderedEnumerable<Person>` into a `List<Person>`.

### 2. Yield-based Filtering
The program provides two methods to filter persons older than 32 years and not from France:

#### a) `ListPersonWithYield1`
This method iterates through the list and uses an `if` statement inside a `foreach` loop to filter results, returning only matching persons using `yield return`.

#### b) `ListPersonWithYield2`
This method utilizes LINQ’s `Where` clause to apply filtering and then yields each result.

### 3. SelectMany-based Filtering
- The `ListPersonWithSelectMany` method filters elements using LINQ’s `Where` and flattens the output using `SelectMany`.
- `yield return` cannot be directly used inside a lambda expression, making `SelectMany` a good alternative for such operations.

### 4. Additional Uses of Yield

#### a) Yield for Transformation (Mapping)
- The `ListPersonNamesWithYield` method demonstrates how `yield return` can be used to format person data into a string before returning.

#### b) Yield for Grouping
- The `GroupPersonsByCountry` method groups persons by country using LINQ’s `GroupBy` and then yields tuples containing the country and the corresponding list of people.

#### c) Yield for Pagination (Lazy Loading)
- The `PaginatePersons` method uses `yield return` to lazily return pages of data, enabling efficient handling of large datasets.

## When to Use `yield return` vs. LINQ

### Description
This program demonstrates various ways of processing collections in C# using `yield return` and LINQ queries. It focuses on performance comparisons and practical use cases for each approach.

<h4>Key Features</h4>

- Uses `yield return` to create an iterator that generates values lazily.
- Implements LINQ queries to filter and transform data.
- Compares the performance and advantages of `yield` vs. LINQ.
- Explains when to use each approach depending on the scenario.

### When to Use What?
| Feature          | `yield return`                  | LINQ Queries                        |
|-----------------|--------------------------------|-------------------------------------|
| **Performance**  | More efficient for large data if only partial results are needed | Optimized for batch processing but may cause overhead |
| **Lazy Evaluation** | Yes, generates items on demand | Some methods are lazy (e.g., `Where`), but others (e.g., `ToList`) force execution |
| **Complexity**   | More complex to write and debug | More readable and declarative |
| **Reusability**  | Useful for streaming scenarios | Easier to use in method chains |
| **Flexibility**  | Allows custom iteration logic | Provides built-in powerful transformations |

### Advantages of `yield`
- Allows iteration without materializing the entire collection.
- Can handle infinite sequences.
- Saves memory for large datasets when only part of the results is needed.

### Advantages of LINQ
- More concise and readable syntax.
- Optimized query execution.
- Built-in support for filtering, projection, and aggregation.

### Why `yield` Cannot Be Used in Lambda Expressions
The `yield` keyword requires a method to be an iterator block, meaning it must return `IEnumerable<T>` and be compiled into a state machine. Lambda expressions are anonymous functions and do not support this transformation.

### Why `SelectMany` Must Be Used
`SelectMany` is necessary when a collection property within an element contains multiple items. It flattens the structure by extracting these items into a single sequence, ensuring proper iteration over nested collections.

### Example
```csharp
public static IEnumerable<int> YieldExample()
{
    yield return 1;
    yield return 2;
    yield return 3;
}
```

```csharp
var numbers = new List<int[]> { new[] { 1, 2 }, new[] { 3, 4 } };
var flattened = numbers.SelectMany(arr => arr); // Flattens to {1, 2, 3, 4}
```


- Use `yield return` when you need **lazy evaluation** (processing elements one by one) and **control flow flexibility** (e.g., early termination).
- Use LINQ when performing **batch operations** on collections, especially when **chaining transformations**.
- `yield return` is ideal for **custom iteration logic**, whereas LINQ is better for **concise and declarative transformations**.

## Example Console Output

```
Hello, program is starting ...

Checking the correctness of the input data by the set accessors:

Proposal:  Person:  Ewa   Swoboda   [F], Poland   Age:  25, Code: 15-432, BirthDate: 2000-07-08.
   Entry:  Person:  Ewa Swoboda     [F], Poland   Age:  25, Code: 15-432, BirthDate: 2000-07-08.
Proposal:  Person:  Iga   Swiatek   [F], Poland   Age:  31, Code: 00-432, BirthDate: 1994-48-01.
           ERROR:    Invalid date format [1994-48-01]. Use YYYY-MM-DD.
Proposal:  Person: Serena   Wiliams [F], USA      Age:  44, Code: 21-432, BirthDate: 1981-01-14.
   Entry:  Person: Serena Wiliams   [F], USA      Age:  44, Code: 21-432, BirthDate: 1981-01-14.
Proposal:  Person:   Mark     Twain [M], USA      Age: 120, Code: 00-432, BirthDate: 1905-03-24.
           ERROR:    The Age [120] is too big.
Proposal:  Person:    Tom    Pid7ck [M], GB       Age:  27, Code: 22-432y, BirthDate: 2000-12-01.
           ERROR:    The name [   Pid7ck] contains numbers!
Proposal:  Person: Marita      Koch [F], Germany  Age:  45, Code: 32-471, BirthDate: 20FR-07-09.
           ERROR:    Invalid date format [20FR-07-09]. Use YYYY-MM-DD.
Proposal:  Person: Thomas    Ceccon [M], Italy    Age:  29, Code: WN-432, BirthDate: 1996-05-05.
           ERROR:    Invalid PostCode format [WN-432]. Expected: '00-000'.
Proposal:  Person: Javier Sotomayor [M], Cuba     Age:  42, Code: 74-832, BirthDate: 1983-07-04.
   Entry:  Person: Javier Sotomayor [M], Cuba     Age:  42, Code: 74-832, BirthDate: 1983-07-04.

cut ....

Correct list (List<Person> listPerson):   (Rows count:18 )
   Person:  Ewa Swoboda     [F], Poland   Age:  25, Code: 15-432, BirthDate: 2000-07-08.
   Person: Julian Alaphilip [M], France   Age:  32, Code: 05-332, BirthDate: 1992-06-11.
   Person: Zhang  Lin       [M], China    Age:  33, Code: 32-471, BirthDate: 1992-11-15.
   Person: Thibaut Pinot    [M], France   Age:  35, Code: 05-332, BirthDate: 1990-05-29.
   Person: Romain Bardet    [M], France   Age:  35, Code: 05-332, BirthDate: 1990-11-09.
   Person: Magda Neuner     [F], Germany  Age:  38, Code: 32-471, BirthDate: 1987-02-09.
   Person: Javier Sotomayor [M], Cuba     Age:  42, Code: 74-832, BirthDate: 1983-07-04.
   Person: Serena Wiliams   [F], USA      Age:  44, Code: 21-432, BirthDate: 1981-01-14.
   Person: Martin Schmitt   [M], Germany  Age:  47, Code: 32-471, BirthDate: 1978-07-09.
   Person: Jan Ullrich      [M], Germany  Age:  52, Code: 32-471, BirthDate: 1973-12-02.
   Person: Erik  Zabel      [M], Germany  Age:  55, Code: 32-471, BirthDate: 1970-07-07.
   Person: Michael Schumach [M], Germany  Age:  56, Code: 63-556, BirthDate: 1969-01-03.
   Person: Steffi Graf      [F], Germany  Age:  56, Code: 32-471, BirthDate: 1969-06-14.
   Person: Boris Becker     [M], Germany  Age:  58, Code: 32-471, BirthDate: 1967-11-22.
   Person: Irena  Szewinska [F], Poland   Age:  60, Code: 00-432, BirthDate: 1965-12-11.
   Person: Monica Belucci   [F], Italy    Age:  61, Code: 05-332, BirthDate: 1964-09-30.
   Person: Raquel Welch     [F], USA      Age:  85, Code: 05-332, BirthDate: 1940-09-05.
   Person: Brigitte Bardot  [F], France   Age:  91, Code: 05-332, BirthDate: 1934-09-28.

1) YIELD-1 series: item.Age > 32 and Country != 'France'
   Person: Zhang  Lin       [M], China    Age:  33, Code: 32-471, BirthDate: 1992-11-15.
   Person: Magda Neuner     [F], Germany  Age:  38, Code: 32-471, BirthDate: 1987-02-09.
   Person: Javier Sotomayor [M], Cuba     Age:  42, Code: 74-832, BirthDate: 1983-07-04.
   Person: Serena Wiliams   [F], USA      Age:  44, Code: 21-432, BirthDate: 1981-01-14.
   Person: Martin Schmitt   [M], Germany  Age:  47, Code: 32-471, BirthDate: 1978-07-09.
   Person: Jan Ullrich      [M], Germany  Age:  52, Code: 32-471, BirthDate: 1973-12-02.
   Person: Erik  Zabel      [M], Germany  Age:  55, Code: 32-471, BirthDate: 1970-07-07.
   Person: Michael Schumach [M], Germany  Age:  56, Code: 63-556, BirthDate: 1969-01-03.
   Person: Steffi Graf      [F], Germany  Age:  56, Code: 32-471, BirthDate: 1969-06-14.
   Person: Boris Becker     [M], Germany  Age:  58, Code: 32-471, BirthDate: 1967-11-22.
   Person: Irena  Szewinska [F], Poland   Age:  60, Code: 00-432, BirthDate: 1965-12-11.
   Person: Monica Belucci   [F], Italy    Age:  61, Code: 05-332, BirthDate: 1964-09-30.
   Person: Raquel Welch     [F], USA      Age:  85, Code: 05-332, BirthDate: 1940-09-05.

2) YIELD-2 series: item.Age > 32 and Country != 'France'
   Person: Zhang  Lin       [M], China    Age:  33, Code: 32-471, BirthDate: 1992-11-15.
   Person: Magda Neuner     [F], Germany  Age:  38, Code: 32-471, BirthDate: 1987-02-09.
   Person: Javier Sotomayor [M], Cuba     Age:  42, Code: 74-832, BirthDate: 1983-07-04.
   Person: Serena Wiliams   [F], USA      Age:  44, Code: 21-432, BirthDate: 1981-01-14.
   Person: Martin Schmitt   [M], Germany  Age:  47, Code: 32-471, BirthDate: 1978-07-09.
   Person: Jan Ullrich      [M], Germany  Age:  52, Code: 32-471, BirthDate: 1973-12-02.
   Person: Erik  Zabel      [M], Germany  Age:  55, Code: 32-471, BirthDate: 1970-07-07.
   Person: Michael Schumach [M], Germany  Age:  56, Code: 63-556, BirthDate: 1969-01-03.
   Person: Steffi Graf      [F], Germany  Age:  56, Code: 32-471, BirthDate: 1969-06-14.
   Person: Boris Becker     [M], Germany  Age:  58, Code: 32-471, BirthDate: 1967-11-22.
   Person: Irena  Szewinska [F], Poland   Age:  60, Code: 00-432, BirthDate: 1965-12-11.
   Person: Monica Belucci   [F], Italy    Age:  61, Code: 05-332, BirthDate: 1964-09-30.
   Person: Raquel Welch     [F], USA      Age:  85, Code: 05-332, BirthDate: 1940-09-05.

3) SelectMany: item.Age > 32 and Country != 'France'
   Person: Zhang  Lin       [M], China    Age:  33, Code: 32-471, BirthDate: 1992-11-15.
   Person: Magda Neuner     [F], Germany  Age:  38, Code: 32-471, BirthDate: 1987-02-09.
   Person: Javier Sotomayor [M], Cuba     Age:  42, Code: 74-832, BirthDate: 1983-07-04.
   Person: Serena Wiliams   [F], USA      Age:  44, Code: 21-432, BirthDate: 1981-01-14.
   Person: Martin Schmitt   [M], Germany  Age:  47, Code: 32-471, BirthDate: 1978-07-09.
   Person: Jan Ullrich      [M], Germany  Age:  52, Code: 32-471, BirthDate: 1973-12-02.
   Person: Erik  Zabel      [M], Germany  Age:  55, Code: 32-471, BirthDate: 1970-07-07.
   Person: Michael Schumach [M], Germany  Age:  56, Code: 63-556, BirthDate: 1969-01-03.
   Person: Steffi Graf      [F], Germany  Age:  56, Code: 32-471, BirthDate: 1969-06-14.
   Person: Boris Becker     [M], Germany  Age:  58, Code: 32-471, BirthDate: 1967-11-22.
   Person: Irena  Szewinska [F], Poland   Age:  60, Code: 00-432, BirthDate: 1965-12-11.
   Person: Monica Belucci   [F], Italy    Age:  61, Code: 05-332, BirthDate: 1964-09-30.
   Person: Raquel Welch     [F], USA      Age:  85, Code: 05-332, BirthDate: 1940-09-05.

4) Yield with Transformation (Mapping)
  Ewa Swoboda         (25 years)
  Julian Alaphilippe  (32 years)
  Zhang Lin           (33 years)
  Thibaut Pinot       (35 years)
  Romain Bardet       (35 years)
  Magda Neuner        (38 years)
  Javier Sotomayor    (42 years)
  Serena Wiliams      (44 years)
  Martin Schmitt      (47 years)
  Jan Ullrich         (52 years)
  Erik Zabel          (55 years)
  Michael Schumacher  (56 years)
  Steffi Graf         (56 years)
  Boris Becker        (58 years)
  Irena Szewinska     (60 years)
  Monica Belucci      (61 years)
  Raquel Welch        (85 years)
  Brigitte Bardot     (91 years)

5) Grouping with Yield:

Country: Poland:
   Person:  Ewa Swoboda     [F], Poland   Age:  25, Code: 15-432, BirthDate: 2000-07-08.
   Person: Irena  Szewinska [F], Poland   Age:  60, Code: 00-432, BirthDate: 1965-12-11.

Country: France:
   Person: Julian Alaphilip [M], France   Age:  32, Code: 05-332, BirthDate: 1992-06-11.
   Person: Thibaut Pinot    [M], France   Age:  35, Code: 05-332, BirthDate: 1990-05-29.
   Person: Romain Bardet    [M], France   Age:  35, Code: 05-332, BirthDate: 1990-11-09.
   Person: Brigitte Bardot  [F], France   Age:  91, Code: 05-332, BirthDate: 1934-09-28.

Country: China:
   Person: Zhang  Lin       [M], China    Age:  33, Code: 32-471, BirthDate: 1992-11-15.

Country: Germany:
   Person: Magda Neuner     [F], Germany  Age:  38, Code: 32-471, BirthDate: 1987-02-09.
   Person: Martin Schmitt   [M], Germany  Age:  47, Code: 32-471, BirthDate: 1978-07-09.
   Person: Jan Ullrich      [M], Germany  Age:  52, Code: 32-471, BirthDate: 1973-12-02.
   Person: Erik  Zabel      [M], Germany  Age:  55, Code: 32-471, BirthDate: 1970-07-07.
   Person: Michael Schumach [M], Germany  Age:  56, Code: 63-556, BirthDate: 1969-01-03.
   Person: Steffi Graf      [F], Germany  Age:  56, Code: 32-471, BirthDate: 1969-06-14.
   Person: Boris Becker     [M], Germany  Age:  58, Code: 32-471, BirthDate: 1967-11-22.

Country: Cuba:
   Person: Javier Sotomayor [M], Cuba     Age:  42, Code: 74-832, BirthDate: 1983-07-04.

Country: USA:
   Person: Serena Wiliams   [F], USA      Age:  44, Code: 21-432, BirthDate: 1981-01-14.
   Person: Raquel Welch     [F], USA      Age:  85, Code: 05-332, BirthDate: 1940-09-05.

Country: Italy:
   Person: Monica Belucci   [F], Italy    Age:  61, Code: 05-332, BirthDate: 1964-09-30.

6) Pagination with Yield (Lazy Loading) (4 items):

 Group 1:
     Person:  Ewa Swoboda     [F], Poland   Age:  25, Code: 15-432, BirthDate: 2000-07-08.
     Person: Julian Alaphilip [M], France   Age:  32, Code: 05-332, BirthDate: 1992-06-11.
     Person: Zhang  Lin       [M], China    Age:  33, Code: 32-471, BirthDate: 1992-11-15.
     Person: Thibaut Pinot    [M], France   Age:  35, Code: 05-332, BirthDate: 1990-05-29.

 Group 2:
     Person: Romain Bardet    [M], France   Age:  35, Code: 05-332, BirthDate: 1990-11-09.
     Person: Magda Neuner     [F], Germany  Age:  38, Code: 32-471, BirthDate: 1987-02-09.
     Person: Javier Sotomayor [M], Cuba     Age:  42, Code: 74-832, BirthDate: 1983-07-04.
     Person: Serena Wiliams   [F], USA      Age:  44, Code: 21-432, BirthDate: 1981-01-14.

 Group 3:
     Person: Martin Schmitt   [M], Germany  Age:  47, Code: 32-471, BirthDate: 1978-07-09.
     Person: Jan Ullrich      [M], Germany  Age:  52, Code: 32-471, BirthDate: 1973-12-02.
     Person: Erik  Zabel      [M], Germany  Age:  55, Code: 32-471, BirthDate: 1970-07-07.
     Person: Michael Schumach [M], Germany  Age:  56, Code: 63-556, BirthDate: 1969-01-03.

 Group 4:
     Person: Steffi Graf      [F], Germany  Age:  56, Code: 32-471, BirthDate: 1969-06-14.
     Person: Boris Becker     [M], Germany  Age:  58, Code: 32-471, BirthDate: 1967-11-22.
     Person: Irena  Szewinska [F], Poland   Age:  60, Code: 00-432, BirthDate: 1965-12-11.
     Person: Monica Belucci   [F], Italy    Age:  61, Code: 05-332, BirthDate: 1964-09-30.

 Group 5:
     Person: Raquel Welch     [F], USA      Age:  85, Code: 05-332, BirthDate: 1940-09-05.
     Person: Brigitte Bardot  [F], France   Age:  91, Code: 05-332, BirthDate: 1934-09-28.
```

## Conclusion
This program effectively demonstrates various collection-processing techniques 
in C#, emphasizing the power of `yield return` for lazy evaluation and LINQ for 
declarative data manipulation. The combination of these techniques ensures both 
efficiency and readability.

