using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Biblioteca.ViewModels;

namespace Biblioteca.Views;

public partial class EditarArticuloView : Window
{
    public EditarArticuloView()
    {
        InitializeComponent();
        
        DataContextChanged += (s, e) =>
        {
            if (DataContext is EditarArticuloViewModel vm)
            {
                vm.RequestClose += articulo =>
                {
                    Close(articulo);
                };
            }
        };

    }
}