import numpy as np
import pandas as pd
from sklearn.datasets import load_iris
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score, classification_report

# Wczytanie zbioru iris
data = load_iris()
X = data.data
y = data.target

# Podział na zbiór treningowy (80%) i testowy (20%)
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=72)

# Utworzenie i wytrenowanie modelu
model = LogisticRegression(max_iter=1000)
model.fit(X_train, y_train)

# Predykcja na zbiorze testowym
y_pred = model.predict(X_test)

# Ocena modelu
accuracy = accuracy_score(y_test, y_pred)
print(f"Accuracy: {accuracy:.4f}")

# Pełny raport klasyfikacji
print("\nClassification Report:")
print(classification_report(y_test, y_pred, target_names=data.target_names))


