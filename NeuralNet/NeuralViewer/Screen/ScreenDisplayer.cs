using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NeuralViewer.Screen
{

    /// <summary>
    /// This Class is used to display all screen elements into spacialy prepered canvas
    /// </summary>

    class ScreenDisplayer
    {
        Canvas mainScreen;
        List<ClassicLayer> testLayers;

        public ScreenDisplayer(Canvas screen)
        {
            mainScreen = screen;

            //************* For test puproses we are creating 5 testLayers *************************//

            testLayers = new List<ClassicLayer>();
            for (int i = 0; i < 4; i++)
            {
                Canvas lscreen = new Canvas();
                lscreen.Height = mainScreen.Height / 5;
                lscreen.Width = mainScreen.Width;

                Canvas.SetTop(lscreen, i * lscreen.Height);
                mainScreen.Children.Add(lscreen);

                testLayers.Add(new ClassicLayer(lscreen, 8 + i * 20));
            }

            Canvas lss = new Canvas();
            lss.Height = mainScreen.Height / 5;
            lss.Width = mainScreen.Width;

            Canvas.SetTop(lss, 4 * lss.Height);
            mainScreen.Children.Add(lss);

            var pL = new PixelLayer(lss, 32);
        }


    }
}
