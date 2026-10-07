import tkinter as tk
from tkinter import ttk, StringVar, messagebox
from PIL import Image, ImageTk
from tkinter import StringVar
import joblib  # Do wczytywania modelu
import os
import pandas as pd

# ===== Wczytywanie modelu ===== #
try:
    model = joblib.load("vacation_model.pkl")  # Wczytaj model zapisany wcześniej
except Exception as e:
    messagebox.showerror("Błąd", f"Nie udało się wczytać modelu: {e}")
    model = None

month_to_number = {
    'January': 4,
    'February': 3,
    'March': 7,
    'April': 0,
    'May': 8,
    'June': 6,
    'July': 5,
    'August': 1,
    'September': 11,
    'October': 10,
    'November': 9,
    'December': 2
}

# konwersja miesiaca na numer
def get_month_number(month):
    return month_to_number.get(month, None)

def predict_days():
    if not model:
        result_label.config(text="Brak modelu do predykcji")
        return

    try:
        # Pobieramy dane wejściowe
        age_value = int(age_var.get())  # Wiek
        month_value = month_var.get()  # Miesiąc

        # Konwertujemy miesiąc na numer
        month_number = get_month_number(month_value)

        if month_number is None:
            messagebox.showerror("Błąd", "Nieprawidłowe miasto")
            return

        # Tworzymy DataFrame, aby zachować tę samą strukturę, co przy treningu
        input_data = pd.DataFrame([[age_value, month_number]], columns=["age", "MonthOfTravellingLabelled"])

        # Przewidywanie
        prediction_number = model.predict(input_data)[0]  # Przewidywany numer hotelu

        result_label.config(text=f"Szacowana długość pobytu: {prediction_number}")  # Wyświetlamy wynik
    except Exception as e:
        messagebox.showerror("Błąd", f"Wystąpił problem z predykcją: {e}")


path_to_project = os.path.dirname(__file__)
path_to_image = os.path.join(path_to_project, "images", "background.jpg")

def run(window):
    for widget in window.winfo_children():  # Czyści ramkę przed załadowaniem nowych widżetów
        widget.destroy()

    # ===== Nagłówek ===== #
    label_title = tk.Label(
        window, text="Sprawdź, jak długo twoi równieśnicy są na wakacjach!",
        font=("Helvetica", 14), bg="white", fg="black", padx=5, pady=5
    )
    label_title.pack(pady=15)

    label_title = tk.Label(
        window, text="Podaj swój wiek",
        font=("Helvetica", 12), bg="white", fg="black"
    )
    label_title.pack(pady=5)

    # ===== Wybór wieku ===== #
    global age_var  # Używamy globalnej zmiennej, by nie znikała
    age_var = StringVar()
    age_options = [str(i) for i in range(15, 100)]  # Zakres wieku

    dropdown_age = ttk.Combobox(
        window, textvariable=age_var, values=age_options,
        font=("Helvetica", 12), state="readonly", width=5, justify="center", height=4
    )
    dropdown_age.set("25")  # Domyślne ustawienie wieku
    dropdown_age.pack(pady=10)

    label_title = tk.Label(
        window, text="Wybierz miesiąc pobytu",
        font=("Helvetica", 12), bg="white", fg="black"
    )
    label_title.pack(pady=5)

    # ===== Wybór miejsca hotelu ===== #
    global month_var  # Używamy globalnej zmiennej dla miejsca hotelu
    month_var = StringVar()
    month_options = ['January', 'February', 'March',
                     'April', 'May', 'June',
                     'July', 'August', 'September', 'October', 'November', 'December']

    dropdown_month = ttk.Combobox(
        window, textvariable=month_var, values=month_options,
        font=("Helvetica", 12), state="readonly", width=20, justify="center", height=4
    )
    dropdown_month.set("Wybierz miesiąc")  # Domyślne ustawienie miejsca
    dropdown_month.pack(pady=10)

    # ===== Przycisk do predykcji ===== #
    button = ttk.Button(window, text="Sprawdź", command=predict_days, style="TButton")
    button.pack(pady=20)

    # ===== Miejsce na wynik ===== #
    global result_label
    result_label = tk.Label(
        window, text="", font=("Helvetica", 12), bg="white", fg="black"
    )
    result_label.pack(pady=15)

    # ===== Style dla przycisku ===== #
    style = ttk.Style(window)
    style.configure("TButton",
                    font=("Helvetica", 12),
                    background="#D3D3D3",
                    foreground="black",
                    borderwidth=1,
                    relief="flat",
                    width=20,
                    anchor="center")
    style.map("TButton", background=[("active", "#A9A9A9")])  # Zmiana koloru przycisku przy aktywności

    # Zastosowanie zaokrąglenia na przyciskach
    style.configure("TButton", borderwidth=1, relief="flat", anchor="center")
    window.option_add("*TButton*padding", [8, 5])  # Padding dla przycisków (wewnętrzne marginesy)

    # Zastosowanie lekkich cieni i zaokrągleń dla innych elementów
    window.config(bg="white")  # Białe tło
    dropdown_age.config(style="Custom.TCombobox")
    dropdown_month.config(style="Custom.TCombobox")

    style.configure("Custom.TCombobox",
                    fieldbackground="white",
                    background="#f0f0f0",
                    arrowcolor="black",
                    relief="flat",
                    font=("Helvetica", 12),
                    padding=8)
