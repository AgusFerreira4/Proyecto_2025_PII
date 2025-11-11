using System;
using System.Collections.Generic;
using System.Linq;
using Library;

namespace ClassLibrary
{
    public class Usuario : Persona
    {
        public bool Suspendido { get; set; }
        public List<Cliente> ListaClientesDeUsuario { get; set; }
        public List<Venta> ListaVentas { get; set; }
        public List<Cotizacion> ListaCotizaciones { get; set; }


        public Usuario(
            string nombre,
            string email,
            string apellido,
            string telefono
        )
            : base(nombre, email, apellido, telefono)
        {
            Suspendido = false;
            ListaClientesDeUsuario = new List<Cliente>();
            ListaVentas = new List<Venta>();
            ListaCotizaciones = new List<Cotizacion>();

        }


        public void CrearCliente(string nombre, string apellido, string email, string telefono, string genero,
            DateTime fechaDeNacimiento, Usuario usuarioAsignado)
        {
            AdministrarClientes.Instancia.CrearCliente(nombre, apellido, telefono, email, genero, fechaDeNacimiento,
                usuarioAsignado);
        }

        public void EliminarCliente(Cliente cliente)
        {
            AdministrarClientes.Instancia.EliminarCliente(cliente);
            this.ListaClientesDeUsuario.Remove(cliente);
        }

        public void ModificarCliente(Cliente cliente, string? unNombre, string? unApellido, string? unTelefono,
            string? unCorreo, DateTime unaFechaNacimiento, string? unGenero)
        {
            AdministrarClientes.Instancia.ModificarCliente(cliente, unNombre, unApellido, unTelefono, unCorreo,
                unaFechaNacimiento, unGenero);
        }

        public void AgregarEtiquetaACliente(Cliente cliente, string etiqueta)
        {
            AdministrarClientes.Instancia.AgregarEtiquetaCliente(cliente, etiqueta);
        }

        public void BuscarCliente(string criterio)
        {
            AdministrarClientes.Instancia.BuscarCliente(criterio);
        }

        public void AgregarInteraccion(Cliente cliente, Interaccion interaccion)
        {
            //cambiando singleto de administrarinteracciones para poder hacerlo
        }

        public void AgregarCliente(Cliente cliente)
        {
            this.ListaClientesDeUsuario.Add(cliente);
        }

        public List<Cliente> VerClientes()
        {
            return ListaClientesDeUsuario;
        }


        public List<Interaccion> VerInteraccionesCliente(Cliente cliente)
        {
            List<Interaccion> interacciones = new List<Interaccion>();
            foreach (Interaccion i in cliente.ListaInteracciones)
            {
                if (i.Emisor == cliente || i.Receptor == cliente)
                {
                    interacciones.Add(i);
                }
            }

            return interacciones;
        }

        public List<Interaccion> VerInteraccionesCliente(Cliente cliente, string? tipo = null, DateTime? fecha = null)
        {
            List<Interaccion> interaccionesFiltradas = new List<Interaccion>();

            // Diccionario para mapear palabras clave a los tipos de clase
            Dictionary<string, string> mapaTipos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "mensaje", "Mensaje" }, { "mensajes", "Mensaje" },
                { "llamada", "Llamada" }, { "llamadas", "Llamada" },
                { "reunion", "Reunion" }, { "reuniones", "Reunion" },
                { "mail", "Email" }, { "mails", "Email" },
                { "correo", "Email" }, { "correos", "Email" }
            };

            foreach (Interaccion interaccion in cliente.ListaInteracciones)
            {
                bool coincideParticipacion = interaccion.Emisor == cliente || interaccion.Receptor == cliente;
                bool coincideTipo = true;
                bool coincideFecha = true;

                // Filtrar por tipo (si se especificó)
                if (!string.IsNullOrEmpty(tipo))
                {
                    // Se normaliza el tipo recibido
                    if (mapaTipos.TryGetValue(tipo.ToLower(), out string tipoNormalizado))
                    {
                        coincideTipo = string.Equals(interaccion.GetType().Name, tipoNormalizado,
                            StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        // Si el tipo ingresado no está en el diccionario, no coincide con nada
                        coincideTipo = false;
                    }
                }

                // Filtrar por fecha (si se especificó)
                if (fecha.HasValue)
                {
                    coincideFecha = interaccion.Fecha.Date == fecha.Value.Date;
                }

                if (coincideParticipacion && coincideTipo && coincideFecha)
                {
                    interaccionesFiltradas.Add(interaccion);
                }
            }

            return interaccionesFiltradas;
        }
        

