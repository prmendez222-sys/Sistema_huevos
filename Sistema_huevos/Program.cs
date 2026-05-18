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
    quinceDias=15,
    treintaDias=30
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
    private string nombre="vacio";
    private double costoUnitario;
    private double costoPorcaja;
    private double precioUnitario;
    private double precioPorcaja;
    private int unidadesPorcaja;
    private int stockactual;

    public Producto(int id, string nombre, double costoUnitario, double costoPorCaja, double precioUnitario, double precioporCaja, int unidadesPorCaja)
    {
        ID = id;
        Nombre = nombre;
        CostoUnitario = costoUnitario;
        CostoPorCaja = costoPorCaja;
        PrecioUnitario = precioUnitario;
        PrecioporCaja = precioporCaja;
        UnidadesPorCaja = unidadesPorCaja;
        Stockactual = 0;
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public string Nombre
    {
        get { return nombre;}
        set 
        {
            if (string.IsNullOrEmpty(nombre)) throw new Exception("Nombre no puede quedar vacio");
            else nombre = value;
        }
    }

    public double CostoUnitario
    {
        get { return costoUnitario;}
        set { costoUnitario = value; }
    }

    public double CostoPorCaja
    {
        get { return costoPorcaja;}
        set {  costoPorcaja = value; }
    }

    public double PrecioUnitario
    {
        get { return precioUnitario;}
        set { precioUnitario = value; }
    }

    public double PrecioporCaja
    {
        get { return precioPorcaja;}
        set { precioPorcaja = value; }
    }

    public int UnidadesPorCaja
    {
        get { return unidadesPorcaja; }
        set {  unidadesPorcaja = value; }
    }

    public int Stockactual
    {
        get { return stockactual; }
        set { stockactual = value; }
    }
}

class Cliente
{
    private int id;
    private string nombre="vacio";
    private string telefono="";
    private TipoCliente tipo;
    private DateTime fecharegistro;

    public Cliente(int iD, string nombre, string telefono, TipoCliente tipo)
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
        set {  nombre = value; }
    }

    public string Telefono
    {
        get { return telefono; }
        set { telefono = value; }
    }

    public TipoCliente Tipo
    {
        get { return tipo; }
        set { tipo = value; }
    }

    public DateTime FechaRegistro
    {
        get { return fecharegistro; }
        set {  fecharegistro = value; }
    }
}
class Venta
{
    private int id;
    private DateTime fecha;
    private int? clienteId;
    private List<DetalleVenta> detalles;
    private double total;
    private bool escredito;
    private double montopagado;
    private double saldo;
    private EstadoVenta estadoventa;

    public int ID
    {
        get { return id; }
        set { id= value; }
    }
    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public int? ClienteID
    {
        get { return clienteId; }
        set {  clienteId= value; }
    }

    public List<DetalleVenta> Detalles
    {
        get { return detalles; }
        set {  detalles = value; }
    }

    public double Total
    {
        get { return total; }
        set {  total = value; }
    }
    public bool Escredito
    {
        get { return escredito; }
        set {  escredito = value; }
    }
    public double Montopagado
    {
        get { return montopagado; }
        set {  montopagado = value; }
    }
    public double Saldo
    {
        get { return saldo; }
        set {  saldo = value; }
    }
    public EstadoVenta EstadoVenta
    {
        get { return estadoventa; }
        set {  estadoventa = value; }
    }

    public Venta(int id, int? idcliente,List<DetalleVenta> detalles, double total, bool escredito, double montopagado,double saldo, EstadoVenta estadoventa)
    {
        ID= id;
        Fecha = DateTime.Now;
        ClienteID= idcliente;
        Detalles= detalles;
        Total = total;
        Escredito= escredito;
        Montopagado= montopagado;
        Saldo= saldo;
        EstadoVenta= estadoventa;
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
        ID= id;
        VentaID= ventaid;
        ProductoID= productoid;
        Cantidad= cantidad;
        PrecioUnitario= preciounitario;
        SubTotal= subtotal;
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
        set { cantidad = value; }
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
        set { id= value; }
    }

    public int ClienteID
    {
        get { return clienteid; }
        set { clienteid= value; }
    }

    public int VentaID
    {
        get { return ventaid; }
        set { ventaid= value; }
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
        set {  fechaotorgado = value; }
    }
    public DateTime FechaLimite
    {
        get { return fechalimite; }
        set { fechalimite = value; }
    }
    public EstadoCredito Estado
    {
        get { return estado; }
        set {  estado = value; }
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
        set {  id = value; }
    }

    public int Clienteid
    {
        get { return clienteid; }
        set {  clienteid = value; }
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
        set {  ID = value; }
    }
    public string Descripcion
    {
        get { return descripcion; }
        set {  descripcion = value; }
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
        set {  id = value; }
    }
    public int ProductoID
    {
        get { return productoid; }
        set {  productoid = value; }
    }
    public int Cantidad
    {
        get { return cantidad; }
        set {  cantidad = value; }
    }
    public double Costounitario
    {
        get { return costounitario; }
        set {  costounitario = value; }
    }
    public double TotalPerdido
    {
        get { return totalperdido; }
        set { totalperdido = value; }
    }
    public TipoPerdida TipoPerdida
    {
        get { return tipoperdida;}
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
        get { return fechainicio;  }
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
        set {  perdidastotales = value; }
    }
    public double Ganacias
    {
        get { return ganacias; }
        set {  ganacias = value; }
    }

}