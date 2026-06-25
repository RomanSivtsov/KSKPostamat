using Microsoft.Web.WebView2.Core;
using System;
using System.Drawing; // Нужно для Size
using System.IO;
using System.Windows.Forms;

namespace Terminal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetupWindow(); // Установка параметров окна
            InitWebView();
        }

        private void SetupWindow()
        {
            // Устанавливаем заголовок
            this.Text = "KSK Postamat Configuration Terminal v1.0";

            // Фиксируем разрешение (объекты больше не будут "съезжать")
            this.ClientSize = new Size(1024, 800);

            // Запрещаем изменять размер окна
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false; // Отключаем кнопку развертывания

            // Центрируем окно при запуске
            this.StartPosition = FormStartPosition.CenterScreen;

            // Растягиваем WebView2 на всё окно
            webView21.Dock = DockStyle.Fill;
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void webView21_Click(object sender, EventArgs e)
        {
            // Оставляем пустым, это нужно только чтобы убрать ошибку дизайнера
        }

        async void InitWebView()
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);
                string htmlPath = Path.Combine(Application.StartupPath, "wwwroot", "index.html");

                if (File.Exists(htmlPath))
                {
                    webView21.CoreWebView2.Navigate(new Uri(htmlPath).AbsoluteUri);
                }
                else
                {
                    MessageBox.Show($"Файл не найден: {htmlPath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}");
            }
        }
    }
}