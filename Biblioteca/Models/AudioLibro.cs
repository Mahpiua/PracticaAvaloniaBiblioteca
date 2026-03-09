using System;

namespace Biblioteca.Models;

// Audiolibro: no es prestable, pero sí valorable
public class Audiolibro : Articulo
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    public override string Tipo => "Audiolibro";
}