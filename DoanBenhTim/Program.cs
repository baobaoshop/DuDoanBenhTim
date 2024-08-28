using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;

namespace DoanBenhTim
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    public class KetQua
    {
        public double DoChinhXac { get;set;}
        public double KqtQuaDuDoan { get; set; }
    }
    public class LogisticProgram
    {
        public KetQua HuanLuyen(double t1, double t2, double t3, double t4, double t5, double t6, double t7, double t8, double t9, double t10, double t11, double t12, double t13)
        {
            MyData mydata = new MyData();
            KetQua kq = new KetQua();
            // load data



            double[][] trainX = new double[303][];
            int[] trainY = new int[303];
            string[] a = File.ReadAllLines("processed.cleveland.txt");
            for (int i = 0; i < 303; i++)
            {
                string[] t = a[i].Split(',');
                double[] d = new double[14];
                for (int j = 0; j < 14; j++)
                {
                    d[j] = double.Parse(t[j]);
                }
                trainX[i] = new double[] { d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8], d[9], d[10], d[11], d[12] };
                mydata.ThemDuLieu(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8], d[9], d[10], d[11], d[12], d[13]);
                int nhan = int.Parse(t[13]);
                if (nhan > 1) nhan = 1;
                trainY[i] = nhan;
            }




            // thực hiện quá trình đào tạo một mô hình sử dụng phương pháp Stochastic Gradient Descent (SGD)
            int maxEpoch = 10000;     //Đặt số lượng epoch (vòng lặp qua toàn bộ tập dữ liệu đào tạo) là 100. Mỗi epoch là một lần duyệt qua toàn bộ tập dữ liệu đào tạo.
            double lr = 0.0001;
            double[] wts = Train(trainX, trainY, lr, maxEpoch, seed: 0);
            //Console.WriteLine("Xay dung XONG \n");

            //Console.WriteLine("Trong so sau khi xay dung mo hinh: ");
            //ShowVector(wts);        // chứa trọng số của mô hình sau khi quá trình đào tạo kết thúc.


            //đo lường độ chính xác của mô hình trên tập dữ liệu đào tạo (training data) sau khi quá trình đào tạo đã hoàn thành
            double accTrain = Accuracy(trainX, trainY, wts);    //trả về độ chính xác của mô hình trên tập dữ liệu đào tạo.
            //Console.Write("Do chinh xac cua mo hinh: ");
            kq.DoChinhXac = accTrain;
            //Console.WriteLine(accTrain.ToString("F4"));     // In 4 chữ số thập phân
            //for (int i = 0; i < trainX.Length; i++)
            //{
            //    double pi = ComputeOutput(trainX[i], wts);
            //    Console.WriteLine("Sac xuat lan {0}: {1}", i, pi);
            //}
            //Console.WriteLine("\nDu doan gioi tinh cho: ");
            //Console.WriteLine(xRaw);
            double[] x = new double[] { t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13};
            //new double[] { 0.36, 0, 0, 1, 0.5200, 0, 1, 0 };
            double p = ComputeOutput(x, wts);
            kq.KqtQuaDuDoan = p;
            return kq;
            //Console.WriteLine("\n XONG");
        } // Main

        static double ComputeOutput(double[] x, double[] wts)
        {
            // bias is last cell of w
            double z = 0.0;
            for (int i = 0; i < x.Length; ++i)
                z += x[i] * wts[i];
            z += wts[wts.Length - 1];
            return LogSig(z);
        }

        static double LogSig(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        static double[] Train(double[][] trainX, int[] trainY, double lr, int maxEpoch, int seed = 0)
        {
            int N = trainX.Length;  // number train items
            int n = trainX[0].Length;  // number predictors
            double[] wts = new double[n + 1];  // bias
            int[] indices = new int[N];
            Random rnd = new Random(seed);
            double lo = -0.01; double hi = 0.01;
            for (int i = 0; i < wts.Length; ++i)
                wts[i] = (hi - lo) * rnd.NextDouble() + lo;

            for (int i = 0; i < N; ++i)
                indices[i] = i;

            for (int epoch = 0; epoch < maxEpoch; ++epoch)
            {
                Shuffle(indices, rnd);

                //for (int i = 0; i < N; ++i)
                foreach (int i in indices)
                {  // each item
                    //int idx = indices[i];
                    double[] x = trainX[i];  // predictors
                    int y = trainY[i];  // target, 0 or 1
                    double p = ComputeOutput(x, wts);

                    for (int j = 0; j < n; ++j)  // each weight
                        wts[j] += lr * x[j] * (y - p) * p * (1 - p);
                    wts[n] += lr * (y - p) * p * (1 - p);
                }

                if (epoch % (maxEpoch / 10) == 0)
                {
                    double acc = Accuracy(trainX, trainY, wts);
                }
            } // iter

            return wts;  // trained weights and bias
        }

        static void Shuffle(int[] vec, Random rnd)
        {
            int n = vec.Length;
            for (int i = 0; i < n; ++i)
            {
                int ri = rnd.Next(i, n);
                int tmp = vec[ri];
                vec[ri] = vec[i];
                vec[i] = tmp;
            }
        }

        static double Accuracy(double[][] dataX,
          int[] dataY, double[] wts)
        {
            int numCorrect = 0; int numWrong = 0;
            int N = dataX.Length;
            for (int i = 0; i < N; ++i)
            {
                double[] x = dataX[i];
                int y = dataY[i];  // actual, 0 or 1
                double p = ComputeOutput(x, wts);

                if (y == 0 && p < 0.5)
                    ++numCorrect;
                else if (y == 1 && p >= 0.5)
                    ++numCorrect;
                else
                    ++numWrong;
            }
            return (1.0 * numCorrect) / (numCorrect + numWrong);
        }


    }

    public class LichSu
    {
        public static DataTable dt;
        static int stt;
        public LichSu()
        {
            dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Độ tuổi", typeof(double));
            dt.Columns.Add("Giới tính", typeof(double));
            dt.Columns.Add("Loại đau ngực", typeof(double));
            dt.Columns.Add("Huyết áp nghỉ", typeof(double));
            dt.Columns.Add("Cholesterol", typeof(double));
            dt.Columns.Add("Đường huyết áp nhanh", typeof(double));
            dt.Columns.Add("Điện tâm đồ nghỉ", typeof(double));
            dt.Columns.Add("Tần soostim tối đa", typeof(double));
            dt.Columns.Add("Đau ngực do hoạt động", typeof(double));
            dt.Columns.Add("Chênh lệch ST trước và sau khi đập", typeof(double));
            dt.Columns.Add("Góc độ của đường ST Segment", typeof(double));
            dt.Columns.Add("Số mạch máu chính", typeof(double));
            dt.Columns.Add("Thalassema", typeof(double));
            dt.Columns.Add("Tỉ lệ mắc bệnh", typeof(double));
            stt = 0;
        }
        public void ThemDuLieu(double t1, double t2, double t3, double t4, double t5, double t6, double t7, double t8, double t9, double t10, double t11, double t12, double t13, double t14)
        {
            dt.Rows.Add(++stt, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }
    }
    public class MyData
    {
        public static DataTable dt;
        static int stt;
        public MyData()
        {
            dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Độ tuổi", typeof(double));
            dt.Columns.Add("Giới tính", typeof(double));
            dt.Columns.Add("Loại đau ngực", typeof(double));
            dt.Columns.Add("Huyết áp nghỉ", typeof(double));
            dt.Columns.Add("Cholesterol", typeof(double));
            dt.Columns.Add("Đường huyết áp nhanh", typeof(double));
            dt.Columns.Add("Điện tâm đồ nghỉ", typeof(double));
            dt.Columns.Add("Tần soostim tối đa", typeof(double));
            dt.Columns.Add("Đau ngực do hoạt động", typeof(double));
            dt.Columns.Add("Chênh lệch ST trước và sau khi đập", typeof(double));
            dt.Columns.Add("Góc độ của đường ST Segment", typeof(double));
            dt.Columns.Add("Số mạch máu chính", typeof(double));
            dt.Columns.Add("Thalassema", typeof(double));
            dt.Columns.Add("Mức độ bệnh", typeof(double));
            stt = 0;
        }
        public void ThemDuLieu(double t1, double t2, double t3, double t4, double t5, double t6, double t7, double t8, double t9, double t10, double t11, double t12, double t13, double t14)
        {
            dt.Rows.Add(++stt, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }
    }
}
