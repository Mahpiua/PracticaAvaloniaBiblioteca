using System;
using Microsoft.Data.Sqlite;
using Biblioteca.Models;
using System.Collections.Generic;

namespace Biblioteca.Services;

// Repositorio que usa SQLite
public class ArticuloRepository
{
    private readonly string _rutaBd;

    public ArticuloRepository(string rutaBd)
    {
        _rutaBd = rutaBd;
    }

    // Crear conexión
    private SqliteConnection CrearConexion()
    {
        var con = new SqliteConnection($"Data Source={_rutaBd}");
        con.Open();
        return con;
    }

    // Insertar artículo
    public void Insertar(Articulo articulo)
    {
        using var con = CrearConexion();
        var cmd = con.CreateCommand();

        cmd.CommandText = """
            INSERT INTO Articulos
            (Tipo, Titulo, Año, FechaAdquisicion, ISBN10, Prestado, FechaInicio, FechaFin)
            VALUES ($tipo, $titulo, $año, $fecha, $isbn, $prestado, $inicio, $fin)
        """;

        cmd.Parameters.AddWithValue("$tipo", articulo.Tipo);
        cmd.Parameters.AddWithValue("$titulo", articulo.Titulo);
        cmd.Parameters.AddWithValue("$año", articulo.Año);
        cmd.Parameters.AddWithValue("$fecha", articulo.FechaAdquisicion.ToString("yyyy-MM-dd"));

        if (articulo is Libro libro)
        {
            cmd.Parameters.AddWithValue("$isbn", libro.ISBN10);
            cmd.Parameters.AddWithValue("$prestado", libro.Prestado ? 1 : 0);
            cmd.Parameters.AddWithValue("$inicio", DBNull.Value);
            cmd.Parameters.AddWithValue("$fin", DBNull.Value);
        }
        else if (articulo is Audiolibro audio)
        {
            cmd.Parameters.AddWithValue("$isbn", DBNull.Value);
            cmd.Parameters.AddWithValue("$prestado", DBNull.Value);
            cmd.Parameters.AddWithValue("$inicio", audio.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"));
            cmd.Parameters.AddWithValue("$fin", audio.FechaFin.ToString("yyyy-MM-ddTHH:mm:ss"));
        }

        cmd.ExecuteNonQuery();
    }

    // Obtener todos los artículos
    public List<Articulo> ObtenerTodos()
    {
        var lista = new List<Articulo>();

        using var con = CrearConexion();
        var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT * FROM Articulos";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string tipo = reader.GetString(1);

            if (tipo == "Libro")
            {
                lista.Add(new Libro
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(2),
                    Año = reader.GetInt32(3),
                    FechaAdquisicion = DateTime.Parse(reader.GetString(4)),
                    ISBN10 = reader.GetString(5),
                    Prestado = reader.GetInt32(6) == 1
                });
            }
            else if (tipo == "Audiolibro")
            {
                lista.Add(new Audiolibro
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(2),
                    Año = reader.GetInt32(3),
                    FechaAdquisicion = DateTime.Parse(reader.GetString(4)),
                    FechaInicio = DateTime.Parse(reader.GetString(7)),
                    FechaFin = DateTime.Parse(reader.GetString(8))
                });
            }
        }

        return lista;
    }

    // Buscar por título
    public List<Articulo> Buscar(string filtro)
    {
        if (string.IsNullOrWhiteSpace(filtro))
            return ObtenerTodos();

        var lista = new List<Articulo>();

        using var con = CrearConexion();
        var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT * FROM Articulos WHERE LOWER(Titulo) LIKE $filtro";
        cmd.Parameters.AddWithValue("$filtro", "%" + filtro.ToLower() + "%");

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string tipo = reader.GetString(1);

            if (tipo == "Libro")
            {
                lista.Add(new Libro
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(2),
                    Año = reader.GetInt32(3),
                    FechaAdquisicion = DateTime.Parse(reader.GetString(4)),
                    ISBN10 = reader.GetString(5),
                    Prestado = reader.GetInt32(6) == 1
                });
            }
            else if (tipo == "Audiolibro")
            {
                lista.Add(new Audiolibro
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(2),
                    Año = reader.GetInt32(3),
                    FechaAdquisicion = DateTime.Parse(reader.GetString(4)),
                    FechaInicio = DateTime.Parse(reader.GetString(7)),
                    FechaFin = DateTime.Parse(reader.GetString(8))
                });
            }
        }

        return lista;
    }
}