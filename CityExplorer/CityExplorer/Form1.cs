using System.Drawing;

namespace CityExplorer
{
    public partial class Form1 : Form
    {
        private Button btnRegister;
        private Button btnSignIn;
        public Form1()
        {
            InitializeComponent();
            // Ustawienie okna na pe³ny ekran z ramk¹
            this.WindowState = FormWindowState.Maximized;  // Maksymalizuje okno
            this.BackgroundImageLayout = ImageLayout.Zoom; //dopasuj rozmiar obrazu do okna

            // Tworzenie przycisków
            btnRegister = new Button();
            btnRegister.Text = "Zarejestruj";
            btnRegister.Size = new Size(120, 40);

            btnSignIn = new Button();
            btnSignIn.Text = "Zaloguj";
            btnSignIn.Size = new Size(120, 40);

            // Dodanie przycisków do formularza
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnSignIn);

            // Zdarzenie klikniêcia przycisku logowanie
            btnSignIn.Click += (sender, e) =>
            {
                Form2 form2 = new Form2();
                form2.Show();
                this.Hide();
            };

            // Zdarzenie klikniêcia przycisku rejestracja
            btnRegister.Click += (sender, e) =>
            {
                Form3 form3 = new Form3();
                form3.Show();
                this.Hide();
            };

            this.Resize += MainForm_Resize;

            PositionButtons();
        }

        // Ustawienie przycisków
        private void PositionButtons()
        {
            // Obliczanie œrodkowego punktu
            int totalWidth = btnRegister.Width + btnSignIn.Width + 20; // Szerokoœæ obu przycisków i odstêp (20 pikseli)
            int x = (this.ClientSize.Width - totalWidth) / 2; // Pozycja X, aby oba przyciski by³y wyœrodkowane
            int y = (this.ClientSize.Height - btnRegister.Height) / 2 + 100; // Pozycja Y, aby przyciski by³y 100px ni¿ej

            // Lokalizacja przycisków
            btnRegister.Location = new Point(x, y);
            btnSignIn.Location = new Point(x + btnRegister.Width + 20, y); // Przycisk 2 jest 20 pikseli na prawo od btnRegister
        }

        // Zdarzenie wywo³ywane przy zmianie rozmiaru okna
        private void MainForm_Resize(object sender, EventArgs e)
        {
            PositionButtons(); // Przemieszczenie przycisków przy ka¿dej zmianie rozmiaru okna
        }



        // Rysowanie prostok¹ta
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Wymiary formularza
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // Wymiary prostok¹ta
            int rectWidth = 700;  // Szerokoœæ prostok¹ta
            int rectHeight = 400; // Wysokoœæ prostok¹ta

            // Pozycja prostok¹ta na œrodku ekranu
            int x = (formWidth - rectWidth) / 2;
            int y = (formHeight - rectHeight) / 2;

            // Przezroczystoœæ
            Color transparentWhite = Color.FromArgb(160, 255, 255, 255);  // 30% przezroczystoœci (0-255)
            using (Brush brush = new SolidBrush(transparentWhite))
            {
                e.Graphics.FillRectangle(brush, x, y, rectWidth, rectHeight);
            }

            // Czcionki
            Font boldFont = new Font("Arial", 22, FontStyle.Bold);
            Font normalFont = new Font("Arial", 14);

            // Teksty
            string powitanie = "Dobrze Ciê widzieæ w naszej aplikacji!";
            string tekst = "\nZbieraj wspomnienia z ka¿dego miejsca, które odwiedzasz.\nNasza aplikacja pozwoli Ci zapisaæ ulubione zak¹tki, odkrywaæ nowe\ni tworzyæ swoj¹ w³asn¹ mapê wspomnieñ.\n\nGotowy na niezapomnian¹ podró¿?";

            // Wyœrodkowanie tekstu
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;  // Wyœrodkowanie w poziomie

            // Rysowanie tekstu
            e.Graphics.DrawString(powitanie, boldFont, Brushes.Black, new RectangleF(x, y + 40, rectWidth, rectHeight), stringFormat);
            e.Graphics.DrawString(tekst, normalFont, Brushes.Black, new RectangleF(x, y + 80, rectWidth, rectHeight), stringFormat);
        }

    }
    
}
