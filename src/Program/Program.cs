using System;
using Proyecto2026.Services;

namespace Proyecto2026
{
    class Program
    {
        private static void Main(string [] args)
        {
            if (args.Length != 0)
            {
                DemoFacade(args);
            }
            else
            {
                DemoBot();
            }
        }

        private static void DemoFacade(string [] args)
        {
            
        }

        private static void DemoBot()
        {
            BotLoader.LoadAsync().GetAwaiter().GetResult();
        }
    }
}