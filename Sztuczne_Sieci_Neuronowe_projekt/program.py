import torch
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import random
from sklearn.preprocessing import StandardScaler
import joblib

# ----- MODELE (muszą być zdefiniowane wcześniej) -----

class SimpleNN(torch.nn.Module):
    def __init__(self):
        super(SimpleNN, self).__init__()
        self.fc1 = torch.nn.Linear(784, 128)
        self.fc2 = torch.nn.Linear(128, 64)
        self.fc3 = torch.nn.Linear(64, 10)

    def forward(self, x):
        x = torch.relu(self.fc1(x))
        x = torch.relu(self.fc2(x))
        return self.fc3(x)

class ValidityNN(torch.nn.Module):
    def __init__(self):
        super(ValidityNN, self).__init__()
        self.fc1 = torch.nn.Linear(784, 128)
        self.fc2 = torch.nn.Linear(128, 64)
        self.fc3 = torch.nn.Linear(64, 2)

    def forward(self, x):
        x = torch.relu(self.fc1(x))
        x = torch.relu(self.fc2(x))
        return self.fc3(x)

# ----- ZAŁADUJ DANE I MODELE -----

df = pd.read_csv('dice_ext_proj.csv')
X_pixels = df.iloc[:, 2:].values.astype('float32')

# Modele
model = SimpleNN()
model.load_state_dict(torch.load('simple_nn_model.pth', map_location='cpu'))
model.eval()

validity_model = ValidityNN()
validity_model.load_state_dict(torch.load('validity_model.pth', map_location='cpu'))
validity_model.eval()

# Skalowanie
scaler = joblib.load('scaler_dots.pkl')

# ----- FUNKCJA SPRAWDZAJĄCA -----

def predict_dice(img_flat):
    img_scaled = scaler.transform([img_flat])
    img_tensor = torch.tensor(img_scaled, dtype=torch.float32)

    with torch.no_grad():
        dots_out = model(img_tensor)
        predicted_dots = torch.argmax(dots_out, dim=1).item()

        if predicted_dots in [0, 7, 8, 9]:
            # Jeśli kostka jest błędna, zwróć tylko komunikat "Kostka błędna"
            return predicted_dots, "Kostka błędna"

        val_out = validity_model(img_tensor)
        predicted_valid = torch.argmax(val_out, dim=1).item()
        if predicted_valid == 1:
            return predicted_dots, "Kostka prawidłowa"
        else:
            return predicted_dots, "Kostka błędna"

# ----- FUNKCJA LOSUJĄCA I WYŚWIETLAJĄCA -----

def test_random_dice():
    idx = random.randint(0, len(X_pixels) - 1)
    img_flat = X_pixels[idx]
    predicted_dots, result_text = predict_dice(img_flat)

    img_2d = img_flat.reshape(28, 28)
    plt.imshow(img_2d, cmap='gray')

    # Jeśli kostka jest błędna, nie pokazujemy liczby oczek
    if result_text == "Kostka błędna":
        plt.title(f'{result_text}')
    else:
        plt.title(f'{result_text} (Oczka: {predicted_dots})')

    plt.axis('off')
    plt.show()

# ----- PROGRAM GŁÓWNY -----

if __name__ == "__main__":
    print("Witaj w detektorze kostek! Wciśnij Enter by losować, 'q' aby zakończyć.")
    while True:
        user_input = input(">>> ")
        if user_input.lower() == 'q':
            print("Do zobaczenia!")
            break
        test_random_dice()
