using System;
using System.IO;
using System.Windows.Forms;

namespace IceCream.Model
{
    public class Station
    {
        public const string PATH = @"DATA.txt";
        private string _StationID;
        public string StationID
        {
            get { return _StationID; }
            set
            {
                if (value.Length > 10 || value.Length == 0)
                    MessageBox.Show("StationID ist nicht angegeben oder zu lang!");
                else
                    _StationID = value;
            }
        }
        private string _Datum;
        public string Datum
        {
            get { return _Datum; }
            set
            {
                if (value.Length > 10 || value.Length ==0)
                    MessageBox.Show("Datum ist nicht angegeben oder zu lang!");
                else
                    _Datum = value;
            }
        }
        private int _Ziel;
        public int Ziel
        {
            get { return _Ziel; }
            set {_Ziel = value; }
        }
        private int _Aktuell;
        public int Aktuell
        {
            get { return _Aktuell; }
            set {_Aktuell = value; }
        }
        private int _Varianz;
        public int Varianz
        {
            get { return _Varianz; }
            set { _Varianz = value; }
        }
        public Station(string id, string datum, int ziel, int aktuell)
        {
            StationID = id;
            Datum = datum;
            Ziel = ziel;
            Aktuell = aktuell;
            Varianz = ziel - aktuell;
        }
        public void updateFileWithStation(Station station)
        {
            string[] lines = File.ReadAllText(PATH).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string text = "";
            foreach (string line in lines)
            {
                string[] txt = line.Split(';');
                if (txt[0] == station.StationID)
                {
                    txt[1] = station.Datum;
                    txt[2] = station.Ziel.ToString();
                    txt[3] = station.Aktuell.ToString();
                    txt[4] = (station.Ziel - station.Aktuell).ToString();
                    text += txt[0] + ';' + txt[1] + ';' + txt[2] + ';' + txt[3] + ';' + txt[4]+'\r'+'\n';
                }
                else
                {
                    text += line+'\r'+'\n';
                }
            }
            File.WriteAllText(PATH, text);
        }
        public void addStationToFile(Station station)
        {
            using (StreamWriter sw = new StreamWriter(PATH,true))
            {
                sw.WriteLine(station.StationID + ';' + station.Datum + ';' + station.Ziel + ';' + station.Aktuell + ';' + (station.Ziel - station.Aktuell));
            }
        }
        public void removeStationFromFile(Station station)
        {
            string[] lines = File.ReadAllText(PATH).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string text = "";
            foreach (string line in lines)
            {
                string[] txt = line.Split(';');
                if (txt[0] == station.StationID)
                {
                    continue;
                }
                else
                {
                    text += line + '\r' + '\n';
                }
            }
            File.WriteAllText(PATH, text); //.TrimEnd('\r').TrimEnd('\n')
        }
    }
}
