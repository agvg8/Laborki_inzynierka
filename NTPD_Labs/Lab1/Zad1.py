import numpy as np
import pandas as pd
from sklearn.datasets import load_iris

# Wczytanie danych
data = load_iris()
df = pd.DataFrame(data.data, columns=data.feature_names)

# Dodanie etykiet
df['target'] = data.target

# Wyświetlenie pierwszych wierszy
print("Pierwsze 5 wierszy:")
print(df.head())

print(("\n-----------------------------------------------------------------------------------------\n"))

# Informacje o danych
print("Informacje o zbiorze:")
print(df.info())

print(("\n-----------------------------------------------------------------------------------------\n"))

# Rozmiar danych
print(f"Rozmiar zbioru: {df.shape}")

print(("\n-----------------------------------------------------------------------------------------\n"))