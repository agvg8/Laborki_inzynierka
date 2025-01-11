using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;
using Button = System.Windows.Forms.Button;

namespace CityExplorer
{
    public partial class Form3 : Form
    {

        // Lista kontrolki Label i TextBox
        private List<Label> labels;
        private List<TextBox> textBoxes;

        // Tworzenie przycisków
        private Button btnSignUp;
        private Button btnBack;

        public Form3()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;  // Maksymalizuje okno
            this.BackgroundImageLayout = ImageLayout.Zoom; // Dopasowanie obrazu do okna

            // Inicjalizacja list
            labels = new List<Label>();
            textBoxes = new List<TextBox>();

            // Tworzenie etykiet i TextBox
            CreateFields();

            // Tworzenie przycisków
            btnSignUp = new Button();
            btnSignUp.Text = "Zarejestruj";
            btnSignUp.Size = new Size(120, 40);
            btnSignUp.Click += btnSignUp_Click;

            btnBack = new Button();
            btnBack.Text = "Powrót";
            btnBack.Size = new Size(120, 40);

            // Dodanie przycisków do formularza
            this.Controls.Add(btnSignUp);
            this.Controls.Add(btnBack);

            // Pozycjonowanie przycisków
            PositionButtons();

            // Zdarzenie kliknięcia przycisku powrotu
            btnBack.Click += (sender, e) =>
            {
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            };

            // Rejestracja zdarzenia Resize
            this.Resize += MainForm_Resize;
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            // Logika rejestracji - np. zapisz dane do bazy, pliku itp.

            // Powiadomienie o poprawnej rejestracji
            DialogResult result = MessageBox.Show("Rejestracja zakończona pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                // Po kliknięciu "OK" przenosimy do Form2
                Form2 form2 = new Form2();
                form2.Show();  // Otwórz Form2
                this.Hide();   // Ukryj obecne okno (Form3)
            }
        }

        // Ustawienie przycisków
        private void PositionButtons()
        {
            // Obliczanie środkowego punktu
            int totalWidth = btnSignUp.Width + btnBack.Width + 20; // Szerokość obu przycisków i odstęp (20 pikseli)
            int x = (this.ClientSize.Width - totalWidth) / 2; // Pozycja X, aby oba przyciski były wyśrodkowane
            int y = (this.ClientSize.Height - btnBack.Height) / 2 + 250; // Pozycja Y, aby przyciski były 100px niżej

            // Lokalizacja przycisków
            btnSignUp.Location = new Point(x, y);
            btnBack.Location = new Point(x + btnSignUp.Width + 20, y); // Przycisk 2 jest 20 pikseli na prawo od btnSignUp
        }

        // Zdarzenie wywoływane przy zmianie rozmiaru okna
        private void MainForm_Resize(object sender, EventArgs e)
        {
            PositionButtons(); // Przemieszczenie przycisków przy każdej zmianie rozmiaru okna
        }

        private void CreateFields()
        {
            // Teksty do pól
            string[] fieldNames = new string[]
            {
                "Nazwa użytkownika:",
                "Imię:",
                "Nazwisko:",
                "Hasło:",
                "Narodowość:",
                "Miasto:"
            };

            int currentYPosition = 100;  // Pozycja początkowa w pionie
            int xPosition = 40;          // Pozycja początkowa w poziomie
            int textBoxWidth = 600;      // Szerokość TextBox
            int labelHeight = 20;        // Wysokość etykiety
            int fieldHeight = 30;        // Wysokość TextBoxa
            int verticalSpacing = 20;    // Odstęp między polami

            for (int i = 0; i < fieldNames.Length; i++)
            {
                // Tworzymy etykiety
                Label label = new Label();
                label.Text = fieldNames[i];
                label.Location = new Point(xPosition, currentYPosition);
                label.Size = new Size(textBoxWidth, labelHeight);
                label.BackColor = Color.Transparent;
                this.Controls.Add(label);
                labels.Add(label);

                // Tworzymy TextBox
                TextBox textBox = new TextBox();
                textBox.Width = textBoxWidth;
                textBox.Location = new Point(xPosition, currentYPosition + labelHeight + 5); // TextBox 5px poniżej etykiety
                this.Controls.Add(textBox);
                textBoxes.Add(textBox);

                // Zwiększamy pozycję Y dla następnego pola
                currentYPosition += labelHeight + fieldHeight + verticalSpacing;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Wymiary formularza
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // Wymiary prostokąta
            int rectWidth = formWidth - 1200;             // Zmieniamy szerokość na szerokość formularza minus 10px
            int rectHeight = formHeight - 400;            // Wysokość prostokąta pozostaje bez zmian

            // Pozycja prostokąta na środku ekranu
            int x = (formWidth - rectWidth) / 2;
            int y = (formHeight - rectHeight) / 2;

            // Przezroczystość
            Color transparentWhite = Color.FromArgb(160, 255, 255, 255);  // 30% przezroczystości (0-255)
            using (Brush brush = new SolidBrush(transparentWhite))
            {
                e.Graphics.FillRectangle(brush, x, y, rectWidth, rectHeight);
            }

            // Tekst
            string text = "Rejestracja";

            e.Graphics.DrawString(text, new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new Point(x + 40, y + 40));

        }

        // Jeśli chcesz, aby kontrolka TextBox była wyświetlana w tym samym czasie, dodaj jej odpowiednią pozycję w konstruktorze
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // Pozycjonowanie TextBoxa tuż poniżej napisu "Wprowadź dane"
            int xPosition = (formWidth - (formWidth - 1200)) / 2 + 40; // Tak, aby TextBox był wewnątrz prostokąta (margines)
            int textBoxWidth = 600;      // Szerokość TextBoxa
            int verticalSpacing = 20;    // Odstęp między polami (etykieta + TextBox)

            int currentYPosition = (formHeight - (formHeight - 400)) / 2 + 100 + 20; // Tekst "Wprowadź dane" znajduje się na y + 70, więc dodajemy 20px, aby TextBox był poniżej

            // Upewnij się, że listy 'labels' i 'textBoxes' zostały zainicjowane
            if (labels == null || textBoxes == null)
            {
                return;  // Zatrzymaj metodę, jeśli listy są niezainicjowane
            }

            // Iterujemy po wszystkich etykietach i TextBoxach
            for (int i = 0; i < labels.Count; i++)
            {
                // Ustawienie pozycji dla Label
                labels[i].Location = new Point(xPosition, currentYPosition);
                labels[i].Width = textBoxWidth;

                // Ustawienie pozycji dla TextBox
                textBoxes[i].Location = new Point(xPosition, currentYPosition + labels[i].Height + 5); // TextBox 5px poniżej etykiety
                textBoxes[i].Width = textBoxWidth;

                // Zwiększamy pozycję Y dla następnego pola
                currentYPosition += labels[i].Height + textBoxes[i].Height + verticalSpacing;
            }
        }
    }
}