        public List<Cliente> VerClientesConPocaInteraccion()
        {
            // Lista auxiliar para guardar cada cliente con su última interacción
            List<(Cliente cliente, DateTime ultimaFecha)> clientesConFechas = new List<(Cliente, DateTime)>();

            foreach (Cliente cliente in ListaClientesDeUsuario)
            {
                DateTime ultimaFecha = DateTime.MinValue;

                // Si el cliente tiene interacciones, buscamos la más reciente
                if (cliente.ListaInteracciones.Count > 0)
                {
                    foreach (Interaccion interaccion in cliente.ListaInteracciones)
                    {
                        if (interaccion.Fecha > ultimaFecha)
                        {
                            ultimaFecha = interaccion.Fecha;
                        }
                    }
                }

                // Guardamos el cliente junto con su última fecha
                clientesConFechas.Add((cliente, ultimaFecha));
            }

            // Ordenamos los clientes según la última interacción (más antigua primero)
            for (int i = 0; i < clientesConFechas.Count - 1; i++)
            {
                for (int j = i + 1; j < clientesConFechas.Count; j++)
                {
                    if (clientesConFechas[i].ultimaFecha > clientesConFechas[j].ultimaFecha)
                    {
                        (Cliente,DateTime) temp = clientesConFechas[i];
                        clientesConFechas[i] = clientesConFechas[j];
                        clientesConFechas[j] = temp;
                    }
                }
            }
            

            // Tomamos los primeros 5 (o menos si hay pocos)
            List<Cliente> resultado = new List<Cliente>();
            int limite = Math.Min(5, clientesConFechas.Count);

            for (int i = 0; i < limite; i++)
            {
                resultado.Add(clientesConFechas[i].cliente);
            }

            return resultado;
        }



        public List<Cliente> VerClientesEnVisto()
        {
            List<Cliente> clientesVistos = new List<Cliente>();

            // Primero actualizamos el estado de las interacciones globales
            // Para modificar AdministrarInteracciones._instancia.ActualizarInteraccionesRespondidas();

            // Luego revisamos los clientes asignados al usuario
            foreach (Cliente cliente in ListaClientesDeUsuario)
            {
                bool tieneInteraccionRespondida = false;

                foreach (Interaccion i in cliente.ListaInteracciones)
                {
                    if (i is IRespondible r && r.Respondido)
                    {
                        tieneInteraccionRespondida = true;
                        break;
                    }
                }

                if (tieneInteraccionRespondida)
                    clientesVistos.Add(cliente);
            }

            return clientesVistos;
        }

        public List<Venta> VerVentasPorPeriodo(DateTime fechaini, DateTime fechafin)
        {
            List<Venta> ventasPorFecha = new List<Venta>();
            foreach (Venta v in ListaVentas)
            {
                if (v.Fecha >= fechaini && v.Fecha <= fechafin)
                {
                    ventasPorFecha.Add(v);
                }
            }

            return ventasPorFecha;
        }

        public void RegistrarCotizacion(double total, DateTime fecha,
            DateTime fechaLimite, string descripcion)
        {
            Cotizacion cotizacion = new Cotizacion(total, fecha, fechaLimite, descripcion);
            this.ListaCotizaciones.Add(cotizacion);
        }

        public void RegistrarVenta(Venta venta)
        {
            ListaVentas.Add(venta);
        }

        public void AgregarNotaAInteraccion(Interaccion interaccion, string nota)
        {
            foreach (Interaccion i in ListaInteracciones)
            {
                if (i.Equals(interaccion))
                {
                    i.Nota = nota;
                    break;
                }
            }
        }

        public void VerPanelResumen()
        {
            DateTime dosSemanasAdelante = DateTime.Now.AddDays(14); // Para ver las proximas reuniones de aca a 2 semanas
            DateTime ahora = DateTime.Now;
            List<Reuniones> proximasReuniones = new List<Reuniones>();
            
            Console.WriteLine($"--------------Resumen de {this.Nombre}--------------");
            Console.WriteLine("Lista de clientes");
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------");
            foreach (Cliente cliente in ListaClientesDeUsuario)
            {
                Console.WriteLine($"Cliente: {cliente.Nombre}");
                
            }
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Proximas Reuniones");
            foreach (Reuniones reunion in this.ListaInteracciones)
            {
                if (reunion.Fecha >= ahora && reunion.Fecha <= dosSemanasAdelante)
                {
                    proximasReuniones.Add(reunion);
                }
            }

            proximasReuniones = proximasReuniones.OrderByDescending(i => i.Fecha).ToList();
            proximasReuniones.ForEach(i => Console.WriteLine($"Programada con : {i.GetReceptor()}" +
                                                                    $"Lugar: {i.Lugar}" +
                                                                    $"Fecha: {i.Fecha} "));
        }
    }
    
}