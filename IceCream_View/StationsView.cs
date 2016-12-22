using System;
using System.Drawing;
using System.Windows.Forms;
using IceCream.Controller;
using IceCream.Model;

namespace IceCream.View
{
    public partial class StationsView : Form, IStationsView
    {
        public StationsView()
        {
            InitializeComponent();
        }
        StationsController _controller;
        #region implementation
        public void SetController(StationsController controller)
        {
            _controller = controller;
        }
        public void ClearGrid()
        {
            this.gridstations.Columns.Clear();
            this.gridstations.Columns.Add("Station ID", 120, HorizontalAlignment.Left);
            this.gridstations.Columns.Add("Date", 120, HorizontalAlignment.Left);
            this.gridstations.Columns.Add("Target", 120, HorizontalAlignment.Left);
            this.gridstations.Columns.Add("Actual", 120, HorizontalAlignment.Left);
            this.gridstations.Columns.Add("Variance", 120, HorizontalAlignment.Left);
            this.gridstations.Items.Clear();
        }
        public void AddStationToGrid(Station station)
        {
            ListViewItem parent;
            parent = this.gridstations.Items.Add(station.StationID);
            parent.SubItems.Add(station.Datum);
            parent.SubItems.Add(station.Ziel.ToString());
            parent.SubItems.Add(station.Aktuell.ToString());
            parent.SubItems.Add(station.Varianz.ToString());
        }
        public void SetSelectedStationInGrid(Station station)
        {
            foreach(ListViewItem row in this.gridstations.Items)
            {
                if(row.Text == station.StationID)
                {
                    row.Selected = true;
                }
            }
        }
        public void UpdateGridWithChangedStation(Station station)
        {
            ListViewItem rowToUpdate = null;
            foreach(ListViewItem row in this.gridstations.Items)
            {
                if(row.Text == station.StationID)
                {
                    rowToUpdate = row;
                }
            }
            if(rowToUpdate != null)
            {
                rowToUpdate.Text = station.StationID;
                rowToUpdate.SubItems[1].Text = station.Datum;
                rowToUpdate.SubItems[2].Text = station.Ziel.ToString();
                rowToUpdate.SubItems[3].Text = station.Aktuell.ToString();
                rowToUpdate.SubItems[4].Text = station.Varianz.ToString();
            }
        }
        public void RemoveStationFromGrid(Station station)
        {
            ListViewItem rowToRemove = null;
            foreach (ListViewItem row in this.gridstations.Items)
            {
                if (row.Text == station.StationID)
                {
                    rowToRemove = row;
                }
            }
            if (rowToRemove != null)
            {
                this.gridstations.Items.Remove(rowToRemove);
                this.gridstations.Focus();
            }
        }
        public string GetIdOfSelectedStationInGrid()
        {
            if (this.gridstations.SelectedItems.Count > 0)
                return this.gridstations.SelectedItems[0].Text;
            else
                return "";
        }
        public void NotifyIcon()
        {
            this.notifyIcon1.ShowBalloonTip(1000);
        }
        public string StationID
        {
            get { return this.txtID.Text; }
            set { this.txtID.Text = value; }
        }
        public string Datum
        {
            get { return this.txtDate.Text; }
            set { this.txtDate.Text = value; }
        }
        public int Ziel
        {
            get { return Int32.Parse(this.txtTarget.Text); }
            set { this.txtTarget.Text = value.ToString(); }
        }
        public int Aktuell
        {
            get { return Int32.Parse(this.txtActual.Text); }
            set { this.txtActual.Text = value.ToString(); }
        }
        public int Varianz
        {
            get { return Int32.Parse(this.txtVariance.Text); }
            set { this.txtVariance.Text = value.ToString(); }
        }
        public bool CanModifyID
        {
            set { this.txtID.Enabled = value; }
        }
        public bool CanModifyTarget
        {
            set { this.txtTarget.Enabled = value; }
        }
        public string Color
        {
            set
            {
                Color color = System.Drawing.Color.White;
                if (value == "red")
                {
                    color = System.Drawing.Color.Tomato;
                }
                if (value == "green")
                {
                    color = System.Drawing.Color.GreenYellow;
                }
                this.txtVariance.BackColor = color;
            }
        }
        public bool StationRandAdded
        {
            set { this.RandAdded.Visible = value;   }
        }
        #endregion
        #region events
        private void gridstations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.gridstations.SelectedItems.Count > 0)
                this._controller.SelectedStationChanged(this.gridstations.SelectedItems[0].Text,0);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this._controller.AddNewStation(0);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            this._controller.Save(0);
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            this._controller.RemoveStation(0);
        }
        private void txtActual_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void txtTarget_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void randStation_Tick(object sender, EventArgs e)
        {
            this._controller.RandStation();
        }
        private void StationsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Dispose();
        }
        #endregion
    }
}