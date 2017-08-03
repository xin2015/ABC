using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.BLL
{
    public class PredictionObservationStatisticalAnalysis
    {
        /// <summary>
        /// 预测值
        /// </summary>
        public double[] Prediction { get; set; }
        /// <summary>
        /// 观测值
        /// </summary>
        public double[] Observation { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 偏差和
        /// </summary>
        public double SB { get; set; }
        /// <summary>
        /// 误差和
        /// </summary>
        public double SE { get; set; }
        /// <summary>
        /// 平均偏差
        /// </summary>
        public double MB { get; set; }
        /// <summary>
        /// 平均误差
        /// </summary>
        public double ME { get; set; }
        /// <summary>
        /// 标准化平均偏差
        /// </summary>
        public double NMB { get; set; }
        /// <summary>
        /// 标准化平均误差
        /// </summary>
        public double NME { get; set; }
        /// <summary>
        /// 误差平方和
        /// </summary>
        public double SSE { get; set; }
        /// <summary>
        /// 均方误差
        /// </summary>
        public double MSE { get; set; }
        /// <summary>
        /// 均方根误差
        /// </summary>
        public double RMSE { get; set; }
        /// <summary>
        /// 协方差
        /// </summary>
        public double Cov { get; set; }
        /// <summary>
        /// 相关系数
        /// </summary>
        public double COR { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pred">预测值</param>
        /// <param name="obs">观测值</param>
        public PredictionObservationStatisticalAnalysis(double[] pred, double[] obs)
        {
            Prediction = pred;
            Observation = obs;
            Length = obs.Length;
            double predAvg = Prediction.Average();
            double obsAvg = Observation.Average();
            double sum1 = 0, sum2 = 0, sum3 = 0;
            for (int i = 0; i < Length; i++)
            {
                double bias = Prediction[i] - Observation[i];
                double error = Math.Abs(bias);
                SB += bias;
                SE += error;
                SSE += Math.Pow(error, 2);
                double predBias = Prediction[i] - predAvg;
                double obsBias = Observation[i] - obsAvg;
                sum1 += predBias * obsBias;
                sum2 += Math.Pow(predBias, 2);
                sum3 += Math.Pow(obsBias, 2);
            }
            Cov = sum1 / Length;
            COR = sum1 / Math.Sqrt(sum2 * sum3);
            MB = SB / Length;
            ME = SE / Length;
            NMB = SB / Observation.Sum();
            NME = SE / Observation.Sum();
            MSE = SSE / Length;
            RMSE = Math.Sqrt(MSE);
        }
    }
}
