using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class PersonListe
    {
        public string Name { get; set; }
        public int Alter { get; set; }
        public string Geschlecht { get; set; }
        public double Grösse { get; set; }
        public double Gewicht { get; set; }
        public double Zielgewicht { get; set; }
        public string Ziel { get; set; }

        public double Grundverbrauch { get; set; }
        public double Aktivitätsfaktor { get; set; }
        public double Zwischentotal { get; set; }
        public double Anpassung { get; set; }
        public double Total { get; set; }

        public double Bedarf_Fett { get; set; }
        public double Bedarf_Kohlenhydrate { get; set; }
        public double Bedarf_Protein { get; set; }

    }
}
