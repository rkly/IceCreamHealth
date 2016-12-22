using System;
using System.Linq;
using System.Threading.Tasks;
using IceCream.Model;
using System.Collections;
using System.IO;

namespace IceCream.Controller
{
    public class StationsController
    {
        IStationsView _view;
        IStationsView _view2;
        IList _stations;
        Station _selectedStation;
        public StationsController(IStationsView view, IStationsView view2, IList stations)
        {
            stations = StationsFromFile(stations);
            _view = view;
            _view2 = view2;
            _stations = stations;
            view.SetController(this);
            view2.SetController(this);
        }
        private IList StationsFromFile(IList stations)
        {
            using (StreamReader sr = File.OpenText(@"DATA.txt"))
            {
                string line;
                string[] station;                
                while (sr.Peek() >= 0)
                {
                    line = sr.ReadLine();
                    station = line.Split(';');
                    if(stations.Count == 0)
                    { 
                        stations.Add(new Station(station[0], station[1], Int32.Parse(station[2]), Int32.Parse(station[3])));
                    }
                    else
                    {
                        foreach (Station st in stations)
                        {
                            if (!station[0].Contains(st.StationID))
                            {
                                stations.Add(new Station(station[0], station[1], Int32.Parse(station[2]), Int32.Parse(station[3])));
                                break;
                            }
                        }
                    }
                }
            }
            return stations; 
        }
        private void updateViewDetailValues(Station station, int a)
        {
            if (a == 0)
            {
                _view.StationID = station.StationID;
                _view.Datum = station.Datum;
                _view.Aktuell = station.Aktuell;
                _view.Ziel = station.Ziel;
                _view.Varianz = station.Ziel - station.Aktuell;
                updateColor(station,0);
            }
            if (a == 1)
            {
                _view2.StationID = station.StationID;
                _view2.Datum = station.Datum;
                _view2.Aktuell = station.Aktuell;
                _view2.Ziel = station.Ziel;
                _view2.Varianz = station.Ziel - station.Aktuell;
                updateColor(station,1);
            }        
        }
        private void updateStationWithViewValues(Station station)
        {
            station.StationID = _view.StationID;
            station.Datum = _view.Datum;
            station.Aktuell = _view.Aktuell;
            station.Ziel = _view.Ziel;
            station.Varianz = _view.Ziel - _view.Aktuell;
        }
        private void updateColor(Station station, int a)
        {
            float target = station.Ziel;
            float actual = station.Aktuell;
            if(target != 0 && actual < target && Math.Abs(actual)/Math.Abs(target) < 0.1)
            {
                if (a == 0)
                {
                    this._view.Color = "red";
                }
                if(a == 1)
                {
                    this._view2.Color = "red";
                }             
            }
            else if(actual != 0 && target < actual && 1f-Math.Abs(target)/Math.Abs(actual) > 0.05)
            {
                if (a == 0)
                {
                    this._view.Color = "green";
                }
                if (a == 1)
                {
                    this._view2.Color = "green";
                }
            }
            else
            {
                if (a == 0)
                {
                    this._view.Color = "default";
                }
                if (a == 1)
                {
                    this._view2.Color = "default";
                }
            }
        }
        public void LoadView()
        {
            _view.ClearGrid();
            _view2.ClearGrid();
            foreach (Station station in _stations)
            {
                _view.AddStationToGrid(station);
                _view2.AddStationToGrid(station);
            }
                
            _view.SetSelectedStationInGrid((Station)_stations[0]);
            _view2.SetSelectedStationInGrid((Station)_stations[0]);
        }
        public void SelectedStationChanged(string selectedStationID, int a)
        {
            foreach(Station station in this._stations)
            {
                if(station.StationID == selectedStationID && a == 0)
                {
                    _selectedStation = station;
                    updateViewDetailValues(station,0);
                    _view.SetSelectedStationInGrid(station);
                    this._view.CanModifyID = false;
                    this._view.CanModifyTarget = false;
                    break;
                }
                if(station.StationID == selectedStationID && a == 1)
                {
                    _selectedStation = station;
                    updateViewDetailValues(station, 1);
                    _view2.SetSelectedStationInGrid(station);
                    this._view2.CanModifyID = false;
                    this._view2.CanModifyTarget = false;
                    break;
                }
            }
        }
        public void AddNewStation(int a)
        {
            _selectedStation = new Station("0", "0", 0, 0);
            this.updateViewDetailValues(_selectedStation,a);
            this._view.CanModifyID = true;
            this._view.CanModifyTarget = true;
            this._view2.CanModifyID = true;
            this._view2.CanModifyTarget = true;
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "qwertzuiopasdfghjklyxcvbnm0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async void RandStation()
        {
            Random rand = new Random();
            Station _randStation = new Station(RandomString(10), DateTime.Today.ToString("d"), rand.Next(1, 10000), rand.Next(1, 10000));
            if (!this._stations.Contains(_randStation))
            {
                this._stations.Add(_randStation);
                this._view.AddStationToGrid(_randStation);
                this._view2.AddStationToGrid(_randStation);
                _randStation.addStationToFile(_randStation);
            }
            else
            {
                this._view.UpdateGridWithChangedStation(_randStation);
                this._view2.UpdateGridWithChangedStation(_randStation);
                updateViewDetailValues(_randStation,0);
                _randStation.updateFileWithStation(_randStation);
            }
            this._view.NotifyIcon();
            this.SetStationLabel(true);
            await Task.Delay(3000);
            this.SetStationLabel(false);           
        }
        public void SetStationLabel(bool value)
        {
            this._view.StationRandAdded = value;
            this._view2.StationRandAdded = value;
        }
        public void Save(int a)
        {
            updateStationWithViewValues(_selectedStation);
            if (!this._stations.Contains(_selectedStation))
            {
                this._stations.Add(_selectedStation);
                this._view.AddStationToGrid(_selectedStation);
                this._view2.AddStationToGrid(_selectedStation);
                _selectedStation.addStationToFile(_selectedStation);
            }
            else
            {
                this._view.UpdateGridWithChangedStation(_selectedStation);
                this._view2.UpdateGridWithChangedStation(_selectedStation);
                updateViewDetailValues(_selectedStation,a);
                _selectedStation.updateFileWithStation(_selectedStation);
            }
            _view.SetSelectedStationInGrid(_selectedStation);
            _view2.SetSelectedStationInGrid(_selectedStation);
            this._view.CanModifyID = false;
            this._view.CanModifyTarget = false;
            this._view2.CanModifyID = false;
            this._view2.CanModifyTarget = false;
        }
        public void RemoveStation(int a)
        {
            string id = "";
            if(a == 0)
            {
                id = this._view.GetIdOfSelectedStationInGrid();
            }
            if(a ==1)
            {
                id = this._view2.GetIdOfSelectedStationInGrid();
            }
            Station stationToRemove = null;
            if (id != "")
            {
                foreach (Station station in this._stations)
                {
                    if (station.StationID == id)
                    {
                        stationToRemove = station;
                        break;
                    }
                }
                if (stationToRemove != null)
                {
                    int newSelectedIndex = this._stations.IndexOf(stationToRemove);
                    this._stations.Remove(stationToRemove);
                    stationToRemove.removeStationFromFile(stationToRemove);
                    this._view.RemoveStationFromGrid(stationToRemove);
                    this._view2.RemoveStationFromGrid(stationToRemove);
                    if (newSelectedIndex > -1 && newSelectedIndex < _stations.Count)
                    {
                        this._view.SetSelectedStationInGrid((Station)_stations[newSelectedIndex]);
                        this._view2.SetSelectedStationInGrid((Station)_stations[newSelectedIndex]);
                    }
                }
            }
        }
    }
}