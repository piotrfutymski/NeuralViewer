using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NeuralViewer
{
    abstract class NumbersRepresentation
    {

        protected Canvas layerScreen;
        protected List<OneNumberRepresentation> neurons;
        protected Dictionary<NumberRepresentationSettings, double> layerSettings;

        public enum NumberRepresentationSettings
        {
            Size,
            Spaces,
            NeuronsOnScreen,
            FirstNeuronOnScreen
        }

        protected abstract void LayerScreen_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e);
        public abstract void Redraw();

        public NumbersRepresentation(Canvas screen)
        {
            layerScreen = screen;
            neurons = null;
            layerScreen.MouseWheel += LayerScreen_MouseWheel;
        }

        public void SetNeurons(double [] v, int beg)
        {
            if (v.Length + beg > neurons.Count)
                throw new ArgumentException();

            for (int i = beg; i < v.Length; i++)
            {
                neurons[i].Value = v[i - beg];
            }
            Redraw();
        }

        public void SetNeuron(double v, int num)
        {
            SetNeurons(new double[] { v }, num);
        } 
        
        public void SetSetting(NumberRepresentationSettings name, double value)
        {
            if(layerSettings.ContainsKey(name))
            {
                if (name == NumberRepresentationSettings.NeuronsOnScreen && value > neurons.Count)
                    return;
                layerSettings[name] = value;
                Redraw();
            }             
            else
                throw new ArgumentException();
        }

        public void SetAllPossibleSettings(double [] v)
        {
            if (v.Length > neurons.Count)
                throw new ArgumentException();

            var keys = layerSettings.Keys.ToArray();

            for (int i = 0; i < v.Length; i++)
            {
                layerSettings[keys[i]] = v[i];
            }
            Redraw();
        }

        public double GetSetting(NumberRepresentationSettings name)
        {
            if (layerSettings.ContainsKey(name))
            {
                return layerSettings[name];
            }
            else
                throw new ArgumentException();
        }
        /*
        private bool CheckingSettingsValues(NumberRepresentationSettings name, double value)
        {
            switch (name)
            {
                case NumberRepresentationSettings.Size:
                    if()
                    break;
                case NumberRepresentationSettings.Spaces:
                    break;
                case NumberRepresentationSettings.NeuronsOnScreen:
                    break;
                case NumberRepresentationSettings.FirstNeuronOnScreen:
                    break;
                default:
                    break;
            }
        }
        */ //Dokończyć!!!

    }
}
