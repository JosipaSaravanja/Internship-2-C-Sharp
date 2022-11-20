var players = new Dictionary<string, (string Position, int Rating)>() { //Već upisani igrači
    { "Luka Modrić", ("MF", 88)},
    { "Marcelo Brozović", ("MF", 86)},
    { "Mateo Kovačić", ("MF", 84)},
    { "Ivan Perišić", ("FM", 84)},
    { "Andrej Kramarić", ("FW", 82)},
    { "Ivan Rakitić", ("MF", 82)},
    { "Joško Gvardiol", ("DF", 81)},
    { "Mario Pašalić", ("MF", 81)},
    { "Lovro Majer", ("MF", 80)},
    { "Dominik Livaković", ("GK", 80)},
    { "Ante Rebić", ("FW", 80)},
    { "Josip Brekalo", ("MF", 79)},
    { "Borna Sosa", ("DF", 78)},
    { "Nikola Vlašić", ("MF", 78)},
    { "Duje Ćaleta-Car", ("DF", 78)},
    { "Dejan Lovren", ("DF",78)},
    { "Mislav Oršić", ("FW", 77)},
    { "Marko Livaja", ("FW", 77)},
    { "Domagoj Vida", ("DF", 76)},
    { "Ante Budimir", ("FW", 76)},
};
var grupa = new Dictionary<string, (int bodovi, int goalDifference)>() {//rezultati za ekipe
    {"Hrvatska", (0, 0)},
    {"Maroko", (0, 0)},
    {"Kanada", (0, 0)},
    {"Belgija", (0, 0)}
};
var strijelci = new Dictionary<string, int>();//ovdje će se izdvajati i spremati podaci o strijelcima
List<(string teamA, int golA, string teamB, int golB)> rezultati = new List<(string, int, string, int)>(){//raspored svih utakmica
    ("Maroko", 0, "Hrvatska", 0),
    ("Belgija", 0, "Kanada", 0),
    ("Belgija", 0, "Maroko", 0),
    ("Hrvatska", 0, "Kanada", 0),
    ("Hrvatska", 0, "Belgija", 0),
    ("Kanada", 0, "Maroko", 0),
};

