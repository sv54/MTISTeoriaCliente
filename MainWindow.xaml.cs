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
using Newtonsoft.Json;
using System.Xml;

namespace MTISTeoriaCliente
{

    public partial class Envio
    {

        public long? Id { get; set; }
        public string Estado { get; set; }
        public string Descripcion { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Peso { get; set; }
        public string Altura { get; set; }
        public string Anchura { get; set; }
        public string Longitud { get; set; }
        public string Importancia { get; set; }
        public string idrepartidor { get; set; }
        public string Coste { get; set; }
    }
        public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public string glob;

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
                    HttpResponseMessage response = await client.GetAsync("http://localhost:9094/asignarEnvio?idEnvio=" + Id_envio.Text);

                    // Verifica si la petición fue exitosa (código de respuesta 200-299)
                    
                    // Lee el contenido de la respuesta como una cadena de texto
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Response.Text = responseBody;
                    if(!responseBody.Contains("No hay"))
                    {
                        SalidaAlmacen(sender, e);
                    }

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

        private async void Consultar_envio_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Realiza una petición GET a una URL específica
                    HttpResponseMessage response = await client.GetAsync("http://localhost:9094/asignarEnvio?idEnvio=" + Id_envio.Text);

                    // Verifica si la petición fue exitosa (código de respuesta 200-299)

