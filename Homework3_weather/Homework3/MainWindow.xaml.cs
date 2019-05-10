using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace Homework3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

        }


        // API key from OpenWeatherAPI
        private const String APIKEY = "a366fdabd265706683be8626e03ac721";

        public static string DegreesToDirection(double degrees)
        {
            string[] directions = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
            return directions[(int)Math.Round(((double)degrees * 10 % 3600) / 225)];
        }

        //the button to click once your location is entered!
        private void GoGet_Click(object sender, RoutedEventArgs e)
        {
            String location = LocationText.GetLineText(0);

            MyLocationMain.Text=(LocationText.GetLineText(0));
            

            // Get the weather information from the API
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(location)}&appid={APIKEY}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Process the weather information
                    string json = reader.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    var dict = jss.Deserialize<Dictionary<string, dynamic>>(json);

                    // Print out the current weather conditions
                   ConditionsInfo.Text=($"Conditions:  {dict["weather"][0]["main"]}");
                   TemperatureInfo.Text=($"Temperature: {Math.Round(Convert.ToDouble(dict["main"]["temp"]) - 273.15)}°C");
                   HumidityInfo.Text=   ($"Humidity:    {dict["main"]["humidity"]}%");
                   PressureInfo.Text=   ($"Pressure:    {dict["main"]["pressure"]} hpa");
                   VisibilityInfo.Text= ($"Visibility:  {Convert.ToInt16(dict["visibility"]) / 1000}km");
                   WindInfo.Text= ($"Wind:        {dict["wind"]["speed"]} m/s {DegreesToDirection(Convert.ToInt16(dict["wind"]["deg"]))}");
                }
            }
            catch (Exception error)
            {
                // Oops... there was an error. Display the problem to the user
                Console.WriteLine("Unable to retrieve data:");
                Console.WriteLine(error.Message);
            }
        }

       
    }
    
}

   
