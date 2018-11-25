using ShapesRecognition.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesRecognition.Models
{
    /// <summary>
    /// Модель входного нейрона
    /// </summary>
    public class NeuronInput : NeuronBase
    {
        public NeuronInput(int numOfInputs)
        {
            Inputs = new double[numOfInputs];
        }
        /// <summary>
        /// Активационная функция у входного нейрона состоит в том, чтобы просто передать дальше то, что он получил на вход
        /// (каждый нейрон входного слоя на вход получит только 1 или 0)
        /// </summary>
        public override void CalculateActivationFunc()
        {
            Output = Input;
        }
    }
}
