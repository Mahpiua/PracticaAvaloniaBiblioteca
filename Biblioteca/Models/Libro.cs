using System.Linq;

namespace Biblioteca.Models;

// Libro: es prestable y valorable
public class Libro : Articulo
{
    public string ISBN10 { get; set; } = "";
    
    public bool Prestado { get; set; }

    public override string Tipo => "Libro";

    // Validación del ISBN-10
    public static bool ValidarISBN10(string isbn)
    {
        if (isbn.Length != 10 || !isbn.All(char.IsDigit))
            return false;

        int suma = 0;
        for (int i = 0; i < 10; i++)
            suma += (10 - i) * (isbn[i] - '0');

        return suma % 11 == 0;
    }
}