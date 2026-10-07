using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Realms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;

namespace CityExplorer
{
    public partial class Administracja : Form
    {
        private Realm realm;

        // Deklaracja przycisków
        private Button btnClear;
        private Button btnBack;
        private Button btnShowUser;



        public Administracja()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; // Maksymalizowanie okna
            this.BackgroundImageLayout = ImageLayout.Zoom;

            // Ścieżka do bazy danych
            string databasePath = @"E:\PROJEKT\CityExplorer\CityExplorer\myrealm.realm";
            realm = Realm.GetInstance(new RealmConfiguration(databasePath));

            
            

            // Tworzenie przycisków
            btnClear = new Button
            {
                Text = "Wyczyść bazę",
                Size = new Size(200, 60)
            };

            btnClear.Click += BtnClear_Click;

            btnBack = new Button
            {
                Text = "Powrót",
                Size = new Size(200, 60)
            };
            btnBack.Click += (sender, e) =>
            {
                Menu menu = new Menu();
                menu.Show();
                this.Hide();
            };
            btnShowUser = new Button
            {
                Text = "Pokaż użytkowników",
                Size = new Size(200, 60)
            };
            btnShowUser.Click += BtnShowUser;

            this.Controls.Add(btnClear);
            this.Controls.Add(btnBack);
            this.Controls.Add(btnShowUser);

            // Rejestracja zdarzenia Resize
            this.Resize += MainForm_Resize;

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
            string text = "Panel Administracyjny";
            Font font = new Font("Arial", 20, FontStyle.Bold);
            SizeF textSize = e.Graphics.MeasureString(text, font);
            int textX = x + (rectWidth - (int)textSize.Width) / 2;
            int textY = y + 40;

            e.Graphics.DrawString(text, font, Brushes.Black, new PointF(textX, textY));

            // Pozycjonowanie przycisków
            PositionButtons();
        }

        private void PositionButtons()
        {
            // Obliczanie środkowego punktu dla przycisków
            int totalHeight = btnClear.Height + btnShowUser.Height + btnBack.Height + 40; // 20 pikseli odstępu między przyciskami
            int x = (this.ClientSize.Width - btnClear.Width) / 2; // Wyśrodkowanie w poziomie
            int y = (this.ClientSize.Height - totalHeight) / 2; // Wyśrodkowanie w pionie

            btnClear.Location = new Point(x, y);
            btnShowUser.Location = new Point(x, y + btnClear.Height + 20);
            btnBack.Location = new Point(x, y + btnClear.Height + btnBack.Height + 40);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Dynamiczne pozycjonowanie przycisków przy każdej zmianie rozmiaru okna
            PositionButtons();

            // Ponowne wyśrodkowanie formularza i kontrolek przy zmianie rozmiaru okna
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            // Sprawdzamy, czy baza danych jest poprawnie zainicjowana
            if (realm != null)
            {
                // Usuwanie wszystkich obiektów z bazy danych
                realm.Write(() =>
                {
                    // Usuwamy wszystkie obiekty typu User
                    realm.RemoveAll<User>();

                    // Jeżeli masz inne modele (np. City, Country), usuń je również
                    // realm.RemoveAll<OtherModel>();
                });

                MessageBox.Show("Baza danych została wyczyszczona.");
            }
            else
            {
                MessageBox.Show("Baza danych nie została poprawnie zainicjowana.");
            }
        }

        private void BtnShowUser(object sender, EventArgs e)
        {
            // Tworzymy nowe okno do wyświetlania rekordów
            Form recordsForm = new Form
            {
                Text = "Rekordy użytkowników",
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterScreen
            };

            // ListBox do wyświetlania danych
            ListBox listBox = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 12)
            };
            recordsForm.Controls.Add(listBox);

            // Pobieramy dane z bazy
            var users = realm.All<User>().ToList();
            if (users.Any())
            {
                foreach (var user in users)
                {
                    listBox.Items.Add($"Username: {user.Username}, FirstName: {user.FirstName}, LastName: {user.LastName}, City: {user.City}, Nationality: {user.Nationality}");
                }
            }
            else
            {
                listBox.Items.Add("Brak rekordów w bazie danych.");
            }

            // Wyświetlamy nowe okno
            recordsForm.ShowDialog();
        }

    }

}
