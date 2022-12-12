using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Umnik
{
    public partial class DronesForm : Form
    {
        internal List<UAV> listOfUAVs = new List<UAV>();
        internal List<GMapOverlay> listOfOverlaysForRemoving = new List<GMapOverlay>();
        internal DronesForm(ref List<UAV> list, ref List<GMapOverlay> overlaysForRemoving)
        {
            InitializeComponent();
            listOfUAVs = list;
            listOfOverlaysForRemoving = overlaysForRemoving;
            foreach (var item in listOfUAVs)
            {
                checkedListBoxOfDrones.Items.Add(item.Name);
            }

        }

        private List<int> GetListOfColours(int maxDroneColour)
        {
            List<int> colours = new List<int>();
            for (int i = 0; i < maxDroneColour; i++)
            {
                colours.Add(i);
            }
            return colours;
        }

        private void btnAddDrone_Click(object sender, EventArgs e)
        {
            if (listOfUAVs.Count < (int)DroneColour.MaxDroneColour)
            {
                List<int> listOfAvailableColours = GetListOfColours((int)DroneColour.MaxDroneColour);
                for (int i = 0; i < listOfUAVs.Count; i++)
                {
                    for (int j = 0; j < (int)DroneColour.MaxDroneColour; j++)
                    {
                        if (listOfUAVs[i].Name == $"Drone {j}")
                        {
                            listOfAvailableColours.Remove(j);
                        }
                    }
                }
                int numberForColourBitmap = listOfAvailableColours.FirstOrDefault();
                double addLat = 0.1;
                double addLong = 0.1;
                UAV newDrone = new UAV(numberForColourBitmap, new PointLatLng(43.9151144529437 + numberForColourBitmap * addLat, 42.7288770675659 + numberForColourBitmap * addLong));

                listOfUAVs.Add(newDrone);
                checkedListBoxOfDrones.Items.Add(newDrone.Name);
            }
        }

        private void btnRemoveDrone_Click(object sender, EventArgs e)
        {
            var listOfCheckedItems = checkedListBoxOfDrones.CheckedItems.Count;
            // С каждой итерацией checkedItems.Count становится на 1 меньше
            // поэтому всегда удаляем 0 элемент
            for (int i = 0; i < listOfCheckedItems; i++)
            {
                UAV uav = listOfUAVs.FirstOrDefault(x => x.Name == (string)checkedListBoxOfDrones.CheckedItems[0]);
                listOfOverlaysForRemoving.Add(uav.MarkersOverlay);
                listOfOverlaysForRemoving.Add(uav.PolygonsOverlay);
                listOfOverlaysForRemoving.Add(uav.RoutesOverlay);
                listOfUAVs.Remove(uav);
                checkedListBoxOfDrones.Items.Remove(checkedListBoxOfDrones.CheckedItems[0]);
            }
        }
    }
}
