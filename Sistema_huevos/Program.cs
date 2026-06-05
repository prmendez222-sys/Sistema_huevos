
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

//enums
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

//base de datos
class BaseDatos
{
    private static string conexion = "Data Source= huevos.db";

    public static SQLiteConnection ObtenerConexion()
    {
        return new SQLiteConnection(conexion);
    }

    public static void CrearTablas()
    {
        using(var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();

            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Productos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                CostoUnitario REAL NOT NULL,
                CostoPorCaja REAL NOT NULL,
                PrecioUnitario REAL NOT NULL,
                PrecioPorCaja REAL NOT NULL,
                UnidadesPorCaja INTEGER NOT NULL,
                StockActual INTEGER NOT NULL,
            );
            CREATE TABLE IF NOT EXISTS Clientes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                Telefono TEXT NOT NULL,
                Tipo TEXT NOT NULL,
                FechaRegistro TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Ventas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Fecha TEXT NOT NULL,
                ClienteId INTEGER,
                Total REAL NOT NULL,
                EsCredito INTEGER NOT NULL,
                MontoPagado REAL NOT NULL,
                Saldo REAL NOT NULL,
                Estado TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS DetalleVenta (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                VentaId INTEGER NOT NULL,
                ProductoId INTEGER NOT NULL,
                Cantidad INTEGER NOT NULL,
                PrecioUnitario REAL NOT NULL,
                Subtotal REAL NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Creditos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ClienteId INTEGER NOT NULL,
                VentaId INTEGER NOT NULL,
                MontoOriginal REAL NOT NULL,
                MontoPendiente REAL NOT NULL,
                FechaOtorgado TEXT NOT NULL,
                FechaLimite TEXT NOT NULL,
                Estado TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Pagos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ClienteId INTEGER NOT NULL,
                CreditoId INTEGER NOT NULL,
                Monto REAL NOT NULL,
                Fecha TEXT NOT NULL,
            );
            CREATE TABLE IF NOT EXISTS Gastos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Descripcion TEXT NOT NULL,
                Monto REAL NOT NULL,
                Fecha TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Perdidas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProductoId INTEGER NOT NULL,
                Cantidad INTEGER NOT NULL,
                CostoUnitario REAL NOT NULL,
                TotalPerdido REAL NOT NULL,
                Motivo TEXT NOT NULL,
                Fecha TEXT NOT NULL,
            );";

