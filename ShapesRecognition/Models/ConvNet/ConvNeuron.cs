using ShapesRecognition.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesRecognition.Models.ConvNet
{
    public class ConvNeuron
    {
        /// <summary>
        /// Массив весов фильтра
        /// </summary>
        public double[,] Weights { get; set; }

        /// <summary>
        /// Результат работы фильтра
        /// </summary>
        public double[,] Output { get; set; }

        /// <summary>
        /// размер фильтра
        /// </summary>
        public int filterSize;

        /// <summary>
        /// Разница в размерности между входным изображением и результатом
        /// Зависит от размера фильтра
        /// </summary>
        public int inputDecrese;

        public ConvNeuron()
        {
            //размер фильтра всегда нечетное число, чтобы был пиксель точно в центре матрицы
            filterSize = 5;
            Weights = new double[filterSize, filterSize];
            inputDecrese = 5 / 2;
            GenerateWeighst();
        }

        /// <summary>
        /// Присваиваем матрице весов случайные значения
        /// </summary>
        private void GenerateWeighst()
        {
            for (int i = 0; i < filterSize; i++)
            {
                for (int j = 0; j < filterSize; j++)
                {
                    Weights[i,j] = RNG.GetInstanse().GetWeight();
                }
            }
        }

        /// <summary>
        /// Обработка входного изображения фильтром
        /// </summary>
        public void CalculateOutput(byte[,] input)
        {
            Output = new double[input.GetLength(0) - inputDecrese * 2, input.GetLength(1) - inputDecrese * 2];
            double[,] currentPart = new double[filterSize, filterSize];
            for (int i = inputDecrese; i < input.GetLength(0) - inputDecrese; i++)
            {
                for (int j = inputDecrese; j < input.GetLength(1) - inputDecrese; j++)
                {
                    currentPart = GetCurrentPart(input, i, j);
                    Output[i - inputDecrese, j - inputDecrese] = ApplyFilter(currentPart);
                }
            }
        }

        /// <summary>
        /// Применяем фильтр к переданному участку картинки
        /// </summary>
        /// <param name="currentPart"></param>
        /// <returns></returns>
        private double ApplyFilter(double[,] currentPart)
        {
            double result = 0;
            for (int i = 0; i < filterSize; i++)
            {
                for (int j = 0; j < filterSize; j++)
                {
                    result += currentPart[i, j] * Weights[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// Получаем часть исходного изображения, размером равным размеру фильтра
        /// с центром в координатах, переданных в метод
        /// </summary>
        /// <param name="input"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private double[,] GetCurrentPart(byte[,] input, int x, int y)
        {
            double[,] currentPart = new double[filterSize, filterSize];
            for (int i = 0; i < filterSize; i++)
            {
                for (int j = 0; j < filterSize; j++)
                {
                    /*Почему так. Возьмем, к примеру, входное изображение 10х10. Метод CalculateOutput будет отправлять нам координаты [x,y]
                    пикселя, к которому применяются фильтры. Они будут принимать значения с [2,2] по [7,7],
                    чтобы применяя фильтр 5х5 к участку с центром в переданных координатах, мы никогда не выходили за границы изображения.
                    Рассмотрим как раз левую верхнюю и правую нижнюю из переданных нам точек, чтобы убедиться, что применим фильтр правильно.
                    Если передано [2,2], то в левый верхний угол фильтра 5х5 (т.е. в точку фильтра [0,0]) попадет значение, расположенное в координатах [х0, y0]
                    исходной картинки, где (в соответствии с вычислениями, которые объясняю):
                    x0 = i + x - inputDecrese = 0 + 2 - 2 = 0;
                    y0 = j + y - inputDecrese = 0 + 2 - 2 = 0;
                    В правый нижний угол фильтра 5х5 (т.е. в точку [4,4] попадет значение, расположенное в координатах [x1,y1]
                    исходной картинки, где (в соответствии с вычислениями, которые объясняю):
                    x1 = i + x - inputDecrese = 4 + 2 - 2 = 4;
                    y1 = j + y - inputDecrese = 4 + 2 - 2 = 4;
                    Если же передано [7,7], то:
                    x0 = 0 + 7 - 2 = 5;
                    y0 = 0 + 7 - 2 = 5;
                    x1 = 4 + 7 - 2 = 9;
                    y1 = 4 + 7 - 2 = 9;
                    Таким образом на фильтр так или иначе повлияют все пиксели исходной картинки и мы не выйдим ни разу за её пределы.
                    */
                    currentPart[i, j] = input[i + x - inputDecrese, j + y - inputDecrese];
                }
            }
            return currentPart;
        }
    }
}