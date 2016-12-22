using IceCream.Model;

namespace IceCream.Controller
{
    public interface IStationsView
    {
        void SetController(StationsController controller);
        void ClearGrid();
        void AddStationToGrid(Station station);
        void RemoveStationFromGrid(Station station);
        void SetSelectedStationInGrid(Station station);
        void UpdateGridWithChangedStation(Station station);
        string GetIdOfSelectedStationInGrid();
        void NotifyIcon();

        string StationID { get; set; }
        string Datum { get; set; }
        int Ziel { get; set; }
        int Aktuell { get; set; }
        int Varianz { get; set; }
        bool CanModifyID { set; }
        bool CanModifyTarget { set; }
        string Color { set; }
        bool StationRandAdded { set; }
    }
}