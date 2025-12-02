using System;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;



namespace Library.Commands
{
    [Group("Cliente")]
    public class ComandosCliente : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada fac = Fachada.Instancia;

        /// <summary>
        /// Crea un nuevo cliente en el sistema con nombre, email, apellido y teléfono.
        /// </summary>
        /// <param name="nombre">Nombre del cliente.</param>
        /// <param name="apellido">Apellido del cliente.</param>
        /// <param name="email">Email del cliente.</param>
        /// <param name="telefono">Teléfono del cliente.</param>
        [Command("crearCliente")]
        [Summary("Crea un cliente nuevo en el sistema.")]
        
        public async Task CrearCLienteAsync(
            string nombres, string apellidos, string telefonos, string emails, string generos, DateTime fechanacimiento, Usuario usuarioasignados)
        {
            try
            {
                fac.CrearCliente(nombres, apellidos, emails, telefonos, generos, fechanacimiento, usuarioasignados);

                await ReplyAsync(
                    $"Cliente **{nombres} {apellidos}** creado correctamente.");
            }
            catch (Exception ex)
            {
                await ReplyAsync(
                    $"No se pudo crear el Cliente: {ex.Message}");
            }
        }
        
        [Command("eliminarCliente")]
        [Summary("Elimina un cliente del sistema.")]
        public async Task EliminarClienteAsync(Cliente UnCliente)
        {
            try
            {
                fac.EliminarCliente(UnCliente);
                await ReplyAsync(
                    $"Cliente **{UnCliente.Nombre} {UnCliente.Apellido}** eliminado correctamente."
                );
            }
            catch (Exception e)
            {
                await ReplyAsync(
                    $"No se pudo crear el CLiente: {e.Message}");
            }
        }
        
        [Command("modificarCliente")]
        [Summary("Modifica un cliente del sistema.")]

        public async Task ModificarClienteAsync(Cliente cliente, string? unNombre, string? unApellido, string? unTelefono, string? unCorreo, DateTime unaFechaNacimiento, string? unGenero )
        {
            try
            {
                fac.ModificarCliente(cliente, unNombre, unApellido, unTelefono, unCorreo, unaFechaNacimiento, unGenero  );
                await ReplyAsync(
                    $"Cliente **{cliente.Nombre} {cliente.Apellido}** modificado correctamente.");
            }
            catch (Exception e)
            {
                await ReplyAsync($"No se pudo modificar el usuario: {e.Message}");
            }
        }
        
        [Command("modificarCliente")]
        [Summary("Modifica un cliente del sistema.")]

        public async Task AgregarEtiquetaClienteAsync(Cliente cliente, string etiqueta)
        {
            try
            {
                fac.AgregarEtiquetaACliente(cliente,  etiqueta);
                await ReplyAsync($"CLiente: {cliente.Nombre} {cliente.Apellido} etiquetado correctamente.");

            }
            catch (Exception e)
            {
                await ReplyAsync($"No se ha podido etiquetar el usuario: {e.Message}");
            }
        }

        [Command("buscarCliente")]
        [Summary("Busca un cliente del sistema.")]
        public async Task BuscarClienteAsync(string criterio, Cliente cliente)
        {
            try
            {
                fac.BuscarCliente(criterio);
                if (cliente != null)
                {
                    await ReplyAsync(
                        $"Cliente encontrado: {cliente.Nombre} {cliente.Apellido} (Teléfono: {cliente.Telefono}, Email: {cliente.Email}, Género: {cliente.Genero}, Fecha de Nacimiento: {cliente.FechaDeNacimiento.ToShortDateString()})");
                }
                else
                {
                    await ReplyAsync("No se encontró ningún cliente con ese criterio.");
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync(
                    $"No se pudo buscar el cliente: {ex.Message}");
            }
        }
    }
}