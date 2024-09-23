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
        medias.Add(media);  // Correction : on ajoute le média à la liste directement.
        Console.WriteLine($"Ajout de {media.title} à la bibliothèque avec {media.nbExemplaire} exemplaire(s).");
    }

    // Retirer un exemplaire d'un média
    public void RetirerMedia(Media media) {
        if (media != null && media.nbExemplaire > 0) {
            media -= 1;
            Console.WriteLine($"Retrait d'un exemplaire de {media.title}. Exemplaires restants : {media.nbExemplaire}");

            if (media.nbExemplaire == 0) {
                medias.Remove(media);
                Console.WriteLine($"{media.title} a été retiré de la bibliothèque car il n'y a plus d'exemplaires.");
            }
        } else {
            Console.WriteLine("Impossible de retirer le média : aucun exemplaire disponible.");
        }
    }


    public bool EmprunterMedia(int reference, string utilisateur) {
        Media media = this[reference];
        if (media != null && media.nbExemplaire > 0) {
            media.nbExemplaire--;
            emprunts.Add(new Emprunt(media, utilisateur));
            Console.WriteLine($"Le média {media.title} a été emprunté par {utilisateur}.");
            return true;
        } else {
            Console.WriteLine("Emprunt impossible : média indisponible.");
            return false;
        }
    }

    // Retourner un média
    public bool RetournerMedia(int reference, string utilisateur) {
        Emprunt emprunt = emprunts.Find(e => e.Media.GetReference() == reference && e.Utilisateur == utilisateur);
        if (emprunt != null) {
            emprunt.Media.nbExemplaire++;
            emprunts.Remove(emprunt);
            Console.WriteLine($"{emprunt.Media.title} a été retourné par {utilisateur}.");
            return true;
        } else {
            Console.WriteLine("Retour impossible : emprunt non trouvé.");
            return false;
        }
    }

    // Rechercher un média par titre ou auteur
    public List<Media> RechercherMedia(string critere) {
        List<Media> resultats = new List<Media>();
        foreach (var media in medias) {
            if (media is Livre livre && (livre.title.Contains(critere) || livre.Auteur.Contains(critere))) {
                resultats.Add(livre);
            } else if (media.title.Contains(critere)) {
                resultats.Add(media);
            }
        }
        return resultats;
    }

    // Lister les médias empruntés par un utilisateur
    public List<Media> ListerMediasEmpruntes(string utilisateur) {
        List<Media> empruntes = new List<Media>();
        foreach (var emprunt in emprunts) {
            if (emprunt.Utilisateur == utilisateur) {
                empruntes.Add(emprunt.Media);
            }
        }
        return empruntes;
    }

    // Afficher les statistiques de la bibliothèque
    public void AfficherStatistiques() {
        int totalMedias = medias.Count;
        int totalExemplaires = 0;
        int exemplairesEmpruntes = emprunts.Count;

        foreach (var media in medias) {
            totalExemplaires += media.nbExemplaire;
        }

        Console.WriteLine($"Total de médias dans la bibliothèque: {totalMedias}");
        Console.WriteLine($"Total d'exemplaires disponibles: {totalExemplaires}");
        Console.WriteLine($"Nombre d'exemplaires empruntés: {exemplairesEmpruntes}");
    }

    // Indexeur pour accéder aux médias par référence
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

// Classe représentant un emprunt
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

        Livre livre = new Livre("Spider-Man", 1234, 10, "Younes");
        DVD dvd = new DVD("Animals 5", 5678, 5, 2.5);
        CD cd = new CD("Sonate au clair de lune", 9876, 15, "Beethoven");

        // Ajout de médias dans la bibliothèque
        library.AjouterMedia(livre);
        library.AjouterMedia(dvd);
        library.AjouterMedia(cd);

        // Emprunt d'un média
        library.EmprunterMedia(1234, "Utilisateur1");

        // Retour du média
        library.RetournerMedia(1234, "Utilisateur1");

        // Affichage des statistiques
        library.AfficherStatistiques();

        // Recherche de médias par titre ou auteur
        var resultatsRecherche = library.RechercherMedia("Younes");
        foreach (var media in resultatsRecherche) {
            media.AfficherInfos();
        }

        // Affichage des médias empruntés par l'utilisateur
        var empruntes = library.ListerMediasEmpruntes("Utilisateur1");
        foreach (var media in empruntes) {
            media.AfficherInfos();
        }

        // Affichage des informations sur les médias
        livre.AfficherInfos();
        dvd.AfficherInfos();
        cd.AfficherInfos();
    }
}