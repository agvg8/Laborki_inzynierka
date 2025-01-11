using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using TextBox = System.Windows.Forms.TextBox;

namespace CityExplorer
{
    public partial class Form2 : Form
    {
        private Button btnLogIn;
        private Button btnBack;

        private TextBox tbLogin;
        private TextBox tbPssw;

        private Label lblLogin;
        private Label lblPassword;

        public Form2()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;  // Maksymalizuje okno
            this.BackgroundImageLayout = ImageLayout.Zoom; //dopasuj rozmiar obrazu do okna


            //* ------------------------------ PRZYCISKI ------------------------------ *//
            btnLogIn = new Button
            {
                Text = "Zaloguj",
                Size = new Size(120, 40)
            };

            btnBack = new Button
            {
                Text = "Powrót",
                Size = new Size(120, 40)
            };

            this.Controls.Add(btnLogIn);
            this.Controls.Add(btnBack);

            btnLogIn.Click += (sender, e) =>
            {
                
                Form4 form4 = new Form4();
                form4.Show();
                this.Hide();
            };

            btnBack.Click += (sender, e) =>
            {
                Form1 form1 = new Form1(); // Powrót do głównego formularza
                form1.Show();
                this.Hide();
            };

            //* ------------------------------ TEXTBOXY ------------------------------ *//
            tbLogin = new TextBox
            {
                Width = 300,
                Height = 50,
                PlaceholderText = "Wpisz login" // Opcjonalnie: tekst pomocniczy
            };

            tbPssw = new TextBox
            {
                Width = 300,
                Height = 50,
                PlaceholderText = "Wpisz hasło",
                UseSystemPasswordChar = true // Ukrywa hasło
            };

            this.Controls.Add(tbLogin);
            this.Controls.Add(tbPssw);

            //* ------------------------------ PODPISY ------------------------------ *//
            lblLogin = new Label
            {
                Text = "Login:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true, // Automatycznie dopasowuje rozmiar do tekstu
                BackColor = Color.Transparent //Kolor transparentny
            };

            lblPassword = new Label
            {
                Text = "Hasło:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            this.Controls.Add(lblLogin);
            this.Controls.Add(lblPassword);

            //* ------------------------------ WYDARZENIA ------------------------------ *//
            this.Resize += MainForm_Resize; // Obsługa zmiany rozmiaru okna

            PositionControls();
        }

        //* ------------------------------ CENTRALIZACJA ------------------------------ *//
        private void PositionControls()
        {
            // Wymiary formularza
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Ustawienia textboxów
            tbLogin.Location = new Point(centerX - tbLogin.Width / 2, centerY - tbLogin.Height - 30);
            tbPssw.Location = new Point(centerX - tbPssw.Width / 2, centerY + 10);

            // Ustawienia podpisów nad textboxami
            lblLogin.Location = new Point(tbLogin.Left, tbLogin.Top - lblLogin.Height - 5);
            lblPassword.Location = new Point(tbPssw.Left, tbPssw.Top - lblPassword.Height - 5);


            // Ustawienia przycisków
            btnLogIn.Location = new Point(centerX - btnLogIn.Width - 10, centerY + tbPssw.Height + 50);
            btnBack.Location = new Point(centerX + 10, centerY + tbPssw.Height + 50);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            PositionControls(); // Ponowne ustawienie pozycji kontrolek przy zmianie rozmiaru okna
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Wymiary formularza
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // Wymiary prostokąta
            int rectWidth = 700;
            int rectHeight = 400;

            // Pozycja prostokąta na środku ekranu
            int x = (formWidth - rectWidth) / 2;
            int y = (formHeight - rectHeight) / 2;

            // Przezroczystość
            Color transparentWhite = Color.FromArgb(160, 255, 255, 255); // 30% przezroczystości (0-255)
            using (Brush brush = new SolidBrush(transparentWhite))
            {
                e.Graphics.FillRectangle(brush, x, y, rectWidth, rectHeight);
            }

            Font boldFont = new Font("Arial", 20, FontStyle.Bold);

            // Tekst
            string text = "Logowanie";

            // Wyśrodkowanie tekstu
            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center
            };

            e.Graphics.DrawString(text, boldFont, Brushes.Black, new RectangleF(x, y + 30, rectWidth, rectHeight), stringFormat);
        }
    }
}