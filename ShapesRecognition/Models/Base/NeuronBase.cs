using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesRecognition.Models.Base
{
    public abstract class NeuronBase
    {
        /// <summary>
        /// Выходной сигнал
        /// </summary>
        public double Output { get; set; }

        /// <summary>
        /// Входные сигналы
        /// </summary>
        public double[] Inputs { get; set; }

        /// <summary>
        /// Сумма взвешенных входных сигналов
        /// </summary>
        public double Input { get; set; }

        /// <summary>
        /// Активационная функция
        /// </summary>
        /// <returns></returns>
        public abstract void CalculateActivationFunc();
    }
}
