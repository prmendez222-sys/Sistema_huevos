
using System.Runtime.CompilerServices;
public enum SiNo
{
    si,
    no
}
public enum TipoPerdida
{
    Roto,
    Vencido,
    Danado,
    otro
}
public enum EstadoCredito
{
    Activo,
    Pagado,
    Vencido
}
public enum FechaLimite
{
    quinceDias = 15,
    treintaDias = 30
}
public enum TipoCliente
{
    Frecuente,
    Credito
}

public enum EstadoVenta
{
    Pagada,
    Parcial,
    Pendiente
}
class Producto
{
    private int id;
    private string nombre = "vacio";
    private double costoUnitario;
    private double costoPorcaja;
    private double precioUnitario;
    private double precioPorcaja;
    private int unidadesPorcaja;
    private int stockactual;

    public Producto(int id, string nombre, double costoUnitario, double precioUnitario)
    {
        ID = id;
        Nombre = nombre;
        CostoUnitario = costoUnitario;
        PrecioUnitario = precioUnitario;
        CostoPorCaja = costoUnitario * 12;
        PrecioporCaja = precioUnitario * 12;
        UnidadesPorCaja = 12;
        Stockactual = 0;
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public string Nombre
    {
        get { return nombre; }
        set
        {
            if (string.IsNullOrEmpty(value)) throw new Exception("El Nombre no puede quedar vacio");
            else nombre = value;
        }
    }

    public double CostoUnitario
    {
        get { return costoUnitario; }
        set
        {
            if (value <= 0) throw new Exception("el costo debe ser mayor a cero");
            else costoUnitario = value;
        }
    }

    public double CostoPorCaja
    {
        get { return costoPorcaja; }
        set { costoPorcaja = value; }
    }

    public double PrecioUnitario
    {
        get { return precioUnitario; }
        set
        {
            if (value <= 0) throw new Exception("el precio debe ser mayor a cero");
            else precioUnitario = value;
        }
    }

    public double PrecioporCaja
    {
        get { return precioPorcaja; }
        set { precioPorcaja = value; }
    }

    public int UnidadesPorCaja
    {
        get { return unidadesPorcaja; }
        set { unidadesPorcaja = value; }
    }

    public int Stockactual
    {
        get { return stockactual; }
        set
        {
            if (stockactual < 0) throw new Exception("el stock no puede ser menor a cero");
            else stockactual = value;
        }
    }
    public void ActualizarStock(int nuevoStock)
    {
        stockactual += nuevoStock;
    }

    public void ModificarNombre(string nombre)
    {
        Nombre = nombre;
    }

    public void Modificarcosto(double nuevoCosto)
    {
        CostoUnitario = nuevoCosto;
    }

    public void ModificarPrecio(double nuevoPrecio)
    {
        PrecioUnitario = nuevoPrecio;
    }
    public void MostrarInfo()
    {
        Console.WriteLine("ID producto: " + ID);
        Console.WriteLine("Nombre del Producto: " + Nombre);
        Console.WriteLine("Costo Unitario: " + CostoUnitario);
        Console.WriteLine("Precio a consumidor: " + PrecioUnitario);
        Console.WriteLine("Stock actual: " + Stockactual);
    }
}

class Cliente
{
    private int id;
    private string nombre = "vacio";
    private int telefono;
    private TipoCliente tipo;
    private DateTime fecharegistro;

    public Cliente(int iD, string nombre, int telefono, TipoCliente tipo)
    {
        ID = iD;
        Nombre = nombre;
        Telefono = telefono;
        Tipo = tipo;
        FechaRegistro = DateTime.Now;
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public string Nombre
    {
        get { return nombre; }
        set
        {
            if (string.IsNullOrEmpty(value)) throw new Exception("el nombre no puede quedar vacio");
            else nombre = value;
        }
    }

    public int Telefono
    {
        get { return telefono; }
        set
        {
            if (value.ToString().Count() == 8) telefono = value;
            else throw new Exception("numero de telefono no valido");
        }
    }

    public TipoCliente Tipo
    {
        get { return tipo; }
        set { tipo = value; }
    }

    public DateTime FechaRegistro
    {
        get { return fecharegistro; }
        set { fecharegistro = value; }
    }

    public void MostrarInfo()
    {
        Console.WriteLine("ID cliente: " + ID);
        Console.WriteLine("Nombre del cliente: " + Nombre);
        Console.WriteLine("Numero de Telefono: " + Telefono);
        Console.WriteLine("Tipo de Cliente: " + Tipo);
        Console.WriteLine("Fecha de Registro: " + FechaRegistro);
    }

    public void ModificarNombre(string nuevonombre)
    {
        Nombre = nuevonombre;
    }

    public void ModificarNumero(int numero)
    {
        Telefono = numero;
    }
    public void ModificarTipo(TipoCliente nuevotipo)
    {
        Tipo = nuevotipo;
    }
}
class Venta
{
    private int id;
    private DateTime fecha;
    private int? clienteId;
    private List<DetalleVenta> detalles;
    private double total;
    private SiNo escredito;
    private double montopagado;
    private double saldo;
    private EstadoVenta estadoventa;

    public int ID
    {
        get { return id; }
        set { id = value; }
    }
    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public int? ClienteID
    {
        get { return clienteId; }
        set { clienteId = value; }
    }

