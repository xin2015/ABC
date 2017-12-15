using AForge.Neuro;
using AForge.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.BLL.Neuro
{
    public class BackPropagationLearningTimeSeriesModel
    {
        private double[] data;
        private int inputsCount;
        private int[] neuronsCount;
        private int iteration;
        private int predictionCount;
        private double sigmoidAlphaValue;
        private double learningRate;
        private double momentum;
        private ActivationNetwork network;
        private BackPropagationLearning teacher;
        private int samples;
        private double max;
        private double min;
        private double factor;

        public BackPropagationLearningTimeSeriesModel(double[] data, int inputsCount, int[] neuronsCount, int iteration, int predictionCount)
        {
            this.data = data;
            this.inputsCount = inputsCount;
            this.neuronsCount = neuronsCount;
            this.iteration = iteration;
            this.predictionCount = predictionCount;
            sigmoidAlphaValue = 2;
            learningRate = 0.1;
            momentum = 0;
            samples = data.Length - inputsCount - predictionCount;
            min = data.Min();
            max = data.Max();
            factor = 1.7 / (max - min);
        }

        private double Normalize(double value, bool reverse = false)
        {
            if (reverse)
            {
                return (value + 0.85) / factor + min;
            }
            else
            {
                return (value - min) * factor - 0.85;
            }
        }

        public void Run()
        {
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];
            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[inputsCount];
                output[i] = new double[1];

                for (int j = 0; j < inputsCount; j++)
                {
                    input[i][j] = Normalize(data[i + j]);
                }
                output[i][0] = Normalize(data[i + inputsCount]);
            }

            network = new ActivationNetwork(new BipolarSigmoidFunction(sigmoidAlphaValue), inputsCount, neuronsCount);
            teacher = new BackPropagationLearning(network);
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;

            int count = 0;
            int solutionCount = data.Length - inputsCount;
            double[] solution = new double[solutionCount];
            double[] networkInput = new double[inputsCount];

            while (true)
            {
                double error = teacher.RunEpoch(input, output) / samples;
                double currentError;
                double learningError = 0;
                double predictionError = 0;

                for (int i = 0; i < solutionCount; i++)
                {
                    for (int j = 0; j < inputsCount; j++)
                    {
                        networkInput[j] = Normalize(data[i + j]);
                    }
                    solution[i] = Normalize(network.Compute(networkInput)[0], true);
                    currentError = Math.Abs(solution[i] - data[i + inputsCount]);
                    if (i > samples)
                    {
                        predictionError += currentError;
                    }
                    else
                    {
                        learningError += currentError;
                    }
                }
                learningError = learningError / samples;
                predictionError = predictionError / predictionCount;

                count++;
                Console.WriteLine("迭代次数：{0}，训练误差：{1}，学习误差：{2}，预报误差：{3}", count, error, learningError, predictionError);
                if (count == iteration)
                {
                    break;
                }
            }
        }
    }
}
