using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesRecognition.Models
{
    public class NeuronOutput:Neuron
    {
        /// <summary>
        /// Вычисляем дельту
        /// </summary>
        /// <param name="idelaOutput">ожидаемое значение на выходе нейрона</param>
        public void CalculateDelta(double idelaOutput)
        {
            //Delta = (idelaOutput - Output) * (1 - Output) * Output;
            Delta = (idelaOutput - Output);
        }
    }
}
