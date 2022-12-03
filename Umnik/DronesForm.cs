using GMap.NET;
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
        internal DronesForm(ref List<UAV> list)
        {
            InitializeComponent();
            listOfUAVs = list;
        }


        private void btnAddDrone_Click(object sender, EventArgs e)
        {
            int numberOfDrones = checkedListBoxOfDrones.Items.Count;
            UAV newDrone = new UAV(numberOfDrones, new GMap.NET.PointLatLng(43.9151144529437, 42.7288770675659));
            listOfUAVs.Add(newDrone);
            checkedListBoxOfDrones.Items.Add(newDrone);
        }
    }
}
