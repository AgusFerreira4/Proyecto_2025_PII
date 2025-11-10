using ClassLibrary;

namespace Library
{
    public class Vendedor : Usuario
    {
        public Vendedor(
            string nombre,
            string email,
            string apellido,
            string telefono
        ) : base(nombre, email, apellido, telefono)
        {
        
        }

        public void CamviarVendedorAsignado(Cliente cliente, Vendedor vendedorNuevo)
        {
            this.ListaClientesDeUsuario.Remove(cliente);
            vendedorNuevo.ListaClientesDeUsuario.Add(cliente);
        }
    }
}