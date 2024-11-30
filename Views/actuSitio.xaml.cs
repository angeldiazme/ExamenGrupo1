using Syncfusion.Maui.SignaturePad;
using Microsoft.Maui.Controls;
using System;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections.ObjectModel;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Newtonsoft.Json;
using Examen_Grupo2.Views;

namespace Examen_Grupo2.Views;

public partial class actuSitio : ContentPage
{
    Controllers.SitiosController controller;
    private int empleadoId;
    public actuSitio(int empleId)
    {
        InitializeComponent();
        empleadoId = empleId;
        LoadEmployeeData(empleadoId);
    }
    private async void saveButton_Clicked(object sender, EventArgs e)
    {
        ImageSource? source = signaturePad.ToImageSource();
        string base64String = await ImageSourceToBase64(source);
        string Descripcion = DescripcionEntry.Text;
        string Latitud = LatitudEntry.Text;
        string Longitud = LongitudEntry.Text;

        if (string.IsNullOrEmpty(Latitud))
        {
            await DisplayAlert("Error", "Por favor ingrese una latitud", "OK");
            return;
        }

        if (string.IsNullOrEmpty(Longitud))
        {
            await DisplayAlert("Error", "Por favor ingrese una longitud", "OK");
            return;
        }

        if (string.IsNullOrEmpty(Descripcion))
        {
            await DisplayAlert("Error", "Por favor ingrese una descripción", "OK");
            return;
        }

        if (string.IsNullOrEmpty(base64String))
        {
            await DisplayAlert("Error", "Por favor ingrese una firma", "OK");
            return;
        }

        double lati = Double.Parse(Latitud);
        double longi = Double.Parse(Longitud);

        var sitio = new Models.Sitios
        {
            descripcion = Descripcion,
            latitud = lati,
            longitud = longi,
            firma = base64String,
            audio = null
        };

        try
        {
            if (controller != null)
            {
                if (await Controllers.SitiosController.Create(sitio) > 0)
                {
                    await DisplayAlert("Aviso", "Registro Ingresado con Exito!", "OK");

                    signaturePad.Clear();
                    LatitudEntry.Text = string.Empty;
                    LongitudEntry.Text = string.Empty;
                    DescripcionEntry.Text = string.Empty;

                    this.OnAppearing();
                }
                else
                {
                    await DisplayAlert("Error", "Ocurrio un Error", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrio un Error: {ex.Message}", "OK");
        }
    }
    private async Task<String> ImageSourceToBase64(ImageSource imageSource)
    {
        if (imageSource is StreamImageSource streamImageSource)
        {
            using (var stream = await streamImageSource.Stream(CancellationToken.None))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
        }
        return null;
    }

    private async void LoadEmployeeData(int empleId)
    {
        var empleados = await Controllers.SitiosController.Get();
        var emple = empleados.FirstOrDefault(e => e.id == empleId);
        if (emple != null)
        {
            LongitudEntry.Text = emple.longitud.ToString();
            LatitudEntry.Text = emple.latitud.ToString();
            DescripcionEntry.Text = emple.descripcion;
        }
    }
    private void ClearFirmaButtonClicked(object? sender, EventArgs e)
    {
        signaturePad.Clear();
    }
}