using System;

namespace Biblioteca.Models;

// Clase base para todos los artículos
public abstract class Articulo
{
    public int Id { get; set; }

    // Título con validación básica
    public string Titulo { get; set; } = "";

    public int Año { get; set; }

    public DateTime FechaAdquisicion { get; set; }

    // Cada clase hija define su tipo
    public abstract string Tipo { get; }

    // Formatea el título (primer letra mayúscula)
    public static string FormatearTitulo(string titulo)
    {
        return titulo.Trim().Length == 0
            ? ""
            : char.ToUpper(titulo.Trim()[0]) + titulo.Trim().Substring(1);
    }

    // Validación del año
    public static bool ValidarAño(int año)
    {
        return año >= 1500 && año <= DateTime.Now.Year;
    }
}