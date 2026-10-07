import pandas as pd
import os
import joblib

from sklearn.model_selection import train_test_split, GridSearchCV, cross_val_score
from sklearn.pipeline import Pipeline
from sklearn.ensemble import RandomForestRegressor
from sklearn.preprocessing import OneHotEncoder
from sklearn.compose import ColumnTransformer
from sklearn.metrics import r2_score

# ===== Ścieżka do pliku CSV ===== #
base_dir = os.path.dirname(__file__)  # Folder, w którym jest ten skrypt
csv_path = os.path.join(base_dir, "data", "data.csv")  # Ścieżka do pliku CSV

# ===== Wczytanie danych ===== #
df = pd.read_csv(csv_path)

# ===== Konwersja cech na stringi (potrzebne do OneHotEncoder) ===== #
categorical_features = ['from', 'to', 'flightType']
for col in categorical_features:
    df[col] = df[col].astype(str)

# ===== Wybór cech ===== #
X = df[categorical_features]  
y = df['flightPrice']

# ===== Podział na dane treningowe i testowe ===== #
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)

# ===== Przetwarzanie danych ===== #
preprocessor = ColumnTransformer(transformers=[ 
    ('cat', OneHotEncoder(handle_unknown='ignore'), categorical_features)
])

# ===== Pipeline ===== #
pipeline = Pipeline([
    ('preprocessor', preprocessor),
    ('regressor', RandomForestRegressor(random_state=42))
])

# ===== Parametry do GridSearch ===== #
param_grid = {
    'regressor__n_estimators': [10, 20],
    'regressor__max_depth': [5, 10],
    'regressor__min_samples_split': [2, 5],
    'regressor__min_samples_leaf': [1, 2]
}

# ===== GridSearchCV ===== #
grid_search = GridSearchCV(pipeline, param_grid, cv=3, n_jobs=-1, scoring='r2')
grid_search.fit(X_train, y_train)

# ===== Predykcja i ocena modelu ===== #
y_pred = grid_search.predict(X_test)
r2 = r2_score(y_test, y_pred)
print(f"Dokładność (R^2 score) Random Forest: {r2:.2%}")

# ===== Cross-validation score ===== #
cv_score = cross_val_score(grid_search.best_estimator_, X_train, y_train, cv=3, scoring='r2')
print(f"Średnia dokładność (CV R^2): {cv_score.mean():.2%}")

# ===== Zapis modelu ===== #
joblib.dump(grid_search.best_estimator_, "flight_price_model.pkl")
print("Model zapisany jako 'flight_price_model.pkl'")
