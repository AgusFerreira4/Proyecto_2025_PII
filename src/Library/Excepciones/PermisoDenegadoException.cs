using System;

namespace Library.Excepciones
{
    public class PermisoDenegadoException: Exception
    {
        public PermisoDenegadoException(string mensaje)
            : base(mensaje) { }
    }
}