            cmd.ExecuteNonQuery();
        }
    }

    public static void InsertarProducto(Producto producto)
    {
        using (var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();

            cmd.CommandText = @"INSERT INTO Productos 
            (Nombre, CostoUnitario, CostoPorCaja, PrecioUnitario, 
             PrecioPorCaja, UnidadesPorCaja, StockActual, 
             GananciaPorUnidad, GananciaPorCaja)
            VALUES 
            (@Nombre, @CostoUnitario, @CostoPorCaja, @PrecioUnitario,
             @PrecioPorCaja, @UnidadesPorCaja, @StockActual,
             @GananciaPorUnidad, @GananciaPorCaja)";

            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
            cmd.Parameters.AddWithValue("@CostoUnitario", producto.CostoUnitario);
            cmd.Parameters.AddWithValue("@CostoPorCaja", producto.CostoPorCaja);
            cmd.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);
            cmd.Parameters.AddWithValue("@PrecioPorCaja", producto.PrecioporCaja);
            cmd.Parameters.AddWithValue("@UnidadesPorCaja", producto.UnidadesPorCaja);
            cmd.Parameters.AddWithValue("@StockActual", producto.Stockactual);

            cmd.ExecuteNonQuery();
        }
    }

    public static Dictionary<int, Producto> ObtenerProductos()
    {
        Dictionary<int, Producto> productos = new Dictionary<int, Producto>();

        using (var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM Productos";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Producto producto = new Producto();
                    producto.ID = reader.GetInt32(0);
                    producto.Nombre = reader.GetString(1);
                    producto.CostoUnitario = reader.GetDouble(2);
                    producto.CostoPorCaja = reader.GetDouble(3);
                    producto.PrecioUnitario = reader.GetDouble(4);
                    producto.PrecioporCaja = reader.GetDouble(5);
                    producto.UnidadesPorCaja = reader.GetInt32(6);
                    producto.Stockactual = reader.GetInt32(7);

                    productos.Add(producto.ID, producto);
                }
            }
        }

        return productos;
    }

    public static void ActualizarStockProducto(int id, int nuevoStock)
    {
        using (var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();

            cmd.CommandText = @"UPDATE Productos 
                            SET StockActual = @StockActual 
                            WHERE Id = @Id";

            cmd.Parameters.AddWithValue("@StockActual", nuevoStock);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
    public static Producto ObtenerProductoPorId(int id)
    {
        using (var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM Productos WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Producto producto = new Producto();
                    producto.ID = reader.GetInt32(0);
                    producto.Nombre = reader.GetString(1);
                    producto.CostoUnitario = reader.GetDouble(2);
                    producto.CostoPorCaja = reader.GetDouble(3);
                    producto.PrecioUnitario = reader.GetDouble(4);
                    producto.PrecioporCaja = reader.GetDouble(5);
                    producto.UnidadesPorCaja = reader.GetInt32(6);
                    producto.Stockactual = reader.GetInt32(7);
                    return producto;
                }
            }
        }
        return null;
    }

    public static Producto ObtenerProductoPorNombre(string nombre)
    {
        using (var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM Productos WHERE Nombre = @Nombre";
            cmd.Parameters.AddWithValue("@Nombre", nombre);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Producto producto = new Producto();
                    producto.ID = reader.GetInt32(0);
                    producto.Nombre = reader.GetString(1);
                    producto.CostoUnitario = reader.GetDouble(2);
                    producto.CostoPorCaja = reader.GetDouble(3);
                    producto.PrecioUnitario = reader.GetDouble(4);
                    producto.PrecioporCaja = reader.GetDouble(5);
                    producto.UnidadesPorCaja = reader.GetInt32(6);
                    producto.Stockactual = reader.GetInt32(7);
                    return producto;
                }
            }
        }
        return null;
    }

    // INSERT
    public static void InsertarCliente(Cliente cliente)
    {
        using (var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();

            cmd.CommandText = @"INSERT INTO Clientes 
            (Nombre, Telefono, Direccion, Tipo, FechaRegistro)
            VALUES 
            (@Nombre, @Telefono, @Direccion, @Tipo, @FechaRegistro)";

            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
            cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
            cmd.Parameters.AddWithValue("@Tipo", cliente.Tipo.ToString());
            cmd.Parameters.AddWithValue("@FechaRegistro", cliente.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss"));

            cmd.ExecuteNonQuery();
        }
    }

    // SELECT todos
    public static Dictionary<int, Cliente> ObtenerClientes()
    {
        Dictionary<int, Cliente> clientes = new Dictionary<int, Cliente>();

        using (var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM Clientes";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.ID = reader.GetInt32(0);
                    cliente.Nombre = reader.GetString(1);
                    cliente.Telefono = reader.GetInt32(0);
                    cliente.Tipo = Enum.Parse<TipoCliente>(reader.GetString(4));
                    cliente.FechaRegistro = DateTime.Parse(reader.GetString(5));

                    clientes.Add(cliente.ID, cliente);
                }
            }
        }
        return clientes;
    }

    // SELECT por Id
    public static Cliente ObtenerClientePorId(int id)
    {
        using (var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM Clientes WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.ID = reader.GetInt32(0);
                    cliente.Nombre = reader.GetString(1);
                    cliente.Telefono = reader.GetInt32(0);
                    cliente.Tipo = Enum.Parse<TipoCliente>(reader.GetString(4));
                    cliente.FechaRegistro = DateTime.Parse(reader.GetString(5));
                    return cliente;
                }
            }
        }
        return null;
    }

    // SELECT por Nombre
    public static Cliente ObtenerClientePorNombre(string nombre)
    {
        using (var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM Clientes WHERE Nombre = @Nombre";
            cmd.Parameters.AddWithValue("@Nombre", nombre);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.ID = reader.GetInt32(0);
                    cliente.Nombre = reader.GetString(1);
                    cliente.Telefono = reader.GetInt32(0);
                    cliente.Tipo = Enum.Parse<TipoCliente>(reader.GetString(4));
                    cliente.FechaRegistro = DateTime.Parse(reader.GetString(5));
                    return cliente;
                }
            }
        }
        return null;
    }

    // UPDATE tipo de cliente
    public static void ActualizarTipoCliente(int id, TipoCliente tipo)
    {
        using (var con = ObtenerConexion())
        {
            con.Open();
            var cmd = con.CreateCommand();

            cmd.CommandText = @"UPDATE Clientes 
                            SET Tipo = @Tipo 
                            WHERE Id = @Id";

            cmd.Parameters.AddWithValue("@Tipo", tipo.ToString());
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}

//clases
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

    public Producto() { }
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
            stockactual= value;
        }
    }
    public void ActualizarStock(int nuevoStock)
    {
        if (nuevoStock > 0)
        {
            stockactual += nuevoStock;
        }
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
    public Cliente() { }
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

    public void Mostrarcreditos()
    {
        Console.WriteLine("ID del Credito: "+ID);
        Console.WriteLine("Cliente ID: "+ClienteID);
        Console.WriteLine("ID de venta: "+VentaID);
        Console.WriteLine("Monto original: "+MontoOriginal);
        Console.WriteLine("Monto pendiente: "+Mtopendiente);
        Console.WriteLine("Fecha de otorgado: "+Fechaotorgado);
        Console.WriteLine("Fecha limite: "+FechaLimite);
        Console.WriteLine("Estado: "+Estado);
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
    private int creditoid;
    private double monto;
    private DateTime fecha;

    public Pago(int iD, int clienteid, int creditoid, double monto)
    {
        ID = iD;
        Clienteid = clienteid;
        Creditoid = creditoid;
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

    public int Creditoid
    {
        get { return creditoid; }
        set {  creditoid = value; }
    }
    public double Monto
    {
        get { return monto; }
        set 
        {
            if (value <= 0) throw new Exception("el monto debe ser mayor a cero");
            else monto = value;
        }
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
    double monto;
    private DateTime fecha;

    public int ID
    {
        get { return id; }
        set { id = value; }
    }
    public string Descripcion
    {
        get { return descripcion; }
        set 
        {
            if (string.IsNullOrEmpty(value)) throw new Exception("la descripcion no puede quedar vacia");
            else descripcion = value;
        }
    }

    public double Monto
    {
        get { return monto; }
        set 
        {
            if (value <= 0) throw new Exception("el monto debe ser mayor a cero");
            else monto = value;
        }
    }
    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public Gasto(int id, string descripcion, double monto)
    {
        ID = id;
        Descripcion = descripcion;
        Monto= monto;
        Fecha = DateTime.Now;
    }

    public void MostrarGastos()
    {
        Console.WriteLine("ID de gasto: "+ID);
        Console.WriteLine("Descripcion: "+Descripcion);
        Console.WriteLine("Monto: "+Monto);
        Console.WriteLine("Fecha de Gasto: "+Fecha);
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
        set 
        {
            if (value < 0) throw new Exception("la cantidad debe ser mayor o igual a cero");
            else cantidad = value;
        }
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

    public void MostrarPerdidas()
    {
        Console.WriteLine("ID de perdida: "+ID);
        Console.WriteLine("ID del producto: "+ProductoID);
        Console.WriteLine("Costo unitario al momento de la perdida: "+Costounitario);
        Console.WriteLine("Total Perdido: "+TotalPerdido);
        Console.WriteLine("Tipo de Perdida: "+TipoPerdida);
        Console.WriteLine("fecha de registro: "+Fecha);
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

    public ReporteFinanciero(DateTime fechaInicio, DateTime fechaFin, double totalVentas, double totalGastos, double totalPerdido, double totalCreditosVencidos, double perdidastotales, double ganancias)
    {
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        TotalVentas = totalVentas;
        TotalGastos = totalGastos;
        TotalPerdido = totalPerdido;
        TotalCreditosVencidos = totalCreditosVencidos;
        PerdidasTotales = perdidastotales;
        Ganacias = ganancias;
    }

    public void MostrarReporte()
    {
        Console.WriteLine("Inicio de Reporte: "+FechaInicio);
        Console.WriteLine("Fin de Reporte: "+FechaFin);
        Console.WriteLine("Total de ventas: "+TotalVentas);
        Console.WriteLine("total de Gastos: "+TotalGastos);
        Console.WriteLine("Total Perdidas: "+TotalPerdido);
        Console.WriteLine("total Creditos vencidos: "+TotalCreditosVencidos);
        Console.ForegroundColor= ConsoleColor.Red;
        Console.WriteLine("Total de Perdidas: "+PerdidasTotales);
        Console.ResetColor();
        Console.ForegroundColor=ConsoleColor.Green;
        Console.WriteLine("Ganancias: "+Ganacias);
        Console.ResetColor();
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
    public double PerdidasTotales
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

//programa principal
class Program
{
    static void Main()
    {
        BaseDatos.ObtenerConexion();
        BaseDatos.CrearTablas();

        void Presionar()
        {
            Console.WriteLine();
            Console.WriteLine("Presione Enter para continuar");
            Console.ReadLine();
        }

        //variables gobales, listas y diccionarios
        string opcion; bool error; int IDproducto = 1, IDcliente = 1; int IDventa = 1; int IDdetalleventa = 1; int IDcredito=1; int IDpago=1;
        int IDgasto=1; int IDperdidas=1;
        Dictionary<int, Producto> productos = new Dictionary<int, Producto>();
        Dictionary<int, Cliente> clientes = new Dictionary<int, Cliente>();
        Dictionary<int, Venta> ventas = new Dictionary<int, Venta>();
        Dictionary<int, Credito> creditos = new Dictionary<int, Credito>(); 
        List<DetalleVenta> detalles = new List<DetalleVenta>();
        Dictionary<int,Pago> pagos = new Dictionary<int, Pago>();
        Dictionary<int, Gasto> gastos = new Dictionary<int, Gasto>();
        Dictionary<int, Perdida> perdidas = new Dictionary<int, Perdida>();
        
        //recibe los errores del catch
        void ErrorCatch(Exception ex)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
            Console.WriteLine();
            Presionar();
            Console.Clear();
            error = false;
        }

        //opcion no valida en los case
        void OpcionoValida()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("opcion no valida");
            Console.ResetColor();
            Presionar();
            Console.Clear();
        }

        //submenuproductos
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
                                ErrorCatch(ex);
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
                                                    ErrorCatch(ex);
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
                                                    ErrorCatch(ex);
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
                                                    ErrorCatch(ex);
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
                                                    ErrorCatch(ex);
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
                                    OpcionoValida();
                                    break;
                            }
                        } while (opcion != "3");
                        break;
                }
                Console.Clear();
            } while (opcion != "5");
        }

        //submenuclientes
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
                                ErrorCatch(ex);
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
                                                ErrorCatch(ex);
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
                                                ErrorCatch(ex);
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
                                                ErrorCatch(ex);
                                            }
                                        } while (!error);
                                        break;
                                    case "4":
                                        break;
                                    default:
                                        OpcionoValida();
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
                                    OpcionoValida();
                                    break;
                            }
                        } while (opcion != "3");
                        break;
                    case "5":
                        do
                        {
                            Console.Write("ingrese ID de Cliente: ");
                            error = int.TryParse(Console.ReadLine(), out IDc);
                        } while (!error);

                        if (clientes.ContainsKey(IDc))
                        {
                            Console.WriteLine();
                            Console.WriteLine("Cliente: " + clientes[IDc].Nombre);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("===Compras realizadas===");
                            Console.WriteLine();
                            Console.ResetColor();
                            foreach (KeyValuePair<int,Venta> v in ventas) 
                            {
                                if(v.Value.ClienteID == IDc)
                                {
                                    v.Value.MostrarVentas();
                                    Console.ForegroundColor= ConsoleColor.Green;
                                    Console.WriteLine("===================================");
                                    Console.ResetColor();
                                }
                            }
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
                        break;
                    case "6":
                        break;
                    default:
                        OpcionoValida();
                        break;
                }
            } while (opcion != "6");
        }

        //submenuventas
        void SubMenuVenta()
        {
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
                            detalles.Clear();
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
                                        OpcionoValida();
                                        error = false;
                                        break;
                                }
                                Console.Clear();
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
                                                Console.ForegroundColor = ConsoleColor.Yellow;
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
                                            IDdetalleventa += 1;
                                            error = true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorCatch(ex);
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
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.WriteLine("Cliente: " + clientes[IDventacliente].Nombre);
                                        Console.ResetColor();
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine("===PRODUCTOS VENDIDOS===");
                                    Console.WriteLine();
                                    foreach (DetalleVenta d in detalles)
                                    {
                                        string nombre;
                                        double precio;
                                        nombre = productos[d.ProductoID].Nombre;
                                        precio = productos[d.ProductoID].PrecioUnitario;
                                        Console.WriteLine("Nombre del producto: " + nombre);
                                        Console.WriteLine("Precio unitario: " + precio);
                                        Console.WriteLine("cantidad: " + d.Cantidad);
                                        Console.WriteLine("=====================================");
                                        Console.WriteLine();
                                    }
                                }
                            } while (!error);


                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Subtotal: " + subtotal);
                            Console.WriteLine();
                            Console.ResetColor();

                            if (subtotal != 0)
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
                                    foreach (DetalleVenta d in detalles)
                                    {
                                        productos[d.ProductoID].Stockactual += d.Cantidad;
                                    }
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
                                            Console.ForegroundColor = ConsoleColor.Blue;
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
                                            }
                                            else if (MontoPagado < subtotal && MontoPagado > 0)
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

                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine();
                                                Venta v = new Venta(IDventa, IDventacliente, detalles, subtotal, escredito, MontoPagado, Saldo, estado);
                                                ventas.Add(IDventa, v);
                                                Credito c = new Credito(IDcredito, IDventacliente, IDventa, subtotal, Saldo, 15, EstadoCredito.Activo);
                                                creditos.Add(IDcredito, c);
                                                IDventa += 1;
                                                IDcredito += 1;
                                                Console.WriteLine("venta registrada con exito");
                                                Console.ResetColor();
                                                Presionar();
                                                Console.Clear();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ErrorCatch(ex);
                                        }
                                    } while (!error);
                                }
                                else
                                {
                                    MontoPagado = subtotal;
                                    Saldo = 0;
                                    Venta v = new Venta(IDventa, IDventacliente, detalles, subtotal, escredito, MontoPagado, Saldo, EstadoVenta.Pagada);
                                    ventas.Add(IDventa, v);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("venta registrada con exito");
                                    Console.ResetColor();
                                    Presionar();
                                    Console.Clear();
                                    IDventa += 1;
                                }
                            }

                        } while (!error);
                        break;
                    case "2":
                        Console.WriteLine("===Todas la ventas===");
                        Console.WriteLine();
                        foreach (KeyValuePair<int, Venta> v in ventas)
                        {
                            v.Value.MostrarVentas();
                            Console.WriteLine("=========================================");
                        }
                        Presionar();
                        break;
                    case "3":
                        int IDv;
                        do
                        {
                            Console.Write("ingrese ID de venta: ");
                            error = int.TryParse(Console.ReadLine(), out IDv);
                        } while (!error);
                        do
                        {

                            if (ventas.ContainsKey(IDv))
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("===venta encontrada===");
                                Console.WriteLine();
                                Console.ResetColor();
                                ventas[IDv].MostrarVentas();
                                Presionar();
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Venta no encontrada");
                                Console.WriteLine();
                                Presionar();
                                Console.Clear();
                            }
                        } while (!error);
                        break;
                    case "4":
                        break;
                    default:
                        OpcionoValida();
                        break;
                }
                Console.Clear();
            } while (opcion != "4");
        }

        //sub menu creditos
        void SubmenuCreditos()
        {
            do
            {
                int contCreditos = 0;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("====Creditos====");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. ver todos los creditos activos");
                Console.WriteLine("2. ver todos los creditos vencidos");
                Console.WriteLine("3. Hacer un abono");
                Console.WriteLine("4. Buscar creditos por cliente");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("5. salir al menu");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("ingrese una opcion: "); opcion = Console.ReadLine();
                Console.Clear();
                switch (opcion)
                {
                    case "1":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("====Creditos Activos===");
                        Console.ResetColor();
                        Console.WriteLine();
                        foreach (KeyValuePair<int, Credito> c in creditos)
                        {
                            if (c.Value.Estado == EstadoCredito.Activo)
                            {
                                c.Value.Mostrarcreditos();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("===============================");
                                Console.ResetColor();
                            }
                        }
                        Presionar();
                        break;
                    case "2":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("====Creditos Vencidos===");
                        Console.ResetColor();
                        Console.WriteLine();
                        foreach (KeyValuePair<int, Credito> c in creditos)
                        {
                            if (c.Value.Estado == EstadoCredito.Vencido)
                            {
                                c.Value.Mostrarcreditos();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("===============================");
                                Console.ResetColor();
                            }
                        }
                        Presionar();
                        break;
                    case "3":
                        do
                        {
                            int IDc;
                            do
                            {
                                Console.Write("ingrese ID de Cliente: ");
                                error = int.TryParse(Console.ReadLine(), out IDc);
                            } while (!error);

                            if (clientes.ContainsKey(IDc))
                            {
                                if (clientes[IDc].Tipo == TipoCliente.Credito)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Nombre del cliente: " + clientes[IDc].Nombre);
                                    Console.ResetColor();
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("===Creditos activos===");
                                    Console.WriteLine();
                                    Console.ResetColor();
                                    foreach (KeyValuePair<int, Credito> c in creditos)
                                    {
                                        if (c.Value.ClienteID == IDc && c.Value.Estado == EstadoCredito.Activo)
                                        {
                                            c.Value.Mostrarcreditos();
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("===================================");
                                            Console.ResetColor();
                                            contCreditos += 1;
                                        }
                                        else
                                        {
                                            if (contCreditos == 0)
                                            {
                                                contCreditos = 0;
                                            }
                                        }
                                    }
                                    if (contCreditos > 0)
                                    {
                                        int IDcreditoAbono;
                                        Console.WriteLine();
                                        do
                                        {
                                            do
                                            {
                                                Console.Write("ingrese ID del credito a abonar: ");
                                                error = int.TryParse(Console.ReadLine(), out IDcreditoAbono);
                                            } while (!error);

                                            if (creditos.ContainsKey(IDcreditoAbono))
                                            {
                                                if(creditos[IDcreditoAbono].ClienteID == IDc)
                                                {
                                                    if (creditos[IDcreditoAbono].Estado == EstadoCredito.Activo)
                                                    {
                                                        do
                                                        {
                                                            if (DateTime.Now > creditos[IDcreditoAbono].FechaLimite) creditos[IDcredito].Estado = EstadoCredito.Vencido;
                                                            Console.Clear();
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("Nombre del cliente: " + clientes[IDc].Nombre);
                                                            Console.ResetColor();
                                                            Console.WriteLine();
                                                            creditos[IDcreditoAbono].Mostrarcreditos();
                                                            Console.WriteLine();
                                                            int monto;
                                                            do
                                                            {
                                                                Console.Write("ingrese monto pagado: ");
                                                                error = int.TryParse(Console.ReadLine(), out monto);
                                                            } while (!error);
                                                            try
                                                            {
                                                                if (monto > creditos[IDcreditoAbono].Mtopendiente)
                                                                {
                                                                    Console.WriteLine("el monto exede el monto pendiente");
                                                                    error = false;
                                                                }
                                                                else if (monto == creditos[IDcreditoAbono].Mtopendiente)
                                                                {
                                                                    int IDabonoventa = creditos[IDcreditoAbono].VentaID;
                                                                    creditos[IDcreditoAbono].Mtopendiente = 0;
                                                                    creditos[IDcreditoAbono].Estado = EstadoCredito.Pagado;
                                                                    ventas[IDabonoventa].Saldo -= monto;
                                                                    ventas[IDabonoventa].Montopagado += monto;
                                                                    ventas[IDabonoventa].EstadoVenta = EstadoVenta.Pagada;
                                                                    error = true;
                                                                }
                                                                else if (monto < creditos[IDcreditoAbono].Mtopendiente && monto > 0)
                                                                {
                                                                    if (DateTime.Now < creditos[IDcreditoAbono].FechaLimite) creditos[IDcreditoAbono].Estado = EstadoCredito.Activo;
                                                                    creditos[IDcreditoAbono].Mtopendiente -= monto;
                                                                    int IDabonoventa = creditos[IDcreditoAbono].VentaID;
                                                                    ventas[IDabonoventa].Saldo -= monto;
                                                                    ventas[IDabonoventa].EstadoVenta = EstadoVenta.Parcial;
                                                                    ventas[IDabonoventa].Montopagado += monto;
                                                                    error = true;
                                                                }
                                                                if (error)
                                                                {
                                                                    Pago p = new Pago(IDpago, IDc, IDcreditoAbono, monto);
                                                                    pagos.Add(IDpago, p);
                                                                    IDpago += 1;
                                                                    error = true;
                                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                                    Console.WriteLine("pago registrado con exito");
                                                                    Console.ResetColor();
                                                                    Presionar();
                                                                }

                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                ErrorCatch(ex);
                                                            }
                                                        } while (!error);
                                                    }
                                                    else
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.WriteLine("Este credito no esta activo");
                                                        error = true;
                                                        Console.ResetColor();
                                                        Presionar();
                                                    }
                                                }
                                                else
                                                {
                                                    error = false;
                                                }
                                            }
                                            else
                                            {
                                                error = false;
                                            }

                                        } while (!error);
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Nombre del cliente: " + clientes[IDc].Nombre);
                                        Console.WriteLine("el cliente no tiene creditos activos");
                                        error = true;
                                        Console.ResetColor();
                                        Presionar();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("el cliente es de tipo frecuente");
                                    error = true;
                                    Console.WriteLine();
                                    Presionar();
                                    Console.Clear();
                                }
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("cliente no encontrado");
                                error = true;
                                Console.WriteLine();
                                Presionar();
                                Console.Clear();
                            }
                        } while (!error);
                        break;
                    case "4":
                        int IDclienteb;
                        do
                        {
                            Console.Write("ingrese ID de Cliente: ");
                            error = int.TryParse(Console.ReadLine(), out IDclienteb);
                        } while (!error);
                        if (clientes.ContainsKey(IDclienteb))
                        {
                            if (clientes[IDclienteb].Tipo == TipoCliente.Credito)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Nombre del cliente: " + clientes[IDclienteb].Nombre);
                                Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("===Creditos activos===");
                                Console.WriteLine();
                                Console.ResetColor();
                                foreach (KeyValuePair<int, Credito> c in creditos)
                                {
                                    if (c.Value.ClienteID == IDclienteb && c.Value.Estado == EstadoCredito.Activo || c.Value.Estado == EstadoCredito.Vencido)
                                    {
                                        c.Value.Mostrarcreditos();
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("===================================");
                                        Console.ResetColor();
                                    }
                                }

                                Presionar();
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("el cliente no tiene creditos activos");
                                error = true;
                                Console.WriteLine();
                                Presionar();
                                Console.Clear();
                            }
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Cliente no encontrado");
                            error = true;
                            Console.WriteLine();
                            Presionar();
                            Console.Clear();
                        }

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
                Console.Clear();
            } while (opcion != "5");
        }

        //
        void SubMenuGastos()
        {
            do
            {
                Console.ForegroundColor= ConsoleColor.Blue;
                Console.WriteLine("====Gastos====");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. registrar un gasto");
                Console.WriteLine("2. Mostrar todos los gastos");
                Console.WriteLine("3. Salir");
                Console.WriteLine();
                Console.Write("ingrese una opcion: ");
                opcion = Console.ReadLine();
                Console.Clear();
                switch (opcion)
                {
                    case "1":
                        do
                        {
                            Console.Write("Ingrese una descripcion del gasto: "); string descripcion = Console.ReadLine();
                            double monto;
                            do
                            {
                                Console.WriteLine();
                                Console.Write("ingrese el monto: ");
                                error = double.TryParse(Console.ReadLine(), out monto);
                            } while (!error);

                            try
                            {
                                Gasto g = new Gasto(IDgasto,descripcion,monto);
                                gastos.Add(IDgasto, g);
                                IDgasto += 1;
                                Console.WriteLine();
                                Console.ForegroundColor=ConsoleColor.Green;
                                Console.WriteLine("gasto ingresado con exito");
                                Console.ResetColor();
                                Presionar();
                            }
                            catch(Exception ex)
                            {
                                ErrorCatch(ex);
                            }
                        } while (!error);
                        break;
                    case "2":
                        Console.ForegroundColor= ConsoleColor.Blue;
                        Console.WriteLine("===Todos los Gastos Registrados===");
                        Console.ResetColor();
                        Console.WriteLine();
                        foreach(KeyValuePair<int,Gasto> g in gastos)
                        {
                            g.Value.MostrarGastos();
                            Console.ForegroundColor= ConsoleColor.Blue;
                            Console.WriteLine("=========================");
                            Console.ResetColor();
                        }
                        Presionar();
                        break;
                }
                Console.Clear();

            } while (opcion != "3");
        }

        //submenu Perdidas
        void subMenuPerdidas()
        {
            do
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("===Perdidas===");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. Resgistrar Perdida");
                Console.WriteLine("2. Ver todas las perdidas");
                Console.WriteLine("3. Ver perdidas por producto");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("4. volver al menu");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("Ingres una opcion: "); opcion = Console.ReadLine();
                Console.Clear();
                int idProducto=0;
                switch (opcion)
                {
                    case "1":
                        do
                        {
                            Console.Write("Ingrese ID del Producto: ");
                            error = int.TryParse(Console.ReadLine(),out idProducto);
                        } while (!error);

                        if (productos.ContainsKey(idProducto))
                        {
                            do
                            {
                                Console.WriteLine();
                                productos[idProducto].MostrarInfo();
                                Console.WriteLine();
                                int cantidad = 0;
                                do
                                {
                                    Console.Write("Ingrese la cantidad de unidades dañadas: ");
                                    error = int.TryParse(Console.ReadLine(), out cantidad);
                                } while (!error);
                                double costoUnitario = Math.Round(productos[idProducto].CostoUnitario / 30, 2);
                                double totalPerdido = Math.Round(costoUnitario * cantidad,2);
                                TipoPerdida tipoSeleccionado = TipoPerdida.Danado;
                                do
                                {
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("===Seleccione el Tipo de perdida===");
                                    Console.WriteLine();
                                    Console.ResetColor();
                                    Console.WriteLine("1. Roto");
                                    Console.WriteLine("2. Vencido");
                                    Console.WriteLine("3. Dañado");
                                    Console.WriteLine("4. Otro");
                                    Console.WriteLine();
                                    Console.Write("ingrese opcion: "); opcion = Console.ReadLine();
                                    switch (opcion)
                                    {
                                        case "1":
                                            tipoSeleccionado = TipoPerdida.Roto;
                                            break;
                                        case "2":
                                            tipoSeleccionado = TipoPerdida.Vencido;
                                            break;
                                        case "3":
                                            tipoSeleccionado = TipoPerdida.Danado;
                                            break;
                                        case "4":
                                            tipoSeleccionado = TipoPerdida.otro;
                                            break;
                                        default:
                                            break;
                                    }
                                } while (opcion != "1" && opcion != "2" && opcion != "3" && opcion != "4");

                                try
                                {
                                    Perdida p = new Perdida(IDperdidas,idProducto,cantidad,costoUnitario,totalPerdido,tipoSeleccionado);
                                    perdidas.Add(IDperdidas,p);
                                    IDperdidas += 1;
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("perdida registrada con exito");
                                    Console.ResetColor();
                                    Presionar();
                                }catch(Exception ex)
                                {
                                    ErrorCatch(ex);
                                }
                            } while (!error);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("producto no encontrado");
                            error = true;
                            Console.WriteLine();
                            Presionar();
                            Console.Clear();
                        }
                        break;
                    case "2":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("==todas las perdidas===");
                        Console.ResetColor();
                        Console.WriteLine();
                        foreach(KeyValuePair<int,Perdida> p in perdidas)
                        {
                            p.Value.MostrarPerdidas();
                            Console.ForegroundColor= ConsoleColor.Green;
                            Console.WriteLine("==========================================");
                            Console.ResetColor();
                        }
                        Presionar();
                        break;
                    case "3":
                        do
                        {
                            Console.Write("Ingrese ID del Producto: ");
                            error = int.TryParse(Console.ReadLine(), out idProducto);
                        } while (!error);

                        if (productos.ContainsKey(idProducto))
                        {
                            Console.WriteLine();
                            Console.ForegroundColor=ConsoleColor.Green;
                            Console.Write("nombre del producto: " + productos[idProducto].Nombre);
                            Console.WriteLine();
                            Console.ResetColor();
                            foreach(KeyValuePair<int,Perdida> p in perdidas)
                            {
                                if (p.Value.ProductoID == idProducto)
                                {
                                    p.Value.MostrarPerdidas();
                                }
                            }
                            Presionar();
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("producto no encontrado");
                            Console.WriteLine();
                            Presionar();
                        }
                        break;
                    case "4":
                        break;
                    default:
                        OpcionoValida();
                        break;
                }
                Console.Clear();
            } while (opcion != "4");
        }

        void subMenuReportes()
        {
            do
            {
                double Totalventas = 0, totalGastos = 0, Totalperdida = 0, TotalCreditosVencidos = 0, totalPerdidas = 0, Ganancias = 0;
                Console.ForegroundColor= ConsoleColor.Blue;
                Console.WriteLine("===Reportes===");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. Reporte Financiero por fecha");
                Console.WriteLine("2, Reporte de ventas");
                Console.WriteLine("3. Reporte de creditos vencidos");
                Console.WriteLine("4. Reporte de perdidas");
                Console.WriteLine("5. Reporte de gastos");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("6. Salir");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("Ingrese una opcion: "); opcion = Console.ReadLine();
                Console.Clear();
                switch (opcion)
                {
                    case "1":

                        do
                        {
                            DateTime FechaInicio, FechaFin;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Ingrese: Mes/dias/año");
                            Console.ResetColor();
                            Console.WriteLine();
                            do
                            {
                                Console.Write("Ingrese Fecha de Inicio: ");
                                error = DateTime.TryParse(Console.ReadLine(), out FechaInicio);
                            } while (!error);

                            Console.WriteLine();
                            do
                            {
                                Console.Write("Ingrese Fecha de Fin: ");
                                error = DateTime.TryParse(Console.ReadLine(), out FechaFin);
                            } while (!error);


                            foreach (KeyValuePair<int, Venta> v in ventas)
                            {
                                if (v.Value.Fecha.Date >= FechaInicio.Date && v.Value.Fecha.Date <= FechaFin.Date)
                                {
                                    Totalventas += v.Value.Montopagado;
                                }
                            }

                            foreach (KeyValuePair<int, Gasto> g in gastos)
                            {
                                if (g.Value.Fecha.Date >= FechaInicio.Date && g.Value.Fecha.Date <= FechaFin.Date)
                                {
                                    totalGastos += g.Value.Monto;
                                }
                            }

                            foreach (KeyValuePair<int, Perdida> p in perdidas)
                            {
                                if (p.Value.Fecha.Date >= FechaInicio.Date && p.Value.Fecha.Date <= FechaFin.Date)
                                {
                                    Totalperdida += p.Value.TotalPerdido;
                                }
                            }

                            foreach (KeyValuePair<int, Credito> c in creditos)
                            {
                                if (c.Value.Estado == EstadoCredito.Vencido && (c.Value.Fechaotorgado.Date >= FechaInicio.Date && c.Value.Fechaotorgado.Date <= FechaFin.Date))
                                {
                                    TotalCreditosVencidos += c.Value.Mtopendiente;
                                }
                            }
                            totalPerdidas = totalGastos + Totalperdida + TotalCreditosVencidos;
                            Ganancias = Totalventas - totalPerdidas;

                            try
                            {
                                ReporteFinanciero r = new ReporteFinanciero(FechaInicio,FechaFin,Totalventas,totalGastos,Totalperdida,TotalCreditosVencidos,totalPerdidas,Ganancias);
                                Console.WriteLine();
                                Console.ForegroundColor= ConsoleColor.Blue;
                                Console.WriteLine("===Reporte en rango de fecha===");
                                Console.ResetColor();
                                r.MostrarReporte();
                            }
                            catch(Exception ex)
                            {
                                ErrorCatch(ex);
                            }
                        } while (!error);
                        Presionar();
                        break;
                    case "2":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("===Total de ventas===");
                        Console.WriteLine();
                        Console.ResetColor();
                        foreach (KeyValuePair<int, Venta> v in ventas)
                        {
                            Totalventas += v.Value.Montopagado;
                        }
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Total de ventas: "+Totalventas);
                        Console.ResetColor();
                        Presionar();
                        break;
                    case "3":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("===Total de Creditos Vencidos===");
                        Console.WriteLine();
                        Console.ResetColor();
                        foreach (KeyValuePair<int,Credito> c in creditos)
                        {
                            if (c.Value.Estado == EstadoCredito.Vencido)
                            {
                                TotalCreditosVencidos += c.Value.Mtopendiente;
                            }
                        }
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Total de Creditos Vencidos: " + TotalCreditosVencidos);
                        Console.ResetColor();
                        Presionar();
                        break;
                    case "4":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("===Total de Perdidas de producto===");
                        Console.WriteLine();
                        Console.ResetColor();
                        foreach (KeyValuePair<int, Perdida> p in perdidas)
                        {
                            Totalperdida += p.Value.TotalPerdido;
                        }
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Total de perdidas de producto: " + Totalperdida);
                        Console.ResetColor();
                        Presionar();
                        break;
                    case "5":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("===Total de Gastos===");
                        Console.WriteLine();
                        Console.ResetColor();
                        foreach (KeyValuePair<int, Gasto> g in gastos)
                        {
                            totalGastos += g.Value.Monto;
                        }
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Total de Gastos: " + totalGastos);
                        Console.ResetColor();
                        Presionar();
                        break;
                    case "6":
                        break;
                    default:
                        OpcionoValida();
                        break;
                }
                Console.Clear();
            } while (opcion != "6");
        }

        //menu principal
        do
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("====MENU====");
            Console.ResetColor();
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
                    SubMenuVenta();
                    break;
                case "4":
                    SubmenuCreditos();
                    break;
                case "5":
                    SubMenuGastos();
                    break;
                case "6":
                    subMenuPerdidas();
                    break;
                case "7":
                    subMenuReportes();
                    break;
                case "8":
                    break;
                default:
                    OpcionoValida();
                    break;
            }
        } while (opcion != "8");
    }
}