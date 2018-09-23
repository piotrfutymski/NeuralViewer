using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NeuralViewer.Screen
{
    class WeightScreen
    {
        ScreenLayer weightLayer;
        protected Canvas weightScreen;
        int numOfNeurons;

        public WeightScreen(Canvas screen, int num)
        {
            weightScreen = screen;
            numOfNeurons = num;

            weightLayer = new PixelLayer(screen, num, 0);
            weightLayer.SetSetting(ScreenLayer.NumberRepresentationSettings.IsWhiteBlack, 0);
        }

        public void SetValues(double [] v)
        {
            if(v.Length == numOfNeurons)
            {
                weightLayer.SetNeurons(v, 0);
            }
        }


    }
}
