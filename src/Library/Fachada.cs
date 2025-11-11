namespace Library
{
    public class Fachada
    {
        private static Fachada _instancia;

        public static Fachada Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new Fachada();
                }

                return _instancia;
            }
            
        }
        
        
    }
}