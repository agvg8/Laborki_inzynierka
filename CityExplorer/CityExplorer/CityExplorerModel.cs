using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace CityExplorer
{
    public class User : RealmObject
    {
        [PrimaryKey]
        public string Username { get; set; } // Nazwa użytkownika (unikalna)

        public string FirstName { get; set; } // Imię

        public string LastName { get; set; } // Nazwisko

        public string Password { get; set; } // Hasło (haszowane)

        public string Nationality { get; set; } // Narodowość

        public string City { get; set; } // Miasto
    }
}
