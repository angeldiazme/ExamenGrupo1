using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace Examen_Grupo2.Views;

public partial class Mapa : ContentPage
{
    public string photo;
    private double latitud;
	private double longitud;
	private string descripcion;
	public Mapa(double latitud, double longitud)
	{
		InitializeComponent();

        var position = new Location(latitud, longitud);
        MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.5)));

        var pin = new Pin
        {
            Label = "Selected Location",
            Location = position,
            Type = PinType.Place
        };

        MyMap.Pins.Add(pin);

    }
    private void btnSalir_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ListSitios());
    }
}