int loop = 1;//služi za povratak na početni izbornik
int utakmice = 0; //broji sveukupne utakmice
int kolo = 0; //broji kolo
do
{
    Console.WriteLine("1 - Odradi trening"); //Ne znam je li bolje koristiti samo \n za nove redove?
    Console.WriteLine("2 - Odigraj utakmicu");
    Console.WriteLine("3 - Statistika");
    Console.WriteLine("4 - Kontrola igrača");
    Console.WriteLine("0 - Izlaz iz aplikacije");
    var choice = Console.ReadLine();
    switch (choice)
    {
        case "0":
            Console.Clear();
            if (Provjera() == true) //returna potvrdu
                loop = 0;//izlaz iz petlje (ne otvara se počeni izbornik više
            break;
        case "1":
            OdradiTrening(players);//prosljeđuje info o igračima da se promjeni rating
            break;
        case "2":
            Console.Clear();
            if (utakmice > 5)//ako je već odigrano 5 utakmica case "2" će se odmah prekiniti
            {
                Console.WriteLine("sve su utakmice odigrane \n\nPritisnite enter za povratak na glavni izbornik");
                Console.ReadLine();
                break;
            }

            var momcad = Momcad(players);//generiranje momčadi
            if (momcad.Count() < 11 && rezultati[utakmice].teamA == "Hrvatska" || momcad.Count() < 11 && rezultati[utakmice].teamB == "Hrvatska")//provjerava brojnost momčadi ukoliko igra Hrvatska
            {
                Console.WriteLine("Premalo igrača u momčadi naše ekipe \n\nPritisnite enter za povratak na glavni izbornik");
                Console.ReadLine();
                break;
            }
            int golA = new Random().Next(0, 5);//nasumični rezltati
            int golB = new Random().Next(0, 5);

            rezultati[utakmice] = (rezultati[utakmice].teamA, golA, rezultati[utakmice].teamB, golB);//spremanje nasumičih rezultata

            Console.Clear();
            Console.WriteLine($"{rezultati[kolo * 2].teamA} {rezultati[kolo * 2].golA}-{rezultati[kolo * 2].golB} {rezultati[kolo * 2].teamB}");//ispis 1. utakmice kola
            if (utakmice % 2 == 1)
            {
                Console.WriteLine($"{rezultati[kolo * 2 + 1].teamA} {rezultati[kolo * 2 + 1].golA}-{rezultati[kolo * 2 + 1].golB} {rezultati[kolo * 2 + 1].teamB}");//ispis 2 utakmice kola ako je upravo unesena i povećavanje brojača za kola jer je to posljednja utakmica u ovome
                kolo++;
            }

            if (golA > golB)//povećavaje i smanjenje bodova grupe (valjda je za pobjedu +3, poraz +1, nerješeno +0)
            {
                grupa[rezultati[utakmice].teamA] = (grupa[rezultati[utakmice].teamA].bodovi + 3, grupa[rezultati[utakmice].teamA].goalDifference + golA - golB);
                grupa[rezultati[utakmice].teamB] = (grupa[rezultati[utakmice].teamB].bodovi, grupa[rezultati[utakmice].teamB].goalDifference + golB - golA);
            }
            else if (golA < golB)
            {
                grupa[rezultati[utakmice].teamB] = (grupa[rezultati[utakmice].teamB].bodovi + 3, grupa[rezultati[utakmice].teamB].goalDifference + golB - golA);
                grupa[rezultati[utakmice].teamA] = (grupa[rezultati[utakmice].teamA].bodovi, grupa[rezultati[utakmice].teamA].goalDifference + golA - golB);
            }

            if (rezultati[utakmice].teamA == "Hrvatska")//povećavanje ili smanjenje raitinga naših igrača te strijelaca (kao i zapis novih strijelaca)
            {
                foreach (var player in momcad)
                {
                    if (golA > golB)
                    {
                        players[player.Key] = (players[player.Key].Position, players[player.Key].Rating * 102 / 100);
                    }
                    else
                    {
                        players[player.Key] = (players[player.Key].Position, players[player.Key].Rating * 98 / 100);
                    }
                }
                for (int i = 0; i < golA; i++)
                {
                    string strijelac = momcad.ElementAt(new Random().Next(0, momcad.Count)).Key;
                    if (strijelci.ContainsKey(strijelac))
                    {
                        strijelci[strijelac]++;
                    }
                    else
                    {
                        strijelci.Add(strijelac, 1);
                    }
                    if (golA > golB)
                    {
                        players[strijelac] = (players[strijelac].Position, players[strijelac].Rating * 105 / 100);
                    }
                    else
                    {
                        players[strijelac] = (players[strijelac].Position, players[strijelac].Rating * 95 / 100);
                    }
                }

            }
            else if (rezultati[utakmice].teamB == "Hrvatska")
            {
                foreach (var player in momcad)
                {
                    if (golA < golB)
                    {
                        players[player.Key] = (players[player.Key].Position, players[player.Key].Rating * 102 / 100);
                    }
                    else
                    {
                        players[player.Key] = (players[player.Key].Position, players[player.Key].Rating * 98 / 100);
                    }
                }
                for (int i = 0; i < golB; i++)
                {
                    string strijelac = momcad.ElementAt(new Random().Next(0, momcad.Count)).Key;
                    if (strijelci.ContainsKey(strijelac))
                    {
                        strijelci[strijelac]++;
                    }
                    else
                    {
                        strijelci.Add(strijelac, 1);
                    }
                    if (golA > golB)
                    {
                        players[strijelac] = (players[strijelac].Position, players[strijelac].Rating * 95 / 100);
                    }
                    else
                    {
                        players[strijelac] = (players[strijelac].Position, players[strijelac].Rating * 105 / 100);
                    }
                }
            }
            Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
            Console.ReadLine();
            utakmice++;
            break;
        case "3":
            Ispis(players, grupa, rezultati, strijelci, utakmice);//prosljeđuje sve potrebne podatke za ispise
            break;
        case "4":
            KontrolaIgraca(players, strijelci);
            break;
        default:
            NepostojeciUnos($"{choice} nije valjani unos"); //samo ispis greske i povratak na pocetni izbornik
            break;
    };
    Console.Clear();
} while (loop == 1);

