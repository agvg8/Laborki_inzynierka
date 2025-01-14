using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Realms;

namespace CityExplorer
{
    public partial class Form3 : Form
    {
        private List<TextBox> textBoxes;
        private List<Label> labels;
        private Realm realm;

        // Deklaracja przycisków
        private Button btnSignUp;
        private Button btnBack;

        public Form3()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; // Maksymalizowanie okna
            this.BackgroundImageLayout = ImageLayout.Zoom;

            // Ścieżka do bazy danych
            string databasePath = @"E:\PROJEKT\CityExplorer\CityExplorer\myrealm.realm";
            realm = Realm.GetInstance(new RealmConfiguration(databasePath));

            textBoxes = new List<TextBox>();
            labels = new List<Label>();

            // Tworzenie przycisków
            btnSignUp = new Button
            {
                Text = "Zarejestruj",
                Size = new Size(120, 40)
            };
            btnSignUp.Click += BtnSignUp_Click;

            btnBack = new Button
            {
                Text = "Powrót",
                Size = new Size(120, 40)
            };
            btnBack.Click += (sender, e) =>
            {
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            };

            this.Controls.Add(btnSignUp);
            this.Controls.Add(btnBack);

            // Rejestracja zdarzenia Resize
            this.Resize += MainForm_Resize;

            AddTextBoxesAndLabels(); // Dodanie kontrolek przy inicjalizacji
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Wymiary formularza
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // Wymiary prostokąta
            int rectWidth = formWidth - 1200; // Szerokość prostokąta
            int rectHeight = formHeight - 400; // Wysokość prostokąta

            // Pozycja prostokąta na środku ekranu
            int x = (formWidth - rectWidth) / 2;
            int y = (formHeight - rectHeight) / 2;

            // Przezroczystość
            Color transparentWhite = Color.FromArgb(160, 255, 255, 255); // 30% przezroczystości
            using (Brush brush = new SolidBrush(transparentWhite))
            {
                e.Graphics.FillRectangle(brush, x, y, rectWidth, rectHeight);
            }

            // Tekst "Rejestracja"
            string text = "Rejestracja";
            Font font = new Font("Arial", 20, FontStyle.Bold);
            SizeF textSize = e.Graphics.MeasureString(text, font);
            int textX = x + (rectWidth - (int)textSize.Width) / 2;
            int textY = y + 40;

            e.Graphics.DrawString(text, font, Brushes.Black, new PointF(textX, textY));

            // Pozycjonowanie przycisków
            PositionButtons();
        }

        private void AddTextBoxesAndLabels()
        {
            // Usuwanie wcześniej wygenerowanych kontrolek
            foreach (var txtBox in textBoxes)
                this.Controls.Remove(txtBox);
            foreach (var lbl in labels)
                this.Controls.Remove(lbl);

            // Czyszczenie list z kontrolek
            textBoxes.Clear();
            labels.Clear();

            string[] labelsText = { "Nazwa użytkownika:", "Imię:", "Nazwisko:", "Hasło:", "Narodowość:", "Miasto:" };
            int startX = this.ClientSize.Width / 2 - 200; // Pozycja X dla kontrolek
            int startY = this.ClientSize.Height / 2 - 150; // Pozycja Y dla kontrolek, żeby wyśrodkować je w pionie

            // Obliczamy, jak rozmieszczać kontrolki
            for (int i = 0; i < labelsText.Length; i++)
            {
                // Dodanie etykiety
                Label lbl = new Label
                {
                    Text = labelsText[i],
                    Location = new Point(startX, startY + (i * 60)), // Rozmieszczanie w pionie
                    AutoSize = true
                };
                this.Controls.Add(lbl);
                labels.Add(lbl);

                // Dodanie TextBoxa
                TextBox txtBox = new TextBox
                {
                    Name = $"txtBox{i}",
                    Location = new Point(startX + 150, startY + (i * 60)), // Rozmieszczanie w pionie
                    Width = 200
                };
                if (i == 3) // Hasło
                    txtBox.UseSystemPasswordChar = true; // Ukrywanie hasła

                this.Controls.Add(txtBox);
                textBoxes.Add(txtBox);
            }

            // Pozycjonowanie przycisków
            PositionButtons();
        }

        private void PositionButtons()
        {
            // Obliczanie środkowego punktu dla przycisków
            int totalWidth = btnSignUp.Width + btnBack.Width + 20; // Szerokość obu przycisków i odstęp (20 pikseli)
            int x = (this.ClientSize.Width - totalWidth) / 2; // Pozycja X, aby oba przyciski były wyśrodkowane
            int y = this.ClientSize.Height / 2 + 220; // Pozycja Y, przyciski poniżej formularza, ale wciąż wyśrodkowane

            btnSignUp.Location = new Point(x, y);
            btnBack.Location = new Point(x + btnSignUp.Width + 20, y); // Przycisk „Powrót” obok „Zarejestruj”
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Dynamiczne pozycjonowanie przycisków przy każdej zmianie rozmiaru okna
            PositionButtons();

            // Ponowne wyśrodkowanie formularza i kontrolek przy zmianie rozmiaru okna
            AddTextBoxesAndLabels();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Zmieniamy hasło na tablicę bajtów
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Przekształcamy tablicę bajtów na ciąg szesnastkowy
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            // Sprawdzenie, czy wszystkie pola tekstowe są wypełnione
            foreach (var txtBox in textBoxes)
            {
                if (string.IsNullOrWhiteSpace(txtBox.Text))
                {
                    MessageBox.Show("Wszystkie pola muszą być wypełnione.");
                    return;
                }
            }

            // Sprawdzenie, czy nazwa użytkownika istnieje w bazie
            string username = textBoxes[0].Text;
            var existingUser = realm.All<User>().FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                MessageBox.Show("Nazwa użytkownika jest zajęta. Proszę wybrać inną.");
                return;
            }

            // Haszowanie hasła przed zapisaniem
            string hashedPassword = HashPassword(textBoxes[3].Text);

            // Tworzenie nowego użytkownika
            realm.Write(() =>
            {
                var newUser = new User
                {
                    Username = textBoxes[0].Text,
                    FirstName = textBoxes[1].Text,
                    LastName = textBoxes[2].Text,
                    Password = hashedPassword, // Zapisujemy zahashowane hasło
                    Nationality = textBoxes[4].Text,
                    City = textBoxes[5].Text
                };
                realm.Add(newUser);
            });

            // Wyświetlenie komunikatu o sukcesie
            MessageBox.Show("Rejestracja zakończona sukcesem.");

            // Przekierowanie do Form2 po rejestracji
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();  // Ukrywanie obecnego formularza (Form3)
        }
    }
}