    public List<DetalleVenta> Detalles
    {
        get { return detalles; }
        set { detalles = value; }
    }

    public double Total
    {
        get { return total; }
        set { total = value; }
    }
    public SiNo Escredito
    {
        get { return escredito; }
        set { escredito = value; }
    }
    public double Montopagado
    {
        get { return montopagado; }
        set 
        {
            if (value < 0) throw new Exception("el monto no puede ser menor a cero");
            else montopagado = value;
        }
    }
    public double Saldo
    {
        get { return saldo; }
        set { saldo = value; }
    }
    public EstadoVenta EstadoVenta
    {
        get { return estadoventa; }
        set { estadoventa = value; }
    }

    public Venta(int id, int? idcliente, List<DetalleVenta> detalles, double total, SiNo escredito, double montopagado, double saldo, EstadoVenta estadoventa)
    {
        ID = id;
        Fecha = DateTime.Now;
        ClienteID = idcliente;
        Detalles = detalles;
        Total = total;
        Escredito = escredito;
        Montopagado = montopagado;
        Saldo = saldo;
        EstadoVenta = estadoventa;
    }

    public void MostrarVentas()
    {
        Console.WriteLine("ID: "+ID);
        Console.WriteLine("ID cliente: "+ClienteID);
        Console.WriteLine("total: "+Total);
        Console.WriteLine("Es credito? "+Escredito);
        Console.WriteLine("Monto pagado: "+Montopagado);
        Console.WriteLine("Saldo: "+Saldo);
        Console.WriteLine("estado: "+EstadoVenta);
    }
}
class DetalleVenta
{
    private int id;
    private int ventaid;
    private int productoid;
    private int cantidad;
    private double preciounitario;
    private double subtotal;

    public DetalleVenta(int id, int ventaid, int productoid, int cantidad, double preciounitario, double subtotal)
    {
        ID = id;
        VentaID = ventaid;
        ProductoID = productoid;
        Cantidad = cantidad;
        PrecioUnitario = preciounitario;
        SubTotal = subtotal;
    }

    public double SubTotal
    {
        get { return subtotal; }
        set { subtotal = value; }
    }


    public double PrecioUnitario
    {
        get { return preciounitario; }
        set { preciounitario = value; }
    }


    public int Cantidad
    {
        get { return cantidad; }
        set
        {
            if (value <= 0) throw new Exception("la cantidad debe ser mayor a cero");
            else
            {
                cantidad = value;
            }
        }
    }


    public int ProductoID
    {
        get { return productoid; }
        set { productoid = value; }
    }

    public int VentaID
    {
        get { return ventaid; }
        set { ventaid = value; }
    }


    public int ID
    {
        get { return id; }
        set { id = value; }
    }

}
class Credito
{
    private int id;
    private int clienteid;
    private int ventaid;
    private double montoOriginal;
    private double montopendiente;
    private DateTime fechaotorgado;
    private DateTime fechalimite;
    private EstadoCredito estado;

    public Credito(int iD, int clienteID, int ventaID, double montoOriginal, double mtopendiente, int diasplazo, EstadoCredito estado)
    {
        ID = iD;
        ClienteID = clienteID;
        VentaID = ventaID;
        MontoOriginal = montoOriginal;
        Mtopendiente = mtopendiente;
        Fechaotorgado = DateTime.Now;
        FechaLimite = DateTime.Now.AddDays(diasplazo);
        Estado = estado;
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public int ClienteID
    {
        get { return clienteid; }
        set { clienteid = value; }
    }

    public int VentaID
    {
        get { return ventaid; }
        set { ventaid = value; }
    }

    public double MontoOriginal
    {
        get { return montoOriginal; }
        set { montoOriginal = value; }
    }

    public double Mtopendiente
    {
        get { return montopendiente; }
        set { montopendiente = value; }
    }
    public DateTime Fechaotorgado
    {
        get { return fechaotorgado; }
        set { fechaotorgado = value; }
    }
    public DateTime FechaLimite
    {
        get { return fechalimite; }
        set { fechalimite = value; }
    }
    public EstadoCredito Estado
    {
        get { return estado; }
        set { estado = value; }
    }
}

class Pago
{
    private int id;
    private int clienteid;
    private double monto;
    private DateTime fecha;

    public Pago(int iD, int clienteid, double monto)
    {
        ID = iD;
        Clienteid = clienteid;
        Monto = monto;
        Fecha = DateTime.Now;
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public int Clienteid
    {
        get { return clienteid; }
        set { clienteid = value; }
    }
    public double Monto
    {
        get { return monto; }
        set { monto = value; }
    }
    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }
}

class Gasto
{
    private int id;
    private string descripcion = "";
    private DateTime fecha;

    public int ID
    {
        get { return id; }
        set { ID = value; }
    }
    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }
    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }
}

class Perdida
{
    private int id;
    private int productoid;
    private int cantidad;
    private double costounitario;
    private double totalperdido;
    private TipoPerdida tipoperdida;
    private DateTime fecha;

