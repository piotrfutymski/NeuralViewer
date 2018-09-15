using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace NeuralViewer
{
    class TestLayerRepresentation : LayerRepresentation
    {
        List<Ellipse> mNeurons;
        double spacesInPixels;

        public TestLayerRepresentation(Canvas screen, double sp = 5): base(screen)
        {
            spacesInPixels = sp;
            mNeurons = new List<Ellipse>();
            Random randomizer = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < 10; i++)
            {
                byte c = (byte)(randomizer.Next() % 256);

                SolidColorBrush randomBrush = new SolidColorBrush(Color.FromArgb(255, c, c, c));

                mNeurons.Add(new Ellipse());
                mNeurons[i].Fill = randomBrush;
                mNeurons[i].StrokeThickness = 2;
                mNeurons[i].Stroke = Brushes.AliceBlue;

                mNeurons[i].Width = (baseWidthOnScreen - 11 * spacesInPixels) / 10;
                mNeurons[i].Height = baseHeightOnScreen - 2 * spacesInPixels;

                Canvas.SetLeft(mNeurons[i], spacesInPixels + i * (spacesInPixels + mNeurons[i].Width));
                Canvas.SetTop(mNeurons[i], spacesInPixels);

                screen.Children.Add(mNeurons[i]);
            }
        }

        public override void Redraw()
        {
            //Nothing to change
        }
    }
}
