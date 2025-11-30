using System;
using System.Collections.Generic;
using ClassLibrary;

namespace Library

{
    public class AdministrarInteracciones
    {
        private List<Interaccion> ListaInteracciones = new List<Interaccion>();
        private static AdministrarInteracciones _instancia;
        
        private AdministrarInteracciones() {}

        public static AdministrarInteracciones Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new AdministrarInteracciones();
                }
                return _instancia;
            }
        }public void AgregarInteraccion(Cliente unCliente, Interaccion unaInteraccion)
        {
            ArgumentNullException.ThrowIfNull(unCliente);
            ArgumentNullException.ThrowIfNull(unaInteraccion);
            ListaInteracciones.Add(unaInteraccion);
            unCliente.ListaInteracciones.Add(unaInteraccion);
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

        public void EliminarInteraccion(Interaccion interaccion, Cliente cliente)
        {
           ArgumentNullException.ThrowIfNull(cliente);
           ArgumentNullException.ThrowIfNull(interaccion);
            bool existeGlobal = ListaInteracciones.Remove(interaccion);
            bool existeCliente = cliente.ListaInteracciones.Remove(interaccion);

            if (!existeGlobal && !existeCliente)
                throw new KeyNotFoundException("La interacción no existe y no se puede eliminar.");
        }

        
        public void AgregarNota(Interaccion unaInteraccion, string nota)
        {
            ArgumentNullException.ThrowIfNull(unaInteraccion);
            if (!string.IsNullOrEmpty(nota))
            {
                unaInteraccion.AddNota(nota);
            } 
        }
        public void ActualizarInteraccionesRespondidas()
        {
            // Filtramos solo las interacciones que implementan IRespondible
            List<IRespondible> respondibles = new List<IRespondible>();

            foreach (Interaccion i in ListaInteracciones)
            {
                if (i is IRespondible r)
                {
                    respondibles.Add(r);
                }
            }

            // Ahora comparamos entre todas las posibles combinaciones
            foreach (IRespondible original in respondibles)
            {
                foreach (IRespondible posibleRespuesta in respondibles)
                {
                    if (original != posibleRespuesta && original.EsRespuestaDe(posibleRespuesta))
                    {
                        original.MarcarComoRespondido();
                    }
                }
            }
        }

        public void AgregarLlamada()
        {
            throw new NotImplementedException();
        }

        public void AgregarReunion()
        {
            throw new NotImplementedException();

        }

        public void AgregarEmails()
        {
            throw new NotImplementedException();

        }

        public void AgregarMensajes()
        {
            throw new NotImplementedException();

        }
    }
}