void OdradiTrening(Dictionary<string, (string Position, int Rating)> players)
{
    Console.Clear();
    foreach (var player in players)
    {
        int napredak = new Random().Next(95, 105);
        Console.WriteLine($"{player.Key}: {player.Value.Rating} | {player.Value.Rating * napredak / 100} ");//nasumično mjenja rating svakome igraču
        players[player.Key] = (player.Value.Position, player.Value.Rating * napredak / 100);

    }
    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
    Console.ReadLine();
}

Dictionary<string, (string Position, int Rating)> Momcad(Dictionary<string, (string Position, int Rating)> players)
{
    int GK = 1, //brojevi pozicija koje se trebaju popunit
        DF = 4,
        MF = 3,
        FW = 3;

    var sortedDict = (from player in players orderby player.Value.Rating ascending select player).Reverse();//Nisam sigurna je li ovo uvijek funkcionira ali trebalo bi sortirat po vrijednostima (od najveće prema najmanjoj).
    var momcad = new Dictionary<string, (string Position, int Rating)>();

    foreach (var el in sortedDict) //popunjava najboljim igračima pozicije
    {
        if (el.Value.Position == "GK" && GK > 0)
        {
            momcad.Add(el.Key, (el.Value.Position, el.Value.Rating));
            GK--;
        }
        else if (el.Value.Position == "DF" && DF > 0)
        {
            momcad.Add(el.Key, (el.Value.Position, el.Value.Rating));
            DF--;
        }
        else if (el.Value.Position == "MF" && MF > 0)
        {
            momcad.Add(el.Key, (el.Value.Position, el.Value.Rating));
            MF--;
        }
        else if (el.Value.Position == "FW" && FW > 0)
        {
            momcad.Add(el.Key, (el.Value.Position, el.Value.Rating));
            FW--;
        }
    }
    return momcad;
}

