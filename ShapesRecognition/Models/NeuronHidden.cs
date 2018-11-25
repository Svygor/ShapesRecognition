using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesRecognition.Models
{
    public class NeuronHidden:Neuron
    {
        public double[] NeuronWeightsNext { get; set; }
        public double[] DeltasNext { get; set; }
        /// <summary>
        /// Вычисляем дельту
        /// </summary>
        /// <param name="weights">массив весов этого нейрона для нейронов следующего слоя</param>
        /// <param name="deltas">массив дельт нейронов следующего слоя</param>
        public void CalculateDelta()
        {
            double deltaSum = 0;
            for (int i = 0; i < DeltasNext.Length; i++)
            {
                deltaSum += NeuronWeightsNext[i] * DeltasNext[i];
            }

            Delta = deltaSum;
        }
    }
}
