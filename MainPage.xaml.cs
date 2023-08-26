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

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        try
        {
            await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
            var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
            await DisplayAlert("FCM token", token, "OK");
            await EnviarDatosPorPost("Maui1","maui@gmail.com","4226",token.ToString());
        }
        catch (System.Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }





    public async Task<bool> EnviarDatosPorPost(string nombre, string contraseña, string email, string tokenNotificacion)
    {


        try
        {
            var Usuarios = new Dictionary<string, object>
            {
            { "nombre",nombre  },
            { "contraseña", contraseña },
            { "email", email},
            { "edad", "35"},
            { "tokenNotificacion", tokenNotificacion }
        };

            var json = JsonConvert.SerializeObject(Usuarios);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.1.36:7186/api/Usuarios/Registro");
            //se debe iniciar la api rest de la siguiente manera: dotnet watch run --urls "http://192.168.1.36:7186"
            request.Content = content;

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return true;
        }
        catch (System.Exception ex)
        {
            // Manejar errores de conexión o de la API
            Console.WriteLine($"Error al enviar datos por POST: {ex.Message}");
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            return false;
        }
    }





}
