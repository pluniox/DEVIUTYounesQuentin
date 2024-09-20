using System;

public class Media {
    private string title;
    private int reference;
    private int nbExemplaire;

    public Media(string title, int reference, int nbExemplaire) {
        this.title = title;
        this.reference = reference;
        this.nbExemplaire = nbExemplaire;
        Console.WriteLine("Bonjour");
    }
}

class Program {
    public static void Main(string[] args) {
        Media media = new Media("Titre", 12134, 5);
    }
}
