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