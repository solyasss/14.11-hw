using System;
using System.Collections;
using System.IO;

public class Academy_Group : ICloneable, IEnumerable,IEnumerator
{
    private ArrayList students;
    private int count;
    private int pos= -1; // для ienumerator

    public Academy_Group()
    {
        students = new ArrayList();
        count = 0;
    }

    public void add(Student student)
    {
        students.Add(student);
        count++;
    }

    public void remove(string surname)
    {
        foreach (Student student in students)
        {
            if (student.Surname == surname)
            {
                students.Remove(student);
                count--;
                Console.WriteLine("You removed student");
                return;
            }
        }
        Console.WriteLine("Can not find student");
    }

    public void edit(string surname, Student new_student)
    {
        for (int i = 0; i < students.Count; i++)
        {
            Student student = (Student)students[i];
            if (student.Surname == surname)
            {
                students[i] = new_student;
                Console.WriteLine("You updated student info");
                return;
            }
        }
        Console.WriteLine("Can not find student");
    }

    public void print()
    {
        Console.WriteLine("Academy group:");
        foreach (Student student in students)
        {
            student.print();
        }
    }

    public void sort(IComparer comparer)
    {
        students.Sort(comparer);
        Console.WriteLine("Sorted!");
    }

    public object Clone()
    {
        Academy_Group new_group = new Academy_Group();
        foreach (Student student in students)
        {
            new_group.add((Student)student.Clone());
        }
        return new_group;
    }

    public void save(string filePath)
    {
        try
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                writer.Write(students.Count);

                foreach (Student student in students)
                {
                    writer.Write(student.Name);
                    writer.Write(student.Surname);
                    writer.Write(student.Age);
                    writer.Write(student.Phone);
                    writer.Write(student.Average);
                    writer.Write(student.Number_Of_Group);
                }
            }
            Console.WriteLine("File saved");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with saving: {ex.Message}");
        }
    }

    public void load(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                students.Clear();
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    int studentCount = reader.ReadInt32();

                    for (int i = 0; i < studentCount; i++)
                    {
                        string name = reader.ReadString();
                        string surname = reader.ReadString();
                        int age = reader.ReadInt32();
                        string phone = reader.ReadString();
                        double average = reader.ReadDouble();
                        int group_num = reader.ReadInt32();

                        Student student = new Student(name, surname, age, phone, average, group_num);
                        students.Add(student);
                    }
                }
                Console.WriteLine("File loaded");
                count = students.Count;
            }
            else
            {
                Console.WriteLine("Can not load file");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with loading: {ex.Message}");
        }
    }

    public void search(string surname)
    {
        foreach (Student student in students)
        {
            if (student.Surname == surname)
            {
                student.print();
                return;
            }
        }
        Console.WriteLine("Can not find student");
    }
    public IEnumerator GetEnumerator()
    {
        return this;
    }
    
    #region enumerator
    // перемещаю к след
    public bool MoveNext()
    {
        if (pos < students.Count - 1)
        {
            pos++;
            return true;
        }
        Reset(); // сброс в конеу
        return false;
    }

    public void Reset()
    {
        pos = -1;
    }

    public object Current
    {
        get
        {
            if (pos == -1 || pos>= students.Count)
                throw new InvalidOperationException();
            return students[pos]; // возвращаю текущий элемент 
        }
    }
}
#endregion enumerator

