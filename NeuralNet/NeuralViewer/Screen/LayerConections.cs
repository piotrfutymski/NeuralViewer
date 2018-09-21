using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace NeuralViewer.Screen
{
    class LayerConections
    {
        public Conection[,] Conections;
        
        public LayerConections(int thislayernum, int backlayernum)
        {
            Conections = new Conection[thislayernum, backlayernum];
            for (int i = 0; i < thislayernum; i++)
            {
                for (int j = 0; j < backlayernum; j++)
                {
                    Conections[i, j] = new Conection();
                }
            }
        }
    }
}
