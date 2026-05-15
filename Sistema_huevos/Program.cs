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

    public Producto(int id, string nombre, double costoUnitario, double costoPorCaja, double precioUnitario, double precioporCaja, int unidadesPorCaja, int stockactual)
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
    private string tipo="";
    private DateTime fecharegistro;

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

    public string Tipo
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