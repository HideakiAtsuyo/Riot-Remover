using Crayon;
using System;

namespace RiotRemover
{
    public static class Logger
    {
        public static void Info(string x) => Console.WriteLine(String.Format($"{Output.Yellow("[")}{Output.Blue("INFOS")}{Output.Yellow("]")}: " + "{0}", x));
        public static void Error(string x) => Console.WriteLine(String.Format($"{Output.Yellow("[")}{Output.Red("ERROR")}{Output.Yellow("]")}: " + "{0}", x));
        public static void Warn(string x) => Console.WriteLine(String.Format($"{Output.Yellow("[")}{Output.Magenta("INFECTED")}{Output.Yellow("]")}: " + "{0}", x));
    }
}
