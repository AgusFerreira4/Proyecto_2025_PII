using System;

namespace Library.Excepciones
{
    public class SuspendedUserException : Exception
    {
        public SuspendedUserException(string mensaje)
            : base(mensaje) { }
    }
}