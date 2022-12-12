using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Umnik
{
    public partial class DronesForm : Form
    {
        internal DronesForm()
        {
            InitializeComponent();
        }

        private void btnAddDrone_Click(object sender, EventArgs e)
        {
            DronesManager.AddDrone();
        }

        private void btnRemoveDrone_Click(object sender, EventArgs e)
        {
            var listOfCheckedItems = checkedListBoxOfDrones.CheckedItems.Count;
            // С каждой итерацией checkedItems.Count становится на 1 меньше
            // поэтому всегда удаляем 0 элемент
            for (int i = 0; i < listOfCheckedItems; i++)
            {
                Drone? drone = DronesManager.Drones?.FirstOrDefault(x => x.Name == (string)checkedListBoxOfDrones.CheckedItems[0]);
                DronesManager.RemoveDrone(drone);
                checkedListBoxOfDrones.Items.Remove(checkedListBoxOfDrones.CheckedItems[0]);
            }
        }

        private void DronesForm_Load(object sender, EventArgs e)
        {
            if (DronesManager.Drones.Count != 0)
            {
                foreach (var drone in DronesManager.Drones)
                {
                    checkedListBoxOfDrones.Items.Add(drone.Name);
                }
            }

            DronesManager.OnAddDroneEvent += OnAddDrone;
            DronesManager.OnRemoveDroneEvent += OnRemoveDrone;
        }

        private void OnAddDrone(Drone drone)
        {
            checkedListBoxOfDrones.Items.Add(drone.Name);
        }

        private void OnRemoveDrone(Drone drone)
        {
            checkedListBoxOfDrones.Items.Add(drone.Name);
        }
    }
}
