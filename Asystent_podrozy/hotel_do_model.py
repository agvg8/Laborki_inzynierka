import pandas as pd
from matplotlib import pyplot as plt
from sklearn.metrics import accuracy_score
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV
from sklearn.ensemble import RandomForestClassifier
from sklearn.pipeline import Pipeline
import joblib
import os

# ===== Ścieżka do pliku CSV ===== #
base_dir = os.path.dirname(__file__)  # Folder, w którym jest ten skrypt
csv_path = os.path.join(base_dir, "data", "data.csv")  # Ścieżka do pliku CSV

# ===== Wczytanie danych ===== #
df = pd.read_csv(csv_path)

# Sprawdźmy, jakie są kolumny w danych
print("Kolumny w danych:", df.columns)

#---------------------------------------------------------------------------------------------------
# Wizualizacja - Histogram wieku
plt.figure(figsize=(10, 6))
plt.hist(df['age'], bins=20, color='skyblue', edgecolor='black', alpha=0.7)
plt.title("Rozkład wieku uczestników", fontsize=16)
plt.xlabel("Wiek", fontsize=12)
plt.ylabel("Liczba osób", fontsize=12)
plt.show()

# Zliczamy liczbę wystąpień każdego hotelu
hotel_counts = df['hotelName'].value_counts()

# Tworzymy wykres słupkowy dla hoteli
plt.figure(figsize=(10, 6))
plt.bar(hotel_counts.index, hotel_counts.values, color='skyblue')

# Ustawienia tytułu i etykiet osi
plt.title("Liczba wystąpień poszczególnych hoteli w zbiorze danych", fontsize=16)
plt.xlabel("Hotel", fontsize=12)
plt.ylabel("Liczba wystąpień", fontsize=12)

# Rotacja etykiet na osi X, aby były czytelniejsze
plt.xticks(rotation=45, ha="right")

# Pokazanie wykresu
plt.show()

#---------------------------------------------------------------------------------------------------
# Przygotowanie danych do trenowania
X = df[['age', 'hotelPlaceLabelled']]  # Dane wejściowe (tu tylko wiek, ale można dodać inne cechy)
y = df['hotelNameLabelled']  # Wartość, którą przewidujemy

# Podział na zbiór treningowy i testowy
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=2137)


# ===== Tworzenie Pipeline i optymalizacja parametrów ===== #
# Pipeline dla RandomForest
rf_pipeline = Pipeline([
    ('clf', RandomForestClassifier(random_state=42))
])

# Parametry do GridSearch dla Random Forest (zmniejszenie liczby drzew, minimalizacja podziałów)
rf_param_grid = {
    'clf__n_estimators': [10, 20],  # Zmniejszona liczba drzew
    'clf__max_depth': [5, 10],       # Głębokość drzewa
    'clf__min_samples_split': [2, 5],  # Minimalna liczba próbek węzła do podziału
    'clf__min_samples_leaf': [1, 2]   # Minimalna liczba próbek w liściu
}

# GridSearch dla RandomForest
rf_grid_search = GridSearchCV(rf_pipeline, rf_param_grid, cv=3, n_jobs=-1)  # Zmniejszenie cross-validation do 3
rf_grid_search.fit(X_train, y_train)

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
joblib.dump(rf_grid_search.best_estimator_, "hotel_model.pkl")

print("Model został zapisany jako 'hotel_model.pkl'")