                    // Lee el contenido de la respuesta como una cadena de texto
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Envio responseEnvio = JsonConvert.DeserializeObject<Envio>(responseBody);
                    envio_estado.Text = responseEnvio.Estado;
                    envio_descripcion.Text = responseEnvio.Descripcion;
                    envio_origen.Text = responseEnvio.Origen;
                    envio_destino.Text = responseEnvio.Destino;
                    envio_peso.Text = responseEnvio.Peso;                    
                    envio_altura.Text = responseEnvio.Altura;
                    envio_anchura.Text = responseEnvio.Anchura;
                    envio_longitud.Text = responseEnvio.Longitud;
                    envio_coste.Text = responseEnvio.Coste;
                    envio_idRepartidor.Text = responseEnvio.idrepartidor;
                    Response.Text = "";
                }
                catch (Exception ex)
                {
                    Response.Text = "Error";
                    Console.WriteLine("Error en la petición: " + ex.Message);
                }
            }
        }

        private async void RegistrarAlmacen(object sender, RoutedEventArgs e, string idPasado)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement root = xmlDocument.CreateElement("root");

            XmlElement idAlmacen = xmlDocument.CreateElement("idAlmacen");
            idAlmacen.InnerText = envio_idAlmacen.Text;
            root.AppendChild(idAlmacen);

            XmlElement paquete = xmlDocument.CreateElement("paquete");

            XmlElement id = xmlDocument.CreateElement("id");
            id.InnerText = idPasado; //envio_id;
            paquete.AppendChild(id);

            XmlElement estado = xmlDocument.CreateElement("estado");
            estado.InnerText = envio_estado.Text;
            paquete.AppendChild(estado);

            XmlElement descripcion = xmlDocument.CreateElement("descripcion");
            descripcion.InnerText = envio_descripcion.Text;
            paquete.AppendChild(descripcion);

            XmlElement origen = xmlDocument.CreateElement("origen");
            origen.InnerText = envio_origen.Text;
            paquete.AppendChild(origen);

            XmlElement destino = xmlDocument.CreateElement("destino");
            destino.InnerText = envio_destino.Text;
            paquete.AppendChild(destino);

            XmlElement peso = xmlDocument.CreateElement("peso");
            peso.InnerText = envio_peso.Text;
            paquete.AppendChild(peso);

            XmlElement altura = xmlDocument.CreateElement("altura");
            altura.InnerText = envio_altura.Text;
            paquete.AppendChild(altura);

            XmlElement anchura = xmlDocument.CreateElement("anchura");
            anchura.InnerText = envio_anchura.Text;
            paquete.AppendChild(anchura);

            XmlElement longitud = xmlDocument.CreateElement("longitud");
            longitud.InnerText = envio_longitud.Text;
            paquete.AppendChild(longitud);

            root.AppendChild(paquete);

            xmlDocument.AppendChild(root);
            string xmlString = xmlDocument.OuterXml;

            using (var client = new HttpClient())
            {
                string url = "http://localhost:9096/Almacen";
                var content = new StringContent(xmlString, Encoding.UTF8, "application/xml");
                var response = await client.PostAsync(url, content);
                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: "+responseString);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseString);

                XmlNode res = doc.DocumentElement;
                String print = "";
                foreach (XmlNode node in res.ChildNodes)
                {
                    if (node.Name == "result")
                    {
                        if (node.InnerText == "true")
                            print += "Entrada al almacen correcta." + "\n";
                    }
                    Console.WriteLine($"{node.Name}: {node.InnerText}");
                    print+= node.Name+": "+node.InnerText+"\n";
                }
                EstadoAlmacen.Text = print;
            }
        }

        private async void SalidaAlmacen(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement root = xmlDocument.CreateElement("root");

            XmlElement idPaquete = xmlDocument.CreateElement("idPaquete");
            idPaquete.InnerText = Id_envio.Text;
            root.AppendChild(idPaquete);

            XmlElement idAlmacen = xmlDocument.CreateElement("idAlmacen");
            idAlmacen.InnerText = envio_idAlmacen.Text;
            root.AppendChild(idAlmacen);

            XmlElement idRepartidor = xmlDocument.CreateElement("idRepartidor");
            idRepartidor.InnerText = envio_idRepartidor.Text;
            root.AppendChild(idRepartidor);

            XmlElement fechaHora = xmlDocument.CreateElement("fechaHora");
            fechaHora.InnerText = "2023-05-24";
            root.AppendChild(fechaHora);

            xmlDocument.AppendChild(root);
            string xmlString = xmlDocument.OuterXml;

            using (var client = new HttpClient())
            {
                string url = "http://localhost:9097/GestorAlmacen";
                var content = new StringContent(xmlString, Encoding.UTF8, "application/xml");
                var response = await client.PostAsync(url, content);
                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseString);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseString);

                XmlNode res = doc.DocumentElement;
                String print = "";
                foreach (XmlNode node in res.ChildNodes)
                {
                    if (node.Name == "result")
                    {
                        if (node.InnerText == "true")
                            print += "Salida del almacen correcta." + "\n";
                        else
                            print += "Fallo en la salida del almacen." + "\n";
                    }
                    Console.WriteLine($"{node.Name}: {node.InnerText}");
                    print += node.Name + ": " + node.InnerText + "\n";
                }
                EstadoAlmacen.Text = print;
            }
        }

        private async void CrearEnvio(object sender, RoutedEventArgs e)
        {
            var data = new
            {
                descripcion = envio_descripcion.Text,
                origen = envio_origen.Text,
                destino = envio_destino.Text,
                peso = envio_peso.Text,
                altura = envio_altura.Text,
                anchura = envio_anchura.Text,
                longitud = envio_longitud.Text,
                importancia = envio_importancia.Text,
                origenCp = envio_CP_origen.Text,
                destinoCp = envio_CP_destino.Text,
                recogerDomisilio = envio_domisilio.Text
            };

            string jsonString = JsonConvert.SerializeObject(data);

            using (var client = new HttpClient())
            {
                string url = "http://localhost:9094/registrarPaquete";
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseString);

                Estado.Text = responseString;
                Id_envio.Text = responseString;
                glob = responseString;
            }


        }

        private async void ConfirmarRecepcion(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Realiza una petición GET a una URL específica
                    HttpResponseMessage response = await client.GetAsync("http://localhost:9094/confirmarRecepcion/" + Id_envio.Text);

                    // Verifica si la petición fue exitosa (código de respuesta 200-299)

                    // Lee el contenido de la respuesta como una cadena de texto
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Estado.Text = responseBody;
                    if (glob!=null && !glob.Contains("Esta direccion esta fuera de la area de cobertura"))
                    {
                        RegistrarAlmacen(sender, e, glob);

                    }

                }
                catch (Exception ex)
                {
                    Response.Text = "Error";
                    Console.WriteLine("Error en la petición: " + ex.Message);
                }
            }
        }

        private async void ActualizarFecha(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Realiza una petición GET a una URL específic
                    HttpResponseMessage response = await client.GetAsync("http://localhost:9094/actualizarFecha/" + Id_envio.Text);

                    // Verifica si la petición fue exitosa (código de respuesta 200-299)

                    // Lee el contenido de la respuesta como una cadena de texto
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Estado.Text = responseBody;

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
