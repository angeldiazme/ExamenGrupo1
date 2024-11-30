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
using Plugin.Maui.Audio;

namespace Examen_Grupo2
{
    public partial class MainPage : ContentPage
    {
        Controllers.SitiosController controller;
        readonly IAudioManager _audioManager;
        readonly IAudioRecorder _audioRecorder;
        private const string GoogleMapsApiKey = "AIzaSyCUM-myzK7lScxEnEDRG2NlbpwXg1A0h0k";

        public MainPage(IAudioManager audioManager)
        {
            InitializeComponent();
            _audioManager = audioManager;
            _audioRecorder = audioManager.CreateRecorder();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("@32362e302e30h0sDyu0BPmlI9Xo3h7kZDX8f8FTPWSPhnp6iIqJ+Oew=");
            controller = new Controllers.SitiosController();
            SfSignaturePad signaturePad = new SfSignaturePad();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

                if (location != null)
                {
                    LatitudEntry.Text = location.Latitude.ToString();
                    LongitudEntry.Text = location.Longitude.ToString();

                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        DescripcionEntry.Text = placemark.Thoroughfare + ", " + placemark.Locality;
                    }
                    else
                    {
                        DescripcionEntry.Text = "No location description available";
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Unable to get location", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
            await CheckGpsStatusAsync();
        }

        private async System.Threading.Tasks.Task CheckGpsStatusAsync()
        {
            var locationStatus = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));

            if (locationStatus == null)
            {
                await DisplayAlert("GPS Not Enabled", "Please enable GPS to use this app.", "OK");
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
        private async void OnStartRecordingClicked(object sender, EventArgs e)
        {
            if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Denied", "Microphone permission is required to record audio.", "OK");
                return;
            }

            if (!_audioRecorder.IsRecording)
            {
                await _audioRecorder.StartAsync();
                StartRecordingButton.IsVisible = false;
                StopRecordingButton.IsVisible = true;
            }
        }

        private async void OnStopRecordingClicked(object sender, EventArgs e)
        {
            if (_audioRecorder.IsRecording)
            {
                var recordedAudio = await _audioRecorder.StopAsync();

                var audioStream = recordedAudio.GetAudioStream();

                StartRecordingButton.IsVisible = true;
                StopRecordingButton.IsVisible = false;
            }
        }

        private void ClearFirmaButtonClicked(object? sender, EventArgs e)
        {
            signaturePad.Clear();
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

        private void listButton_Clicked(object sender, EventArgs e)
        {
           Navigation.PushAsync(new ListSitios());
        }
    }

}
