using Microsoft.Data.Sqlite;

namespace Biblioteca.Services;

// Servicio simple para manejar SQLite
public class DatabaseService
{
    private readonly string _ruta;

    public DatabaseService(string ruta)
    {
        _ruta = ruta;
    }

    // Crea y abre la conexión
    public SqliteConnection CrearConexion()
    {
        var con = new SqliteConnection($"Data Source={_ruta}");
        con.Open();
        return con;
    }

    // Crea la tabla si no existe
    public void Inicializar()
    {
        using var con = CrearConexion();
        var cmd = con.CreateCommand();

        cmd.CommandText = """
                              CREATE TABLE IF NOT EXISTS Articulos(
                                  Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                  Tipo TEXT,
                                  Titulo TEXT,
                                  Año INTEGER,
                                  FechaAdquisicion TEXT,
                                  ISBN10 TEXT,
                                  Prestado INTEGER,
                                  FechaInicio TEXT,
                                  FechaFin TEXT
                              );
                          """;

        cmd.ExecuteNonQuery();
        
        cmd.CommandText = """
                          CREATE TABLE IF NOT EXISTS Valoraciones(
                              Id INTEGER PRIMARY KEY AUTOINCREMENT,
                              IdArticulo INTEGER NOT NULL,
                              Puntuacion INTEGER NOT NULL,
                              Comentario TEXT,
                              Fecha TEXT NOT NULL,
                              FOREIGN KEY(IdArticulo) REFERENCES Articulos(Id)
                          );
                          """;
        cmd.ExecuteNonQuery();

    }
}