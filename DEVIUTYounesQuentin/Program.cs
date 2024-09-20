using System;
using System.Collections.Generic;

public class Media {

    public string title;
    protected int reference;
    public int nbExemplaire;

    public Media(string title, int reference, int nbExemplaire) {
        this.title = title;
        this.reference = reference;
        this.nbExemplaire = nbExemplaire;
    }

    public static Media operator +(Media media, int nombreExemplaires)
    {
        media.nbExemplaire += nombreExemplaires;
        return media;
    }

    // Surcharge de l'opérateur -
    public static Media operator -(Media media, int nombreExemplaires)
    {
        if (media.nbExemplaire - nombreExemplaires < 0)
        {
            throw new InvalidOperationException("Pas assez d'exemplaires disponibles pour effectuer cette opération.");
        }

        media.nbExemplaire -= nombreExemplaires;
        return media;
     }

     public virtual void AfficherInfos()
     {
         Console.WriteLine($"Titre : {title}, Référence : {reference}, Exemplaires disponibles : {nbExemplaire}");
     }

     public int GetReference()
     {
         return reference;
     }
}

public class Livre : Media {
    public string Auteur;

    public Livre(string title, int reference, int nbExemplaire, string auteur)
        : base(title, reference, nbExemplaire) {
        this.Auteur = auteur;
    }

    public override void AfficherInfos() {
        Console.WriteLine($"Auteur: {Auteur}, Titre : {title}, Référence : {reference}, Nombre Exemplaire : {nbExemplaire}");
    }
}

public class DVD : Media {
    public double Duree;

    public DVD(string title, int reference, int nbExemplaire, double duree)
        : base(title, reference, nbExemplaire) {
        this.Duree = duree;
    }

    public override void AfficherInfos() {
        Console.WriteLine($"Durée: {Duree} heures, Titre : {title}, Référence : {reference}, Nombre Exemplaire : {nbExemplaire}");
    }
}

public class CD : Media {
    public string Artiste;

    public CD(string title, int reference, int nbExemplaire, string artiste)
        : base(title, reference, nbExemplaire) {
        this.Artiste = artiste;
    }

    public override void AfficherInfos() {
        Console.WriteLine($"Artiste: {Artiste}, Titre : {title}, Référence : {reference}, Nombre Exemplaire : {nbExemplaire}");
    }
}

public class Library {

    protected List<Media> medias;
    protected List<Emprunt> emprunts;

    public Library() {
        medias = new List<Media>();
        emprunts = new List<Emprunt>();
    }

    public void AjouterMedia(Media media) {
        media += 1;
    }

    public void RetirerMedia(Media media) {
        if (media != null && media.nbExemplaire > 0) {
            media-=1;
        }
    }

    public bool EmprunterMedia(Media media, int reference, string utilisateur) {
        if (media.GetReference() == reference && media.nbExemplaire > 0) {
            media.nbExemplaire--;
            emprunts.Add(new Emprunt(media, utilisateur));
            return true;
        }
        return false;
    }

    public bool RetournerMedia(int reference, string utilisateur) {
        Emprunt emprunt = emprunts.Find(e => e.Media.GetReference() == reference && e.Utilisateur == utilisateur);
        if (emprunt != null) {
            emprunt.Media.nbExemplaire++;
            emprunts.Remove(emprunt);
            return true;
        }
        return false;
    }

    public List<Media> RechercherMedia(string critere) {
        List<Media> resultats = new List<Media>();
        foreach (var media in medias) {
            if (media is Livre livre && (livre.title.Contains(critere) || livre.Auteur.Contains(critere))) {
                resultats.Add(livre);
            }
            else if (media.title.Contains(critere)) {
                resultats.Add(media);
            }
        }
        return resultats;
    }

    public List<Media> ListerMediasEmpruntes(string utilisateur) {
        List<Media> empruntes = new List<Media>();
        foreach (var emprunt in emprunts) {
            if (emprunt.Utilisateur == utilisateur) {
                empruntes.Add(emprunt.Media);
            }
        }
        return empruntes;
    }

    public void AfficherStatistiques() {
        int totalMedias = medias.Count;
        int totalExemplaires = 0;
        int exemplairesEmpruntes = emprunts.Count;

        foreach (var media in medias) {
            totalExemplaires += media.nbExemplaire;
        }

        Console.WriteLine($"Total de médias: {totalMedias}");
        Console.WriteLine($"Total d'exemplaires disponibles: {totalExemplaires}");
        Console.WriteLine($"Nombre d'exemplaires empruntés: {exemplairesEmpruntes}");
    }

    public Media this[int reference] {
        get {
            foreach (var media in medias) {
                if (media.GetReference() == reference) {
                    return media;
                }
            }
            return null;
        }
    }
}




public class Emprunt {
    public Media Media;
    public string Utilisateur;
    public DateTime DateEmprunt;

    public Emprunt(Media media, string utilisateur) {
        Media = media;
        Utilisateur = utilisateur;
        DateEmprunt = DateTime.Now;
    }
}


class Program {
    public static void Main(string[] args) {

        Library library = new Library();

        Livre livre = new Livre("Siderman", 1234, 10, "Younes");
        DVD dvd = new DVD("Animals 5", 5678, 5, 2.5);
        CD cd = new CD("Sonate au clair de lune", 9876, 15, "Beethoven");

      /*library.AjouterMedia(livre);
        library.AjouterMedia(dvd);
        library.AjouterMedia(cd);
        */


       //library.RetirerMedia(livre);

        if (library.EmprunterMedia(livre, 1234, "Utilisateur1")) {
            Console.WriteLine("Média emprunté avec succès !");
        } else {
            Console.WriteLine("Échec de l'emprunt.");
        }
        

/*        var resultatsRecherche = library.RechercherMedia("Younes");
        foreach (var media in resultatsRecherche) {
            media.AfficherInfos();
        }

        var empruntes = library.ListerMediasEmpruntes("Utilisateur1");
        foreach (var media in empruntes) {
            media.AfficherInfos();
        }

        library.AfficherStatistiques();
        */
        
        livre.AfficherInfos();
        dvd.AfficherInfos();
        cd.AfficherInfos();
    }
}
