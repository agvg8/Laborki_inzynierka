import pandas as pd
from matplotlib import pyplot as plt
from sklearn.metrics import accuracy_score
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV
from sklearn.ensemble import RandomForestClassifier
from sklearn.pipeline import Pipeline
import joblib
import os
import time
import sys
from tqdm import tqdm  # Biblioteka do śledzenia postępu w pętli

# ===== Ścieżka do pliku CSV ===== #
base_dir = os.path.dirname(__file__)  # Folder, w którym jest ten skrypt
csv_path = os.path.join(base_dir, "data", "data.csv")  # Ścieżka do pliku CSV

# ===== Wczytanie danych ===== #
df = pd.read_csv(csv_path)

# ---------------------------------------------------------------------------------------------------
# Wizualizacja - Histogram wieku
plt.figure(figsize=(10, 6))
plt.hist(df['age'], bins=20, color='skyblue', edgecolor='black', alpha=0.7)
plt.title("Rozkład wieku uczestników", fontsize=16)
plt.xlabel("Wiek", fontsize=12)
plt.ylabel("Liczba osób", fontsize=12)
plt.show()

# Zliczamy liczbę wystąpień każdego monthu
month_counts = df['MonthOfTravelling'].value_counts()

# Tworzymy wykres słupkowy dla monthi
plt.figure(figsize=(10, 6))
plt.bar(month_counts.index, month_counts.values, color='skyblue')

# Ustawienia tytułu i etykiet osi
plt.title("Liczba wystąpień poszczególnych miesięcy w zbiorze danych", fontsize=16)
plt.xlabel("miesiac", fontsize=12)
plt.ylabel("Liczba wystąpień", fontsize=12)

# Rotacja etykiet na osi X, aby były czytelniejsze
plt.xticks(rotation=45, ha="right")

# Pokazanie wykresu
plt.show()

# Wykres na dni
days_counts = df['Day_of_week_back'].value_counts()

# Tworzymy wykres słupkowy dla miesięcy
plt.figure(figsize=(10, 6))
plt.bar(days_counts.index, days_counts.values, color='skyblue')

# Ustawienia tytułu i etykiet osi
plt.title("Liczba wystąpień poszczególnych Dni w zbiorze danych", fontsize=16)
plt.xlabel("dzień", fontsize=12)
plt.ylabel("Liczba wystąpień", fontsize=12)

# Rotacja etykiet na osi X, aby były czytelniejsze
plt.xticks(rotation=45, ha="right")

# Pokazanie wykresu
plt.show()

# ---------------------------------------------------------------------------------------------------

# Przygotowanie danych do trenowania
X = df[['age', 'MonthOfTravellingLabelled', 'Day_of_week_back_Labelled']]  # Dane wejściowe (tu tylko wiek, ale można dodać inne cechy)
y = df['days']  # Wartość, którą przewidujemy

# Podział na zbiór treningowy i testowy
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=2137)

# ===== Tworzenie Pipeline i optymalizacja parametrów ===== #
# Pipeline dla RandomForest
rf_pipeline = Pipeline([
    ('clf', RandomForestClassifier(random_state=42))
])

# Parametry do GridSearch dla Random Forest (zmniejszenie liczby drzew, minimalizacja podziałów)
rf_param_grid = {
    'clf__n_estimators': [50, 100],  # Zmniejszenie liczby drzew
    'clf__max_depth': [10, None],     # Zmniejszenie głębokości
    'clf__min_samples_split': [2],    # Zmniejszenie liczby próbek węzła
    'clf__min_samples_leaf': [1, 2],  # Zmniejszenie liczby próbek w liściu
    'clf__max_features': ['sqrt'],    # Jedna opcja dla cech
}


# Funkcja do śledzenia postępu obliczeń GridSearch
def grid_search_with_progress(rf_grid_search, X_train, y_train, interval=1):
    # Zliczanie wszystkich prób do przetworzenia
    total_combinations = len(rf_grid_search.param_grid['clf__n_estimators']) * len(
        rf_grid_search.param_grid['clf__max_depth']) * len(rf_grid_search.param_grid['clf__min_samples_split']) * len(
        rf_grid_search.param_grid['clf__min_samples_leaf'])

    print("Rozpoczynamy obliczanie najlepszych parametrów...")

    # Monitorowanie postępu w trakcie działania GridSearch
    for i in tqdm(range(total_combinations), desc="Optymalizacja parametrów", ncols=100):
        if i % interval == 0:  # Co jakiś czas, np. co 10 iteracji
            sys.stdout.write(".")
            sys.stdout.flush()  # Wydrukuj na ekranie
            time.sleep(0.1)  # Pauza na chwilę

        rf_grid_search.fit(X_train, y_train)

    print("\nGridSearch zakończony!")


# GridSearch dla RandomForest
rf_grid_search = GridSearchCV(rf_pipeline, rf_param_grid, cv=3, n_jobs=-1)

# Uruchomienie GridSearch z postępem
grid_search_with_progress(rf_grid_search, X_train, y_train)

# ===== Predykcja i ocena dokładności ===== #
# Predykcja dla Random Forest
y_pred_rf = rf_grid_search.predict(X_test)

# Obliczanie dokładności dla Random Forest
accuracy_rf = accuracy_score(y_test, y_pred_rf)

# Wydrukowanie wyników
print(f"Dokładność Random Forest: {accuracy_rf:.2%}")

# Cross-validation dla Random Forest
cv_score_rf = cross_val_score(rf_grid_search, X_train, y_train, cv=3, n_jobs=-1)
print(f"Średnia dokładność Random Forest (Cross-Validation): {cv_score_rf.mean():.2%}")

# ===== Zapis modeli ===== #
joblib.dump(rf_grid_search.best_estimator_, "vacation_model.pkl")

print("Model został zapisany jako 'vacation_model.pkl'")
