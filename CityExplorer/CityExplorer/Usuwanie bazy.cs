/*using Realms;
using System;

class Program
{
    static void Main()
    {
        string databasePath = @"E:\PROJEKT\CityExplorer\CityExplorer\myrealm.realm";

        try
        {
            // Otwórz bazę danych
            var realm = Realm.GetInstance(databasePath);

            // Usuwanie wszystkich obiektów typu User
            realm.Write(() =>
            {
                realm.RemoveAll<User>();  // Usuń wszystkie obiekty User
            });

            Console.WriteLine("Baza danych została wyczyszczona.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd: {ex.Message}");
        }
    }
}
*/