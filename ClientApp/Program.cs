using System;
using System.Reflection;
using System.IO;
using System.Collections;

class Main_Class
{
    static void Main(string[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Clear();
        
        string dllDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DLL");
        
        string academyGroupDll = Path.Combine(dllDirectory, "AcademyGroupLibrary.dll");
        Assembly academyGroupAssembly = Assembly.LoadFrom(academyGroupDll);
        
        Type[] types = academyGroupAssembly.GetTypes();
        Console.WriteLine("All types found");
        foreach (var t in types)
        {
            Console.WriteLine(t.FullName);
        }
        
        Type academyGroupType = academyGroupAssembly.GetType("Academy_Group");

        if (academyGroupType == null)
        {
            Console.WriteLine("Not found");
            return;
        }
        
        object group = Activator.CreateInstance(academyGroupType);

        string filePath = "students.dat";

        object new_group = null;
        while (true)
        {
            Console.WriteLine("\n-----Academy menu-----");
            Console.WriteLine("1. Add student");
            Console.WriteLine("2. Remove student");
            Console.WriteLine("3. Edit student");
            Console.WriteLine("4. Print group");
            Console.WriteLine("5. Search student");
            Console.WriteLine("6. Save to file");
            Console.WriteLine("7. Load from file");
            Console.WriteLine("8. Sort students");
            Console.WriteLine("9. Clone group");
            Console.WriteLine("10. Print cloned group");
            Console.WriteLine("11. Enumerate students");
            Console.WriteLine("12. Exit");

            Console.Write("Enter your choice: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid choice, please enter a number.");
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        add(group, academyGroupType, dllDirectory);
                        break;
                    case 2:
                        remove(group, academyGroupType);
                        break;
                    case 3:
                        edit(group, academyGroupType, dllDirectory);
                        break;
                    case 4:
                        invokeMethod(group, "print");
                        break;
                    case 5:
                        search(group, academyGroupType);
                        break;
                    case 6:
                        invokeMethod(group, "save", new object[] { filePath });
                        break;
                    case 7:
                        invokeMethod(group, "load", new object[] { filePath });
                        break;
                    case 8:
                        sort(group, academyGroupType, dllDirectory);
                        break;
                    case 9:
                        {
                            MethodInfo cloneMethod = academyGroupType.GetMethod("Clone");
                            new_group = cloneMethod.Invoke(group, null);
                            Console.WriteLine("Cloned!");
                            break;
                        }
                    case 10:
                        if (new_group != null)
                        {
                            invokeMethod(new_group, "print");
                        }
                        else
                            Console.WriteLine("Not found");
                        break;
                    case 11:
                        print_enum(group);
                        break;
                    case 12:
                        return;
                    default:
                        Console.WriteLine("Incorrect choice");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static void invokeMethod(object obj, string methodName, object[] parameters = null)
    {
        MethodInfo method = obj.GetType().GetMethod(methodName);
        method.Invoke(obj, parameters);
    }

    private static void print_enum(object group)
    {
        Console.WriteLine("Students:");
        foreach (var student in (IEnumerable)group)
        {
            invokeMethod(student, "print");
        }
    }

    private static void sort(object group, Type academyGroupType, string dllDirectory)
    {
        Console.WriteLine("\nChoose sorting method:");
        Console.WriteLine("1. Sort by surname");
        Console.WriteLine("2. Sort by group number");
        Console.Write("Enter your sorting choice: ");

        if (int.TryParse(Console.ReadLine(), out int sort_choice))
        {
            object comparer = null;

            switch (sort_choice)
            {
                case 1:
                    string studentDll = Path.Combine(dllDirectory, "StudentLibrary.dll");
                    Assembly studentAssembly = Assembly.LoadFrom(studentDll);

                    Type sortSurnameType = studentAssembly.GetType("Student+sort_surname");
                    if (sortSurnameType == null)
                    {
                        Console.WriteLine("Not found");
                        return;
                    }
                    comparer = Activator.CreateInstance(sortSurnameType);
                    break;
                case 2:
                    studentDll = Path.Combine(dllDirectory, "StudentLibrary.dll");
                    studentAssembly = Assembly.LoadFrom(studentDll);

                    Type sortGroupType = studentAssembly.GetType("Student+sort_group");
                    if (sortGroupType == null)
                    {
                        Console.WriteLine("Not found");
                        return;
                    }
                    comparer = Activator.CreateInstance(sortGroupType);
                    break;
                default:
                    Console.WriteLine("Incorrect sorting choice");
                    return;
            }

            MethodInfo sortMethod = academyGroupType.GetMethod("sort");
            sortMethod.Invoke(group, new object[] { comparer });

            Console.WriteLine("Students sorted");
            invokeMethod(group, "print");
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }

    private static void add(object group, Type academyGroupType, string dllDirectory)
    {
        Console.Write("Enter name: ");
        string name = Console.ReadLine();
        Console.Write("Enter surname: ");
        string surname = Console.ReadLine();
        Console.Write("Enter age: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("Invalid age");
            return;
        }
        Console.Write("Enter phone: ");
        string phone = Console.ReadLine();
        Console.Write("Enter average: ");
        if (!double.TryParse(Console.ReadLine(), out double average))
        {
            Console.WriteLine("Invalid average");
            return;
        }
        Console.Write("Enter group number: ");
        if (!int.TryParse(Console.ReadLine(), out int group_num))
        {
            Console.WriteLine("Invalid group number");
            return;
        }

        string studentDll = Path.Combine(dllDirectory, "StudentLibrary.dll");
        Assembly studentAssembly = Assembly.LoadFrom(studentDll);
        Type studentType = studentAssembly.GetType("Student");

        if (studentType == null)
        {
            Console.WriteLine("Not found");
            return;
        }

        object new_student = Activator.CreateInstance(studentType, new object[] { name, surname, age, phone, average, group_num });

        MethodInfo addMethod = academyGroupType.GetMethod("add");
        addMethod.Invoke(group, new object[] { new_student });
    }

    private static void remove(object group, Type academyGroupType)
    {
        Console.Write("Enter surname of student to remove: ");
        string remove_surname = Console.ReadLine();
        MethodInfo removeMethod = academyGroupType.GetMethod("remove");
        removeMethod.Invoke(group, new object[] { remove_surname });
    }

    private static void edit(object group, Type academyGroupType, string dllDirectory)
    {
        Console.Write("Enter surname of student to edit: ");
        string edit_surname = Console.ReadLine();

        Console.Write("Enter new name: ");
        string name = Console.ReadLine();
        Console.Write("Enter new surname: ");
        string surname = Console.ReadLine();
        Console.Write("Enter new age: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("Invalid age");
            return;
        }
        Console.Write("Enter new phone: ");
        string phone = Console.ReadLine();
        Console.Write("Enter new average: ");
        if (!double.TryParse(Console.ReadLine(), out double average))
        {
            Console.WriteLine("Invalid average");
            return;
        }
        Console.Write("Enter new group number: ");
        if (!int.TryParse(Console.ReadLine(), out int group_num))
        {
            Console.WriteLine("Invalid group number");
            return;
        }

        string studentDll = Path.Combine(dllDirectory, "StudentLibrary.dll");
        Assembly studentAssembly = Assembly.LoadFrom(studentDll);
        Type studentType = studentAssembly.GetType("Student");

        if (studentType == null)
        {
            Console.WriteLine("Not found");
            return;
        }

        object ready_student = Activator.CreateInstance(studentType, new object[] { name, surname, age, phone, average, group_num });

        MethodInfo editMethod = academyGroupType.GetMethod("edit");
        editMethod.Invoke(group, new object[] { edit_surname, ready_student });
    }

    private static void search(object group, Type academyGroupType)
    {
        Console.Write("Enter surname of student to search: ");
        string search_surname = Console.ReadLine();
        MethodInfo searchMethod = academyGroupType.GetMethod("search");
        searchMethod.Invoke(group, new object[] { search_surname });
    }
}
