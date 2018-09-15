using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NeuralViewer
{

    /// <summary>
    /// This Class is used to display all screen elements into spacialy prepered canvas
    /// </summary>

    class ScreenDisplayer
    {
        Canvas mainScreen;
        List<TestLayerRepresentation> testLayers;

        public ScreenDisplayer(Canvas screen)
        {
            mainScreen = screen;

            //************* For test puproses we are creating 5 testLayers *************************//

            testLayers = new List<TestLayerRepresentation>();

            for (int i = 0; i < 5; i++)
            {
                Canvas lscreen = new Canvas();
                lscreen.Height = mainScreen.Height / 5;
                lscreen.Width = mainScreen.Width;

                Canvas.SetTop(lscreen, i * lscreen.Height);
                mainScreen.Children.Add(lscreen);

                testLayers.Add(new TestLayerRepresentation(lscreen));
            }
        }


    }
}
