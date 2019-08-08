using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JSON_API_WEB
{
    class Program
    {
        public static string lastError = "";
        public static string weather = "";//"{\"coord\":{\"lon\":30.73,\"lat\":46.48},\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"ясно\",\"icon\":\"01d\"}],\"base\":\"stations\",\"main\":{\"temp\":300.15,\"pressure\":1013,\"humidity\":54,\"temp_min\":300.15,\"temp_max\":300.15},\"visibility\":10000,\"wind\":{\"speed\":10,\"deg\":190},\"clouds\":{\"all\":0},\"dt\":1565267352,\"sys\":{\"type\":1,\"id\":8915,\"message\":0.0061,\"country\":\"UA\",\"sunrise\":1565232363,\"sunset\":1565284775},\"timezone\":10800,\"id\":698740,\"name\":\"Odessa\",\"cod\":200}";

        static void Main(string[] args)
        {
            // Получение данных в синхронном режиме
            GetWeatherDataSync();
            Console.WriteLine("Sync data load: " + lastError);

            // Получение данных в асинхронном режиме
            /*            
            GetWeatherDataAsync().Wait();
            Console.WriteLine("Async data load: " + lastError);
            */

            // Получение данных в асинхронном режиме
            /*
            GetWeatherDataAsync2();

            DateTime start = DateTime.Now, now;
            double ms = 0;
            while (weather == "" && ms < 3000)
            {
                now = DateTime.Now;
                ms = (now - start).TotalMilliseconds;
                Console.SetCursorPosition(0, 2);
                Console.Write($"Async data load: {(int)ms} ms");
            }
            if (weather == "")
            {
                Console.SetCursorPosition(15, 2);
                Console.Write(" request timout\n" + lastError);
            }
            else
            {
                Console.WriteLine($"Async weather data load: successfuly");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            */

            Console.WriteLine(weather);
            if (weather != "")
            {
//                JObject details = JObject.Parse(weather);
//                Console.WriteLine(details);
//                Console.WriteLine(details.GetValue("sys"));
//                Console.WriteLine(details.GetValue("sys").Value<String>("country"));

//                WeatherInfo currentWeather = JsonConvert.DeserializeObject<WeatherInfo>(weather);
//                Console.WriteLine($"{currentWeather.Name}({currentWeather.Coord.Lat} - {currentWeather.Coord.Lon})");
            }
            else
            {
                Console.WriteLine("No weather data available");
            }
        }

        private static void GetWeatherDataSync()
        {
            lastError = "";
            WebClient client = new WebClient();
            try
            {
                using (Stream stream = client.OpenRead("https://api.openweathermap.org/data/2.5/weather?q=Odessa,ua&appid=dac392b2d2745b3adf08ca26054d78c4&lang=ru"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        weather = reader.ReadToEnd();
                    }
                }
                lastError = "successfuly";
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }

        private static async Task GetWeatherDataAsync()
        {
            lastError = "";
            try
            {
                WebClient client = new WebClient();
                StreamReader reader = new StreamReader(await client.OpenReadTaskAsync(new Uri("https://api.openweathermap.org/data/2.5/weather?q=Odessa,ua&appid=dac392b2d2745b3adf08ca26054d78c4&lang=ru", UriKind.Absolute)));
                weather = reader.ReadLine();
                lastError = "successfuly";
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }

        private static void GetWeatherDataAsync2()
        {
            lastError = "";
            WebClient client = new WebClient();

            try
            {
                client.OpenReadCompleted += client_OpenReadCompleted;
                client.OpenReadAsync(new Uri("https://api.openweathermap.org/data/2.5/weather?q=Odessa,ua&appid=dac392b2d2745b3adf08ca26054d78c4&lang=ru"));

                void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
                {
                    if (e.Error == null)
                        weather = new StreamReader(e.Result).ReadToEnd().ToString();
                }
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }
    }
}
