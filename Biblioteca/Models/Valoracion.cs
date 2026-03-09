using System;

namespace Biblioteca.Models;

public class Valoracion
{
    public int Id { get; set; }
    public int IdArticulo { get; set; }
    public int Puntuacion { get; set; } // 1–5
    public string Comentario { get; set; } = "";
    public DateTime Fecha { get; set; }
}