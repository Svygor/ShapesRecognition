using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesRecognition.Models
{
    /// <summary>
    /// Класс, реализующий нейронную сеть из 3 слоёв (вхоной, скрытый и выходной)
    /// </summary>
    public class NeuroNet
    {
        /// <summary>
        /// Входной слой нейросети (для него и нейроны особые - которые нихрена не делают)
        /// </summary>
        public NeuronInput[] InputLayer { get; set; }

        /// <summary>
        /// Скрытый слой нейросети
        /// </summary>
        public NeuronHidden[] HiddenLayer { get; set; }

        /// <summary>
        /// Выходной слой нейросети
        /// </summary>
        public NeuronOutput[] OutputLayer { get; set; }

        /// <summary>
        /// Скорость обучения (коэффициент такой, отображающий, как быстро изменяются веса нейронов)
        /// </summary>
        public double LearningSpeed { get; set; }

        /// <summary>
        /// Тоже коэффициент, влияющий на скорость обучения
        /// </summary>
        public double Momentum { get; set; }

        /// <summary>
        /// Текущий результат работы нейросети
        /// </summary>
        public double[] CurrentResult { get; set; }

        /// <summary>
        /// Конструктор сети
        /// Мб неплохо было бы передавать в конструктор количество нейронов, момент и скорость обучения в качестве параметров
        /// Тогда этим классом можно будет делать совершенно разные сети. Наверно
        /// </summary>
        public NeuroNet()
        {
            //т.к. делаем нейросеть для распознавания картинок 20х20, то входных нейронов будет 400
            //InputLayer = new NeuronInput[2];
            //for (int i = 0; i < InputLayer.Length; i++)
            //{
            //    InputLayer[i] = new NeuronInput(InputLayer.Length);
            //}

            ////цифра в 1000 нейронов в скрытом слое вообще наугад взята
            //HiddenLayer = new NeuronHidden[2];
            //for (int i = 0; i < HiddenLayer.Length; i++)
            //{
            //    HiddenLayer[i] = new NeuronHidden();
            //    HiddenLayer[i].Weights = new double[2];
            //    HiddenLayer[i]._weightsChanges = new double[2];
            //}

            ////выходных нейронов 6, т.к. мы должны на выходе получить ответ, что картинка относится к одному из 6 классов
            //OutputLayer = new NeuronOutput[1];
            //for (int i = 0; i < OutputLayer.Length; i++)
            //{
            //    OutputLayer[i] = new NeuronOutput();
            //    OutputLayer[i].Weights = new double[2];
            //    OutputLayer[i]._weightsChanges = new double[2];
            //}

            //т.к. делаем нейросеть для распознавания картинок 20х20, то входных нейронов будет 400
            InputLayer = new NeuronInput[400];
            for (int i = 0; i < InputLayer.Length; i++)
            {
                InputLayer[i] = new NeuronInput(InputLayer.Length);
            }

            //цифра в 1000 нейронов в скрытом слое вообще наугад взята
            HiddenLayer = new NeuronHidden[1000];
            for (int i = 0; i < HiddenLayer.Length; i++)
            {
                HiddenLayer[i] = new NeuronHidden();
            }

            //выходных нейронов 6, т.к. мы должны на выходе получить ответ, что картинка относится к одному из 6 классов
            OutputLayer = new NeuronOutput[6];
            for (int i = 0; i < OutputLayer.Length; i++)
            {
                OutputLayer[i] = new NeuronOutput();
            }

            //Момент и скорость обучения - это так называемые гиперпараметры. Выбираем их от балды для начала.
            Momentum = 0.3;
            LearningSpeed = 0.7;
        }

        /// <summary>
        /// Обучение нейронной сети
        /// </summary>
        /// <param name="inputsList">Список массивов входных значений</param>
        /// <param name="outputsList">Список массивов входных значений</param>
        public void Learn(List<byte[]> inputsList, List<double[]> outputsList, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < inputsList.Count; j++)
                {
                    Think(inputsList[j]);
                    UpdateWeights(CurrentResult, outputsList[j]);
                }
            }
        }

        private void UpdateWeights(double[] actualResult, double[] idealResult)
        {
            //для обновления весов скрытого слоя нужно знать вес синапса каждого из выходных нейронов, зависящих от каждого срытого нейрона
            //Поэтому каждому нейрону мы даем инфу об этом
            for (int i = 0; i < HiddenLayer.Length; i++)
            {
                HiddenLayer[i].NeuronWeightsNext = new double[OutputLayer.Length];
                for (int j = 0; j < OutputLayer.Length; j++)
                {
                    HiddenLayer[i].NeuronWeightsNext[j] = OutputLayer[j].Weights[i];
                }
            }

            //Вычисляем дельту и новые веса для выходного слоя
            for (int i = 0; i < OutputLayer.Length; i++)
            {
                OutputLayer[i].CalculateDelta(idealResult[i]);
                OutputLayer[i].UpdateWeights(LearningSpeed, Momentum);
            }

            //теперь нужно каждому нейрону выходного слоя дать инфу о дельте всех нейронов предыдущего слоя
            for (int i = 0; i < HiddenLayer.Length; i++)
            {
                HiddenLayer[i].DeltasNext = new double[OutputLayer.Length];
                for (int j = 0; j < OutputLayer.Length; j++)
                {
                    HiddenLayer[i].DeltasNext[j] = OutputLayer[j].Delta;
                }
            }

            //Наконец вычисляем дельту и новые веса для скрытого слоя
            for (int i = 0; i < HiddenLayer.Length; i++)
            {
                HiddenLayer[i].CalculateDelta();
                HiddenLayer[i].UpdateWeights(LearningSpeed, Momentum);
            }
        }

        /// <summary>
        /// Обучение нейронной сети
        /// </summary>
        /// <param name="inputs">Массивов входных сигналов</param>
        public void Think(byte[] inputs)
        {
            //Для начала передаем входные параметры на выходной слой и считаем его выход
            double[] inputLayerOutputs = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                    InputLayer[i].Output = inputs[i];
                    inputLayerOutputs[i] = InputLayer[i].Output;
            }

            //Теперь считаем выход всех нейронов скрытого слоя
            double[] hiddenLayerOutputs = new double[HiddenLayer.Length];
            for (int i = 0; i < HiddenLayer.Length; i++)
            {
                HiddenLayer[i].CalculateInput(inputLayerOutputs);
                HiddenLayer[i].CalculateActivationFunc();
                 hiddenLayerOutputs[i] = HiddenLayer[i].Output;
            }

            //И, наконец, считаем выход всех нейронов выходного слоя
            double[] outputLayerOutputs = new double[OutputLayer.Length];
            for (int i = 0; i < OutputLayer.Length; i++)
            {
                OutputLayer[i].CalculateInput(hiddenLayerOutputs);
                OutputLayer[i].CalculateActivationFunc();
                outputLayerOutputs[i] = OutputLayer[i].Output;
            }

            CurrentResult = outputLayerOutputs;
        }
    }
}
