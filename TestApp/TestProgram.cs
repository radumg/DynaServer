using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public static class TestProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            var s = DynamoServer.ServiceRunner.Run();

            Console.WriteLine("Server has now started, doing other things...");
            Console.ReadLine();
        }
    }
}
