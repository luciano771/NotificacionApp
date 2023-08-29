using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotificacionApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
 

namespace NotificacionApp.Views;

public partial class PantallaPrincipal : ContentPage
{
    private int noteCounter = 0;
    private Timer timer;
    public PantallaPrincipal()
    {
        InitializeComponent();
        DisplayAlert("JWT Bearer token", Preferences.Get("token", "").ToString(), "OK");
        // de login llege con un pushasync, volver con un popasync a login o generar otro pushasync para ir a otra pagina.
        timer = new Timer(ExpiracionTokenCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
        listarNotas();
    }


    private void CreateDivButton_Clicked(object sender, EventArgs e)
    {
        CrearNotas();
    }

    private async void CrearNotas() 
    {
        string TituloNote = await DisplayPromptAsync("Titulo de la nota", "Ingresa el Titulo:", "Guardar", "Cancelar");
        string newNoteText = await DisplayPromptAsync(TituloNote, "Ingresa la nota:", "Guardar", "Cancelar");

        if (TituloNote == null || newNoteText == null) { return; }
        
            noteCounter++;

        var noteFrame = new Frame
        {
            Style = (Style)Resources["NoteStyle"],
            Content = new StackLayout
            {
                Children =
                {
                    new Label { Text = $"{TituloNote}", FontSize = 20, FontAttributes = FontAttributes.Bold },
                    new Label { Text = newNoteText }
                }
            }
        };

        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += async (s, e) =>
        {
            await DisplayAlert("Note Tapped", $"You tapped on Note {noteCounter}", "OK");
        };

        noteFrame.GestureRecognizers.Add(tapGestureRecognizer);

        DivsFlexLayout.Children.Add(noteFrame);

        Notas notas = new()
        {
            UsuarioId = int.Parse(Preferences.Get("UsuarioId", " ").ToString()),
            Titulo = TituloNote,
            Contenido = newNoteText,
            FechaCreacion = DateTime.Now
        };

        await GuardarNotasPost(notas);
       
    }

    public async Task<bool> GuardarNotasPost(Notas notas)
    {
       
        if (notas == null) { return false; }
        try

        {
            var content = new StringContent(JsonConvert.SerializeObject(notas), Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Preferences.Get("token", "").ToString());
                HttpResponseMessage response = await client.PostAsync("https://8136-186-128-188-105.ngrok-free.app/api/Usuarios/Notas", content);
                return true;
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            return false;
        }
         
    }

    private async void listarNotas() 
    {
        List<Notas> notas = await RecibirNotasGet();
        foreach (var nota in notas) 
        {
            var noteFrame = new Frame
            {
                Style = (Style)Resources["NoteStyle"],
                Content = new StackLayout
                {
                    Children =
                {
                    new Label { Text = $"{nota.Titulo}", FontSize = 20, FontAttributes = FontAttributes.Bold },
                    new Label { Text = nota.Contenido }
                }
                }
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                await DisplayAlert($"{nota.Titulo}", $"{nota.Contenido}", "OK");
            };

            noteFrame.GestureRecognizers.Add(tapGestureRecognizer);

            DivsFlexLayout.Children.Add(noteFrame);
        }
    }


    public async Task<List<Notas>> RecibirNotasGet()
     {
          
        try
        {
            List<Notas> notas = new List<Notas>();
            using (HttpClient client = new HttpClient())
            {   
                 
                // Agregar el token al encabezado
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Preferences.Get("token", "").ToString());

                // Realizar la solicitud GET
                HttpResponseMessage response = await client.GetAsync("https://8136-186-128-188-105.ngrok-free.app/api/Usuarios/ListarNotas?idusuario="+Preferences.Get("UsuarioId", " ").ToString());

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    notas = JsonConvert.DeserializeObject<List<Notas>>(content);
                     
                }
            }
            return notas;
        }
        catch (System.Exception ex)
        {
            // Manejar errores de conexión o de la API
            Console.WriteLine($"Error al enviar datos por POST: {ex.Message}");
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            return null;
        }
    }

    //expiracion del token:

    private void ExpiracionTokenCallback(object state)
    {
        bool expiro = ExpiracionToken(Preferences.Get("token", "").ToString());
        if (expiro)
        {
            this.Dispatcher.Dispatch(() =>
            {
                DisplayAlert("Vencio Token", "Debe logearse nuevamente", "OK");
                Navigation.PushAsync(new Login());

            });
        }
        
    }

    public bool ExpiracionToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        DateTime expirationDate = DateTimeOffset.FromUnixTimeSeconds((long)jwtToken.Payload.Exp).UtcDateTime;
        TimeSpan timeUntilExpiration = expirationDate - DateTime.UtcNow;

        return timeUntilExpiration <= TimeSpan.Zero;
    }





}