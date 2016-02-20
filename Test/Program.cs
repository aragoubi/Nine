using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using HeatMap;
using MuPdf;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IntPtr r = HeatMap.HeatMapWrapper.NativeMethods.NewHeatmap(10, 20);
            Console.WriteLine(r.ToString());

            /**
            FileSource pdf = new FileSource("C:/Users/aymen/OneDrive/Documents/zrgzrgfz.pdf");
            Size size = new Size(30, 10);
            MuPdf.MuPdfWrapper.GeneratePagesAtSize(pdf, "C:/Users/aymen/OneDrive/Documents/",size);**/
        }
    }
}
