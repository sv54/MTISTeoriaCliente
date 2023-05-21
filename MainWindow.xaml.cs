using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace MTISTeoriaCliente
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetSeguimiento_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Realiza una petición GET a una URL específica
                    //TODO: Cambiar la url a localhost
                    HttpResponseMessage response = await client.GetAsync("http://localhost:9095/seguimiento?oper=get&codigo=" + Codigo1.Text);
                    //HttpResponseMessage response = await client.GetAsync("http://192.168.1.122:9095/seguimiento?oper=get&codigo=" + Codigo1.Text);

                    // Verifica si la petición fue exitosa (código de respuesta 200-299)
                    if (response.IsSuccessStatusCode)
                    {
                        // Lee el contenido de la respuesta como una cadena de texto
                        string responseBody = await response.Content.ReadAsStringAsync();
                        if (responseBody.Contains(Codigo1.Text + " no es valido"))
                        {
                            Console.WriteLine("Respuesta: " + responseBody);
                            Estado.Text = responseBody;
                        }
                        else
                        {
                            //Quitamos los corchetes
                            responseBody = responseBody.Substring(1, responseBody.Length - 2);
                            Console.WriteLine("Respuesta: " + responseBody);
                            JObject json = JObject.Parse(responseBody);
                            Console.WriteLine("Respuesta: " + json["estado"]);
                            Estado.Text = (string)json["estado"];
                        }
                    }
                    else
                    {
                        Console.WriteLine("La petición no fue exitosa. Código de respuesta: " + response.StatusCode);
                        Estado.Text = "El codigo no corresponde con ningun envio";

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en la petición: " + ex.Message);
                }
            }
        }

        private async void Asignar_y_enviar_envio_pendiente_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Realiza una petición GET a una URL específica
                    HttpResponseMessage response = await client.GetAsync("http://localhost:9094/envio?idEnvio=" + Id_envio.Text);

                    // Verifica si la petición fue exitosa (código de respuesta 200-299)
                    
                    // Lee el contenido de la respuesta como una cadena de texto
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Response.Text = responseBody;
                        
                }
                catch (Exception ex)
                {
                    Response.Text = "Error";
                    Console.WriteLine("Error en la petición: " + ex.Message);
                }
            }
        }

        private async void Cliente_no_disponible_en_entrega_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Realiza una petición GET a una URL específica
                    HttpResponseMessage response = await client.GetAsync(" http://localhost:9094/entrega/clienteNoDisponible?idEnvio=" + Id_envio.Text);

                    // Verifica si la petición fue exitosa (código de respuesta 200-299)

                    // Lee el contenido de la respuesta como una cadena de texto
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Response.Text = responseBody;

                }
                catch (Exception ex)
                {
                    Response.Text = "Error";
                    Console.WriteLine("Error en la petición: " + ex.Message);
                }
            }
        }

        private async void Cliente_disponible_en_entrega_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Realiza una petición GET a una URL específica
                    HttpResponseMessage response = await client.GetAsync("http://localhost:9094/entrega/clienteDisponible?idEnvio=" + Id_envio.Text);

                    // Verifica si la petición fue exitosa (código de respuesta 200-299)

                    // Lee el contenido de la respuesta como una cadena de texto
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Response.Text = responseBody;

                }
                catch (Exception ex)
                {
                    Response.Text = "Error";
                    Console.WriteLine("Error en la petición: " + ex.Message);
                }
            }
        }
    }
}
