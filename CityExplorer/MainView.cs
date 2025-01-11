using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace CityExplorer
{
    public class MainView
    {
        private readonly string text;
        private readonly Font font;
        private readonly Color rectangleColor;
        private readonly Color textColor;
        private readonly int rectMarginX;
        private readonly int rectMarginY;

        public MainView(Font font, Color rectangleColor, Color textColor, int rectMarginX, int rectMarginY)
        {
            this.font = font;
            this.rectangleColor = rectangleColor;
            this.textColor = textColor;
            this.rectMarginX = rectMarginX;
            this.rectMarginY = rectMarginY;
        }

        public void Paint(Graphics graphics, int formWidth, int formHeight)
        {
            // Obliczanie wymiarów głównego prostokąta
            int rectWidth = formWidth - (2 * rectMarginX);
            int rectHeight = formHeight - (2 * rectMarginY);

            // Pozycja głównego prostokąta
            int x = rectMarginX;
            int y = rectMarginY;

            // Rysowanie głównego prostokąta
            using (Brush brush = new SolidBrush(rectangleColor))
            {
                graphics.FillRectangle(brush, x, y, rectWidth, rectHeight);
            }

            // Rysowanie tekstu w głównym prostokącie
            using (Brush textBrush = new SolidBrush(textColor))
            {
                graphics.DrawString(text, font, textBrush, x + 40, y + 40);
            }

            // Dodanie okna wyszukiwania
            DrawSearchBox(graphics, formWidth, formHeight);
        }

        private void DrawSearchBox(Graphics graphics, int formWidth, int formHeight)
        {
            // Wymiary i pozycja okna wyszukiwania
            int searchBoxWidth = formWidth - (2 * rectMarginX) -30;  // Okno wyszukiwania zajmuje połowę szerokości formularza
            int searchBoxHeight = 50;           // Wysokość okna wyszukiwania
            int searchBoxX = (formWidth - searchBoxWidth) / 2; // Wyśrodkowanie na osi X
            int searchBoxY = rectMarginY + 20;  // Umieszczenie okna wyszukiwania poniżej głównego prostokąta

            // Kolor okna wyszukiwania
            Color searchBoxColor = Color.FromArgb(200, 230, 230, 230); // Delikatnie szary z przezroczystością
            using (Brush brush = new SolidBrush(searchBoxColor))
            {
                graphics.FillRectangle(brush, searchBoxX, searchBoxY, searchBoxWidth, searchBoxHeight);
            }

            // Rysowanie obramowania okna wyszukiwania
            using (Pen pen = new Pen(Color.Gray, 2))
            {
                graphics.DrawRectangle(pen, searchBoxX, searchBoxY, searchBoxWidth, searchBoxHeight);
            }

            // Opcjonalnie: Rysowanie przykładowego tekstu w polu wyszukiwania
            string placeholder = "Wpisz, aby wyszukać...";
            using (Brush placeholderBrush = new SolidBrush(Color.DarkGray))
            {
                Font placeholderFont = new Font("Arial", 12, FontStyle.Italic);
                graphics.DrawString(placeholder, placeholderFont, placeholderBrush, searchBoxX + 10, searchBoxY + 15);
            }
        }
    }
}
