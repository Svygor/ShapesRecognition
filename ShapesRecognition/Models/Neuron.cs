using ShapesRecognition.Models.Base;
using ShapesRecognition.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesRecognition.Models
{
    public class Neuron : NeuronBase
    {
        /// <summary>
        /// Веса синапсов нейрона
        /// </summary>
        public double[] Weights { get; set; }

        /// <summary>
        /// дельта - пареметр обучения, зависящий от ошибки и входной величины
        /// </summary>
        public double Delta { get; set; }

        //зменения весов (нужны для вычисления следующего изменения веса)
        //Изначально все = 0
        public double[] _weightsChanges;

        /// <summary>
        /// Подсчет взвешенной суммы входных сигналов
        /// </summary>
        /// <param name="inputs"></param>
        public void CalculateInput(double[] inputs)
        {
            if (Weights == null)
            {
                InitializeWeights(inputs.Length);
            }

            double sum = 0;
            Inputs = inputs;

            //перемонажем каждый входной сигнал на соотствующий ему вес
            for (int i = 0; i < Inputs.Length; i++)
            {
                sum += Inputs[i] * Weights[i];
            }

            Input = sum;
        }

        /// <summary>
        /// Инициализируем веса
        /// </summary>
        /// <param name="length"></param>
        private void InitializeWeights(int length)
        {
            Weights = new double[length];
            
            //Искренне надеюсь, что значения в массиве по умолчанию будут = 0
            _weightsChanges = new double[length];

            for (int i = 0; i < length; i++)
            {
                Weights[i] = RNG.GetInstanse().GetWeight();
            }
        }

        /// <summary>
        /// Активационная функциона. Сигмоид
        /// </summary>
        public override void CalculateActivationFunc()
        {
            Output = 1 / (1 + Math.Exp(-Input));
        }

        /// <summary>
        /// Обновляем веса синапсов нейрона
        /// </summary>
        /// <param name="learningSpeed">скорость обучения</param>
        /// <param name="momentum">момент</param>
        public void UpdateWeights(double learningSpeed, double momentum)
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                _weightsChanges[i] = learningSpeed * Delta * Inputs[i] + momentum * _weightsChanges[i];
                Weights[i] += _weightsChanges[i];
            }
        }
    }
}
