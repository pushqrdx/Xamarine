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
            Startup.Initialize();
            await Task.Delay(5000);
            WebView.Source = "http://localhost:9696/";
        }
    }
}
