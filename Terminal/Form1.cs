using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows.Forms;

namespace Terminal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitWebView();
        }

        // Этот метод нужен, так как на него ссылается дизайнер
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        async void InitWebView()
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);

                // Путь к папке с HTML
                string htmlPath = Path.Combine(Application.StartupPath, "wwwroot", "index.html");

                if (File.Exists(htmlPath))
                {
                    webView21.CoreWebView2.Navigate(new Uri(htmlPath).AbsoluteUri);
                }
                else
                {
                    // Если файла нет в bin/Debug, выведем понятную ошибку
                    MessageBox.Show($"Файл не найден: {htmlPath}\n\nУбедитесь, что в свойствах файла index.html стоит 'Копировать в выходной каталог'!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}");
            }
        }
    }
}