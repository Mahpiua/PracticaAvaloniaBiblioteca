using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Biblioteca.Models;

namespace Biblioteca.Services;

public class ValoracionRepository
{
    private readonly string _rutaBd;

    public ValoracionRepository(string rutaBd)
    {
        _rutaBd = rutaBd;
    }

    private SqliteConnection CrearConexion()
    {
        var con = new SqliteConnection($"Data Source={_rutaBd}");
        con.Open();
        return con;
    }

    public void Insertar(Valoracion val)
    {
        using var con = CrearConexion();
        var cmd = con.CreateCommand();

        cmd.CommandText = """
                              INSERT INTO Valoraciones (IdArticulo, Puntuacion, Comentario, Fecha)
                              VALUES ($id, $p, $c, $f)
                          """;

        cmd.Parameters.AddWithValue("$id", val.IdArticulo);
        cmd.Parameters.AddWithValue("$p", val.Puntuacion);
        cmd.Parameters.AddWithValue("$c", val.Comentario);
        cmd.Parameters.AddWithValue("$f", val.Fecha.ToString("yyyy-MM-ddTHH:mm:ss"));

        cmd.ExecuteNonQuery();
    }

    public List<Valoracion> ObtenerPorArticulo(int idArticulo)
    {
        var lista = new List<Valoracion>();

        using var con = CrearConexion();
        var cmd = con.CreateCommand();

        cmd.CommandText = "SELECT * FROM Valoraciones WHERE IdArticulo = $id";
        cmd.Parameters.AddWithValue("$id", idArticulo);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Valoracion
            {
                Id = reader.GetInt32(0),
                IdArticulo = reader.GetInt32(1),
                Puntuacion = reader.GetInt32(2),
                Comentario = reader.GetString(3),
                Fecha = DateTime.Parse(reader.GetString(4))
            });
        }

        return lista;
    }
}