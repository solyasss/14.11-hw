using System;

public class Person
{
    protected string name;
    protected string surname;
    protected int age;
    protected string phone;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Surname
    {
        get { return surname; }
        set { surname = value; }
    }

    public int Age
    {
        get { return age; }
        set
        {
            if (value < 0 || value > 100)
                throw new ArgumentException("Your age is not correct ");
            age = value;
        }
    }

    public string Phone
    {
        get { return phone; }
        set { phone = value; }
    }

    public Person() { }

    public Person(string name, string surname, int age, string phone)
    {
        Name = name;
        Surname = surname;
        Age = age;
        Phone = phone;
    }

    public virtual void print()
    {
        Console.WriteLine("Information:");
        Console.WriteLine("------------------------------");
        Console.WriteLine($"Name:      {Name}");
        Console.WriteLine($"Surname:   {Surname}");
        Console.WriteLine($"Age:       {Age}");
        Console.WriteLine($"Phone:     {Phone}");
        Console.WriteLine("------------------------------");

    }
}