void Ispis(Dictionary<string, (string Position, int Rating)> players, Dictionary<string, (int bodovi, int goalDifference)> grupa, List<(string teamA, int golA, string teamB, int golB)> rezultati, Dictionary<string, int> strijelci, int utakmicaBr)
{
    Console.Clear();
    Console.WriteLine("1 - Ispis svih igrača");
    Console.WriteLine("0 - Povratak na glavni izbornik");
    var choice = Console.ReadLine(); //odabir izbornika
    string search;
    switch (choice)
    {
        case "0":
            break; //povratak u loop
        case "1":
            Console.Clear();
            Console.WriteLine("1 - Ispis onako kako su spremljeni");
            Console.WriteLine("2 - Ispis po rating uzlazno");
            Console.WriteLine("3 - Ispis po ratingu silazno");
            Console.WriteLine("4 - Ispis igrača po imenu i prezimenu (ispis pozicije i ratinga)");
            Console.WriteLine("5 - Ispis igrača po ratingu (ako ih je više ispisati sve)");
            Console.WriteLine("6 - Ispis igrača po poziciji (ako ih je više ispisati sve)");
            Console.WriteLine("7 - Ispis trenutnih prvih 11 igrača (na pozicijama odabrati igrače s najboljim ratingom)");
            Console.WriteLine("8 - Ispis strijelaca i koliko golova imaju");
            Console.WriteLine("9 - Ispis svih rezultata ekipe");
            Console.WriteLine("10 - Ispis rezultat svih ekipa");
            Console.WriteLine("11 - Ispis tablice grupe (mjesto na tablici, ime ekipe, broj bodova, gol razlika)");
            Console.WriteLine("0 - Povratak na glavni izbornik");
            choice = Console.ReadLine(); //obabir na novom izborniku
            Console.Clear();
            var sortedDict = (from player in players orderby player.Value.Rating ascending select player); //sortirano pa raitingu za kasnije (da ne sortiramo 2 puta)
            switch (choice)
            {
                case "0":
                    break;//povratak u loop
                case "1":
                    foreach (var player in players)
                    {
                        Console.WriteLine($"ime: {player.Key}, pozicija: {player.Value.Position}, rating: {player.Value.Rating}"); //ispis elemenata dictionary-a
                    };
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                case "2":
                    foreach (var player in sortedDict)
                    {
                        Console.WriteLine($"ime: {player.Key}, pozicija: {player.Value.Position}, rating: {player.Value.Rating}");//ispis elemenata dictionary-a sortiranog po raiting
                    };
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                case "3":
                    foreach (var player in sortedDict.Reverse())
                    {
                        Console.WriteLine($"ime: {player.Key}, pozicija: {player.Value.Position}, rating: {player.Value.Rating}");//ispis elemenata dictionary-a sortiranog po raiting i reversanog
                    };
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                case "4":
                    Console.WriteLine("Unesite ime traženog igrača: ");
                    search = Console.ReadLine(); //pretraga traženog igrača
                    if (players.ContainsKey(search) == false) //provjera postojanja traženog igrača
                    {
                        Console.WriteLine($"Igrač {search} ne postoji");
                        break;
                    }
                    Console.WriteLine($"ime: {search}, pozicija: {players[search].Position}, rating: {players[search].Rating}"); //Ispis traženog igrača
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                case "5":
                    Console.WriteLine("Unesite rating traženog/ih igrača: ");
                    search = Console.ReadLine();//unos traženog raitinga
                    foreach (var player in players)
                    {
                        if (player.Value.Rating.ToString() == search)
                        {
                            Console.WriteLine($"ime: {player.Key}, pozicija: {player.Value.Position}, rating: {player.Value.Rating}");//pretraga i ispis igrača traženog raitinga
                        }
                    }
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                case "6":
                    Console.WriteLine("Unesite poziciju traženog/ih igrača: ");
                    search = Console.ReadLine();//unos tražene pozicije
                    foreach (var player in players)
                    {
                        if (player.Value.Position == search)
                        {
                            Console.WriteLine($"ime: {player.Key}, pozicija: {player.Value.Position}, rating: {player.Value.Rating}");//pretraga i ispis igrača tražene pozicije
                        }
                    }
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                case "7":
                    foreach (var player in Momcad(players))
                    {
                        Console.WriteLine($"ime: {player.Key}, pozicija: {player.Value.Position}, rating: {player.Value.Rating}");//ispis igrača momčadi
                    };
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                case "8":
                    foreach (var player in strijelci)
                    {
                        Console.WriteLine($"ime: {player.Key}, golovi: {player.Value}");//ispi strijelaca i njihovih golova
                    };
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                case "9":
                    for (int j = 0; j < utakmicaBr; j++)
                    {
                        if (rezultati[j].teamA == "Hrvatska" || rezultati[j].teamB == "Hrvatska")
                        {
                            Console.WriteLine($"{rezultati[j].teamA} {rezultati[j].golA} : {rezultati[j].golB} {rezultati[j].teamB}");//ispis rezultata utakmica u kojima smo sudjelovali
                        }
                    }
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                case "10":
                    foreach (var utakmica in rezultati)
                    {
                        Console.WriteLine($"{utakmica.teamA} {utakmica.golA}-{utakmica.golB} {utakmica.teamB}");//ispis rezultata svih utakmica
                    }
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                case "11":
                    var sortedGroup = from entry in grupa orderby (entry.Value.bodovi * 1000 + entry.Value.goalDifference) ascending select entry;
                    int i = 1;
                    foreach (var ekipa in sortedGroup.Reverse())
                    {
                        Console.WriteLine($"{i}. ime: {ekipa.Key} broj bodova: {ekipa.Value.bodovi} razlika u golovima: {ekipa.Value.goalDifference}");//ispis ekipa grupe
                        i++;
                    }
                    Console.WriteLine("\nPritisnite enter za povratak na glavni izbornik");
                    Console.ReadLine();
                    break;
                default:
                    NepostojeciUnos($"{choice} nije valjani odabir");
                    break;
            }
            break;
        default:
            NepostojeciUnos($"{choice} nije valjani odabir"); //samo ispisuje poruku da je doslo do greske
            break;
    }
    foreach (var player in players)
    {

    }
}

