using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace NeuralViewer
{
    /// <summary>
    /// Logika interakcji dla klasy LayerOptionWindow.xaml
    /// </summary>
    public partial class LayerOptionWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LayerOptionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        //BINDINGS
        private int advInt;

        public int AdvInt
        {
            get { return advInt; }
            set
            {
                if (value != advInt)
                {
                    advInt = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AdvInt"));
                }
            }
        }

        private int sizeInt;

        public int SizeInt
        {
            get { return sizeInt; }
            set
            {
                if (value != sizeInt)
                {
                    sizeInt = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SizeInt"));
                }
            }
        }
    }
}
