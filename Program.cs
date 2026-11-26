using System;

// Ejercicio 1: Registro y notificación de ventas
namespace Ej1
{
    public class VentaEventArgs : EventArgs
    {
        public string Producto { get; }
        public decimal Precio { get; }
        public VentaEventArgs(string producto, decimal precio) { Producto = producto; Precio = precio; }
    }

    public class RegistroVentas
    {
        public event EventHandler<VentaEventArgs> VentaRealizada;

        public void ProcesarVenta(string producto, decimal precio)
        {
            Console.WriteLine($"Procesando venta de {producto}...");
            VentaRealizada?.Invoke(this, new VentaEventArgs(producto, precio));
        }
    }

    public class ServicioRegistro
    {
        public void RegistrarVenta(object sender, VentaEventArgs e) =>
            Console.WriteLine($"[Registro] Venta guardada: {e.Producto} - {e.Precio} €");
    }

    public class ServicioNotificacion
    {
        public void EnviarNotificacionVenta(object sender, VentaEventArgs e) =>
            Console.WriteLine($"[Notificación] Se ha vendido {e.Producto} por {e.Precio} €");
    }

    public static class Demo
    {
        public static void Run()
        {
            Console.WriteLine("===== Ejercicio 1 =====");
            var ventas = new RegistroVentas();
            var registro = new ServicioRegistro();
            var notif = new ServicioNotificacion();

            ventas.VentaRealizada += registro.RegistrarVenta;
            ventas.VentaRealizada += notif.EnviarNotificacionVenta;

            ventas.ProcesarVenta("Portátil", 899.99m);
            ventas.ProcesarVenta("Ratón", 19.50m);
        }
    }
}

// Ejercicio 2: Control de temperatura en un invernadero
namespace Ej2
{
    public class TemperaturaEventArgs : EventArgs
    {
        public double TemperaturaActual { get; }
        public double Umbral { get; }
        public TemperaturaEventArgs(double actual, double umbral) { TemperaturaActual = actual; Umbral = umbral; }
    }

    public class ControlTemperatura
    {
        public double Umbral { get; }
        public event EventHandler<TemperaturaEventArgs> TemperaturaAlta;

        public ControlTemperatura(double umbral) { Umbral = umbral; }

        public void Medir(double temperatura)
        {
            Console.WriteLine($"Temperatura medida: {temperatura} °C");
            if (temperatura > Umbral)
                TemperaturaAlta?.Invoke(this, new TemperaturaEventArgs(temperatura, Umbral));
        }
    }

    public class ServicioAlerta
    {
        public void Alertar(object sender, TemperaturaEventArgs e) =>
            Console.WriteLine($"[ALERTA] ¡{e.TemperaturaActual} °C supera el umbral de {e.Umbral} °C!");
    }

    public class ServicioRegistroTemperatura
    {
        public void Registrar(object sender, TemperaturaEventArgs e) =>
            Console.WriteLine($"[Registro] {DateTime.Now:HH:mm:ss} - Temperatura alta: {e.TemperaturaActual} °C");
    }

    public static class Demo
    {
        public static void Run()
        {
            Console.WriteLine("\n===== Ejercicio 2 =====");
            var control = new ControlTemperatura(30);
            control.TemperaturaAlta += new ServicioAlerta().Alertar;
            control.TemperaturaAlta += new ServicioRegistroTemperatura().Registrar;

            control.Medir(25);
            control.Medir(34.5);
            control.Medir(31);
        }
    }
}

// Ejercicio 3: Backup y notificación de archivos
namespace Ej3
{
    public class BackupEventArgs : EventArgs
    {
        public string NombreArchivo { get; }
        public DateTime Fecha { get; }
        public BackupEventArgs(string nombre, DateTime fecha) { NombreArchivo = nombre; Fecha = fecha; }
    }

    public class GestorBackups
    {
        public event EventHandler<BackupEventArgs> BackupCompletado;

        public void RealizarBackup(string archivo)
        {
            Console.WriteLine($"Copiando {archivo}...");
            BackupCompletado?.Invoke(this, new BackupEventArgs(archivo, DateTime.Now));
        }
    }

    public class ServicioNotificacion
    {
        public void Notificar(object sender, BackupEventArgs e) =>
            Console.WriteLine($"[Notificación] Backup de '{e.NombreArchivo}' completado.");
    }

    public class ServicioLog
    {
        public void Registrar(object sender, BackupEventArgs e) =>
            Console.WriteLine($"[Log] {e.Fecha:dd/MM/yyyy HH:mm:ss} - Respaldo realizado: {e.NombreArchivo}");
    }

