'''

Plik odpowiadający za predykcje ceny lotów

'''

import tkinter as tk
from tkinter import ttk, StringVar, messagebox
from PIL import Image, ImageTk
import joblib  # Do wczytywania modelu
import os
import pandas as pd

# ===== Wczytywanie modelu ===== #
try:
    model = joblib.load("flight_price_model.pkl")  # Wczytaj model zapisany wcześniej
except Exception as e:
    messagebox.showerror("Błąd", f"Nie udało się wczytać modelu: {e}")
    model = None

# ===== Opcje ===== #

cities = ['Florianopolis (SC)', 'Salvador (BH)', 'Campo Grande (MS)',
          'Aracaju (SE)', 'Sao Paulo (SP)', 'Natal (RN)', 'Recife (PE)',
          'Brasilia (DF)', 'Rio de Janeiro (RJ)']

flight_types = ['economic', 'premium', 'firstClass']

# ===== Funkcja predykcji ===== #
def predict_flight_price():
    if not model:
        result_label.config(text="Brak modelu do predykcji")
        return

    try:
        # Pobieramy dane wejściowe
        from_city = from_city_var.get()  # Miasto początkowe
        to_city = to_city_var.get()  # Miasto docelowe
        flight_type = flight_type_var.get()  # Typ lotu

        # Sprawdzamy, czy wszystkie dane zostały wprowadzone
        if not all([from_city, to_city, flight_type]):
            messagebox.showerror("Błąd", "Wypełnij wszystkie pola")
            return

        # Tworzymy DataFrame bez mapowania – model oczekuje stringów
        input_data = pd.DataFrame([[from_city, to_city, flight_type]],
                          columns=["from", "to", "flightType"])

        # Predykcja ceny
        predicted_price = model.predict(input_data)[0]
        result_label.config(text=f"Szacowana cena lotu: {predicted_price:.2f}")  # Wyświetlamy wynik
    except Exception as e:
        messagebox.showerror("Błąd", f"Wystąpił problem z predykcją: {e}")

# ===== Zbudowanie GUI ===== #
def run(window):
    for widget in window.winfo_children():  # Czyści ramkę przed załadowaniem nowych widżetów
        widget.destroy()

    # ===== Nagłówek ===== #
    label_title = tk.Label(
        window, text="Sprawdź ile wydasz za lot", font=("Helvetica", 16), bg="white", fg="black", padx=5, pady=5
    )
    label_title.pack(pady=10)

    label_title = tk.Label(
        window, text="Wybierz miasto początkowe", font=("Helvetica", 12), bg="white", fg="black"
    )
    label_title.pack(pady=5)

    # ===== Miasto początkowe ===== #
    global from_city_var  # Używamy globalnej zmiennej dla miasta początkowego
    from_city_var = StringVar()
    dropdown_from = ttk.Combobox(
        window, textvariable=from_city_var, values=cities, font=("Helvetica", 12), state="readonly", width=30
    )
    dropdown_from.set(cities[0])  # Domyślnie ustawione pierwsze miasto
    dropdown_from.pack(pady=5)

    label_title = tk.Label(
        window, text="Wybierz miasto docelowe", font=("Helvetica", 12), bg="white", fg="black"
    )
    label_title.pack(pady=5)

    # ===== Miasto docelowe ===== #
    global to_city_var  # Używamy globalnej zmiennej dla miasta docelowego
    to_city_var = StringVar()
    dropdown_to = ttk.Combobox(
        window, textvariable=to_city_var, values=cities, font=("Helvetica", 12), state="readonly", width=30
    )
    dropdown_to.set(cities[0])  # Domyślnie ustawione pierwsze miasto
    dropdown_to.pack(pady=5)

    label_title = tk.Label(
        window, text="Wybierz typ lotu", font=("Helvetica", 12), bg="white", fg="black"
    )
    label_title.pack(pady=5)

    # ===== Typ lotu ===== #
    global flight_type_var  # Używamy globalnej zmiennej dla typu lotu
    flight_type_var = StringVar()
    dropdown_flight_type = ttk.Combobox(
        window, textvariable=flight_type_var, values=flight_types, font=("Helvetica", 12), state="readonly", width=20
    )
    dropdown_flight_type.set(flight_types[0])  # Domyślnie ustawiony typ lotu: Economy
    dropdown_flight_type.pack(pady=5)

    # ===== Przycisk do predykcji ===== #
    button = ttk.Button(window, text="Przewiduj cenę lotu", command=predict_flight_price, style="TButton")
    button.pack(pady=10)

    # ===== Miejsce na wynik ===== #
    global result_label
    result_label = tk.Label(
        window, text="", font=("Helvetica", 12), bg="white", fg="black"
    )
    result_label.pack(pady=5)

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
    dropdown_from.config(style="Custom.TCombobox")
    dropdown_to.config(style="Custom.TCombobox")
    dropdown_flight_type.config(style="Custom.TCombobox")

    style.configure("Custom.TCombobox",
                    fieldbackground="white",
                    background="#f0f0f0",
                    arrowcolor="black",
                    relief="flat",
                    font=("Helvetica", 12),
                    padding=8)
