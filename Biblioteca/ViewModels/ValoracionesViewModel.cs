using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Biblioteca.Models;
using Biblioteca.Services;
using System.Collections.ObjectModel;

namespace Biblioteca.ViewModels;

public partial class ValoracionesViewModel : ObservableObject
{
    private readonly ValoracionRepository _repo;

    public ObservableCollection<Valoracion> Lista { get; } = new();

    public List<int> Puntuaciones { get; } = new() { 1, 2, 3, 4, 5 };

    [ObservableProperty]
    private int puntuacionNueva = 5;

    [ObservableProperty]
    private string comentarioNuevo = "";

    private readonly int _idArticulo;

    public ValoracionesViewModel(int idArticulo, ValoracionRepository repo)
    {
        _idArticulo = idArticulo;
        _repo = repo;

        foreach (var v in repo.ObtenerPorArticulo(idArticulo))
            Lista.Add(v);
    }

    [RelayCommand]
    private void Añadir()
    {
        var val = new Valoracion
        {
            IdArticulo = _idArticulo,
            Puntuacion = PuntuacionNueva,
            Comentario = ComentarioNuevo,
            Fecha = DateTime.Now
        };

        _repo.Insertar(val);
        Lista.Add(val);

        ComentarioNuevo = "";
        PuntuacionNueva = 5;
    }
}