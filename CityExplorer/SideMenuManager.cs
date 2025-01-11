using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace CityExplorer
{
    public class SideMenuManager
    {
        private Panel sideMenu;
        private Button[] menuButtons;
        private string[] buttonLabels = { "Strona Główna", "Znajomi", "Dodaj miejsce", "Powiadomienia", "Profil" };
        private Form parentForm;

        public SideMenuManager(Panel sideMenuPanel)
        {
            sideMenu = sideMenuPanel; 
           
            InitializeSideMenu();
        }

        private void InitializeSideMenu()
        {
            // Ustawienie koloru tła jako półprzezroczystego
            sideMenu.Paint += (s, e) =>
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(70, Color.Gray))) // 70 to poziom przezroczystości
                {
                    e.Graphics.FillRectangle(brush, sideMenu.ClientRectangle);
                }
            };

            sideMenu.BackColor = Color.Transparent; // Ustawienie tła panelu jako przezroczystego
            sideMenu.Invalidate(); // Wymuszenie odświeżenia

            // Przycisk Profil
            Button profileButton = new Button
            {
                Text = "                      Profil",
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(255, 65, 105, 225),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ImageAlign = ContentAlignment.MiddleLeft,
                Image = LoadIconFromFile(@"E:\\PROJEKT\\CityExplorer\\CityExplorer\\res\\user.png"),
                Height = sideMenu.Height / 5,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            profileButton.Click += (s, e) =>
            {
                Form8 form8 = new Form8();
                form8.Show();
                var currentForm = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.Contains(sideMenu));
                if (currentForm != null)
                {
                    currentForm.Hide();
                }
            };
            sideMenu.Controls.Add(profileButton);

            // Przycisk 4: Powiadomienia
            Button notiButton = new Button
            {
                Text = "                      Powiadomienia",
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(255, 65, 105, 225),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ImageAlign = ContentAlignment.MiddleLeft,
                Image = LoadIconFromFile(@"E:\\PROJEKT\\CityExplorer\\CityExplorer\\res\\notification.png"),
                Height = sideMenu.Height / 5,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            notiButton.Click += (s, e) =>
            {
                Form7 form7 = new Form7();
                form7.Show();
                var currentForm = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.Contains(sideMenu));
                if (currentForm != null)
                {
                    currentForm.Hide();
                }
            };
            sideMenu.Controls.Add(notiButton);

            // Przycisk 3: Dodaj miejsce
            Button addButton = new Button
            {
                Text = "                      Dodaj miejsce",
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(255, 65, 105, 225),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ImageAlign = ContentAlignment.MiddleLeft,
                Image = LoadIconFromFile(@"E:\\PROJEKT\\CityExplorer\\CityExplorer\\res\\add.png"),
                Height = sideMenu.Height / 5,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            addButton.Click += (s, e) =>
            {
                Form6 form6 = new Form6();
                form6.Show();
                var currentForm = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.Contains(sideMenu));
                if (currentForm != null)
                {
                    currentForm.Hide();
                }
            };
            sideMenu.Controls.Add(addButton);
            

            // Przycisk 2: Znajomi
            Button friendsButton = new Button
            {
                Text = "                      Znajomi",
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(255, 65, 105, 225),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ImageAlign = ContentAlignment.MiddleLeft,
                Image = LoadIconFromFile(@"E:\\PROJEKT\\CityExplorer\\CityExplorer\\res\\friends.png"),
                Height = sideMenu.Height / 5,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            friendsButton.Click += (s, e) =>
            {
                Form5 form5 = new Form5();
                form5.Show();
                var currentForm = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.Contains(sideMenu));
                if (currentForm != null)
                {
                    currentForm.Hide();
                }
            };
            sideMenu.Controls.Add(friendsButton);

            // Przycisk  Strona Główna
            Button homePage = new Button
            {
                Text = "                      Strona Główna",
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(255, 65, 105, 225),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ImageAlign = ContentAlignment.MiddleLeft,
                Image = LoadIconFromFile(@"E:\\PROJEKT\\CityExplorer\\CityExplorer\\res\\home.png"),
                Height = sideMenu.Height / 5,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            homePage.Click += (s, e) =>
            {
                Form4 form4 = new Form4();
                form4.Show();
                var currentForm = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.Contains(sideMenu));
                if (currentForm != null)
                {
                    currentForm.Hide();
                }
            };
            sideMenu.Controls.Add(homePage);

            // Obsługa zdarzenia zmiany rozmiaru
            sideMenu.Resize += (s, e) => AdjustButtonHeights(homePage, friendsButton, addButton, notiButton, profileButton);


        }



        private void AdjustButtonHeights(params Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Height = sideMenu.Height / buttons.Length;
            }
        }

        private Image LoadIconFromFile(string filePath)
        {
            // Wczytanie ikony z pliku
            Image icon = Image.FromFile(filePath);
            // Opcjonalnie, możesz dodać kod, aby dostosować rozmiar ikony
            return new Bitmap(icon, new Size(60, 60)); // Skalowanie do 60x60 px
        }
    }
}