using System;
using System.Collections.Generic;

public class Media {

    protected string title;
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

    public Library() {
        medias = new List<Media>();
    }

    public void AjouterMedia(Media media) {
        medias.Add(media);
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

class Program {
    public static void Main(string[] args) {

        Livre livre = new Livre("Siderman", 1234, 10, "Younes");
        livre.nbExemplaire += 3;
        livre.AfficherInfos();

        Console.WriteLine();

        DVD dvd = new DVD("Animals 5", 5678, 5, 2.5);
        dvd.AfficherInfos();

        Console.WriteLine();

        CD cd = new CD("Sonate au clair de lune", 9876, 15, "Beethoven");
        cd.AfficherInfos();

        Console.WriteLine();



    }
}
