using System.Drawing;

namespace CityExplorer
{
    public partial class Menu : Form
    {
        private Button btnProjekt;
        private Button btnAdministracja;
        public Menu()
        {
            InitializeComponent();
            // Ustawienie okna na pełny ekran z ramką
            this.WindowState = FormWindowState.Maximized;  // Maksymalizuje okno
            this.BackgroundImageLayout = ImageLayout.Zoom; //dopasuj rozmiar obrazu do okna

            // Tworzenie przycisków
            btnProjekt = new Button();
            btnProjekt.Text = "Projekt";
            btnProjekt.Size = new Size(120, 40);

            btnAdministracja = new Button();
            btnAdministracja.Text = "Administracja";
            btnAdministracja.Size = new Size(120, 40);

            // Dodanie przycisków do formularza
            this.Controls.Add(btnProjekt);
            this.Controls.Add(btnAdministracja);

            // Zdarzenie kliknięcia przycisku Projekt
            btnProjekt.Click += (sender, e) =>
            {
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            };

            // Zdarzenie kliknięcia przycisku Administracja
            btnAdministracja.Click += (sender, e) =>
            {
                Logowanie logowanie = new Logowanie();
                logowanie.Show();
                this.Hide();
            };

            this.Resize += MainForm_Resize;

            PositionButtons();
        }

        // Ustawienie przycisków
        private void PositionButtons()
        {
            // Obliczanie środkowego punktu
            int totalWidth = btnProjekt.Width + btnAdministracja.Width + 20; // Szerokość obu przycisków i odstęp (20 pikseli)
            int x = (this.ClientSize.Width - totalWidth) / 2; // Pozycja X, aby oba przyciski były wyśrodkowane
            int y = (this.ClientSize.Height - btnProjekt.Height) / 2 + 100; // Pozycja Y, aby przyciski były 100px niżej

            // Lokalizacja przycisków
            btnProjekt.Location = new Point(x, y);
            btnAdministracja.Location = new Point(x + btnProjekt.Width + 20, y); // Przycisk 2 jest 20 pikseli na prawo od btnRegister
        }

        // Zdarzenie wywoływane przy zmianie rozmiaru okna
        private void MainForm_Resize(object sender, EventArgs e)
        {
            PositionButtons(); // Przemieszczenie przycisków przy każdej zmianie rozmiaru okna
        }



        // Rysowanie prostokąta
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Wymiary formularza
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // Wymiary prostokąta
            int rectWidth = 700;  // Szerokość prostokąta
            int rectHeight = 400; // Wysokość prostokąta

            // Pozycja prostokąta na środku ekranu
            int x = (formWidth - rectWidth) / 2;
            int y = (formHeight - rectHeight) / 2;

            // Przezroczystość
            Color transparentWhite = Color.FromArgb(160, 255, 255, 255);  // 30% przezroczystości (0-255)
            using (Brush brush = new SolidBrush(transparentWhite))
            {
                e.Graphics.FillRectangle(brush, x, y, rectWidth, rectHeight);
            }

            // Czcionki
            Font boldFont = new Font("Arial", 22, FontStyle.Bold);
            Font normalFont = new Font("Arial", 14);

            // Teksty
            string powitanie = "Dobrze Cię widzieć w naszej aplikacji Adminie!";
            string tekst = "\nCo chcesz zrobić?";

            // Wyśrodkowanie tekstu
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;  // Wyśrodkowanie w poziomie

            // Rysowanie tekstu
            e.Graphics.DrawString(powitanie, boldFont, Brushes.Black, new RectangleF(x, y + 40, rectWidth, rectHeight), stringFormat);
            e.Graphics.DrawString(tekst, normalFont, Brushes.Black, new RectangleF(x, y + 80, rectWidth, rectHeight), stringFormat);
        }

    }

}
