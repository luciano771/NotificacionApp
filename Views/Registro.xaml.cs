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
using NotificacionApp.Models;

namespace NotificacionApp.Views;

public partial class Registro : ContentPage
{
    public Registro()
	{
		InitializeComponent();
         

    }
    private async void OnLabelTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Login");
    }
    private async void Registro_clicked(object sender, EventArgs args) 
	{
        try
        {
            await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
            var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
            await DisplayAlert("FCM token", token, "OK");

            Usuarios usuario = new Usuarios
            {
                NombreUsuario = Nombre.Text.ToString(),
                CorreoElectronico = Email.Text.ToString(),
                Contraseña = Contraseña.Text.ToString(),
            };

            await EnviarDatosPorPost(usuario);
        }
        catch (System.Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }

        Nombre.Text = "";
        Email.Text = "";
        Contraseña.Text = "";

        
    }

    public async Task<bool> EnviarDatosPorPost(Usuarios usuario)
    {
        try
        { 
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "https://6279-186-128-168-6.ngrok-free.app/api/Usuarios/Registro");
            //se debe iniciar la api rest de la siguiente manera: dotnet watch run --urls "http://192.168.1.36:7186"
            request.Content = content;

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            await Navigation.PushAsync(new Login());
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