    public Perdida(int iD, int productoID, int cantidad, double costounitario, double totalPerdido, TipoPerdida perdida)
    {
        ID = iD;
        ProductoID = productoID;
        Cantidad = cantidad;
        Costounitario = costounitario;
        TotalPerdido = totalPerdido;
        TipoPerdida = perdida;
        Fecha = DateTime.Now;
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }
    public int ProductoID
    {
        get { return productoid; }
        set { productoid = value; }
    }
    public int Cantidad
    {
        get { return cantidad; }
        set { cantidad = value; }
    }
    public double Costounitario
    {
        get { return costounitario; }
        set { costounitario = value; }
    }
    public double TotalPerdido
    {
        get { return totalperdido; }
        set { totalperdido = value; }
    }
    public TipoPerdida TipoPerdida
    {
        get { return tipoperdida; }
        set { tipoperdida = value; }
    }
    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }
}

class ReporteFinanciero
{
    private DateTime fechainicio;
    private DateTime fechafin;
    private double totalventas;
    private double totalgastos;
    private double totalperdido;
    private double totalcreditosvencidos;
    private double perdidastotales;
    private double ganacias;

    public ReporteFinanciero(DateTime fechaInicio, DateTime fechaFin, double totalVentas, double totalGastos, double totalPerdido, double totalCreditosVencidos)
    {
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        TotalVentas = totalVentas;
        TotalGastos = totalGastos;
        TotalPerdido = totalPerdido;
        TotalCreditosVencidos = totalCreditosVencidos;
    }

    public DateTime FechaInicio
    {
        get { return fechainicio; }
        set { fechainicio = value; }
    }
    public DateTime FechaFin
    {
        get { return fechafin; }
        set { fechafin = value; }
    }
    public double TotalVentas
    {
        get { return totalventas; }
        set { totalventas = value; }
    }
    public double TotalGastos
    {
        get { return totalgastos; }
        set { totalgastos = value; }
    }
    public double TotalPerdido
    {
        get { return totalperdido; }
        set { totalperdido = value; }
    }
    public double TotalCreditosVencidos
    {
        get { return totalcreditosvencidos; }
        set { totalcreditosvencidos = value; }
    }
    public double PerdidadTotales
    {
        get { return perdidastotales; }
        set { perdidastotales = value; }
    }
    public double Ganacias
    {
        get { return ganacias; }
        set { ganacias = value; }
    }
}