    public static class Demo
    {
        public static void Run()
        {
            Console.WriteLine("\n===== Ejercicio 3 =====");
            var gestor = new GestorBackups();
            gestor.BackupCompletado += new ServicioNotificacion().Notificar;
            gestor.BackupCompletado += new ServicioLog().Registrar;

            gestor.RealizarBackup("informe.docx");
            gestor.RealizarBackup("fotos.zip");
        }
    }
}

// Ejercicio 4: Monitoreo de sensores de puertas y ventanas
namespace Ej4
{
    public class IntrusoEventArgs : EventArgs
    {
        public string Sensor { get; }
        public DateTime Hora { get; }
        public IntrusoEventArgs(string sensor, DateTime hora) { Sensor = sensor; Hora = hora; }
    }

    public class SensorMonitoreo
    {
        // Horario permitido de apertura: entre 07:00 y 22:00
        private readonly TimeSpan _inicio = new TimeSpan(7, 0, 0);
        private readonly TimeSpan _fin = new TimeSpan(22, 0, 0);

        public event EventHandler<IntrusoEventArgs> AlertaIntruso;

        public void DetectarApertura(string sensor, DateTime hora)
        {
            Console.WriteLine($"{sensor} abierta a las {hora:HH:mm}");
            TimeSpan h = hora.TimeOfDay;
            if (h < _inicio || h > _fin)
                AlertaIntruso?.Invoke(this, new IntrusoEventArgs(sensor, hora));
        }
    }

    public class ServicioAlarma
    {
        public void Activar(object sender, IntrusoEventArgs e) =>
            Console.WriteLine($"[ALARMA] ¡Sonando! Apertura sospechosa en {e.Sensor}");
    }

    public class ServicioRegistroIncidencias
    {
        // Simulamos la base de datos con un mensaje por consola
        public void Guardar(object sender, IntrusoEventArgs e) =>
            Console.WriteLine($"[BD] Incidencia guardada: {e.Sensor} a las {e.Hora:dd/MM/yyyy HH:mm}");
    }

    public static class Demo
    {
        public static void Run()
        {
            Console.WriteLine("\n===== Ejercicio 4 =====");
            var sensor = new SensorMonitoreo();
            sensor.AlertaIntruso += new ServicioAlarma().Activar;
            sensor.AlertaIntruso += new ServicioRegistroIncidencias().Guardar;

            var hoy = DateTime.Today;
            sensor.DetectarApertura("Puerta principal", hoy.AddHours(10));   // permitido
            sensor.DetectarApertura("Ventana cocina", hoy.AddHours(3));      // intruso
            sensor.DetectarApertura("Puerta garaje", hoy.AddHours(23));      // intruso
        }
    }
}

// Ejercicio 5: Supervisión de consumo de energía 
namespace Ej5
{
    public class EnergiaEventArgs : EventArgs
    {
        public double ConsumoActual { get; }
        public double Umbral { get; }
        public EnergiaEventArgs(double consumo, double umbral) { ConsumoActual = consumo; Umbral = umbral; }
    }

    public class MonitorEnergia
    {
        public double Umbral { get; }
        public event EventHandler<EnergiaEventArgs> ConsumoExcesivoDetectado;

        public MonitorEnergia(double umbral) { Umbral = umbral; }

        public void RegistrarConsumo(double kwh)
        {
            Console.WriteLine($"Consumo registrado: {kwh} kWh");
            if (kwh > Umbral)
                ConsumoExcesivoDetectado?.Invoke(this, new EnergiaEventArgs(kwh, Umbral));
        }
    }

    public class ServicioNotificacion
    {
        public void Advertir(object sender, EnergiaEventArgs e) =>
            Console.WriteLine($"[Aviso] Consumo de {e.ConsumoActual} kWh por encima del límite de {e.Umbral} kWh");
    }

    public class ServicioAjusteAutomatizado
    {
        public void Ajustar(object sender, EnergiaEventArgs e)
        {
            double exceso = e.ConsumoActual - e.Umbral;
            Console.WriteLine($"[Ajuste] Reduciendo calefacción y apagando dispositivos en standby (exceso: {exceso} kWh)");
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            Console.WriteLine("\n===== Ejercicio 5 =====");
            var monitor = new MonitorEnergia(100);
            monitor.ConsumoExcesivoDetectado += new ServicioNotificacion().Advertir;
            monitor.ConsumoExcesivoDetectado += new ServicioAjusteAutomatizado().Ajustar;

            monitor.RegistrarConsumo(80);
            monitor.RegistrarConsumo(125.5);
        }
    }
}


class Program
{
    static void Main()
    {
        Ej1.Demo.Run();
        Ej2.Demo.Run();
        Ej3.Demo.Run();
        Ej4.Demo.Run();
        Ej5.Demo.Run();
    }
}