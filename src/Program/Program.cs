//--------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//--------------------------------------------------------------------------------

using System;
using ClassLibrary;
using Library;
using Library.Services;

namespace ConsoleApplication
{
    /// <summary>
    /// Programa de consola de demostración.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Punto de entrada al programa.
        /// </summary>
        private static void Main(string [] args)
        {
            if (args.Length != 0)
            {
                Console.WriteLine("Por consola");
            }
            else
            {
                DemoBot();
            }
        }
        

        private static void DemoBot()
        {
            Console.WriteLine($"Token leído: '{Environment.GetEnvironmentVariable("DiscordToken")}'");
            BotLoader.LoadAsync().GetAwaiter().GetResult();
        }
    }
}
