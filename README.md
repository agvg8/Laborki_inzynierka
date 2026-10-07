<h1 align="center">🛠️ Projekty i laboratoria – studia inżynierskie</h1>

<p align="center">
  <b>Informatyka Stosowana · specjalność Data Science</b><br>
  Politechnika Bydgoska
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Python-3776AB?style=for-the-badge&logo=python&logoColor=white"/>
  <img src="https://img.shields.io/badge/PyTorch-EE4C2C?style=for-the-badge&logo=pytorch&logoColor=white"/>
  <img src="https://img.shields.io/badge/scikit--learn-F7931E?style=for-the-badge&logo=scikitlearn&logoColor=white"/>
  <img src="https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/Realm-39477F?style=for-the-badge&logo=realm&logoColor=white"/>
</p>

<p align="center">
  Projekty z uczenia maszynowego, sieci neuronowych i programowania aplikacji desktopowych,<br>
  zrealizowane w trakcie studiów inżynierskich.
</p>

---

## 📚 Spis projektów

| | Projekt | Opis | Technologie |
|:-:|---|---|---|
| ✈️ | [**Asystent podróży**](./Asystent_podrozy) | Aplikacja desktopowa z trzema modelami ML przewidującymi koszty i preferencje podróży | scikit-learn, Tkinter |
| 🎲 | [**Detektor kostek do gry**](./Sztuczne_Sieci_Neuronowe_projekt) | Sieci neuronowe liczące oczka na kostce i odróżniające kostki od liter | PyTorch |
| 🏙️ | [**CityExplorer**](./CityExplorer) | Aplikacja społecznościowa do odkrywania i dodawania miejsc w mieście | C#, WinForms, Realm |
| 🧪 | [**NTPD Labs**](./NTPD_Labs) | Podstawy pracy z modelami: eksploracja danych, klasyfikacja, serializacja | scikit-learn |

---

## ✈️ Asystent podróży

Aplikacja desktopowa łącząca trzy modele uczenia maszynowego w jednym interfejsie z menu kafelkowym.

**Moduły:**
- 💸 **Koszt podróży** – przewiduje cenę lotu na podstawie trasy i profilu podróżnego (`RandomForestRegressor` + `OneHotEncoder` w pipeline)
- 🏨 **Wybór hotelu** – rekomenduje hotel na podstawie wieku podróżnika (`RandomForestClassifier`)
- 📅 **Długość pobytu** – przewiduje liczbę dni wakacji na podstawie wieku i miesiąca wyjazdu (`RandomForestClassifier`)

**Co pokazuje:**
- strojenie hiperparametrów przez `GridSearchCV` i walidację krzyżową
- pełny cykl: czyszczenie danych → trening → zapis modelu (`joblib`) → użycie w aplikacji
- osobne skrypty treningowe (`*_do_model.py`) i moduły interfejsu

**Dane:** [Argo Datathon 2019](https://www.kaggle.com/datasets/leomauro/argodatathon2019) (Kaggle) – loty, hotele i użytkownicy.

<details>
<summary><b>▶️ Jak uruchomić</b></summary>

```bash
cd Asystent_podrozy
pip install pandas scikit-learn joblib pillow matplotlib tqdm
```
1. Pobierz zbiór danych z Kaggle i wypakuj do folderu `data/`
2. Uruchom notebook `data/czyszczenie_pliku.ipynb`
3. Uruchom aplikację:
```bash
python menu.py
```
</details>

---

## 🎲 Detektor kostek do gry

Projekt z przedmiotu **Sztuczne Sieci Neuronowe**. Dwa modele w PyTorch rozpoznają obrazy 28×28 px:

```
obraz 28×28 ──► SimpleNN (784 → 128 → 64 → 10) ──► liczba oczek
            └─► ValidityNN (784 → 128 → 64 → 2) ──► kostka prawidłowa / błędna
```

- **SimpleNN** – klasyfikuje liczbę oczek na kostce
- **ValidityNN** – sprawdza, czy obraz to poprawna kostka, a nie np. litera ze zbioru **EMNIST Letters**
- Wynik spoza zakresu 1–6 automatycznie oznacza kostkę jako błędną
- W raporcie porównanie skuteczności modelu z klasyfikatorem bez uczenia (baseline losowy)

**Pliki:**
| Plik | Zawartość |
|---|---|
| `data_cleaning.ipynb` | Przygotowanie danych: obrazy kostek + litery EMNIST |
| `Raport_2.ipynb` | Trening modelu liczącego oczka |
| `Raport_3.ipynb` | Trening klasyfikatora kostka vs litera, wykresy dokładności |
| `program.py` | Interaktywny tester: losuje obraz i pokazuje predykcję |

---

## 🏙️ CityExplorer

Aplikacja desktopowa w **C# / Windows Forms (.NET 8)** z lokalną bazą danych **Realm**.

**Funkcje:**
- 🔐 rejestracja i logowanie (hasła haszowane **SHA-256**)
- 🏠 strona główna z boczną nawigacją
- 📍 dodawanie nowych miejsc
- 👥 znajomi i 🔔 powiadomienia
- 👤 profil użytkownika
- 🛡️ panel administracyjny

<details>
<summary><b>▶️ Jak uruchomić</b></summary>

Otwórz `CityExplorer.sln` w **Visual Studio 2022** (wymagany .NET 8 SDK) i uruchom projekt (F5). Pakiet NuGet `Realm` pobierze się automatycznie.
</details>

---

## 🧪 NTPD Labs

Laboratoria z podstaw pracy z modelami na zbiorze **Iris**:

| Zadanie | Temat |
|---|---|
| `Zad1.py` | Eksploracja danych: struktura, typy, statystyki |
| `Zad2.py` | Regresja logistyczna, accuracy i `classification_report` |
| `Zad3.py` | Zapis wytrenowanego modelu przez `pickle` i `joblib` |

---

<p align="center">
  <sub>Repozytorium powstało przez scalenie osobnych repozytoriów projektowych z zachowaniem pełnej historii commitów.</sub>
</p>
