using System;
using System.Collections.Generic;
using ClassLibrary;
using Library;

public class GestorVentas
{
    
    /*
     * + CrearVenta(vendedor: Vendedor, Cliente: cliente, Productos productos, DateTime fecha,):Venta
+ ObtenerVentas(): List<Venta>
     */
    private static GestorVentas instancia;
    public static List<Venta> ventas;

    private GestorVentas() 
    {
    }
    public static GestorVentas Instancia
    {
        get
        {
            if (instancia == null)
            {
                instancia = new GestorVentas();
            }
            return instancia;
        }
    }

    public Venta crearVenta(Vendedor vendedor, Cliente cliente, Dictionary<Producto, int> productosCantidad, DateTime fecha)
    {
        try
        {
            Venta venta = new Venta(productosCantidad, fecha, cliente, vendedor);
            ventas.Add(venta);
            return venta;
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public List<Venta> ObtenerVentas()
    {
        return ventas;
    }
}