using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Thread_Demo
{
    internal class Program
    {
        private static int num=0;
        private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            //中断线程
            discontinuetask();
            //
            mytimer();

            mutex();

            //System.Threading.ManualResetEvent manualResetEvent = new System.Threading.ManualResetEvent(false);
            //System.Threading.CountdownEvent countdownEvent = new System.Threading.CountdownEvent(10);
            


            Console.ReadKey();
        }

        static void mutex()
        {
            //bool creatednew = false;
            //using (Mutex mutex = new Mutex(true, Application.ProductName, out creatednew))
            //{
            //    if (creatednew)
            //    {
            //        Application.EnableVisualStyles();
            //        Application.SetCompatibleTextRenderingDefault(false);
            //        Application.Run(new Form1());
            //    }
            //    else
            //    {
            //        MessageBox.Show("程序已运行", Application.ProductName);
            //        Environment.Exit(1);
            //    }
            //}
            Console.WriteLine("winforme例子");
        }

        #region
        static void discontinuetask()
        {
            CancellationTokenSource ct = new CancellationTokenSource();
            taskrun(ct.Token);
            Console.WriteLine("中断前");
            Thread.Sleep(5000);
            ct.Cancel();
            Console.WriteLine("线程已中断");
            Console.ReadKey();
        }

        static async void taskrun(CancellationToken token)
        {
            await Task.Factory.StartNew(() => {
                int i = 0;
                while (true)
                {
                    i++;
                    Console.WriteLine("计数：" + i);
                    Thread.Sleep(500);
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            });
        }
        #endregion

        #region

        static void mytimer()
        {
            System.Threading.Timer timer = new Timer(new TimerCallback(GetTime), autoResetEvent, 0, 1000);
            autoResetEvent.WaitOne();
            autoResetEvent.Close();
            timer.Dispose();
        }

        /// <summary>
        /// timer 控制器
        /// </summary>
        /// <param name="state"></param>
        private static void GetTime(object state)
        {
            System.Threading.AutoResetEvent autoEvent = (System.Threading.AutoResetEvent)state;
            num++;
            if (num > 180)
            {
                num = 0;
                autoEvent.Set();//恢复默认状态
            }
        }
        #endregion
    }
}
