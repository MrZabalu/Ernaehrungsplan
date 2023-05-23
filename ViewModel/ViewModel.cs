using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        // Allgemein
        public event PropertyChangedEventHandler PropertyChanged;

        // CommandInitialisierungen
        public ICommand Informationen_CalculateCommand { get; set; } = new Informationen_CalculateCommand();
        public ICommand Informationen_SaveCommand { get; set; } = new Informationen_SaveCommand();
        public ICommand Tagesplaner_LoadCommand { get; set; } = new Tagesplaner_LoadCommand();
        public ICommand Produktdatenbank_DeleteCommand { get; set; } = new Produktdatenbank_DeleteCommand();
        public ICommand Produktdatenbank_LoadCommand { get; set; } = new Produktdatenbank_LoadCommand();
        public ICommand Produktdatenbank_SaveCommand { get; set; } = new Produktdatenbank_SaveCommand();

        // AllgemeinInitialisierungen
        public static ObservableCollection<ProduktListe> Listview { get; } = new ObservableCollection<ProduktListe>();

        // ViewEigenschaften - Informationen
        public string Name { get; set; }
        public int Alter { get; set; }
        public List<string> Geschlecht { get; set; } = new List<string> { "Mann", "Frau" };
        public string Geschlecht_Selected { get; set; }
        public int Grösse { get; set; }
        public int Gewicht { get; set; }
        public int Zielgewicht { get; set; }
        public List<string> Ziel { get; set; } = new List<string> { "Muskelaufbau", "Abnehmen" };
        public string Ziel_Selected { get; set; }
        public double Grundverbrauch { get; set; }
        public double Aktivitätsfaktor { get; set; }
        public double Zwischentotal { get; set; }
        public double Anpassung { get; set; }
        public double Total { get; set; }
        public double BedarfFettg { get; set; }
        public double BedarfKohlenhydrateg { get; set; }
        public double BedarfProteineg { get; set; }
        public double BedarfFettkcal { get; set; } = 0;
        public double BedarfKohlenhydratekcal { get; set; } = 0;
        public double BedarfProteinekcal { get; set; } = 0;

        // ViewEigenschaften - Tagesplaner
        public double Soll_Zunahmen { get; set; } = 0;
        public double Soll_Fett { get; set; } = 0;
        public double Soll_Kohlenhydrate { get; set; } = 0;
        public double Soll_Proteins { get; set; } = 0;
        public double Ist_Zunahmen { get; set; } = 0;
        public double Ist_Fett { get; set; } = 0;
        public double Ist_Kohlenhydrate { get; set; } = 0;
        public double Ist_Proteins { get; set; } = 0;
        public double Diff_Zunahmen { get; set; } = 0;
        public double Diff_Fett { get; set; } = 0;
        public double Diff_Kohlenhydrate { get; set; } = 0;
        public double Diff_Proteins { get; set; } = 0;

        public ProduktListe Frühstück_Selected_1 { get; set; } = new ProduktListe();
        public List<string> Frühstück_Kategorie_1 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Frühstück_Kategorie_1_Selected { get; set; }

        private string frühstück_Kategorie_1_Action;
        public string Frühstück_Kategorie_1_Action
        {
            get
            {
                return frühstück_Kategorie_1_Action;
            }
            set
            {
                frühstück_Kategorie_1_Action = Frühstück_Kategorie_1_Selected;

                defineProdukt(frühstück_Kategorie_1_Action, "Frühstück", 1);
            }
        }
        public List<string> Frühstück_Produkt_1 { get; set; } = new List<string>();
        public string Frühstück_Produkt_1_Selected { get; set; }

        private string frühstück_Produkt_1_Action;
        public string Frühstück_Produkt_1_Action
        {
            get
            {
                return frühstück_Produkt_1_Action;
            }
            set
            {
                frühstück_Produkt_1_Action = Frühstück_Produkt_1_Selected;

                defineWerte(frühstück_Produkt_1_Action, "Frühstück", 1);
                Frühstück_Menge_1 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Menge_1)));

            }
        }

        private double frühstück_Menge_1;
        public double Frühstück_Menge_1
        {
            get
            {
                return frühstück_Menge_1;
            }
            set
            {
                frühstück_Menge_1 = value;

                Frühstück_Energie_1 = (Frühstück_Selected_1.Energie / 100) * frühstück_Menge_1;
                Frühstück_Fett_1 = (Frühstück_Selected_1.Fett / 100) * frühstück_Menge_1;
                Frühstück_Kohlenhydrate_1 = (Frühstück_Selected_1.KHydrate / 100) * frühstück_Menge_1;
                Frühstück_Proteins_1 = (Frühstück_Selected_1.Protein / 100) * frühstück_Menge_1;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Energie_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Fett_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Kohlenhydrate_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Proteins_1)));

                tagesübersicht();
            }
        }
        public double Frühstück_Energie_1 { get; set; } = 0;
        public double Frühstück_Fett_1 { get; set; } = 0;
        public double Frühstück_Kohlenhydrate_1 { get; set; } = 0;
        public double Frühstück_Proteins_1 { get; set; } = 0;

        public ProduktListe Frühstück_Selected_2 { get; set; } = new ProduktListe();
        public List<string> Frühstück_Kategorie_2 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Frühstück_Kategorie_2_Selected { get; set; }

        private string frühstück_Kategorie_2_Action;
        public string Frühstück_Kategorie_2_Action
        {
            get
            {
                return frühstück_Kategorie_2_Action;
            }
            set
            {
                frühstück_Kategorie_2_Action = Frühstück_Kategorie_2_Selected;

                defineProdukt(frühstück_Kategorie_2_Action, "Frühstück", 2);
            }
        }
        public List<string> Frühstück_Produkt_2 { get; set; } = new List<string>();
        public string Frühstück_Produkt_2_Selected { get; set; }

        private string frühstück_Produkt_2_Action;
        public string Frühstück_Produkt_2_Action
        {
            get
            {
                return frühstück_Produkt_2_Action;
            }
            set
            {
                frühstück_Produkt_2_Action = Frühstück_Produkt_2_Selected;

                defineWerte(frühstück_Produkt_1_Action, "Frühstück", 2);
                Frühstück_Menge_2 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Menge_2)));

            }
        }

        private double frühstück_Menge_2;
        public double Frühstück_Menge_2
        {
            get
            {
                return frühstück_Menge_2;
            }
            set
            {
                frühstück_Menge_2 = value;

                Frühstück_Energie_2 = (Frühstück_Selected_2.Energie / 100) * frühstück_Menge_2;
                Frühstück_Fett_2 = (Frühstück_Selected_2.Fett / 100) * frühstück_Menge_2;
                Frühstück_Kohlenhydrate_2 = (Frühstück_Selected_2.KHydrate / 100) * frühstück_Menge_2;
                Frühstück_Proteins_2 = (Frühstück_Selected_2.Protein / 100) * frühstück_Menge_2;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Energie_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Fett_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Kohlenhydrate_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Proteins_2)));

                tagesübersicht();
            }
        }
        public double Frühstück_Energie_2 { get; set; } = 0;
        public double Frühstück_Fett_2 { get; set; } = 0;
        public double Frühstück_Kohlenhydrate_2 { get; set; } = 0;
        public double Frühstück_Proteins_2 { get; set; } = 0;

        public ProduktListe Frühstück_Selected_3 { get; set; } = new ProduktListe();
        public List<string> Frühstück_Kategorie_3 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Frühstück_Kategorie_3_Selected { get; set; }

        private string frühstück_Kategorie_3_Action;
        public string Frühstück_Kategorie_3_Action
        {
            get
            {
                return frühstück_Kategorie_3_Action;
            }
            set
            {
                frühstück_Kategorie_3_Action = Frühstück_Kategorie_3_Selected;

                defineProdukt(frühstück_Kategorie_3_Action, "Frühstück", 3);
            }
        }
        public List<string> Frühstück_Produkt_3 { get; set; } = new List<string>();
        public string Frühstück_Produkt_3_Selected { get; set; }

        private string frühstück_Produkt_3_Action;
        public string Frühstück_Produkt_3_Action
        {
            get
            {
                return frühstück_Produkt_3_Action;
            }
            set
            {
                frühstück_Produkt_3_Action = Frühstück_Produkt_3_Selected;

                defineWerte(frühstück_Produkt_1_Action, "Frühstück", 3);
                Frühstück_Menge_3 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Menge_3)));

            }
        }

        private double frühstück_Menge_3;
        public double Frühstück_Menge_3
        {
            get
            {
                return frühstück_Menge_3;
            }
            set
            {
                frühstück_Menge_3 = value;

                Frühstück_Energie_3 = (Frühstück_Selected_3.Energie / 100) * frühstück_Menge_3;
                Frühstück_Fett_3 = (Frühstück_Selected_3.Fett / 100) * frühstück_Menge_3;
                Frühstück_Kohlenhydrate_3 = (Frühstück_Selected_3.KHydrate / 100) * frühstück_Menge_3;
                Frühstück_Proteins_3 = (Frühstück_Selected_3.Protein / 100) * frühstück_Menge_3;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Energie_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Fett_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Kohlenhydrate_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Proteins_3)));

                tagesübersicht();
            }
        }
        public double Frühstück_Energie_3 { get; set; } = 0;
        public double Frühstück_Fett_3 { get; set; } = 0;
        public double Frühstück_Kohlenhydrate_3 { get; set; } = 0;
        public double Frühstück_Proteins_3 { get; set; } = 0;

        public ProduktListe Frühstück_Selected_4 { get; set; } = new ProduktListe();
        public List<string> Frühstück_Kategorie_4 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Frühstück_Kategorie_4_Selected { get; set; }

        private string frühstück_Kategorie_4_Action;
        public string Frühstück_Kategorie_4_Action
        {
            get
            {
                return frühstück_Kategorie_4_Action;
            }
            set
            {
                frühstück_Kategorie_4_Action = Frühstück_Kategorie_4_Selected;

                defineProdukt(frühstück_Kategorie_4_Action, "Frühstück", 4);
            }
        }
        public List<string> Frühstück_Produkt_4 { get; set; } = new List<string>();
        public string Frühstück_Produkt_4_Selected { get; set; }

        private string frühstück_Produkt_4_Action;
        public string Frühstück_Produkt_4_Action
        {
            get
            {
                return frühstück_Produkt_4_Action;
            }
            set
            {
                frühstück_Produkt_4_Action = Frühstück_Produkt_4_Selected;

                defineWerte(frühstück_Produkt_1_Action, "Frühstück", 4);
                Frühstück_Menge_4 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Menge_4)));

            }
        }

        private double frühstück_Menge_4;
        public double Frühstück_Menge_4
        {
            get
            {
                return frühstück_Menge_4;
            }
            set
            {
                frühstück_Menge_4 = value;

                Frühstück_Energie_4 = (Frühstück_Selected_4.Energie / 100) * frühstück_Menge_4;
                Frühstück_Fett_4 = (Frühstück_Selected_4.Fett / 100) * frühstück_Menge_4;
                Frühstück_Kohlenhydrate_4 = (Frühstück_Selected_4.KHydrate / 100) * frühstück_Menge_4;
                Frühstück_Proteins_4 = (Frühstück_Selected_4.Protein / 100) * frühstück_Menge_4;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Energie_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Fett_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Kohlenhydrate_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Proteins_4)));

                tagesübersicht();
            }
        }
        public double Frühstück_Energie_4 { get; set; } = 0;
        public double Frühstück_Fett_4 { get; set; } = 0;
        public double Frühstück_Kohlenhydrate_4 { get; set; } = 0;
        public double Frühstück_Proteins_4 { get; set; } = 0;

        public ProduktListe Mittagessen_Selected_1 { get; set; } = new ProduktListe();
        public List<string> Mittagessen_Kategorie_1 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Mittagessen_Kategorie_1_Selected { get; set; }

        private string mittagessen_Kategorie_1_Action;
        public string Mittagessen_Kategorie_1_Action
        {
            get
            {
                return mittagessen_Kategorie_1_Action;
            }
            set
            {
                mittagessen_Kategorie_1_Action = Mittagessen_Kategorie_1_Selected;

                defineProdukt(mittagessen_Kategorie_1_Action, "Mittagessen", 1);
            }
        }
        public List<string> Mittagessen_Produkt_1 { get; set; } = new List<string>();
        public string Mittagessen_Produkt_1_Selected { get; set; }

        private string mittagessen_Produkt_1_Action;
        public string Mittagessen_Produkt_1_Action
        {
            get
            {
                return mittagessen_Produkt_1_Action;
            }
            set
            {
                mittagessen_Produkt_1_Action = Mittagessen_Produkt_1_Selected;

                defineWerte(mittagessen_Produkt_1_Action, "Mittagessen", 1);
                Mittagessen_Menge_1 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Menge_1)));

            }
        }

        private double mittagessen_Menge_1;
        public double Mittagessen_Menge_1
        {
            get
            {
                return mittagessen_Menge_1;
            }
            set
            {
                mittagessen_Menge_1 = value;

                Mittagessen_Energie_1 = (Mittagessen_Selected_1.Energie / 100) * mittagessen_Menge_1;
                Mittagessen_Fett_1 = (Mittagessen_Selected_1.Fett / 100) * mittagessen_Menge_1;
                Mittagessen_Kohlenhydrate_1 = (Mittagessen_Selected_1.KHydrate / 100) * mittagessen_Menge_1;
                Mittagessen_Proteins_1 = (Mittagessen_Selected_1.Protein / 100) * mittagessen_Menge_1;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Energie_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Fett_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Kohlenhydrate_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Proteins_1)));

                tagesübersicht();
            }
        }
        public double Mittagessen_Energie_1 { get; set; } = 0;
        public double Mittagessen_Fett_1 { get; set; } = 0;
        public double Mittagessen_Kohlenhydrate_1 { get; set; } = 0;
        public double Mittagessen_Proteins_1 { get; set; } = 0;

        public ProduktListe Mittagessen_Selected_2 { get; set; } = new ProduktListe();
        public List<string> Mittagessen_Kategorie_2 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Mittagessen_Kategorie_2_Selected { get; set; }

        private string mittagessen_Kategorie_2_Action;
        public string Mittagessen_Kategorie_2_Action
        {
            get
            {
                return mittagessen_Kategorie_2_Action;
            }
            set
            {
                mittagessen_Kategorie_2_Action = Mittagessen_Kategorie_2_Selected;

                defineProdukt(mittagessen_Kategorie_2_Action, "Mittagessen", 2);
            }
        }
        public List<string> Mittagessen_Produkt_2 { get; set; } = new List<string>();
        public string Mittagessen_Produkt_2_Selected { get; set; }

        private string mittagessen_Produkt_2_Action;
        public string Mittagessen_Produkt_2_Action
        {
            get
            {
                return mittagessen_Produkt_2_Action;
            }
            set
            {
                mittagessen_Produkt_2_Action = Mittagessen_Produkt_2_Selected;

                defineWerte(mittagessen_Produkt_2_Action, "Mittagessen", 2);
                Mittagessen_Menge_2 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Menge_2)));

            }
        }

        private double mittagessen_Menge_2;
        public double Mittagessen_Menge_2
        {
            get
            {
                return mittagessen_Menge_2;
            }
            set
            {
                mittagessen_Menge_2 = value;

                Mittagessen_Energie_2 = (Mittagessen_Selected_2.Energie / 100) * mittagessen_Menge_2;
                Mittagessen_Fett_2 = (Mittagessen_Selected_2.Fett / 100) * mittagessen_Menge_2;
                Mittagessen_Kohlenhydrate_2 = (Mittagessen_Selected_2.KHydrate / 100) * mittagessen_Menge_2;
                Mittagessen_Proteins_2 = (Mittagessen_Selected_2.Protein / 100) * mittagessen_Menge_2;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Energie_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Fett_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Kohlenhydrate_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Proteins_2)));

                tagesübersicht();
            }
        }
        public double Mittagessen_Energie_2 { get; set; } = 0;
        public double Mittagessen_Fett_2 { get; set; } = 0;
        public double Mittagessen_Kohlenhydrate_2 { get; set; } = 0;
        public double Mittagessen_Proteins_2 { get; set; } = 0;

        public ProduktListe Mittagessen_Selected_3 { get; set; } = new ProduktListe();
        public List<string> Mittagessen_Kategorie_3 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Mittagessen_Kategorie_3_Selected { get; set; }

        private string mittagessen_Kategorie_3_Action;
        public string Mittagessen_Kategorie_3_Action
        {
            get
            {
                return mittagessen_Kategorie_3_Action;
            }
            set
            {
                mittagessen_Kategorie_3_Action = Mittagessen_Kategorie_3_Selected;

                defineProdukt(mittagessen_Kategorie_3_Action, "Mittagessen", 3);
            }
        }
        public List<string> Mittagessen_Produkt_3 { get; set; } = new List<string>();
        public string Mittagessen_Produkt_3_Selected { get; set; }

        private string mittagessen_Produkt_3_Action;
        public string Mittagessen_Produkt_3_Action
        {
            get
            {
                return mittagessen_Produkt_3_Action;
            }
            set
            {
                mittagessen_Produkt_3_Action = Mittagessen_Produkt_3_Selected;

                defineWerte(mittagessen_Produkt_3_Action, "Mittagessen", 3);
                Mittagessen_Menge_3 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Menge_3)));

            }
        }

        private double mittagessen_Menge_3;
        public double Mittagessen_Menge_3
        {
            get
            {
                return mittagessen_Menge_3;
            }
            set
            {
                mittagessen_Menge_3 = value;

                Mittagessen_Energie_3 = (Mittagessen_Selected_3.Energie / 100) * mittagessen_Menge_3;
                Mittagessen_Fett_3 = (Mittagessen_Selected_3.Fett / 100) * mittagessen_Menge_3;
                Mittagessen_Kohlenhydrate_3 = (Mittagessen_Selected_3.KHydrate / 100) * mittagessen_Menge_3;
                Mittagessen_Proteins_3 = (Mittagessen_Selected_3.Protein / 100) * mittagessen_Menge_3;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Energie_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Fett_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Kohlenhydrate_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Proteins_3)));

                tagesübersicht();
            }
        }
        public double Mittagessen_Energie_3 { get; set; } = 0;
        public double Mittagessen_Fett_3 { get; set; } = 0;
        public double Mittagessen_Kohlenhydrate_3 { get; set; } = 0;
        public double Mittagessen_Proteins_3 { get; set; } = 0;

        public ProduktListe Mittagessen_Selected_4 { get; set; } = new ProduktListe();
        public List<string> Mittagessen_Kategorie_4 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Mittagessen_Kategorie_4_Selected { get; set; }

        private string mittagessen_Kategorie_4_Action;
        public string Mittagessen_Kategorie_4_Action
        {
            get
            {
                return mittagessen_Kategorie_4_Action;
            }
            set
            {
                mittagessen_Kategorie_4_Action = Mittagessen_Kategorie_4_Selected;

                defineProdukt(mittagessen_Kategorie_4_Action, "Mittagessen", 4);
            }
        }
        public List<string> Mittagessen_Produkt_4 { get; set; } = new List<string>();
        public string Mittagessen_Produkt_4_Selected { get; set; }

        private string mittagessen_Produkt_4_Action;
        public string Mittagessen_Produkt_4_Action
        {
            get
            {
                return mittagessen_Produkt_4_Action;
            }
            set
            {
                mittagessen_Produkt_4_Action = Mittagessen_Produkt_4_Selected;

                defineWerte(mittagessen_Produkt_4_Action, "Mittagessen", 4);
                Mittagessen_Menge_4 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Menge_4)));

            }
        }

        private double mittagessen_Menge_4;
        public double Mittagessen_Menge_4
        {
            get
            {
                return mittagessen_Menge_4;
            }
            set
            {
                mittagessen_Menge_4 = value;

                Mittagessen_Energie_4 = (Mittagessen_Selected_4.Energie / 100) * mittagessen_Menge_4;
                Mittagessen_Fett_4 = (Mittagessen_Selected_4.Fett / 100) * mittagessen_Menge_4;
                Mittagessen_Kohlenhydrate_4 = (Mittagessen_Selected_4.KHydrate / 100) * mittagessen_Menge_4;
                Mittagessen_Proteins_4 = (Mittagessen_Selected_4.Protein / 100) * mittagessen_Menge_4;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Energie_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Fett_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Kohlenhydrate_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Proteins_4)));

                tagesübersicht();
            }
        }
        public double Mittagessen_Energie_4 { get; set; } = 0;
        public double Mittagessen_Fett_4 { get; set; } = 0;
        public double Mittagessen_Kohlenhydrate_4 { get; set; } = 0;
        public double Mittagessen_Proteins_4 { get; set; } = 0;

        public ProduktListe Abendessen_Selected_1 { get; set; } = new ProduktListe();
        public List<string> Abendessen_Kategorie_1 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Abendessen_Kategorie_1_Selected { get; set; }

        private string abendessen_Kategorie_1_Action;
        public string Abendessen_Kategorie_1_Action
        {
            get
            {
                return abendessen_Kategorie_1_Action;
            }
            set
            {
                abendessen_Kategorie_1_Action = Abendessen_Kategorie_1_Selected;

                defineProdukt(abendessen_Kategorie_1_Action, "Abendessen", 1);
            }
        }
        public List<string> Abendessen_Produkt_1 { get; set; } = new List<string>();
        public string Abendessen_Produkt_1_Selected { get; set; }

        private string abendessen_Produkt_1_Action;
        public string Abendessen_Produkt_1_Action
        {
            get
            {
                return abendessen_Produkt_1_Action;
            }
            set
            {
                abendessen_Produkt_1_Action = Abendessen_Produkt_1_Selected;

                defineWerte(abendessen_Produkt_1_Action, "Abendessen", 1);
                Abendessen_Menge_1 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Menge_1)));

            }
        }

        private double abendessen_Menge_1;
        public double Abendessen_Menge_1
        {
            get
            {
                return abendessen_Menge_1;
            }
            set
            {
                abendessen_Menge_1 = value;

                Abendessen_Energie_1 = (Abendessen_Selected_1.Energie / 100) * abendessen_Menge_1;
                Abendessen_Fett_1 = (Abendessen_Selected_1.Fett / 100) * abendessen_Menge_1;
                Abendessen_Kohlenhydrate_1 = (Abendessen_Selected_1.KHydrate / 100) * abendessen_Menge_1;
                Abendessen_Proteins_1 = (Abendessen_Selected_1.Protein / 100) * abendessen_Menge_1;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Energie_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Fett_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Kohlenhydrate_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Proteins_1)));

                tagesübersicht();
            }
        }
        public double Abendessen_Energie_1 { get; set; } = 0;
        public double Abendessen_Fett_1 { get; set; } = 0;
        public double Abendessen_Kohlenhydrate_1 { get; set; } = 0;
        public double Abendessen_Proteins_1 { get; set; } = 0;

        public ProduktListe Abendessen_Selected_2 { get; set; } = new ProduktListe();
        public List<string> Abendessen_Kategorie_2 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Abendessen_Kategorie_2_Selected { get; set; }

        private string abendessen_Kategorie_2_Action;
        public string Abendessen_Kategorie_2_Action
        {
            get
            {
                return abendessen_Kategorie_2_Action;
            }
            set
            {
                abendessen_Kategorie_2_Action = Abendessen_Kategorie_2_Selected;

                defineProdukt(abendessen_Kategorie_2_Action, "Abendessen", 2);
            }
        }
        public List<string> Abendessen_Produkt_2 { get; set; } = new List<string>();
        public string Abendessen_Produkt_2_Selected { get; set; }

        private string abendessen_Produkt_2_Action;
        public string Abendessen_Produkt_2_Action
        {
            get
            {
                return abendessen_Produkt_2_Action;
            }
            set
            {
                abendessen_Produkt_2_Action = Abendessen_Produkt_2_Selected;

                defineWerte(abendessen_Produkt_2_Action, "Abendessen", 2);
                Abendessen_Menge_2 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Menge_2)));

            }
        }

        private double abendessen_Menge_2;
        public double Abendessen_Menge_2
        {
            get
            {
                return abendessen_Menge_2;
            }
            set
            {
                abendessen_Menge_2 = value;

                Abendessen_Energie_2 = (Abendessen_Selected_2.Energie / 100) * abendessen_Menge_2;
                Abendessen_Fett_2 = (Abendessen_Selected_2.Fett / 100) * abendessen_Menge_2;
                Abendessen_Kohlenhydrate_2 = (Abendessen_Selected_2.KHydrate / 100) * abendessen_Menge_2;
                Abendessen_Proteins_2 = (Abendessen_Selected_2.Protein / 100) * abendessen_Menge_2;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Energie_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Fett_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Kohlenhydrate_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Proteins_2)));

                tagesübersicht();
            }
        }
        public double Abendessen_Energie_2 { get; set; } = 0;
        public double Abendessen_Fett_2 { get; set; } = 0;
        public double Abendessen_Kohlenhydrate_2 { get; set; } = 0;
        public double Abendessen_Proteins_2 { get; set; } = 0;

        public ProduktListe Abendessen_Selected_3 { get; set; } = new ProduktListe();
        public List<string> Abendessen_Kategorie_3 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Abendessen_Kategorie_3_Selected { get; set; }

        private string abendessen_Kategorie_3_Action;
        public string Abendessen_Kategorie_3_Action
        {
            get
            {
                return abendessen_Kategorie_3_Action;
            }
            set
            {
                abendessen_Kategorie_3_Action = Abendessen_Kategorie_3_Selected;

                defineProdukt(abendessen_Kategorie_3_Action, "Abendessen", 3);
            }
        }
        public List<string> Abendessen_Produkt_3 { get; set; } = new List<string>();
        public string Abendessen_Produkt_3_Selected { get; set; }

        private string abendessen_Produkt_3_Action;
        public string Abendessen_Produkt_3_Action
        {
            get
            {
                return abendessen_Produkt_3_Action;
            }
            set
            {
                abendessen_Produkt_3_Action = Abendessen_Produkt_3_Selected;

                defineWerte(abendessen_Produkt_3_Action, "Abendessen", 3);
                Abendessen_Menge_3 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Menge_3)));

            }
        }

        private double abendessen_Menge_3;
        public double Abendessen_Menge_3
        {
            get
            {
                return abendessen_Menge_3;
            }
            set
            {
                abendessen_Menge_3 = value;

                Abendessen_Energie_3 = (Abendessen_Selected_3.Energie / 100) * abendessen_Menge_3;
                Abendessen_Fett_3 = (Abendessen_Selected_3.Fett / 100) * abendessen_Menge_3;
                Abendessen_Kohlenhydrate_3 = (Abendessen_Selected_3.KHydrate / 100) * abendessen_Menge_3;
                Abendessen_Proteins_3 = (Abendessen_Selected_3.Protein / 100) * abendessen_Menge_3;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Energie_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Fett_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Kohlenhydrate_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Proteins_3)));

                tagesübersicht();
            }
        }
        public double Abendessen_Energie_3 { get; set; } = 0;
        public double Abendessen_Fett_3 { get; set; } = 0;
        public double Abendessen_Kohlenhydrate_3 { get; set; } = 0;
        public double Abendessen_Proteins_3 { get; set; } = 0;

        public ProduktListe Abendessen_Selected_4 { get; set; } = new ProduktListe();
        public List<string> Abendessen_Kategorie_4 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Abendessen_Kategorie_4_Selected { get; set; }

        private string abendessen_Kategorie_4_Action;
        public string Abendessen_Kategorie_4_Action
        {
            get
            {
                return abendessen_Kategorie_4_Action;
            }
            set
            {
                abendessen_Kategorie_4_Action = Abendessen_Kategorie_4_Selected;

                defineProdukt(abendessen_Kategorie_4_Action, "Abendessen", 4);
            }
        }
        public List<string> Abendessen_Produkt_4 { get; set; } = new List<string>();
        public string Abendessen_Produkt_4_Selected { get; set; }

        private string abendessen_Produkt_4_Action;
        public string Abendessen_Produkt_4_Action
        {
            get
            {
                return abendessen_Produkt_4_Action;
            }
            set
            {
                abendessen_Produkt_4_Action = Abendessen_Produkt_4_Selected;

                defineWerte(abendessen_Produkt_4_Action, "Abendessen", 4);
                Abendessen_Menge_4 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Menge_4)));

            }
        }

        private double abendessen_Menge_4;
        public double Abendessen_Menge_4
        {
            get
            {
                return abendessen_Menge_4;
            }
            set
            {
                abendessen_Menge_4 = value;

                Abendessen_Energie_4 = (Abendessen_Selected_4.Energie / 100) * abendessen_Menge_4;
                Abendessen_Fett_4 = (Abendessen_Selected_4.Fett / 100) * abendessen_Menge_4;
                Abendessen_Kohlenhydrate_4 = (Abendessen_Selected_4.KHydrate / 100) * abendessen_Menge_4;
                Abendessen_Proteins_4 = (Abendessen_Selected_4.Protein / 100) * abendessen_Menge_4;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Energie_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Fett_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Kohlenhydrate_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Proteins_4)));

                tagesübersicht();
            }
        }
        public double Abendessen_Energie_4 { get; set; } = 0;
        public double Abendessen_Fett_4 { get; set; } = 0;
        public double Abendessen_Kohlenhydrate_4 { get; set; } = 0;
        public double Abendessen_Proteins_4 { get; set; } = 0;

        public ProduktListe Sonstiges_Selected_1 { get; set; } = new ProduktListe();
        public List<string> Sonstiges_Kategorie_1 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Sonstiges_Kategorie_1_Selected { get; set; }

        private string sonstiges_Kategorie_1_Action;
        public string Sonstiges_Kategorie_1_Action
        {
            get
            {
                return sonstiges_Kategorie_1_Action;
            }
            set
            {
                sonstiges_Kategorie_1_Action = Sonstiges_Kategorie_1_Selected;

                defineProdukt(sonstiges_Kategorie_1_Action, "Sonstiges", 1);
            }
        }
        public List<string> Sonstiges_Produkt_1 { get; set; } = new List<string>();
        public string Sonstiges_Produkt_1_Selected { get; set; }

        private string sonstiges_Produkt_1_Action;
        public string Sonstiges_Produkt_1_Action
        {
            get
            {
                return sonstiges_Produkt_1_Action;
            }
            set
            {
                sonstiges_Produkt_1_Action = Sonstiges_Produkt_1_Selected;

                defineWerte(sonstiges_Produkt_1_Action, "Sonstiges", 1);
                Sonstiges_Menge_1 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Menge_1)));

            }
        }

        private double sonstiges_Menge_1;
        public double Sonstiges_Menge_1
        {
            get
            {
                return sonstiges_Menge_1;
            }
            set
            {
                sonstiges_Menge_1 = value;

                Sonstiges_Energie_1 = (Sonstiges_Selected_1.Energie / 100) * sonstiges_Menge_1;
                Sonstiges_Fett_1 = (Sonstiges_Selected_1.Fett / 100) * sonstiges_Menge_1;
                Sonstiges_Kohlenhydrate_1 = (Sonstiges_Selected_1.KHydrate / 100) * sonstiges_Menge_1;
                Sonstiges_Proteins_1 = (Sonstiges_Selected_1.Protein / 100) * sonstiges_Menge_1;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Energie_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Fett_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Kohlenhydrate_1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Proteins_1)));

                tagesübersicht();
            }
        }
        public double Sonstiges_Energie_1 { get; set; } = 0;
        public double Sonstiges_Fett_1 { get; set; } = 0;
        public double Sonstiges_Kohlenhydrate_1 { get; set; } = 0;
        public double Sonstiges_Proteins_1 { get; set; } = 0;

        public ProduktListe Sonstiges_Selected_2 { get; set; } = new ProduktListe();
        public List<string> Sonstiges_Kategorie_2 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Sonstiges_Kategorie_2_Selected { get; set; }

        private string sonstiges_Kategorie_2_Action;
        public string Sonstiges_Kategorie_2_Action
        {
            get
            {
                return sonstiges_Kategorie_2_Action;
            }
            set
            {
                sonstiges_Kategorie_2_Action = Sonstiges_Kategorie_2_Selected;

                defineProdukt(sonstiges_Kategorie_2_Action, "Sonstiges", 2);
            }
        }
        public List<string> Sonstiges_Produkt_2 { get; set; } = new List<string>();
        public string Sonstiges_Produkt_2_Selected { get; set; }

        private string sonstiges_Produkt_2_Action;
        public string Sonstiges_Produkt_2_Action
        {
            get
            {
                return sonstiges_Produkt_2_Action;
            }
            set
            {
                sonstiges_Produkt_2_Action = Sonstiges_Produkt_2_Selected;

                defineWerte(sonstiges_Produkt_2_Action, "Sonstiges", 2);
                Sonstiges_Menge_2 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Menge_2)));

            }
        }

        private double sonstiges_Menge_2;
        public double Sonstiges_Menge_2
        {
            get
            {
                return sonstiges_Menge_2;
            }
            set
            {
                sonstiges_Menge_2 = value;

                Sonstiges_Energie_2 = (Sonstiges_Selected_2.Energie / 100) * sonstiges_Menge_2;
                Sonstiges_Fett_2 = (Sonstiges_Selected_2.Fett / 100) * sonstiges_Menge_2;
                Sonstiges_Kohlenhydrate_2 = (Sonstiges_Selected_2.KHydrate / 100) * sonstiges_Menge_2;
                Sonstiges_Proteins_2 = (Sonstiges_Selected_2.Protein / 100) * sonstiges_Menge_2;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Energie_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Fett_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Kohlenhydrate_2)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Proteins_2)));

                tagesübersicht();
            }
        }
        public double Sonstiges_Energie_2 { get; set; } = 0;
        public double Sonstiges_Fett_2 { get; set; } = 0;
        public double Sonstiges_Kohlenhydrate_2 { get; set; } = 0;
        public double Sonstiges_Proteins_2 { get; set; } = 0;

        public ProduktListe Sonstiges_Selected_3 { get; set; } = new ProduktListe();
        public List<string> Sonstiges_Kategorie_3 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Sonstiges_Kategorie_3_Selected { get; set; }

        private string sonstiges_Kategorie_3_Action;
        public string Sonstiges_Kategorie_3_Action
        {
            get
            {
                return sonstiges_Kategorie_3_Action;
            }
            set
            {
                sonstiges_Kategorie_3_Action = Sonstiges_Kategorie_3_Selected;

                defineProdukt(sonstiges_Kategorie_3_Action, "Sonstiges", 3);
            }
        }
        public List<string> Sonstiges_Produkt_3 { get; set; } = new List<string>();
        public string Sonstiges_Produkt_3_Selected { get; set; }

        private string sonstiges_Produkt_3_Action;
        public string Sonstiges_Produkt_3_Action
        {
            get
            {
                return sonstiges_Produkt_3_Action;
            }
            set
            {
                sonstiges_Produkt_3_Action = Sonstiges_Produkt_3_Selected;

                defineWerte(sonstiges_Produkt_3_Action, "Sonstiges", 3);
                Sonstiges_Menge_3 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Menge_3)));

            }
        }

        private double sonstiges_Menge_3;
        public double Sonstiges_Menge_3
        {
            get
            {
                return sonstiges_Menge_3;
            }
            set
            {
                sonstiges_Menge_3 = value;

                Sonstiges_Energie_3 = (Sonstiges_Selected_3.Energie / 100) * sonstiges_Menge_3;
                Sonstiges_Fett_3 = (Sonstiges_Selected_3.Fett / 100) * sonstiges_Menge_3;
                Sonstiges_Kohlenhydrate_3 = (Sonstiges_Selected_3.KHydrate / 100) * sonstiges_Menge_3;
                Sonstiges_Proteins_3 = (Sonstiges_Selected_3.Protein / 100) * sonstiges_Menge_3;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Energie_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Fett_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Kohlenhydrate_3)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Proteins_3)));

                tagesübersicht();
            }
        }
        public double Sonstiges_Energie_3 { get; set; } = 0;
        public double Sonstiges_Fett_3 { get; set; } = 0;
        public double Sonstiges_Kohlenhydrate_3 { get; set; } = 0;
        public double Sonstiges_Proteins_3 { get; set; } = 0;

        public ProduktListe Sonstiges_Selected_4 { get; set; } = new ProduktListe();
        public List<string> Sonstiges_Kategorie_4 { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Sonstiges_Kategorie_4_Selected { get; set; }

        private string sonstiges_Kategorie_4_Action;
        public string Sonstiges_Kategorie_4_Action
        {
            get
            {
                return sonstiges_Kategorie_4_Action;
            }
            set
            {
                sonstiges_Kategorie_4_Action = Sonstiges_Kategorie_4_Selected;

                defineProdukt(sonstiges_Kategorie_4_Action, "Sonstiges", 4);
            }
        }
        public List<string> Sonstiges_Produkt_4 { get; set; } = new List<string>();
        public string Sonstiges_Produkt_4_Selected { get; set; }

        private string sonstiges_Produkt_4_Action;
        public string Sonstiges_Produkt_4_Action
        {
            get
            {
                return sonstiges_Produkt_4_Action;
            }
            set
            {
                sonstiges_Produkt_4_Action = Sonstiges_Produkt_4_Selected;

                defineWerte(sonstiges_Produkt_4_Action, "Sonstiges", 4);
                Sonstiges_Menge_4 = 100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Menge_4)));

            }
        }

        private double sonstiges_Menge_4;
        public double Sonstiges_Menge_4
        {
            get
            {
                return sonstiges_Menge_4;
            }
            set
            {
                sonstiges_Menge_4 = value;

                Sonstiges_Energie_4 = (Sonstiges_Selected_4.Energie / 100) * sonstiges_Menge_4;
                Sonstiges_Fett_4 = (Sonstiges_Selected_4.Fett / 100) * sonstiges_Menge_4;
                Sonstiges_Kohlenhydrate_4 = (Sonstiges_Selected_4.KHydrate / 100) * sonstiges_Menge_4;
                Sonstiges_Proteins_4 = (Sonstiges_Selected_4.Protein / 100) * sonstiges_Menge_4;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Energie_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Fett_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Kohlenhydrate_4)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Proteins_4)));

                tagesübersicht();
            }
        }
        public double Sonstiges_Energie_4 { get; set; } = 0;
        public double Sonstiges_Fett_4 { get; set; } = 0;
        public double Sonstiges_Kohlenhydrate_4 { get; set; } = 0;
        public double Sonstiges_Proteins_4 { get; set; } = 0;



        // ViewEigenschaften - Produktdatenbank
        public string Bezeichnung { get; set; }
        public List<string> Kategorie { get; set; } = new List<string> { "Hauptteil", "Beilage", "Gemüse", "Sonstiges" };
        public string Kategorie_Selected { get; set; }
        public double Energie { get; set; }
        public double Fett { get; set; }
        public double Kohlenhydrate { get; set; }
        public double Balaststoffe { get; set; }
        public double Protein { get; set; }

        // AllgemeinEigenschaften

        // Konstruktor
        public ViewModel()
        {
            loadProdukt();


        }

        // CommandMethoden
        public void informationen_CalculateCommand()
        {
            Zwischentotal = Grundverbrauch * Aktivitätsfaktor;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Zwischentotal)));

            Total = Zwischentotal - Anpassung;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Total)));

            BedarfFettkcal = BedarfFettg * 9.3;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BedarfFettkcal)));

            BedarfKohlenhydratekcal = BedarfKohlenhydrateg * 4.1;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BedarfKohlenhydratekcal)));

            BedarfProteinekcal = BedarfProteineg * 4.1;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BedarfProteinekcal)));
        }
        public void informationen_SaveCommand()
        {
            savePersonenData();
        }
        public void tagesplaner_LoadCommand()
        {
            // Sollwert definieren
            loadPersonData();
            // Tageswerte definieren

            // Istwert definieren (inkl. Diff.)


        }
        public void produktdatenbank_DeleteCommand()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(SQLHelper.CnnVal("Ernaehrungsplan_Datenbank")))
            {
                string Delete = "DELETE FROM Produkt_Datenbank";
                connection.Execute(Delete);
            }
        }
        public void produktdatenbank_LoadCommand()
        {
            Listview.Clear();
            loadProdukt();
        }
        public void produktdatenbank_SaveCommand()
        {
            saveProdukt();
        }

        // FunktionsMethoden
        public void savePersonenData()
        {
            PersonListe daten = new PersonListe();

            daten.Name = Name;
            daten.Alter = Alter;
            daten.Geschlecht = Geschlecht_Selected;
            daten.Grösse = Grösse;
            daten.Gewicht = Gewicht;
            daten.Zielgewicht = Zielgewicht;
            daten.Ziel = Ziel_Selected;
            daten.Grundverbrauch = Grundverbrauch;
            daten.Aktivitätsfaktor = Aktivitätsfaktor;
            daten.Zwischentotal = Zwischentotal;
            daten.Anpassung = Anpassung;
            daten.Total = Total;

            daten.Bedarf_Fett = Fett;
            daten.Bedarf_Kohlenhydrate = Kohlenhydrate;
            daten.Bedarf_Protein = Protein;

            // Schnittstelle zu Datenbank
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(SQLHelper.CnnVal("Ernaehrungsplan_Datenbank")))
            {
                string processQuery = "INSERT INTO Personen_Datenbank VALUES(@Name, @Alter, @Geschlecht, @Grösse, @Gewicht, @Zielgewicht, @Ziel, @Grundverbrauch, @Aktivitätsfaktor, @Zwischentotal, @Anpassung, @Total, @Bedarf_Fett, @Bedarf_Kohlenhydrate, @Bedarf_Protein)";
                connection.Execute(processQuery, daten);
            }

        }
        public void saveProdukt()
        {
            ProduktListe daten = new ProduktListe();

            daten.Bez = Bezeichnung;
            daten.Kat = Kategorie_Selected;
            daten.Energie = Energie;
            daten.Fett = Fett;
            daten.KHydrate = Kohlenhydrate;
            daten.Balaststoffe = Balaststoffe;
            daten.Protein = Protein;

            // Schnittstelle zu Datenbank
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(SQLHelper.CnnVal("Ernaehrungsplan_Datenbank")))
            {
                string processQuery = "INSERT INTO Produkt_Datenbank VALUES(@Bez, @Kat, @Energie, @Fett, @KHydrate, @Balaststoffe, @Protein)";
                connection.Execute(processQuery, daten);
            }

        }
        public void loadProdukt()
        {
            ProduktListe daten = new ProduktListe();

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(SQLHelper.CnnVal("Ernaehrungsplan_Datenbank")))
            {
                var output = connection.Query<ProduktListe>($"select * from Produkt_Datenbank").ToList();

                for (int i = 0; i < output.Count(); i++)
                {
                    daten.Bez = output.Skip(i).First().Bez;
                    daten.Kat = output.Skip(i).First().Kat;
                    daten.Energie = output.Skip(i).First().Energie;
                    daten.Fett = output.Skip(i).First().Fett;
                    daten.KHydrate = output.Skip(i).First().KHydrate;
                    daten.Balaststoffe = output.Skip(i).First().Balaststoffe;
                    daten.Protein = output.Skip(i).First().Protein;

                    ProduktListe clone = cloneProduktData(daten);
                    Listview.Add(clone);

                }
            }
        }
        public void loadPerson()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(SQLHelper.CnnVal("Ernaehrungsplan_Datenbank")))
            {
                try
                {
                    var output = connection.Query<PersonListe>($"select * from Personen_Datenbank").ToList();

                    Ist_Zunahmen = output.First().Total;
                    Ist_Fett = output.First().Bedarf_Fett;
                    Ist_Kohlenhydrate = output.First().Bedarf_Kohlenhydrate;
                    Ist_Proteins = output.First().Bedarf_Protein;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ist_Zunahmen)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ist_Fett)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ist_Kohlenhydrate)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ist_Proteins)));
                }
                catch
                {

                }

            }
        }
        public void loadPersonData()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(SQLHelper.CnnVal("Ernaehrungsplan_Datenbank")))
            {
                var output = connection.Query<PersonListe>($"select * from Personen_Datenbank").ToList();

                Soll_Zunahmen = output.First().Total;
                Soll_Fett = output.First().Bedarf_Fett;
                Soll_Kohlenhydrate = output.First().Bedarf_Kohlenhydrate;
                Soll_Proteins = output.First().Bedarf_Protein;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Soll_Zunahmen)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Soll_Fett)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Soll_Kohlenhydrate)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Soll_Proteins)));

            }
        }
        public void defineProdukt(string kat, string mahlzeit, int Zeile)
        {
            List<ProduktListe> liste = new List<ProduktListe>();
            ProduktListe daten = new ProduktListe();

            for (int i = 0; i < Listview.Count(); i++)
            {
                if (Listview.Skip(i).First().Kat == kat)
                {
                    daten.Bez = Listview.Skip(i).First().Bez;
                    daten.Kat = Listview.Skip(i).First().Kat;
                    daten.Energie = Listview.Skip(i).First().Energie;
                    daten.Fett = Listview.Skip(i).First().Fett;
                    daten.KHydrate = Listview.Skip(i).First().KHydrate;
                    daten.Balaststoffe = Listview.Skip(i).First().Balaststoffe;
                    daten.Protein = Listview.Skip(i).First().Protein;

                    ProduktListe clone = cloneProduktData(daten);
                    liste.Add(clone);
                }
            }

            for (int i = 0; i < liste.Count(); i++)
            {
                if (mahlzeit == "Frühstück")
                {
                    if (Zeile == 1)
                    {
                        Frühstück_Produkt_1.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 2)
                    {
                        Frühstück_Produkt_2.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 3)
                    {
                        Frühstück_Produkt_3.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 4)
                    {
                        Frühstück_Produkt_4.Add(liste.Skip(i).First().Bez);
                    }
                }
                if (mahlzeit == "Mittagessen")
                {
                    if (Zeile == 1)
                    {
                        Mittagessen_Produkt_1.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 2)
                    {
                        Mittagessen_Produkt_2.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 3)
                    {
                        Mittagessen_Produkt_3.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 4)
                    {
                        Mittagessen_Produkt_4.Add(liste.Skip(i).First().Bez);
                    }
                }
                if (mahlzeit == "Abendessen")
                {
                    if (Zeile == 1)
                    {
                        Abendessen_Produkt_1.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 2)
                    {
                        Abendessen_Produkt_2.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 3)
                    {
                        Abendessen_Produkt_3.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 4)
                    {
                        Abendessen_Produkt_4.Add(liste.Skip(i).First().Bez);
                    }
                }
                if (mahlzeit == "Sonstiges")
                {
                    if (Zeile == 1)
                    {
                        Sonstiges_Produkt_1.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 2)
                    {
                        Sonstiges_Produkt_2.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 3)
                    {
                        Sonstiges_Produkt_3.Add(liste.Skip(i).First().Bez);
                    }
                    if (Zeile == 4)
                    {
                        Sonstiges_Produkt_4.Add(liste.Skip(i).First().Bez);
                    }
                }
            }

        }
        public void defineWerte(string produkt, string mahlzeit, int zeile)
        {
            for (int i = 0; i < Listview.Count(); i++)
            {
                if (Listview.Skip(i).First().Bez == produkt)
                {
                    if (mahlzeit == "Frühstück")
                    {
                        if (zeile == 1)
                        {
                            Frühstück_Selected_1.Energie = Listview.Skip(i).First().Energie;
                            Frühstück_Selected_1.Fett = Listview.Skip(i).First().Fett;
                            Frühstück_Selected_1.KHydrate = Listview.Skip(i).First().KHydrate;
                            Frühstück_Selected_1.Protein = Listview.Skip(i).First().Protein;

                            Frühstück_Energie_1 = Frühstück_Selected_1.Energie;
                            Frühstück_Fett_1 = Frühstück_Selected_1.Fett;
                            Frühstück_Kohlenhydrate_1 = Frühstück_Selected_1.KHydrate;
                            Frühstück_Proteins_1 = Frühstück_Selected_1.Protein;

                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Energie_1)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Fett_1)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Kohlenhydrate_1)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Proteins_1)));
                        }
                        if (zeile == 2)
                        {
                            Frühstück_Selected_2.Energie = Listview.Skip(i).First().Energie;
                            Frühstück_Selected_2.Fett = Listview.Skip(i).First().Fett;
                            Frühstück_Selected_2.KHydrate = Listview.Skip(i).First().KHydrate;
                            Frühstück_Selected_2.Protein = Listview.Skip(i).First().Protein;

                            Frühstück_Energie_2 = Frühstück_Selected_2.Energie;
                            Frühstück_Fett_2 = Frühstück_Selected_2.Fett;
                            Frühstück_Kohlenhydrate_2 = Frühstück_Selected_2.KHydrate;
                            Frühstück_Proteins_2 = Frühstück_Selected_2.Protein;

                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Energie_2)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Fett_2)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Kohlenhydrate_2)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Proteins_2)));
                        }
                        if (zeile == 3)
                        {
                            Frühstück_Selected_3.Energie = Listview.Skip(i).First().Energie;
                            Frühstück_Selected_3.Fett = Listview.Skip(i).First().Fett;
                            Frühstück_Selected_3.KHydrate = Listview.Skip(i).First().KHydrate;
                            Frühstück_Selected_3.Protein = Listview.Skip(i).First().Protein;

                            Frühstück_Energie_3 = Frühstück_Selected_3.Energie;
                            Frühstück_Fett_3 = Frühstück_Selected_3.Fett;
                            Frühstück_Kohlenhydrate_3 = Frühstück_Selected_3.KHydrate;
                            Frühstück_Proteins_3 = Frühstück_Selected_3.Protein;

                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Energie_3)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Fett_3)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Kohlenhydrate_3)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Proteins_3)));
                        }
                        if (zeile == 4)
                        {
                            Frühstück_Selected_4.Energie = Listview.Skip(i).First().Energie;
                            Frühstück_Selected_4.Fett = Listview.Skip(i).First().Fett;
                            Frühstück_Selected_4.KHydrate = Listview.Skip(i).First().KHydrate;
                            Frühstück_Selected_4.Protein = Listview.Skip(i).First().Protein;

                            Frühstück_Energie_4 = Frühstück_Selected_4.Energie;
                            Frühstück_Fett_4 = Frühstück_Selected_4.Fett;
                            Frühstück_Kohlenhydrate_4 = Frühstück_Selected_4.KHydrate;
                            Frühstück_Proteins_4 = Frühstück_Selected_4.Protein;

                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Energie_4)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Fett_4)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Kohlenhydrate_4)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frühstück_Proteins_4)));
                        }
                    }
                }
                if (mahlzeit == "Mittagessen")
                {
                    if (zeile == 1)
                    {
                        Mittagessen_Selected_1.Energie = Listview.Skip(i).First().Energie;
                        Mittagessen_Selected_1.Fett = Listview.Skip(i).First().Fett;
                        Mittagessen_Selected_1.KHydrate = Listview.Skip(i).First().KHydrate;
                        Mittagessen_Selected_1.Protein = Listview.Skip(i).First().Protein;

                        Mittagessen_Energie_1 = Mittagessen_Selected_1.Energie;
                        Mittagessen_Fett_1 = Mittagessen_Selected_1.Fett;
                        Mittagessen_Kohlenhydrate_1 = Mittagessen_Selected_1.KHydrate;
                        Mittagessen_Proteins_1 = Mittagessen_Selected_1.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Energie_1)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Fett_1)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Kohlenhydrate_1)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Proteins_1)));
                    }
                    if (zeile == 2)
                    {
                        Mittagessen_Selected_2.Energie = Listview.Skip(i).First().Energie;
                        Mittagessen_Selected_2.Fett = Listview.Skip(i).First().Fett;
                        Mittagessen_Selected_2.KHydrate = Listview.Skip(i).First().KHydrate;
                        Mittagessen_Selected_2.Protein = Listview.Skip(i).First().Protein;

                        Mittagessen_Energie_2 = Mittagessen_Selected_2.Energie;
                        Mittagessen_Fett_2 = Mittagessen_Selected_2.Fett;
                        Mittagessen_Kohlenhydrate_2 = Mittagessen_Selected_2.KHydrate;
                        Mittagessen_Proteins_2 = Mittagessen_Selected_2.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Energie_2)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Fett_2)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Kohlenhydrate_2)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Proteins_2)));
                    }
                    if (zeile == 3)
                    {
                        Mittagessen_Selected_3.Energie = Listview.Skip(i).First().Energie;
                        Mittagessen_Selected_3.Fett = Listview.Skip(i).First().Fett;
                        Mittagessen_Selected_3.KHydrate = Listview.Skip(i).First().KHydrate;
                        Mittagessen_Selected_3.Protein = Listview.Skip(i).First().Protein;

                        Mittagessen_Energie_3 = Mittagessen_Selected_3.Energie;
                        Mittagessen_Fett_3 = Mittagessen_Selected_3.Fett;
                        Mittagessen_Kohlenhydrate_3 = Mittagessen_Selected_3.KHydrate;
                        Mittagessen_Proteins_3 = Mittagessen_Selected_3.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Energie_3)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Fett_3)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Kohlenhydrate_3)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Proteins_3)));
                    }
                    if (zeile == 4)
                    {
                        Mittagessen_Selected_4.Energie = Listview.Skip(i).First().Energie;
                        Mittagessen_Selected_4.Fett = Listview.Skip(i).First().Fett;
                        Mittagessen_Selected_4.KHydrate = Listview.Skip(i).First().KHydrate;
                        Mittagessen_Selected_4.Protein = Listview.Skip(i).First().Protein;

                        Mittagessen_Energie_4 = Mittagessen_Selected_4.Energie;
                        Mittagessen_Fett_4 = Mittagessen_Selected_4.Fett;
                        Mittagessen_Kohlenhydrate_4 = Mittagessen_Selected_4.KHydrate;
                        Mittagessen_Proteins_4 = Mittagessen_Selected_4.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Energie_4)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Fett_4)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Kohlenhydrate_4)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mittagessen_Proteins_4)));
                    }
                }
                if (mahlzeit == "Abendessen")
                {
                    if (zeile == 1)
                    {
                        Abendessen_Selected_1.Energie = Listview.Skip(i).First().Energie;
                        Abendessen_Selected_1.Fett = Listview.Skip(i).First().Fett;
                        Abendessen_Selected_1.KHydrate = Listview.Skip(i).First().KHydrate;
                        Abendessen_Selected_1.Protein = Listview.Skip(i).First().Protein;

                        Abendessen_Energie_1 = Abendessen_Selected_1.Energie;
                        Abendessen_Fett_1 = Abendessen_Selected_1.Fett;
                        Abendessen_Kohlenhydrate_1 = Abendessen_Selected_1.KHydrate;
                        Abendessen_Proteins_1 = Abendessen_Selected_1.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Energie_1)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Fett_1)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Kohlenhydrate_1)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Proteins_1)));
                    }
                    if (zeile == 2)
                    {
                        Abendessen_Selected_2.Energie = Listview.Skip(i).First().Energie;
                        Abendessen_Selected_2.Fett = Listview.Skip(i).First().Fett;
                        Abendessen_Selected_2.KHydrate = Listview.Skip(i).First().KHydrate;
                        Abendessen_Selected_2.Protein = Listview.Skip(i).First().Protein;

                        Abendessen_Energie_2 = Abendessen_Selected_2.Energie;
                        Abendessen_Fett_2 = Abendessen_Selected_2.Fett;
                        Abendessen_Kohlenhydrate_2 = Abendessen_Selected_2.KHydrate;
                        Abendessen_Proteins_2 = Abendessen_Selected_2.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Energie_2)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Fett_2)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Kohlenhydrate_2)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Proteins_2)));
                    }
                    if (zeile == 3)
                    {
                        Abendessen_Selected_3.Energie = Listview.Skip(i).First().Energie;
                        Abendessen_Selected_3.Fett = Listview.Skip(i).First().Fett;
                        Abendessen_Selected_3.KHydrate = Listview.Skip(i).First().KHydrate;
                        Abendessen_Selected_3.Protein = Listview.Skip(i).First().Protein;

                        Abendessen_Energie_3 = Abendessen_Selected_3.Energie;
                        Abendessen_Fett_3 = Abendessen_Selected_3.Fett;
                        Abendessen_Kohlenhydrate_3 = Abendessen_Selected_3.KHydrate;
                        Abendessen_Proteins_3 = Abendessen_Selected_3.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Energie_3)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Fett_3)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Kohlenhydrate_3)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Proteins_3)));
                    }
                    if (zeile == 4)
                    {
                        Abendessen_Selected_4.Energie = Listview.Skip(i).First().Energie;
                        Abendessen_Selected_4.Fett = Listview.Skip(i).First().Fett;
                        Abendessen_Selected_4.KHydrate = Listview.Skip(i).First().KHydrate;
                        Abendessen_Selected_4.Protein = Listview.Skip(i).First().Protein;

                        Abendessen_Energie_4 = Abendessen_Selected_4.Energie;
                        Abendessen_Fett_4 = Abendessen_Selected_4.Fett;
                        Abendessen_Kohlenhydrate_4 = Abendessen_Selected_4.KHydrate;
                        Abendessen_Proteins_4 = Abendessen_Selected_4.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Energie_4)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Fett_4)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Kohlenhydrate_4)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abendessen_Proteins_4)));
                    }
                }
                if (mahlzeit == "Sonstiges")
                {
                    if (zeile == 1)
                    {
                        Sonstiges_Selected_1.Energie = Listview.Skip(i).First().Energie;
                        Sonstiges_Selected_1.Fett = Listview.Skip(i).First().Fett;
                        Sonstiges_Selected_1.KHydrate = Listview.Skip(i).First().KHydrate;
                        Sonstiges_Selected_1.Protein = Listview.Skip(i).First().Protein;

                        Sonstiges_Energie_1 = Sonstiges_Selected_1.Energie;
                        Sonstiges_Fett_1 = Sonstiges_Selected_1.Fett;
                        Sonstiges_Kohlenhydrate_1 = Sonstiges_Selected_1.KHydrate;
                        Sonstiges_Proteins_1 = Sonstiges_Selected_1.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Energie_1)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Fett_1)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Kohlenhydrate_1)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Proteins_1)));
                    }
                    if (zeile == 2)
                    {
                        Sonstiges_Selected_2.Energie = Listview.Skip(i).First().Energie;
                        Sonstiges_Selected_2.Fett = Listview.Skip(i).First().Fett;
                        Sonstiges_Selected_2.KHydrate = Listview.Skip(i).First().KHydrate;
                        Sonstiges_Selected_2.Protein = Listview.Skip(i).First().Protein;

                        Sonstiges_Energie_2 = Sonstiges_Selected_2.Energie;
                        Sonstiges_Fett_2 = Sonstiges_Selected_2.Fett;
                        Sonstiges_Kohlenhydrate_2 = Sonstiges_Selected_2.KHydrate;
                        Sonstiges_Proteins_2 = Sonstiges_Selected_2.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Energie_2)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Fett_2)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Kohlenhydrate_2)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Proteins_2)));
                    }
                    if (zeile == 3)
                    {
                        Sonstiges_Selected_3.Energie = Listview.Skip(i).First().Energie;
                        Sonstiges_Selected_3.Fett = Listview.Skip(i).First().Fett;
                        Sonstiges_Selected_3.KHydrate = Listview.Skip(i).First().KHydrate;
                        Sonstiges_Selected_3.Protein = Listview.Skip(i).First().Protein;

                        Sonstiges_Energie_3 = Sonstiges_Selected_3.Energie;
                        Sonstiges_Fett_3 = Sonstiges_Selected_3.Fett;
                        Sonstiges_Kohlenhydrate_3 = Sonstiges_Selected_3.KHydrate;
                        Sonstiges_Proteins_3 = Sonstiges_Selected_3.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Energie_3)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Fett_3)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Kohlenhydrate_3)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Proteins_3)));
                    }
                    if (zeile == 4)
                    {
                        Sonstiges_Selected_4.Energie = Listview.Skip(i).First().Energie;
                        Sonstiges_Selected_4.Fett = Listview.Skip(i).First().Fett;
                        Sonstiges_Selected_4.KHydrate = Listview.Skip(i).First().KHydrate;
                        Sonstiges_Selected_4.Protein = Listview.Skip(i).First().Protein;

                        Sonstiges_Energie_4 = Sonstiges_Selected_4.Energie;
                        Sonstiges_Fett_4 = Sonstiges_Selected_4.Fett;
                        Sonstiges_Kohlenhydrate_4 = Sonstiges_Selected_4.KHydrate;
                        Sonstiges_Proteins_4 = Sonstiges_Selected_4.Protein;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Energie_4)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Fett_4)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Kohlenhydrate_4)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sonstiges_Proteins_4)));
                    }
                }
            }
        }
        public void tagesübersicht()
        {
            double Frühstück_Energie = Frühstück_Energie_1 + Frühstück_Energie_2 + Frühstück_Energie_3 + Frühstück_Energie_4;
            double Frühstück_Fett = Frühstück_Fett_1 + Frühstück_Fett_2 + Frühstück_Fett_3 + Frühstück_Fett_4;
            double Frühstück_KHydrate = Frühstück_Kohlenhydrate_1 + Frühstück_Kohlenhydrate_2 + Frühstück_Kohlenhydrate_3 + Frühstück_Kohlenhydrate_4;
            double Frühstück_Protein = Frühstück_Proteins_1 + Frühstück_Proteins_2 + Frühstück_Proteins_3 + Frühstück_Proteins_4;

            double Mittagessen_Energie = Mittagessen_Energie_1 + Mittagessen_Energie_2 + Mittagessen_Energie_3 + Mittagessen_Energie_4;
            double Mittagessen_Fett = Mittagessen_Fett_1 + Mittagessen_Fett_2 + Mittagessen_Fett_3 + Mittagessen_Fett_4;
            double Mittagessen_KHydrate = Mittagessen_Kohlenhydrate_1 + Mittagessen_Kohlenhydrate_2 + Mittagessen_Kohlenhydrate_3 + Mittagessen_Kohlenhydrate_4;
            double Mittagessen_Protein = Mittagessen_Proteins_1 + Mittagessen_Proteins_2 + Mittagessen_Proteins_3 + Mittagessen_Proteins_4;

            double Abendessen_Energie = Abendessen_Energie_1 + Abendessen_Energie_2 + Abendessen_Energie_3 + Abendessen_Energie_4;
            double Abendessen_Fett = Abendessen_Fett_1 + Abendessen_Fett_2 + Abendessen_Fett_3 + Abendessen_Fett_4;
            double Abendessen_KHydrate = Abendessen_Kohlenhydrate_1 + Abendessen_Kohlenhydrate_2 + Abendessen_Kohlenhydrate_3 + Abendessen_Kohlenhydrate_4;
            double Abendessen_Protein = Abendessen_Proteins_1 + Abendessen_Proteins_2 + Abendessen_Proteins_3 + Abendessen_Proteins_4;

            double Sonstiges_Energie = Sonstiges_Energie_1 + Sonstiges_Energie_2 + Sonstiges_Energie_3 + Sonstiges_Energie_4;
            double Sonstiges_Fett = Sonstiges_Fett_1 + Sonstiges_Fett_2 + Sonstiges_Fett_3 + Sonstiges_Fett_4;
            double Sonstiges_KHydrate = Sonstiges_Kohlenhydrate_1 + Sonstiges_Kohlenhydrate_2 + Sonstiges_Kohlenhydrate_3 + Sonstiges_Kohlenhydrate_4;
            double Sonstiges_Protein = Sonstiges_Proteins_1 + Sonstiges_Proteins_2 + Sonstiges_Proteins_3 + Sonstiges_Proteins_4;

            Ist_Zunahmen = Frühstück_Energie + Mittagessen_Energie + Abendessen_Energie + Sonstiges_Energie;
            Ist_Fett = Frühstück_Fett + Mittagessen_Fett + Abendessen_Fett + Sonstiges_Fett;
            Ist_Kohlenhydrate = Frühstück_KHydrate + Mittagessen_KHydrate + Abendessen_KHydrate + Sonstiges_KHydrate;
            Ist_Proteins = Frühstück_Protein + Mittagessen_Protein + Abendessen_Protein + Sonstiges_Protein;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ist_Zunahmen)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ist_Fett)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ist_Kohlenhydrate)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ist_Proteins)));

            Diff_Zunahmen = Soll_Zunahmen - Ist_Zunahmen;
            Diff_Fett = Soll_Fett - Ist_Fett;
            Diff_Kohlenhydrate = Soll_Kohlenhydrate - Ist_Kohlenhydrate;
            Diff_Proteins = Soll_Proteins - Ist_Proteins;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Diff_Zunahmen)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Diff_Fett)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Diff_Kohlenhydrate)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Diff_Proteins)));
        }

    


        // HilfsMethoden
        private ProduktListe cloneProduktData(ProduktListe daten)
        {
            ProduktListe clone = new ProduktListe();

            clone.Bez = daten.Bez;
            clone.Kat = daten.Kat;
            clone.Energie = daten.Energie;
            clone.Fett = daten.Fett;
            clone.KHydrate = daten.KHydrate;
            clone.Balaststoffe = daten.Balaststoffe;
            clone.Protein = daten.Protein;

            return clone;

        }
    }
}
