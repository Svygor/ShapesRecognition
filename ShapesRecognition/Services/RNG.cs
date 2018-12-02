using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesRecognition.Services
{
    public static class RNG
    {
        private static RandomGenerator instanse { get; set; }

        public  class RandomGenerator
        {
            Random rnd;
            public RandomGenerator()
            {
                rnd = new Random();
            }
            public double GetWeight()
            {
                return rnd.Next(10,90)*0.001;
            }
        }

        public static RandomGenerator GetInstanse()
        {
            if (instanse ==  null)
            {
                instanse = new RandomGenerator();
            }
            return instanse;
        }
    }
}