class Program
{
    static void Main()
    {
        void Presionar()
        {
            Console.WriteLine();
            Console.WriteLine("Presione Enter para continuar");
            Console.ReadLine();
        }

        string opcion; bool error; int IDproducto = 1, IDcliente = 1; int IDventa = 1; int IDdetalleventa = 1;

        Dictionary<int, Producto> productos = new Dictionary<int, Producto>();
        Dictionary<int, Cliente> clientes = new Dictionary<int, Cliente>();
        Dictionary<int, Venta> ventas = new Dictionary<int, Venta>();
        List<DetalleVenta> detalles = new List<DetalleVenta>();

        void submenuProductos()
        {
            do
            {
                Console.WriteLine("====Productos====");
                Console.WriteLine();
                Console.WriteLine("1. Registrar Producto");
                Console.WriteLine("2. modificar datos de producto");
                Console.WriteLine("3. ver todos los productos");
                Console.WriteLine("4. Buscar producto");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("5. Volver al MENU");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("Ingrese una opcion: "); opcion = Console.ReadLine();
                Console.Clear();
                switch (opcion)
                {
                    case "1":

                        do
                        {
                            Console.Write("Ingrese Nombre del Producto: "); string nombre = Console.ReadLine();
                            nombre = nombre.ToLower();
                            Console.WriteLine();

                            double costounitario;
                            do
                            {
                                Console.Write("ingrese el costo Unitario: ");
                                error = double.TryParse(Console.ReadLine(), out costounitario);
                            } while (!error);
                            Console.WriteLine();

                            double preciounitario;
                            do
                            {
                                Console.Write("ingrese el precio al consumidor: ");
                                error = double.TryParse(Console.ReadLine(), out preciounitario);
                            } while (!error);

                            try
                            {
                                Producto p1 = new Producto(IDproducto, nombre, costounitario, preciounitario);
                                productos.Add(IDproducto, p1);
                                IDproducto += 1;
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Producto Ingresado con Exito");
                                Console.ResetColor();
                                Presionar();
                                Console.Clear();
                                error = true;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                error = false;
                                Presionar();
                                Console.Clear();
                            }

                        } while (!error);
                        break;
                    case "2":
                        string sub3;
                        if (productos.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("no hay productos registrados");
                            Console.ResetColor();
                            Presionar();
                        }
                        else
                        {
                            int IDp;
                            do
                            {
                                Console.Write("ingrese ID de Producto: ");
                                error = int.TryParse(Console.ReadLine(), out IDp);
                            } while (!error);

                            if (productos.ContainsKey(IDp))
                            {

                                do
                                {
                                    Console.WriteLine();
                                    productos[IDp].MostrarInfo();
                                    Console.WriteLine();
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("opciones para producto: ");
                                    Console.WriteLine();
                                    Console.ResetColor();
                                    Console.WriteLine("1. Modificar Nombre del Producto");
                                    Console.WriteLine("2. modificar Costo unitario");
                                    Console.WriteLine("3. modificar Precio unitario");
                                    Console.WriteLine("4. agregar Stock");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("5. Volver a PRODUCTOS");
                                    Console.ResetColor();
                                    Console.WriteLine();
                                    Console.Write("Ingrese una opcion: "); sub3 = Console.ReadLine();
                                    Console.Clear();
                                    switch (sub3)
                                    {
                                        case "1":
                                            do
                                            {
                                                try
                                                {
                                                    Console.Write("Ingrese el nombre nuevo: ");
                                                    productos[IDp].ModificarNombre(Console.ReadLine());
                                                    Console.WriteLine();
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Nombre modificado con exito");
                                                    Console.ResetColor();
                                                    Presionar();
                                                    error = true;
                                                    Console.Clear();
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                    Presionar();
                                                    Console.Clear();
                                                    error = false;
                                                }
                                            } while (!error);
                                            break;
                                        case "2":
                                            do
                                            {
                                                double nuevocosto;
                                                do
                                                {
                                                    Console.Write("Ingrese el nuevo costo: ");
                                                    error = double.TryParse(Console.ReadLine(), out nuevocosto);
                                                } while (!error);

                                                try
                                                {
                                                    productos[IDp].Modificarcosto(nuevocosto);
                                                    Console.WriteLine();
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Costo unitario modificado con exito");
                                                    Console.ResetColor();
                                                    Presionar();
                                                    error = true; Console.Clear();
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine();
                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.WriteLine(ex.Message);
                                                    Console.ResetColor();
                                                    Presionar();
                                                    Console.Clear();
                                                    error = false;
                                                }
                                            } while (!error);
                                            break;
                                        case "3":
                                            do
                                            {
                                                double nuevoprecio;
                                                do
                                                {
                                                    Console.Write("Ingrese el nuevo precio: ");
                                                    error = double.TryParse(Console.ReadLine(), out nuevoprecio);
                                                } while (!error);

                                                try
                                                {
                                                    productos[IDp].ModificarPrecio(nuevoprecio);
                                                    Console.WriteLine();
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Precio unitario modificado con exito");
                                                    Console.ResetColor();
                                                    Presionar();
                                                    error = true; Console.Clear();
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine();
                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.WriteLine(ex.Message);
                                                    Console.ResetColor();
                                                    Presionar();
                                                    Console.Clear();
                                                    error = false;
                                                }
                                            } while (!error);
                                            break;
                                        case "4":
                                            do
                                            {
                                                int nuevostock;
                                                do
                                                {
                                                    Console.Write("Ingrese cantidad a incrementar: ");
                                                    error = int.TryParse(Console.ReadLine(), out nuevostock);
                                                } while (!error);

                                                try
                                                {
                                                    productos[IDp].ActualizarStock(nuevostock);
                                                    Console.WriteLine();
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Stock actualizado con exito");
                                                    Console.ResetColor();
                                                    Presionar();
                                                    error = true; Console.Clear();
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine();
                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.WriteLine(ex.Message);
                                                    Console.ResetColor();
                                                    Presionar();
                                                    Console.Clear();
                                                    error = false;
                                                }
                                            } while (!error);
                                            break;
                                        case "5":
                                            break;
                                        default:
                                            Console.WriteLine();
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("opcion no valida");
                                            Console.ResetColor();
                                            Presionar();
                                            Console.Clear();
                                            break;
                                    }
                                } while (sub3 != "5");
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Producto no encontrado");
                                Console.WriteLine();
                                Console.Write("volviendo a PRODUCTOS");
                                Console.Write(".");
                                Thread.Sleep(1000);
                                Console.Write(".");
                                Thread.Sleep(1000);
                                Console.Write(".");
                                Thread.Sleep(1000);
                                Console.ResetColor();
                            }
                        }
                        break;
                    case "3":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("===Todos los Productos===");
                        Console.WriteLine();
                        Console.ResetColor();
                        if (productos.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("no hay productos registrados");
                            Console.ResetColor();
                        }
                        else
                        {
                            foreach (KeyValuePair<int, Producto> p in productos)
                            {
                                p.Value.MostrarInfo();
                                Console.WriteLine();
                                Console.WriteLine("=====================================");
                            }
                        }
                        Presionar();
                        break;
                    case "4":
                        do
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("===Buscar Producto===");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine("1. Buscar por ID de producto: ");
                            Console.WriteLine("2. Buscar por Nombre de producto: ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("3. Salir");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.Write("Ingrese una opcion: "); opcion = Console.ReadLine();
                            Console.Clear();
                            switch (opcion)
                            {
                                case "1":
                                    int IDp;
                                    do
                                    {
                                        Console.Write("ingrese ID de Producto: ");
                                        error = int.TryParse(Console.ReadLine(), out IDp);
                                    } while (!error);
                                    do
                                    {

                                        if (productos.ContainsKey(IDp))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.WriteLine("===Producto encontrado===");
                                            Console.WriteLine();
                                            Console.ResetColor();
                                            productos[IDp].MostrarInfo();
                                            Presionar();
                                            Console.Clear();
                                        }
                                        else
                                        {
                                            Console.WriteLine();
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Producto no encontrado");
                                            Console.WriteLine();
                                            Presionar();
                                            Console.Clear();
                                        }
                                    } while (!error);
                                    break;
                                case "2":
                                    Console.Write("ingrese nombre del producto: ");
                                    string nombre = Console.ReadLine();
                                    nombre = nombre.ToLower();
                                    Console.WriteLine();
                                    int cont = 0;
                                    foreach (KeyValuePair<int, Producto> p in productos)
                                    {
                                        if (p.Value.Nombre == nombre)
                                        {
                                            Console.WriteLine();
                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.WriteLine("===Producto Encontrado===");
                                            Console.ResetColor();
                                            Console.WriteLine();
                                            p.Value.MostrarInfo();
                                            cont = 1;
                                        }
                                        break;
                                    }
                                    if (cont == 0)
                                    {
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("===Producto no encontrado===");
                                        Console.ResetColor();
                                        Console.WriteLine();
                                    }
                                    Presionar();
                                    Console.Clear();
                                    break;
                                case "3":
                                    break;
                                default:
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("opcion no valida");
                                    Console.ResetColor();
                                    Presionar();
                                    Console.Clear();
                                    break;
                            }
                        } while (opcion != "3");
                        break;
                }
                Console.Clear();
            } while (opcion != "5");
        }

