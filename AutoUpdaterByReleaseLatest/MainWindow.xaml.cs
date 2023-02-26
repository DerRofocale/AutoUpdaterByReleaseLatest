using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using static AutoUpdaterByReleaseLatest.GitHubJSON;

namespace AutoUpdaterByReleaseLatest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        string CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
        public MainWindow()
        {
            InitializeComponent();
            CurVer_TB.Text = CurrentVersion;
            MessageBox.Show(AppDomain.CurrentDomain.FriendlyName);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient http = new HttpClient())
            {
                if (Internet.OK())
                {
                    try
                    {
                        System.IO.File.Delete("Update.zip");
                    }
                    catch
                    { }
                    http.DefaultRequestHeaders.Add("User-Agent", "SikursApp");
                    var a = await http.GetAsync("https://api.github.com/repos/DerRofocale/AutoUpdaterByReleaseLatest/releases/latest");
                    string curver = a.Content.ReadAsStringAsync().Result;
                    Rootobject? deserializedProduct = JsonConvert.DeserializeObject<Rootobject>(curver);
                    if (Convert.ToDouble(CurrentVersion, CultureInfo.InvariantCulture) == Convert.ToDouble(deserializedProduct.tag_name, CultureInfo.InvariantCulture))
                    {
                        MessageBox.Show("У Вас установлена актуальная версия");
                    }
                    else
                    {
                        MessageBoxResult dialogResult = MessageBox.Show("Хотите обновить?", "ОБНОВЛЕНИЕ", MessageBoxButton.YesNo);
                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            Directory.CreateDirectory("temp");
                            var file = await http.GetStreamAsync(deserializedProduct.assets[0].browser_download_url);
                            using (var fileStream = new FileStream("Update.zip", FileMode.CreateNew))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            //var curPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                            var curPath = AppDomain.CurrentDomain.BaseDirectory;
                            //MessageBox.Show(curPath);
                            System.IO.Compression.ZipFile.ExtractToDirectory("Update.zip", curPath + "\\temp", true);
                            System.IO.File.Delete("Update.zip");
                            CMD($"taskkill /f /im AutoUpdaterByReleaseLatest.exe && timeout /t 1 && move /y {curPath}\\temp\\*.* {curPath} && start AutoUpdaterByReleaseLatest.exe");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Нет подключения к серверу");
                }
            }
        }

        private void CMD(string line)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c {line}",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            });
        }
    }
}
