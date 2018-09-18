using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace NeuralViewer.Screen
{
    class PixelLayer : NumbersRepresentation<Rectangle>
    {
        public PixelLayer(Canvas screen) : base(screen)
        {
        }

        public override void Redraw()
        {
            throw new NotImplementedException();
        }

        protected override void LayerScreen_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
