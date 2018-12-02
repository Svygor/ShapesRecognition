using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesRecognition.Models.ConvNet
{
    public class ConvNetwork
    {
        /// <summary>
        /// Картинка на входе
        /// </summary>
        public byte[,] Input { get; set; }

        /// <summary>
        /// Слой ConvNeuron-ов
        /// </summary>
        public ConvNeuron[] ConvLayer { get; set; }

        /// <summary>
        /// Слой fully-connected-нейронов
        /// </summary>
        public Neuron[] FCLayer { get; set; }

        /// <summary>
        /// Слой выходных нейронов
        /// </summary>
        public NeuronOutput[] OutputLayer { get; set; }

        /// <summary>
        /// Слой выходных значений
        /// </summary>
        public double[] Output { get; set; }

        public ConvNetwork()
        {
            ConvLayer = new ConvNeuron[5];
            for (int i = 0; i < ConvLayer.Length; i++)
            {
                ConvLayer[i] = new ConvNeuron();
            }

            FCLayer = new Neuron[320];
            for (int i = 0; i < FCLayer.Length; i++)
            {
                FCLayer[i] = new Neuron();
            }

            Output = new double[3];
            OutputLayer = new NeuronOutput[3];
            for (int i = 0; i < OutputLayer.Length; i++)
            {
                OutputLayer[i] = new NeuronOutput();
            }
        }

        /// <summary>
        /// Вычисление выхода по заданному входу
        /// </summary>
        public void Think(byte[,] inputs)
        {
            Input = inputs;
            double[][,] _convLayerOutputs = new double[ConvLayer.Length][,];
            for (int i = 0; i < ConvLayer.Length; i++)
            {
                ConvLayer[i].CalculateOutput(Input);
                _convLayerOutputs[i] = ConvLayer[i].Output;
            }

            double[] _fcInputs = ConvertToOneDemenision(MaxPool(_convLayerOutputs));

            double[] _fcLayerOutputs = new double[FCLayer.Length];
            for (int i = 0; i < FCLayer.Length; i++)
            {
                FCLayer[i].CalculateInput(_fcInputs);
                FCLayer[i].CalculateActivationFunc();
                _fcLayerOutputs[i] = FCLayer[i].Output;

            }

            for (int i = 0; i < OutputLayer.Length; i++)
            {
                OutputLayer[i].CalculateInput(_fcLayerOutputs);
                OutputLayer[i].CalculateActivationFunc();
                Output[i] = OutputLayer[i].Output;
            }
        }

        /// <summary>
        /// Преобрпзование массива двумерных массивов в одномерный массив
        /// </summary>
        /// <param name="arrays"></param>
        /// <returns></returns>
        private double[] ConvertToOneDemenision(double[][,] arrays)
        {
            int arraysNum = arrays.Length;
            int arrayHeight = arrays[0].GetLength(0);
            int arrayWidth = arrays[0].GetLength(1);
            double[] result = new double[arraysNum*arrayHeight*arrayWidth];
            for (int i = 0; i < arraysNum; i++)
            {
                for (int j = 0; j < arrayHeight; j++)
                {
                    for (int k = 0; k < arrayWidth; k++)
                    {
                        result[i + j + k] = arrays[i][j, k];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// max pooling 2x2
        /// </summary>
        /// <param name="_convLayerOutputs"></param>
        /// <returns></returns>
        private double[][,] MaxPool(double[][,] _convLayerOutputs)
        {
            double[][,] result = new double[_convLayerOutputs.Length][,];
            for (int i = 0; i < _convLayerOutputs.Length; i++)
            {
                result[i] = new double[_convLayerOutputs[i].GetLength(0) / 2, _convLayerOutputs[i].GetLength(1) / 2];
                for (int j = 0; j < result[i].GetLength(0); j++)
                {
                    for (int k = 0; k < result[i].GetLength(0); k++)
                    {
                        result[i][j, k] = Math.Max(Math.Max(_convLayerOutputs[i][j * 2, k * 2], _convLayerOutputs[i][j * 2 + 1, k * 2]), 
                            Math.Max(_convLayerOutputs[i][j * 2, k * 2 + 1], _convLayerOutputs[i][j * 2 + 1, k * 2 + 1]));
                    }
                }
            }
            return result;
        }
    }
}