        void submenuClientes()
        {
            do
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("===CLIENTES===");
                Console.WriteLine();
                Console.ResetColor();
                Console.WriteLine("1. Ingresar Cliente");
                Console.WriteLine("2. Ver a Todos los clientes");
                Console.WriteLine("3. Modificar Cliente");
                Console.WriteLine("4. Buscar Cliente");
                Console.WriteLine("5. Ver historial de compras");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("6. Volver al MENU");
                Console.WriteLine();
                Console.ResetColor();
                Console.Write("Ingres una opcion: "); opcion = Console.ReadLine();
                Console.Clear();
                switch (opcion)
                {
                    case "1":
                        do
                        {
                            Console.Write("ingrese Nombre del cliente: ");
                            string nombre = Console.ReadLine();
                            nombre = nombre.ToLower();
                            Console.WriteLine();
                            int numero;
                            do
                            {
                                Console.Write("ingrese el numero de telefono: ");
                                error = int.TryParse(Console.ReadLine(), out numero);
                            } while (!error);
                            Console.WriteLine();
                            string subClientes; TipoCliente tipoSeleccionado = TipoCliente.Frecuente;
                            do
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("===Seleccione el Tipo de Cliente===");
                                Console.WriteLine();
                                Console.ResetColor();
                                Console.WriteLine("1. Frecuente");
                                Console.WriteLine("2. Credito");
                                Console.WriteLine();
                                Console.Write("ingrese opcion: "); subClientes = Console.ReadLine();
                                switch (subClientes)
                                {
                                    case "1":
                                        tipoSeleccionado = TipoCliente.Frecuente;
                                        break;
                                    case "2":
                                        tipoSeleccionado = TipoCliente.Credito;
                                        break;
                                    default:
                                        Console.Clear();
                                        break;
                                }
                            } while (subClientes != "1" && subClientes != "2");

                            try
                            {
                                Cliente c1 = new Cliente(IDcliente, nombre, numero, tipoSeleccionado);
                                clientes.Add(IDcliente, c1);
                                IDcliente += 1;
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Cliente ingresado con Exito");
                                Console.ResetColor();
                                Console.WriteLine();
                                Presionar();
                                error = true;
                                Console.Clear();
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine();
                                Console.WriteLine(ex.Message);
                                Console.ResetColor();
                                Console.WriteLine();
                                Presionar();
                                error = false;
                                Console.Clear();
                            }
                        } while (!error);
                        break;
                    case "2":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("===Todos los clientes===");
                        Console.WriteLine();
                        Console.ResetColor();

                        if (clientes.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("no hay clientes resgistrados");
                            Console.ResetColor();
                        }
                        else
                        {
                            foreach (KeyValuePair<int, Cliente> c in clientes)
                            {
                                c.Value.MostrarInfo();
                                Console.WriteLine();
                                Console.WriteLine("======================================");
                            }
                        }
                        Presionar();
                        Console.Clear();
                        break;
                    case "3":
                        int IDc;
                        do
                        {
                            Console.Write("Ingrese ID del Cliente: ");
                            error = int.TryParse(Console.ReadLine(), out IDc);
                        } while (!error);

                        if (clientes.ContainsKey(IDc))
                        {
                            Console.Clear();
                            do
                            {
                                clientes[IDc].MostrarInfo();
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("===opciones para cliente===");
                                Console.WriteLine();
                                Console.ResetColor();
                                Console.WriteLine("1. Modificar Nombre");
                                Console.WriteLine("2. Modificar numero de telefono");
                                Console.WriteLine("3. Modificar Tipo de Cliente");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("4. Volver a Clientes");
                                Console.ResetColor();
                                Console.WriteLine();
                                Console.Write("Ingrese una opcion: "); opcion = Console.ReadLine();
                                Console.Clear();
                                switch (opcion)
                                {
                                    case "1":
                                        do
                                        {
                                            try
                                            {
                                                Console.Write("Ingrese el nuevo Nombre: ");
                                                clientes[IDc].ModificarNombre(Console.ReadLine());
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine();
                                                Console.WriteLine("Nombre modificado con exito");
                                                Console.ResetColor();
                                                Console.WriteLine();
                                                Presionar();
                                                error = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine();
                                                Console.WriteLine(ex.Message);
                                                Console.ResetColor();
                                                Console.WriteLine();
                                                Presionar();
                                                error = false;
                                                Console.Clear();
                                            }
                                        } while (!error);
                                        break;
                                    case "2":
                                        do
                                        {
                                            int nuevonumero;
                                            do
                                            {
                                                Console.Write("Ingrese el nuevo numero: ");
                                                error = int.TryParse(Console.ReadLine(), out nuevonumero);
                                            } while (!error);
                                            try
                                            {

                                                clientes[IDc].ModificarNumero(nuevonumero);
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine();
                                                Console.WriteLine("numero modificado con exito");
                                                Console.ResetColor();
                                                Console.WriteLine();
                                                Presionar();
                                                error = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine();
                                                Console.WriteLine(ex.Message);
                                                Console.ResetColor();
                                                Console.WriteLine();
                                                Presionar();
                                                error = false;
                                                Console.Clear();
                                            }
                                        } while (!error);
                                        break;
                                    case "3":
                                        do
                                        {
                                            TipoCliente nuevotipo = TipoCliente.Frecuente;
                                            do
                                            {
                                                Console.ForegroundColor = ConsoleColor.Blue;
                                                Console.WriteLine("Seleccione Tipo de Cliente");
                                                Console.ResetColor();
                                                Console.WriteLine();
                                                Console.WriteLine("1. Frecuentes");
                                                Console.WriteLine("2 Credito");
                                                Console.WriteLine();
                                                Console.Write("ingrese una opcion: ");
                                                opcion = Console.ReadLine();
                                                Console.Clear();
                                                switch (opcion)
                                                {
                                                    case "1":
                                                        nuevotipo = TipoCliente.Frecuente;
                                                        break;
                                                    case "2":
                                                        nuevotipo = TipoCliente.Credito;
                                                        break;
                                                }
                                            } while (opcion != "1" && opcion != "2");
                                            try
                                            {

                                                clientes[IDc].ModificarTipo(nuevotipo);
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine();
                                                Console.WriteLine("Tipo de cliente modificado con exito");
                                                Console.ResetColor();
                                                Console.WriteLine();
                                                Presionar();
                                                error = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine();
                                                Console.WriteLine(ex.Message);
                                                Console.ResetColor();
                                                Console.WriteLine();
                                                Presionar();
                                                error = false;
                                                Console.Clear();
                                            }
                                        } while (!error);
                                        break;
                                    case "4":
                                        break;
                                    default:
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("opcion no valida");
                                        Console.ResetColor();
                                        Presionar();

                                        break;
                                }
                                Console.Clear();

                            } while (opcion != "4");
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Cliente no Encontrado");
                            Console.WriteLine();
                            Console.ResetColor();
                            Presionar();
                            Console.Clear();
                        }
                        break;
                    case "4":
                        do
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("===Buscar Cliente===");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine("1. Buscar por ID de Cliente: ");
                            Console.WriteLine("2. Buscar por Nombre de Cliente: ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("3. Salir");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.Write("Ingrese una opcion: "); opcion = Console.ReadLine();
                            Console.Clear();
                            switch (opcion)
                            {
                                case "1":
                                    do
                                    {
                                        Console.Write("ingrese ID de Cliente: ");
                                        error = int.TryParse(Console.ReadLine(), out IDc);
                                    } while (!error);
                                    do
                                    {

                                        if (clientes.ContainsKey(IDc))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.WriteLine("===Cliente encontrado===");
                                            Console.WriteLine();
                                            Console.ResetColor();
                                            clientes[IDc].MostrarInfo();
                                            Presionar();
                                            Console.Clear();
                                        }
                                        else
                                        {
                                            Console.WriteLine();
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Cliente no encontrado");
                                            Console.WriteLine();
                                            Presionar();
                                            Console.Clear();
                                        }
                                    } while (!error);
                                    break;
                                case "2":
                                    Console.Write("ingrese nombre del Cliente: ");
                                    string nombre = Console.ReadLine();
                                    nombre = nombre.ToLower();
                                    Console.WriteLine();
                                    int cont = 0;
                                    foreach (KeyValuePair<int, Cliente> c in clientes)
                                    {
                                        if (c.Value.Nombre == nombre)
                                        {
                                            Console.WriteLine();
                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.WriteLine("===Cliente Encontrado===");
                                            Console.ResetColor();
                                            Console.WriteLine();
                                            c.Value.MostrarInfo();
                                            cont = 1;
                                        }
                                        break;
                                    }
                                    if (cont == 0)
                                    {
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("===Cliente no encontrado===");
                                        Console.ResetColor();
                                        Console.WriteLine();
                                    }
                                    Presionar();
                                    Console.Clear();
                                    break;
                                case "3":
                                    break;
                                default:
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("opcion no valida");
                                    Console.ResetColor();
                                    Presionar();
                                    Console.Clear();
                                    break;
                            }
                        } while (opcion != "3");
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                    default:
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("opcion no valida");
                        Console.ResetColor();
                        Presionar();
                        Console.Clear();
                        break;
                }
            } while (opcion != "6");
        }
        do
        {
            Console.WriteLine("====MENU====");
            Console.WriteLine();
            Console.WriteLine("1. Productos");
            Console.WriteLine("2. Clientes");
            Console.WriteLine("3. ventas");
            Console.WriteLine("4. Creditos");
            Console.WriteLine("5. Gastos");
            Console.WriteLine("6. perdidas");
            Console.WriteLine("7. Reportes");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("8. Salir");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Ingrese una opcion: ");
            opcion = Console.ReadLine();
            Console.Clear();

            switch (opcion)
            {
                case "1":
                    submenuProductos();
                    break;
                case "2":
                    submenuClientes();
                    break;
                case "3":
                    do
                    {
                        int IDventacliente = 0, IDproductoventa;
                        int cantidad = 0; double MontoPagado = 0; double Saldo = 0; EstadoVenta estado = EstadoVenta.Pagada;
                        double Preciounitario = 0, subtotal = 0;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("===VENTAS===");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine("1. Registrar Venta");
                        Console.WriteLine("2. Ver todas Las Ventas");
                        Console.WriteLine("3. Buscar Venta");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("4. Volver al Menu");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.Write("ingrese una opcion: "); opcion = Console.ReadLine();
                        Console.Clear();
                        switch (opcion)
                        {
                            case "1":
                                do
                                {
                                    do
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("La venta se Realiza a un Cliente?");

                                        Console.ResetColor();
                                        Console.WriteLine();
                                        Console.WriteLine("1. si");
                                        Console.WriteLine("2. no");
                                        Console.WriteLine();
                                        Console.Write("ingres una opcion: "); opcion = Console.ReadLine();
                                        Console.Clear();
                                        switch (opcion)
                                        {
                                            case "1":
                                                Console.Write("Ingrese ID del cliente: ");
                                                error = int.TryParse(Console.ReadLine(), out IDventacliente);

                                                if (error)
                                                {
                                                    if (clientes.ContainsKey(IDventacliente))
                                                    {
                                                        IDventacliente = clientes[IDventacliente].ID;
                                                        Console.ForegroundColor = ConsoleColor.Blue;
                                                        Console.WriteLine();
                                                        Console.WriteLine("cliente: " + clientes[IDventacliente].Nombre);
                                                        Console.ResetColor();
                                                        Console.WriteLine();
                                                        Presionar();
                                                        Console.Clear();
                                                        error = true;
                                                    }
                                                    else
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                                        Console.WriteLine("Cliente no encontrado");
                                                        Console.WriteLine();
                                                        IDventacliente = 0;
                                                        Console.ResetColor();
                                                        Thread.Sleep(1000);
                                                        Console.Clear();
                                                        error = false;
                                                    }
                                                }
                                                break;
                                            case "2":
                                                error = true;
                                                break;
                                            default:
                                                Console.WriteLine();
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("opcion no valida");
                                                Console.ResetColor();
                                                Presionar();
                                                Console.Clear();
                                                error = false;
                                                break;
                                        }
                                    } while ((opcion != "1" && opcion != "2") || !error);
                                    do
                                    {
                                        do
                                        {
                                            Console.Write("Ingrese ID del Producto: ");
                                            error = int.TryParse(Console.ReadLine(), out IDproductoventa);
                                            if (error)
                                            {
                                                if (productos.ContainsKey(IDproductoventa))
                                                {
                                                    if (productos[IDproductoventa].Stockactual <= 0)
                                                    {
                                                        Console.ForegroundColor= ConsoleColor.Yellow;
                                                        Console.WriteLine("error: no hay stock de este producto");
                                                        Console.ResetColor();
                                                        Presionar();
                                                    }
                                                    else
                                                    {
                                                        Preciounitario = productos[IDproductoventa].PrecioUnitario;
                                                        Console.ForegroundColor = ConsoleColor.Blue;
                                                        Console.WriteLine();
                                                        Console.WriteLine("===producto===");
                                                        Console.ResetColor();
                                                        Console.WriteLine();
                                                        Console.ResetColor();
                                                        Console.WriteLine("Nombre del producto: " + productos[IDproductoventa].Nombre);
                                                        Console.WriteLine("Precio unitario: " + productos[IDproductoventa].PrecioUnitario);
                                                        Console.WriteLine("Stock Actual: " + productos[IDproductoventa].Stockactual);
                                                        error = true;
                                                    }
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.WriteLine("Producto no encontrado");
                                                    Console.WriteLine();
                                                    Console.ResetColor();
                                                    Thread.Sleep(100);
                                                    error = false;
                                                }
                                            }
                                        } while (!error);

                                        do
                                        {
                                            if (productos[IDproductoventa].Stockactual > 0)
                                            {
                                                do
                                                {
                                                    Console.WriteLine();
                                                    Console.Write("Ingrese la cantidad comprada: ");
                                                    error = int.TryParse(Console.ReadLine(), out cantidad);
                                                } while (!error);
                                            }

                                            try
                                            {
                                                if (cantidad > productos[IDproductoventa].Stockactual)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.WriteLine();
                                                    Console.WriteLine("error: la cantidad supera al stock con el que se cuenta");
                                                    cantidad = 0;
                                                    Console.ResetColor();
                                                    error = true;
                                                }
                                                else if (productos[IDproductoventa].Stockactual == 0)
                                                {
                                                    cantidad = 0;
                                                }
                                                else
                                                {
                                                    if (cantidad > 0)
                                                    {
                                                        subtotal += cantidad * Preciounitario;
                                                    }
                                                    DetalleVenta d1 = new DetalleVenta(IDdetalleventa, IDventa, IDproductoventa, cantidad, Preciounitario, subtotal);
                                                    productos[IDproductoventa].Stockactual = productos[IDproductoventa].Stockactual - cantidad;
                                                    detalles.Add(d1);
                                                    error = true;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine(ex.Message);
                                                Console.ResetColor();
                                                Console.WriteLine();
                                                Thread.Sleep(100);
                                                error = false;
                                            }
                                        } while (!error);


                                        do
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine();
                                            Console.WriteLine("desea ingresar otro producto?");
                                            Console.WriteLine();
                                            Console.ResetColor();
                                            Console.WriteLine("1. si");
                                            Console.WriteLine("2. no");
                                            Console.Write("ingres una opcion: "); opcion = Console.ReadLine();
                                        } while (opcion != "1" && opcion != "2");

                                        if (opcion == "1")
                                        {
                                            Console.Clear();
                                            error = false;
                                        }
                                        else if (opcion == "2")
                                        {
                                            Console.Clear();
                                            if (IDventacliente != 0)
                                            {
                                                Console.ForegroundColor= ConsoleColor.Blue;
                                                Console.WriteLine("Cliente: "+clientes[IDventacliente].Nombre);
                                                Console.ResetColor();
                                                Console.WriteLine();
                                            }
                                            Console.WriteLine("===PRODUCTOS VENDIDOS===");
                                            Console.WriteLine();
                                            foreach (DetalleVenta d in detalles)
                                            {
                                                if (IDventa == IDdetalleventa)
                                                {
                                                    string nombre = productos[d.ProductoID].Nombre;
                                                    double precio = productos[d.ProductoID].PrecioUnitario;
                                                    Console.WriteLine("Nombre del producto: " + nombre);
                                                    Console.WriteLine("Precio unitario: " + precio);
                                                    Console.WriteLine("cantidad: " + d.Cantidad);
                                                    Console.WriteLine("=====================================");
                                                    Console.WriteLine();
                                                }
                                            }

                                            IDdetalleventa += 1;
                                        }
                                    } while (!error);


                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("Subtotal: " + subtotal);
                                    Console.WriteLine();
                                    Console.ResetColor();

                                    if (subtotal!=0)
                                    {
                                        SiNo escredito = SiNo.no;
                                        do
                                        {
                                            Console.WriteLine();
                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.WriteLine("Es credito? ");
                                            Console.ResetColor();
                                            Console.WriteLine();
                                            Console.WriteLine("1. Si");
                                            Console.WriteLine("2. no");
                                            Console.WriteLine();
                                            Console.Write("Ingrese una opcion: "); opcion = Console.ReadLine();
                                            switch (opcion)
                                            {
                                                case "1":
                                                    escredito = SiNo.si;
                                                    break;
                                                case "2":
                                                    escredito = SiNo.no;
                                                    break;
                                            }
                                        } while (opcion != "1" && opcion != "2");
                                        if (IDventacliente == 0 && opcion == "1")
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Error: Primero debe registrar a un cliente");
                                            Console.ResetColor();
                                            Presionar();
                                            Console.Clear();
                                            error = true;
                                        }
                                        else if (IDcliente != 0 && (opcion == "1"))
                                        {
                                            clientes[IDventacliente].ModificarTipo(TipoCliente.Credito);
                                            do
                                            {
                                                do
                                                {
                                                    Console.WriteLine();
                                                    Console.ForegroundColor= ConsoleColor.Blue;
                                                    Console.Write("Ingrese el Monto pagado: ");
                                                    Console.ResetColor();
                                                    error = double.TryParse(Console.ReadLine(), out MontoPagado);
                                                } while (!error);

                                                try
                                                {
                                                    if (MontoPagado >= subtotal)
                                                    {
                                                        Console.WriteLine("error el monto supera el total");
                                                        error = false;
                                                    }else if(MontoPagado < subtotal && MontoPagado > 0)
                                                    {
                                                        Saldo = subtotal - MontoPagado;
                                                        estado = EstadoVenta.Parcial;
                                                        error = true;
                                                    }
                                                    if (MontoPagado == 0)
                                                    {
                                                        Saldo = subtotal;
                                                        estado = EstadoVenta.Pendiente;
                                                        error = true;
                                                    }
                                                    if (error)
                                                    {
                                                        Venta v = new Venta(IDventa, IDventacliente, detalles, subtotal, escredito, MontoPagado, Saldo, estado);
                                                        ventas.Add(IDventa, v);
                                                        IDventa += 1;
                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.WriteLine();
                                                        Console.WriteLine("venta registrada con exito");
                                                        Console.ResetColor();
                                                        Presionar();
                                                        Console.Clear();
                                                    }
                                                }
                                                catch(Exception ex)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.WriteLine(ex.Message);
                                                    Console.ResetColor();
                                                    Console.WriteLine();
                                                    Thread.Sleep(100);
                                                    error = false;
                                                }
                                            } while (!error);
                                        }
                                        else
                                        {
                                            Presionar();
                                        }
                                    }

                                } while (!error);
                                break;
                            case "2":
                                Console.WriteLine("===Todas la ventas===");
                                Console.WriteLine();
                                foreach(KeyValuePair<int,Venta> v in ventas)
                                {
                                    v.Value.MostrarVentas();
                                    Console.WriteLine("=========================================");
                                }
                                Presionar();
                                break;
                        }
                        Console.Clear();
                    } while (opcion != "4");
                    break;
            }
        } while (opcion != "8");
    }
}