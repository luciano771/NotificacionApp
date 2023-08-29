using Microsoft.Maui.Controls;
using NotificacionApp.Views;
namespace NotificacionApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(Login), typeof(Login));
        Routing.RegisterRoute(nameof(Registro), typeof(Registro));
        Routing.RegisterRoute(nameof(PantallaPrincipal), typeof(PantallaPrincipal));
       


    }

}
