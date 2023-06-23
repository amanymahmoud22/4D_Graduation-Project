namespace Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.Clear();
            //Console.WriteLine("Hello, World!");
            //Console.Write( "Enetr your Name ");
            //string name= (Console.ReadLine());
            //Console.Clear();
            //Console.WriteLine($"Hi {name}");


            //Console.Clear();
            //Console.WriteLine("Hello, World!");
            //Console.Write("Enetr your age ");
            //int age =int.Parse(Console.ReadLine());
            //Console.Clear();
            //int year = 2023 - age;
            //Console.WriteLine($"you born in  {year}");


            //Console.Clear();
            //Console.WriteLine("Hello, World!");
            //Console.Write("Enetr your Grade ");
            //int grade = int.Parse(Console.ReadLine());
            //string op;
            //Console.Clear();
            
            //if (grade<=100 && grade >= 0) 
            //{
            //    if (grade >= 85)
            //    {
            //        Console.WriteLine($"Grade = A");
            //    }

            //    else if (grade >= 75)
            //    {
            //        Console.WriteLine($"Grade = B");
            //    }

            //    else if (grade >= 65)
            //    {
            //        Console.WriteLine($"Grade = C");
            //    }

            //    else if (grade >= 50)
            //    {
            //        Console.WriteLine($"Grade = D");
            //    }

            //    else if (grade < 50)
            //    {
            //        Console.WriteLine($"Grade = F");
            //    }

            //    else
            //    {
            //        Console.WriteLine($"ERROR");
            //    }

            //}

            //switch (grade) 
            //{
            //    case (grade>=90):
            //        Console.WriteLine($"Grade = A");
            //    break;

            //    case (grade >= 80):
            //        Console.WriteLine($"Grade = B");
            //    break;

            //    case (grade >= 60):
            //        Console.WriteLine($"Grade = C");
            //    break;

            //    case (grade >= 50):
            //        Console.WriteLine($"Grade = D");
            //    break;

            //    case (grade < 50):
            //        Console.WriteLine($"Grade = F");
            //        break;

            Console.Clear();
            int sum=0;
            do
            {
                Console.WriteLine("Enter a Number");
                sum += int.Parse(Console.ReadLine());

            } while (sum <= 100);

            Console.WriteLine($"Total={sum}");
            Console.ReadLine();



        }
    }
} 