using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using System.Net; //To upload the webhook to Discord

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace Informator
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string V = "Hello Windows";

        public MainPage()
        {
            this.InitializeComponent();
            var applicationView = ApplicationView.GetForCurrentView();
            var titleBar = applicationView.TitleBar;

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.White;
            this.customurl.Text = string.Empty;
        }

        public static void sendDiscordWebhook(string URL, string escapedjson)
        {
            var wr = WebRequest.Create(URL);
            wr.ContentType = "application/json";
            wr.Method = "POST";
            using (var sw = new StreamWriter(wr.GetRequestStream()))
                sw.Write(escapedjson);
            wr.GetResponse();
        }

        private void SendClick(object sender, RoutedEventArgs e)
        {
            string selectedurl = this.customurl.Text;
            sendDiscordWebhook(selectedurl,
          "{\"username\": \"" + this.type.Text + "\", \"embeds\":[    {\"description\":\"" + this.message.Text + "\", \"title\":\"" + this.title.Text + "\"}]  }");
        }

    }
}
