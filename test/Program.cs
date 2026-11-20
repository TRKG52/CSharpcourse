namespace test
{
    using System;
    internal class Program
    {
        static void Main(string[] args)
        {
            int minutes = int.Parse(Console.ReadLine()!);
            int hours = minutes / 60;
            int remainingminutes = minutes % 60;
            Console.WriteLine($"{hours} {remainingminutes}");
        }
    }
}
