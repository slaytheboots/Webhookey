using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using System.Net; //To upload the webhook to Discord
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using System;
using Windows.Storage;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace Informator
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string V = "Hello Windows";
        ApplicationDataContainer roamingSettings = null;

        const string SettingName = "exampleSetting";

        public MainPage()
        {
            this.InitializeComponent();
            roamingSettings = ApplicationData.Current.RoamingSettings;
            var applicationView = ApplicationView.GetForCurrentView();
            var titleBar = applicationView.TitleBar;

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.White;
            this.customurl.Text = string.Empty;
            this.customurl.Visibility = Visibility.Collapsed;
            this.Default.IsChecked = true;
            if (SystemInformation.Instance.IsFirstRun)
            {
                WelcomeMessage();
            }
            else
            {
            }
        }


        public static void WelcomeMessage() {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Notice",
                Content = "Remember to set your default webhook URL in Settings.",
                PrimaryButtonText = "Ok"
            };
            // Finally, show the dialog
            dialog.ShowAsync();
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

        private void defaultradio(object sender, RoutedEventArgs e) {
        }

        private void customradio(object sender, RoutedEventArgs e)
        {
            this.customurl.Visibility = Visibility.Visible;
            this.theselectors.Visibility = Visibility.Collapsed;
        }

        private void SendClick(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            Windows.Storage.ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)roamingSettings.Values["RoamingFontInfo"];
            if (composite != null)
            {
                this.defaulturl.Text = composite["defaulturlsetting"] as string;
            }
            string selectedurl = this.customurl.Text + this.defaulturl.Text;
            sendDiscordWebhook(selectedurl,
          "{\"username\": \"" + this.type.Text + "\", \"embeds\":[    {\"description\":\"" + this.message.Text + "\", \"title\":\"" + this.title.Text + "\"}]  }");
        }

        private void girlboss(object sender, RoutedEventArgs e)
        {
            roamingSettings.Values[SettingName] = this.defaulturl.Text;
            Object value = roamingSettings.Values[SettingName];
            string ayo = Convert.ToString(value);
            this.defaulturl.Text = Convert.ToString(value);
        }

    }
}
