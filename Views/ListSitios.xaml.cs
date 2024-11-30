using System.Collections.ObjectModel;
using Examen_Grupo2.Models;
using Examen_Grupo2.Controllers;
using MediaManager;

namespace Examen_Grupo2.Views;

public partial class ListSitios : ContentPage
{
    Models.Sitios selectedEmple;
    public Command<Sitios> UpdateCommand { get; }
    public Command<Sitios> DeleteCommand { get; }
    public ObservableCollection<Sitios> empleado { get; set; }
    public ListSitios()
	{
		InitializeComponent();
        empleado = new ObservableCollection<Sitios>();
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        List<Models.Sitios> emplelist = new List<Models.Sitios>();
        emplelist = await SitiosController.Get();
        listemple.ItemsSource = emplelist;
    }

    private void listemple_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        selectedEmple = e.CurrentSelection.FirstOrDefault() as Models.Sitios;
    }

    private async void Actualizar_Clicked(object sender, EventArgs e)
    {
        if (selectedEmple != null)
        {
            await Navigation.PushAsync(new actuSitio(selectedEmple.id));
        }
        else
        {
            await DisplayAlert("Error", "Seleccione una ubicacion primero", "OK");
        }
    }

    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Confirmar", "¿Está seguro que desea eliminar esta ubicacion?", "Sí", "No");

        if (selectedEmple != null)
        {
            if (result)
            {
                await SitiosController.Delete(selectedEmple.id);
                empleado.Remove(selectedEmple);

                var currentPage = this;
                await Navigation.PushAsync(new ListSitios());
                Navigation.RemovePage(currentPage);
            }
            else
            {
                return;
            }
        }
        else
        {
            await DisplayAlert("Error", "Seleccione una ubicacion primero", "OK");
        }
    }
    private async void Mapa_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Mapa(selectedEmple.latitud, selectedEmple.longitud));
    }
}