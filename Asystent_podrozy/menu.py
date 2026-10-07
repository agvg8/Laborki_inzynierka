import tkinter as tk
from tkinter import messagebox
from PIL import Image, ImageTk
import os

# Importujemy moduły
#import distance
import hotel
import travel_cost
import vacation

# Ścieżki do plików
path_to_project = os.path.dirname(__file__)
path_to_image = os.path.join(path_to_project, "images", "background.png")
icon_paths = [
    os.path.join(path_to_project, "images", "img1.png"),
    os.path.join(path_to_project, "images", "img3.png"),
    os.path.join(path_to_project, "images", "img4.png")
]

# Nowe obrazki po kliknięciu
clicked_icon_paths = [
    os.path.join(path_to_project, "images", "clicked_img1.png"),
    os.path.join(path_to_project, "images", "clicked_img3.png"),
    os.path.join(path_to_project, "images", "clicked_img4.png")
]

# Mapowanie modułów do przycisków
modules = [travel_cost,  hotel, vacation]

# Zmienna przechowująca referencję do ostatnio klikniętego przycisku
last_clicked_button_idx = None

def show_module(script_module, button_idx):
    """Czyści główną ramkę i uruchamia funkcję `run(window)` z wybranego modułu."""
    global last_clicked_button_idx

    # Jeśli istnieje poprzednio kliknięty przycisk, przywróć jego obrazek do pierwotnego
    if last_clicked_button_idx is not None and last_clicked_button_idx != button_idx:
        toggle_button_icon(buttons[last_clicked_button_idx], last_clicked_button_idx, "default")

    # Zmiana obrazu przycisku po kliknięciu na ten, który został wybrany
    toggle_button_icon(buttons[button_idx], button_idx, "clicked")

    # Ustawiamy ostatnio kliknięty przycisk
    last_clicked_button_idx = button_idx

    # Czyści główną ramkę i uruchamia funkcję `run(window)` z wybranego modułu
    for widget in frame.winfo_children():
        widget.destroy()  # Usuwa poprzednie widżety

    if hasattr(script_module, "run"):
        script_module.run(frame)  # Przekazuje główną ramkę do modułu
    else:
        messagebox.showerror("Błąd", f"Moduł {script_module.__name__} nie zawiera funkcji run().")

def create_tooltip(widget, text):
    """Dodaje podpowiedź (tooltip) do widgetu."""
    tooltip = tk.Toplevel(widget)
    tooltip.withdraw()
    tooltip.overrideredirect(True)
    label = tk.Label(tooltip, text=text, bg="#333", fg="white", relief="flat", borderwidth=1, padx=8, pady=5,
                     font=("Arial", 10))
    label.pack()
    tooltip.attributes('-alpha', 0.85)

    def enter(event):
        x, y, _, _ = widget.bbox("all")
        x += widget.winfo_rootx() + 15
        y += widget.winfo_rooty() + 15
        tooltip.geometry(f"+{x}+{y}")
        tooltip.deiconify()

    def leave(event):
        tooltip.withdraw()

    widget.bind("<Enter>", enter)
    widget.bind("<Leave>", leave)

def toggle_button_icon(button, button_idx, state):
    """Zmienia obrazek przycisku w zależności od stanu (clicked lub default)."""
    if state == "clicked":
        button.config(image=clicked_icons[button_idx])
        button.image = clicked_icons[button_idx]
    else:  # default
        button.config(image=icons[button_idx])
        button.image = icons[button_idx]

# Główne okno
root = tk.Tk()
root.title("Asystent Podróży")
root.geometry("1000x700")
root.configure(bg='white')

# Wczytanie obrazu tła
background_image = Image.open(path_to_image).resize((1000, 700))
background_photo = ImageTk.PhotoImage(background_image)
background_label = tk.Label(root, image=background_photo)
background_label.place(relwidth=1, relheight=1)

# Przezroczysta ramka na dynamiczną treść
frame = tk.Frame(root, bg="white", bd=10)
frame.place(relx=0.5, rely=0.5, relwidth=0.6, relheight=0.6, anchor="center")

# Napis powitalny
welcome_label = tk.Label(
    frame,
    text="👋 Witaj w Asystencie Podróży dla Ameryki Południowej!\nTutaj możesz:\n\n"
         "✈️ Sprawdzić koszty podróży\n"
         "🏨 Znaleźć idealny hotel\n"
         "👥 Sprawdzić, na ile dni średnio inni latają na wczasy",
    font=("Arial", 14),
    bg="white",
    justify="center"
)
welcome_label.pack(expand=True)  # Sprawia, że napis będzie na środku

# Teksty do przycisków
button_texts = [
    "Lecisz na wakacje i nie wiesz,\nna jakie koszta się przygotować? Sprawdź!",
    "Nie wiesz jaki hotel będzie dla Ciebie odpowiedni?\nMy Ci podpowiemy!",
    "Chcesz się dowiedzieć jaka generacja będzie Cię otaczała podczas wakacji?\nMy to wiemy, Sprawdź!"
]

# Panel z przyciskami
button_frame = tk.Frame(root, bg="white")
button_frame.place(relx=0.5, rely=0.1, anchor="center")

# Wczytanie ikon przycisków
icons = [ImageTk.PhotoImage(Image.open(icon_paths[i]).resize((100, 100))) for i in range(3)]
clicked_icons = [ImageTk.PhotoImage(Image.open(clicked_icon_paths[i]).resize((100, 100))) for i in range(3)]

# Tworzenie przycisków
buttons = []
for i in range(3):
    btn = tk.Button(
        button_frame, image=icons[i], width=150, height=80, bg="white", relief="flat",
        bd=2, cursor="hand2", highlightthickness=0, highlightbackground="#e0e0e0",
        command=lambda m=modules[i], idx=i: show_module(m, idx)  # Przełącza zawartość w `frame`
    )
    btn.image = icons[i]  # Zabezpieczenie referencji
    btn.grid(row=0, column=i, padx=10, pady=10)

    create_tooltip(btn, button_texts[i])
    buttons.append(btn)

# Uruchomienie aplikacji
root.mainloop()
