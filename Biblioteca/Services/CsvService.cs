using System.Collections.Generic;
using System.IO;
using Biblioteca.Models;

namespace Biblioteca.Services;

// Servicio simple para exportar CSV
public class CsvService
{
    public void Exportar(string ruta, List<Articulo> articulos)
    {
        using var sw = new StreamWriter(ruta);

        // Cabecera
        sw.WriteLine("Tipo,Titulo,Año,FechaAdquisicion");

        // Filas
        foreach (var a in articulos)
        {
            sw.WriteLine($"{a.Tipo},{a.Titulo},{a.Año},{a.FechaAdquisicion}");
        }
    }
}