void KontrolaIgraca(Dictionary<string, (string Position, int Rating)> players, Dictionary<string, int> strijelci)
{
    Console.Clear();
    Console.WriteLine("1 - Unos novog igrača"); //Ne znam jeli bolje koristiti samo \n za nove redove?
    Console.WriteLine("2 - Brisanje igrača");
    Console.WriteLine("3 - Uređivanje igrača");
    Console.WriteLine("0 - Povratak na glavni izbornik");
    var choice = Console.ReadLine(); //unos za izbornik
    Console.Clear();
    string searchByName; //varijabla za pretragu po imenu
    string newName; //varijabla za nove unose
    string newPosition;
    int newRating;
    switch (choice)
    {
        case "0":
            break; //samo se vraća u loop
        case "1":
            if (players.Count > 25) //ako je ekipa već popunjena case "1" se odmah prekida
            {
                Console.WriteLine("Već imate 26 igrača\nPritisnite bilo koju tipku za povratak na glavni izbornik");
                Console.ReadLine();
                break;
            }
            Console.WriteLine("Unesite ime novog igrača");
            newName = Console.ReadLine();//novo ime
            if (players.ContainsKey(newName) == true || newName == "")//provjera je li zauzeto
            {
                NepostojeciUnos($"{newName} je nevaljano ime");
                break;
            }

            Console.WriteLine("\nUnesite poziciju novog igrača");
            newPosition = Console.ReadLine(); //nova pozicija
            if (newPosition != "GK" && newPosition != "DF" && newPosition != "MF" && newPosition != "FW") //provjera je li točna
            {
                NepostojeciUnos($"{newPosition} je nevaljana pozicija");
                break;
            }

            Console.WriteLine("\nUnesite rating novog igrača");
            newRating = int.Parse(Console.ReadLine()); //novi rating
            if (newRating < 1 || newRating > 100) //provjera je li zadovoljavajući
            {
                NepostojeciUnos($"{newRating} je nevaljani rating");
                break;
            }

            if (Provjera()) //zatraživanje potvrde (true/false)
                players.Add(newName, (newPosition, newRating));//dodavanje novog igrača
            break;
        case "2":
            Console.WriteLine("Unesite ime igrača kojeg želite urediti: ");
            searchByName = Console.ReadLine(); //unos traženog igrača
            if (players.ContainsKey(searchByName) == false) //pretraga traženim imenom
            {
                NepostojeciUnos($"{searchByName} je nepostojeće ime");
                break;
            }
            Console.Clear();
            Console.WriteLine("1 - Brisanje igrača unosom imena i prezimena");
            Console.WriteLine("0 - Povratak na glavni izbornik");
            choice = Console.ReadLine(); //unos za daljnji izbornik

            switch (choice)
            {
                case "0":
                    break;//samo se vraća u loop
                case "1":
                    if (Provjera()) //traženje potvrde (true/false) 
                        Console.WriteLine(players.Remove(searchByName));
                    break;
                default:
                    NepostojeciUnos($"{choice} nije valjani unos za odabir"); //samo ispis greske i povratak na pocetni izbornik
                    break;
            }
            break;
        case "3":
            Console.WriteLine("Unesite ime igrača kojeg želite urediti: ");
            searchByName = Console.ReadLine(); //unos imena traženog igrača
            if (players.ContainsKey(searchByName) == false) //provjera traženim imenom
            {
                NepostojeciUnos($"{searchByName} je nepostojeće ime"); //samo ispis greske i povratak na pocetni izbornik
                break;
            }
            Console.Clear();
            Console.WriteLine("1 - Uredi ime i prezime igrača");
            Console.WriteLine("2 - Uredi poziciju igrača (GK, DF, MF ili FW)");
            Console.WriteLine("3 - Uredi rating igrača (od 1 do 100) ");
            Console.WriteLine("0 - Povratak na glavni izbornik");
            choice = Console.ReadLine(); //unos na izborniku
            Console.Clear();

            switch (choice)
            {
                case "0":
                    break; //povratak u loop
                case "1":
                    Console.WriteLine("Unesite novo ime igrača");
                    newName = Console.ReadLine(); //novo ime igrača
                    if (players.ContainsKey(newName) == true || newName == "") //provjera je li zauzeto ili opće išta napisano
                    {
                        NepostojeciUnos($"{newName} je nevažeće ime");
                        break;
                    }
                    players.Add(newName, players[searchByName]); //dodavavanje identičnog tuple samo sa promjenjenim imenom
                    players.Remove(searchByName); //uklanjanje stalnog tuple
                    if (strijelci.ContainsKey(searchByName))//mjenja ime strijelcu
                    {
                        strijelci.Add(newName, strijelci[searchByName]); //dodavavanje identičnog tuple samo sa promjenjenim imenom
                        strijelci.Remove(searchByName); //uklanjanje stalnog tuple
                    }
                    break;
                case "2":
                    Console.WriteLine("Unesite novu pozicju ime igrača");
                    newPosition = Console.ReadLine(); //nova pozicija igrača
                    if (newPosition != "GK" && newPosition != "DF" && newPosition != "MF" && newPosition != "FW") //provjera postojanja pozicije
                    {
                        NepostojeciUnos($"{newPosition} je nevažeća pozicija");
                        break;
                    }
                    players[searchByName] = (newPosition, players[searchByName].Rating); //promjena value elementa dictionary-a prema ranije unesenom imenu
                    break;
                case "3":
                    Console.WriteLine("Unesite novi rating igrača");
                    newRating = int.Parse(Console.ReadLine()); //novi rating igrača
                    if (newRating < 1 || newRating > 100)//provjera valjanosti ratinga
                    {
                        NepostojeciUnos($"{newRating} je nevažeća pozicija");
                        break;
                    }
                    players[searchByName] = (players[searchByName].Position, newRating);//promjena value elemnta dictionary-a prema ranije unesenom imenu
                    break;
                default:
                    NepostojeciUnos($"{choice} nije valjani unos za odabir");
                    break;
            }
            break;
        default:
            NepostojeciUnos($"{choice} nije valjani odabir");
            break;
    }
}

void NepostojeciUnos(string message)//samo ispisuje poruku da je doslo do greske
{
    Console.Clear();
    Console.WriteLine($"{message} \nPritisnite enter za povratak na glavni izbornik");
    Console.ReadLine();
}

bool Provjera() //returna potvrdu za činjenje neke promjene
{
    Console.Clear();
    Console.WriteLine("Potvrdite želite li uistinu izvršiti zatraženu radnju");
    Console.WriteLine("0 - Da \n1 - Ne");
    var confirmation = Console.ReadLine();
    if (confirmation == "0")//Jedino ako odabere da vratit će true, a za sve ostale (odabir "Ne" i netočne unose) vratit će false
        return true; //returna potvrdu
    else
        return false;
}

