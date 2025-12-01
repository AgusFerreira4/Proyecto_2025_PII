using System;
using ClassLibrary;
using Library;

namespace Ucu.Poo.DiscordBot.Domain
{
    /// <summary>
    /// Esta clase recibe las acciones y devuelve los resultados que permiten
    /// implementar las historias de usuario. Otras clases que implementan el bot
    /// usan esta <see cref="Facade"/> pero no conocen el resto de las clases del
    /// dominio. Esta clase es un singleton.
    /// </summary>
    public class Facade
    {
        #region Singleton
        
        private static Facade instance;

        // Este constructor privado impide que otras clases puedan crear instancias
        // de esta.
        private Facade()
        {
            this.usersRepository = new UsersRepository();
        }
        
        // Este constructor es interno para que en las pruebas se pueda injectar
        // un mock del repositorio de usuarios en lugar de un repositorio real.
        internal Facade(IUsersRepository repository)
        {
            ArgumentNullException.ThrowIfNull(repository);
            
            this.usersRepository = repository;
        }

        /// <summary>
        /// Obtiene la única instancia de la clase <see cref="Facade"/>.
        /// </summary>
        public static Facade Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Facade();
                }

                return instance;
            }
        }

        // /// <summary>
        // /// Inicializa este singleton. Es necesario solo en los tests.
        // /// </summary>
        // public static void Reset()
        // {
        //     instance = null;
        // }
        
        #endregion

        private IUsersRepository usersRepository;

        /// <summary>
        /// Devuelve información del usuario cuyo nombre de usuario en Discord
        /// se recibe como parámetro.
        /// </summary>
        /// <param name="userName">El nombre de usuario de Discord del usuario
        /// buscado. </param>
        /// <returns>Un texto con la información del usuario <see cref="User"/>
        /// con el nombre de usuario provisto, o texto que indica que no existe
        /// un usuario con ese nombre.
        /// </returns>
        public string GetUserInfo(string userName)
        {
            string result;

            User userFound = this.usersRepository.Find(userName);
            if (userFound == null)
            {
                result =
                    $"El usuario de Discord '{userName}' no es usuario de esta aplicación.";
            }
            else
            {
                string roles = string.Join(", ", userFound.Roles);
                result = $"El usuario '{userName}' tiene los roles " +
                         $"'{roles}' en esta aplicación.";
            }

            return result;
        }
        public void SetUsuario(Usuario unUsuario)
        {
            user = unUsuario;
        }

        public void CrearCliente(string nombre, string apellido, string email, string telefono, string genero,
            DateTime fechaNacimiento, Usuario usuarioAsignado)
        {
            foreach (var campo in new[] { nombre, apellido, email, telefono, genero })
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(campo);
            }
            ArgumentNullException.ThrowIfNull(usuarioAsignado);
            fac.CrearCliente(nombre, apellido, email, telefono, genero, fechaNacimiento, usuarioAsignado);
        }

        public void EliminarCliente(Cliente cliente)
        {
            ArgumentNullException.ThrowIfNull(cliente);
            fac.EliminarCliente(cliente);
        }
        public void ModificarCliente(Cliente cliente, string? nombre, string? apellido, string? telefono,
            string? correo, DateTime fechaNacimiento, string? genero)
        {
            fac.ModificarCliente(cliente, nombre, apellido, telefono, correo, fechaNacimiento, genero);
        }
        public void AgregarEtiquetaACliente(Cliente cliente, string etiqueta)
        {
            fac.AgregarEtiquetaACliente(cliente, etiqueta);
        }
        
    }
}
