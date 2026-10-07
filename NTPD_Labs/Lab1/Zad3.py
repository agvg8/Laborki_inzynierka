import joblib
import pickle
from sklearn.datasets import load_iris
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LogisticRegression

# Wczytanie zbioru iris
data = load_iris()
X = data.data
y = data.target

# Podział na zbiór treningowy i testowy
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=72)

# Trenowanie modelu
model = LogisticRegression(max_iter=1000)
model.fit(X_train, y_train)

# Zapis modelu do pliku (pickle)
with open("../model.pkl", "wb") as file:
    pickle.dump(model, file)

# Zapis modelu do pliku (joblib)
joblib.dump(model, "../model.joblib")

print("Model zapisany jako model.pkl i model.joblib")
