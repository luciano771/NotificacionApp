using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Newtonsoft.Json;
using NotificacionApp.Models;
using Plugin.Firebase.CloudMessaging;
using Newtonsoft.Json.Linq;

namespace NotificacionApp.Views;

public partial class Login : ContentPage
{  
	public Login()
	{
		InitializeComponent();
        
	}

    private async void OnLabelTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(("//Registro"));

    }

    private async void Login_clicked(object sender, EventArgs args)
    {     
        Usuarios usuario = new Usuarios
        {
            NombreUsuario = Nombre.Text.ToString(),
            CorreoElectronico = Email.Text.ToString(),
            Contraseña = Contraseña.Text.ToString(),
        };
   
        await EnviarDatosPorPost(usuario);

        Nombre.Text = "";
        Email.Text = "";
        Contraseña.Text = "";
    }



    public async Task<bool> EnviarDatosPorPost(Usuarios usuario)
    {

        if (usuario == null) { return false; }

        try
        {
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://8136-186-128-188-105.ngrok-free.app/api/Usuarios/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseBody);
                var token = jsonResponse["token"].ToString();
                var UsuarioId = jsonResponse["idUsuario"].ToString(); 
                Preferences.Set("token", token);
                Preferences.Set("UsuarioId", UsuarioId);
                // Redirigir a otra página (sustituye "PantallaPrincipal" por el nombre de tu página)
                //await Shell.Current.GoToAsync("//PantallaPrincipal");
                await Navigation.PushAsync(new PantallaPrincipal());
            }
            else
            {
                Console.WriteLine("Error en el registro");
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al enviar datos por POST: {ex.Message}");
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            return false;
        }
    
         
    }
}