using System.Collections.Generic;
using System.Linq;

namespace Biblioteca.Models;

public abstract class Valorable : Articulo
{
    public List<Valoracion> Valoraciones { get; set; } = new();

    public double ValoracionMedia =>
        Valoraciones.Count == 0 ? 0 : Valoraciones.Average(v => v.Puntuacion);
}