using Plugin.Firebase.CloudMessaging;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace NotificacionApp;

public partial class MainPage : ContentPage
{
     
    public MainPage()
    {
        InitializeComponent();
    }

    private async void Token(object sender, EventArgs e)
    {
        try
        {
            await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
            var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
            await DisplayAlert("FCM token", token, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }





    public async Task<bool> EnviarDatosPorPost(int pasos, string distancia, string tiempo, string calorias, string fecha)
    {


        try
        {
            var Notificaciones = new Dictionary<string, object>
        {
            { "pasos", pasos },
            { "distancia", distancia },
            { "tiempo", tiempo},
            { "calorias", calorias },
            { "fecha", fecha }
        };

            var json = JsonConvert.SerializeObject(Notificaciones);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://127.0.0.1:7186/api/Usuarios/");
            request.Content = content;

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return true;
        }
        catch (System.Exception ex)
        {
            // Manejar errores de conexión o de la API
            Console.WriteLine($"Error al enviar datos por POST: {ex.Message}");
            return false;
        }
    }





}
