using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace CityExplorer
{
    public partial class Form4 : Form
    {
        private Panel sideMenu;
        private SideMenuManager sideMenuManager;

        public Form4()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;  // Maksymalizuje okno
            this.BackgroundImageLayout = ImageLayout.Zoom; // Dopasowuje rozmiar obrazu do okna

            InitializeSideMenu();
        }
        //dodanie menu bocznego
        private void InitializeSideMenu()
        {
            // Tworzenie panelu bocznego
            sideMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250, // Szerokość początkowa panelu
                BackColor = Color.LightGray
            };

            this.Controls.Add(sideMenu);
           
            // Tworzenie menedżera menu bocznego
            sideMenuManager = new SideMenuManager(sideMenu);
        }
        // Dodanie środkowego panelu
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Utworzenie obiektu klasy MainView
            var mainView = new MainView(
                font: new Font("Arial", 20, FontStyle.Bold),
                rectangleColor: Color.FromArgb(160, 255, 255, 255), // 30% przezroczystości
                textColor: Color.Black,
                rectMarginX: 300,
                rectMarginY: 5
            );

            // Rysowanie prostokąta i tekstu
            mainView.Paint(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
        }

    }
}
