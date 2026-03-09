using System;
using Biblioteca.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Biblioteca.Services;
using Biblioteca.Views;

namespace Biblioteca.ViewModels;

// ViewModel principal
public partial class MainWindowViewModel : ViewModelBase
{
    
    private readonly ArticuloRepository _repo = new("BD/biblioteca.db");
    
    // Lista observable para el DataGrid
    [ObservableProperty]
    private ObservableCollection<Articulo> articulos = new();

    // Texto del buscador
    [ObservableProperty]
    private string filtro = "";

    public MainWindowViewModel()
    {
        // Cargar artículos desde SQLite
        _repo = new ArticuloRepository("BD/biblioteca.db");
        articulos = new ObservableCollection<Articulo>(_repo.ObtenerTodos());
    }
    
    // Comando para crear un artículo
    [RelayCommand]
    private async void CrearArticulo()
    {
        // Crear la ventana secundaria
        var ventana = new EditarArticuloView();

        // Crear su ViewModel
        var vm = new EditarArticuloViewModel();

        // Asignar DataContext
        ventana.DataContext = vm;

        // Obtener la ventana principal (owner)
        var lifetime = App.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var mainWindow = lifetime!.MainWindow;

        // Mostrar la ventana secundaria como diálogo
        var resultado = await ventana.ShowDialog<Articulo?>(mainWindow);

        // Si el usuario canceló, no hacemos nada
        if (resultado == null)
            return;


        // Añadir al repositorio
        _repo.Insertar(resultado);

        // Actualizar la lista observable
        Articulos.Add(resultado);
    }

    [RelayCommand]
    private async void ExportarCsv()
    {
        // Diálogo para elegir archivo
        var dialogo = new SaveFileDialog
        {
            Title = "Exportar catálogo",
            Filters =
            {
                new FileDialogFilter { Name = "CSV", Extensions = { "csv" } }
            }
        };

        var ventana = App.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        // Mostrar diálogo
        var ruta = await dialogo.ShowAsync(ventana!.MainWindow);

        if (ruta == null)
            return;

        // Exportar usando el servicio
        var csv = new CsvService();
        csv.Exportar(ruta, _repo.ObtenerTodos());

        Console.WriteLine("Catálogo exportado correctamente.");
    }

    [RelayCommand]
    private async void VerValoraciones(Articulo articulo)
    {
        var repoVal = new ValoracionRepository("BD/biblioteca.db");

        var vm = new ValoracionesViewModel(articulo.Id, repoVal);
        var ventana = new ValoracionesView { DataContext = vm };

        var lifetime = App.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        await ventana.ShowDialog(lifetime!.MainWindow);
    }
    
    // Comando para buscar
    [RelayCommand]
    private void Buscar()
    {
        // Obtener resultados del repositorio
        var resultados = _repo.Buscar(Filtro);

        // Limpiar la colección observable
        Articulos.Clear();

        // Añadir los resultados
        foreach (var art in resultados)
            Articulos.Add(art);
    }
}