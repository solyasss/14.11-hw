using System;
using System.Collections;

public class Student : Person, IComparable, ICloneable
{
    protected double average;
    protected int number_of_group;

    public double Average
    {
        get { return average; }
        set
        {
            if (value < 0 || value > 10)
                throw new ArgumentException("Your average must be from 0 to 10");
            average = value;
        }
    }

    public int Number_Of_Group
    {
        get { return number_of_group; }
        set { number_of_group = value; }
    }

    public Student() { }

    public Student(string name, string surname, int age, string phone, double average, int number_of_group)
        : base(name, surname, age, phone)
    {
        Average = average;
        Number_Of_Group = number_of_group;
    }

    public override void print()
    {
        base.print();
        Console.WriteLine("------------------------------");
        Console.WriteLine($"Average:        {Average}");
        Console.WriteLine($"Group number:   {Number_Of_Group}");
        Console.WriteLine("------------------------------");
    }

    public int CompareTo(object obj)  //метод реализует сравнение студентов
    {
        if (obj is Student second_student)
            return Average.CompareTo(second_student.Average);
        throw new ArgumentException("not a student");
    }

    public object Clone() 
    {
        return new Student(Name, Surname, Age, Phone, Average, Number_Of_Group);
    }
      //сортировка по имени 
    public class sort_surname : IComparer
    {
        public int Compare(object obj_1, object obj_2)
        {
            if (obj_1 is Student st1 && obj_2 is Student st2)
                return st1.Surname.CompareTo(st2.Surname);
            throw new ArgumentException(" not a student");
        }
    }
     // сортировка по номеру группы
    public class sort_group : IComparer
    {
        public int Compare(object obj_1, object obj_2)
        {
            if (obj_1 is Student st1 && obj_2 is Student st2)
                return st1.Number_Of_Group.CompareTo(st2.Number_Of_Group);
            throw new ArgumentException("Object is not a student");
        }
    }
}
