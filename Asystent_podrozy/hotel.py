import tkinter as tk
from tkinter import ttk, StringVar, messagebox
from PIL import Image, ImageTk
from tkinter import StringVar
import joblib  # Do wczytywania modelu
import os
import pandas as pd

# ===== Wczytywanie modelu ===== #
try:
    model = joblib.load("hotel_model.pkl")  # Wczytaj model zapisany wcześniej
except Exception as e:
    messagebox.showerror("Błąd", f"Nie udało się wczytać modelu: {e}")
    model = None

# ===== Słownik z nazwami hoteli ===== #
number_to_hotel = {
    0: "Hotel A",
    1: "Hotel AF",
    2: "Hotel AU",
    3: "Hotel BD",
    4: "Hotel BP",
    5: "Hotel BW",
    6: "Hotel CB",
    7: "Hotel K",
    8: "Hotel Z",
    9: "no hotel"
}

# ===== Funkcja do konwersji miasta na numer ===== #
place_to_number = {
    'Florianopolis (SC)': 3,
    'Salvador (BH)': 7,
    'Natal (RN)': 4,
    'Aracaju (SE)': 0,
    'Recife (PE)': 5,
    'Sao Paulo (SP)': 8,
    'Campo Grande (MS)': 2,
    'Rio de Janeiro (RJ)': 6,
    'Brasilia (DF)': 1
}

# Funkcja do konwersji miasta na numer
def get_place_number(place):
    return place_to_number.get(place, None)  # Zwróci None, jeśli miasto nie jest w słowniku

# Funkcja do konwersji numeru hotelu na nazwę
def get_hotel_name(hotel_number):
    return number_to_hotel.get(hotel_number, "Brak hotelu")

# ===== Funkcja do predykcji ===== #
def predict_hotel():
    if not model:
        result_label.config(text="Brak modelu do predykcji")
        return

    try:
        # Pobieramy dane wejściowe
        age_value = int(age_var.get())  # Wiek
        place_value = place_var.get()  # Miasto

        # Konwertujemy miasto na numer
        place_number = get_place_number(place_value)

        if place_number is None:
            messagebox.showerror("Błąd", "Nieprawidłowe miasto")
            return

        # Tworzymy DataFrame, aby zachować tę samą strukturę, co przy treningu
        input_data = pd.DataFrame([[age_value, place_number]], columns=["age", "hotelPlaceLabelled"])

        # Przewidywanie
        prediction_number = model.predict(input_data)[0]  # Przewidywany numer hotelu

        # Przekształcamy numer na nazwę hotelu
        prediction_hotel = get_hotel_name(prediction_number)

        result_label.config(text=f"Rekomendowany hotel: {prediction_hotel}")  # Wyświetlamy wynik
    except Exception as e:
        messagebox.showerror("Błąd", f"Wystąpił problem z predykcją: {e}")

path_to_project = os.path.dirname(__file__)
path_to_image = os.path.join(path_to_project, "images", "background.jpg")

def run(window):
    for widget in window.winfo_children():  # Czyści ramkę przed załadowaniem nowych widżetów
        widget.destroy()

    # ===== Nagłówek ===== #
    label_title = tk.Label(
        window, text="Znajdź swój idealny hotel!",
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
        window, text="Wybierz miasto pobytu",
        font=("Helvetica", 12), bg="white", fg="black"
    )
    label_title.pack(pady=5)

    # ===== Wybór miejsca hotelu ===== #
    global place_var  # Używamy globalnej zmiennej dla miejsca hotelu
    place_var = StringVar()
    place_options = ['Florianopolis (SC)', 'Salvador (BH)', 'Natal (RN)',
                     'Aracaju (SE)', 'Recife (PE)', 'Sao Paulo (SP)',
                     'Campo Grande (MS)', 'Rio de Janeiro (RJ)', 'Brasilia (DF)']
    dropdown_place = ttk.Combobox(
        window, textvariable=place_var, values=place_options,
        font=("Helvetica", 12), state="readonly", width=20, justify="center", height=4
    )
    dropdown_place.set("Wybierz miasto")  # Domyślne ustawienie miejsca
    dropdown_place.pack(pady=10)

    # ===== Przycisk do predykcji ===== #
    button = ttk.Button(window, text="Znajdź hotel", command=predict_hotel, style="TButton")
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
    dropdown_place.config(style="Custom.TCombobox")

    style.configure("Custom.TCombobox",
                    fieldbackground="white",
                    background="#f0f0f0",
                    arrowcolor="black",
                    relief="flat",
                    font=("Helvetica", 12),
                    padding=8)
