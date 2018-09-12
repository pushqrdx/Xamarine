using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarine
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitializeEmbeddedHost();
        }

        private async void InitializeEmbeddedHost()
        {
#pragma warning disable 4014
            Startup.Initialize();
#pragma warning restore 4014
            WebView.Source = "http://localhost:9696/";
        }
    }
}
