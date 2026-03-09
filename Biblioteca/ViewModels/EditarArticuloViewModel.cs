using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Biblioteca.Models;

namespace Biblioteca.ViewModels;

public partial class EditarArticuloViewModel : ObservableObject
{
    // Tipos disponibles
    public List<string> Tipos { get; } = new() { "Libro", "Audiolibro" };

    [ObservableProperty]
    private string tipoSeleccionado = "Libro";

    // Propiedades comunes
    [ObservableProperty]
    private string titulo = "";

    [ObservableProperty]
    private int año = DateTime.Now.Year;

    // Propiedades de Libro
    [ObservableProperty]
    private string isbn10 = "";

    // Propiedades de Audiolibro
    [ObservableProperty]
    private DateTimeOffset? fechaInicio = DateTimeOffset.Now;

    [ObservableProperty]
    private DateTimeOffset? fechaFin = DateTimeOffset.Now;

    // Propiedades calculadas para mostrar/ocultar campos
    public bool EsLibro => TipoSeleccionado == "Libro";
    public bool EsAudiolibro => TipoSeleccionado == "Audiolibro";

    public static EditarArticuloViewModel DesignInstance => new()
    {
        Titulo = "Ejemplo",
        Año = 2024,
        Isbn10 = "1234567890",
        TipoSeleccionado = "Libro"
    };

    partial void OnTipoSeleccionadoChanged(string value)
    {
        OnPropertyChanged(nameof(EsLibro));
        OnPropertyChanged(nameof(EsAudiolibro));
    }

    // Evento para devolver el artículo
    public event Action<Articulo?>? RequestClose;

    // Comando Guardar
    [RelayCommand]
    private void Guardar()
    {
        if (string.IsNullOrWhiteSpace(Titulo))
        {
            Console.WriteLine("El título no puede estar vacío.");
            return;
        }

        Articulo articulo;

        if (EsLibro)
        {
            articulo = new Libro
            {
                Titulo = Titulo,
                Año = Año,
                FechaAdquisicion = DateTime.Now,
                ISBN10 = Isbn10,
                Prestado = false
            };
        }
        else
        {
            articulo = new Audiolibro
            {
                Titulo = Titulo,
                Año = Año,
                FechaAdquisicion = DateTime.Now,
                FechaInicio = FechaInicio?.DateTime ?? DateTime.Now,
                FechaFin = FechaFin?.DateTime ?? DateTime.Now
            };
        }

        RequestClose?.Invoke(articulo);
    }
}