using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificacionApp.Views
{
    public class UsuarioCell : ViewCell
    {
        public UsuarioCell()
        {
            var nombreLabel = new Label();
            nombreLabel.SetBinding(Label.TextProperty, new Binding("Nombre"));

            var correoLabel = new Label();
            correoLabel.SetBinding(Label.TextProperty, new Binding("Email"));

            var layout = new StackLayout
            {
                Children = { nombreLabel, correoLabel }
            };

            View = layout;
        }
    }
}
