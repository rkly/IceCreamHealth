using System;
using System.Windows.Forms;
using System.Collections;
using IceCream.Model;
using IceCream.View;
using IceCream.View2;
using IceCream.Controller;
using System.IO;

namespace IceCream_App
{
    static class Program
    {
        public const string PATH = @"DATA.txt";
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            StationsView view = new StationsView();
            StationsView2 view2 = new StationsView2();
            view.Visible =  false;
            view2.Visible = false;
            IList stations = new ArrayList();
            if (!File.Exists(PATH))
            {
                StreamWriter sw = File.CreateText(PATH);
                //stations.Add(new Station("jb8tvx2k8", "1st_laucnh", 2700, 2650));
            }          
            StationsController controller = new StationsController(view, view2, stations);
            controller.LoadView();
            view2.Show();
            Application.Run(view);
        }
    }
}