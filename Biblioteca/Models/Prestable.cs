namespace Biblioteca.Models;

public abstract class Prestable : Articulo
{
    public bool Prestado { get; set; }
    public static int DiasMaximoPrestamo { get; set; } = 31;
}