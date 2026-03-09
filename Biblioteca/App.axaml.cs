using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Biblioteca.Services;
using Biblioteca.Views;
using Biblioteca.ViewModels;

namespace Biblioteca;

public partial class App : Application
{
    // Servicio de base de datos
    private DatabaseService? _db;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 1. Crear el servicio de base de datos
        //    La BD se guardará en un archivo llamado biblioteca.db
        String _ruta = Path.Combine(AppContext.BaseDirectory, "BD", "biblioteca.db");

        // Crear carpeta si no existe
        Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "BD"));

        _db = new DatabaseService(_ruta);

        // 2. Inicializar la base de datos (crear tablas si no existen)
        _db.Inicializar();

        // 3. Crear la ventana principal
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindowView
            {
                // 4. Asignar el ViewModel
                DataContext = new MainWindowViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}