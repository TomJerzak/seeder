using System;

namespace Seeder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Seeder!");
            if (args != null && args.Length > 0)
            {
                foreach (var arg in args)
                {
                    Console.WriteLine(arg);
                }
            }
        }
    }
}
