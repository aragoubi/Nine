using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;


namespace HeatMap
{
    public static class HeatMapWrapper
    {
        struct heatmap_colorscheme_t
        {
            string color;
            uint ncolors;
        }

     /**   heatmap_colorscheme_t cs_spectral = { mixed_data, sizeof(mixed_data) / sizeof(mixed_data[0]) / 4 };

        heatmap_colorscheme_t* heatmap_cs_default = &cs_spectral_mixed;


        public static void GenerateHeatMapWithStamp(uint x, uint y, uint stamp, int[,] points, BitmapSource source)
        {
            IntPtr hm = NativeMethods.NewHeatmap(x, y);
            uint r = stamp;
            IntPtr st =NativeMethods.stamp_gen(r);

            heatmap_colorscheme_t* colorscheme = heatmap_cs_default;
            List<char> image;
            NativeMethods.Render_to(hm, colorscheme, image[0]);
            NativeMethods.FreeHeatmap(hm);
                   
        }*/




        public static class NativeMethods
        {
            const string DLL = "heatmap.dll";

            [DllImport(DLL, EntryPoint = "heatmap_init", CallingConvention = CallingConvention.Cdecl)]
            public static extern void HeatMap_init(IntPtr hm, uint w, uint h);

            [DllImport(DLL, EntryPoint = "heatmap_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NewHeatmap(uint w, uint h);

            [DllImport(DLL, EntryPoint = "heatmap_free", CallingConvention = CallingConvention.Cdecl)]
            public static extern void FreeHeatmap(IntPtr hm);

            [DllImport(DLL, EntryPoint = " heatmap_add_point", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Add_points(IntPtr hm, uint w, uint h);

            [DllImport(DLL, EntryPoint = "  heatmap_add_point_with_stamp", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Add_points_with_stamps(IntPtr hm, uint w, uint h, IntPtr stamp);

            [DllImport(DLL, EntryPoint = "heatmap_add_weighted_point", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Add_weighted_point(IntPtr hm, uint w, uint h, float stamp);

            [DllImport(DLL, EntryPoint = "heatmap_add_weighted_point_with_stamp", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Add_weighted_point_with_stamp(IntPtr hm, uint w, uint h, float k, IntPtr stamp);

            [DllImport(DLL, EntryPoint = "heatmap_render_default_to", CallingConvention = CallingConvention.Cdecl)]
            public static extern string Render_default_to(IntPtr h, string colorbuf);

            [DllImport(DLL, EntryPoint = "heatmap_render_to", CallingConvention = CallingConvention.Cdecl)]
            public static extern string Render_to(IntPtr h, IntPtr colorscheme, string colorbuf);

            [DllImport(DLL, EntryPoint = " heatmap_render_saturated_to", CallingConvention = CallingConvention.Cdecl)]
            public static extern string Render_saturated_to(IntPtr h, IntPtr colorscheme, float saturation, string colorbuf);

            [DllImport(DLL, EntryPoint = " heatmap_stamp_init", CallingConvention = CallingConvention.Cdecl)]
            public static extern void stamp_init(IntPtr stamp, uint w, uint h, IntPtr data);

            [DllImport(DLL, EntryPoint = " heatmap_stamp_new_with", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr stamp_new_with(uint w, uint h, IntPtr data);

            [DllImport(DLL, EntryPoint = " heatmap_stamp_load", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr stamp_load(uint w, uint h, IntPtr data);

            [DllImport(DLL, EntryPoint = "linear_dist", CallingConvention = CallingConvention.Cdecl)]
            public static extern float linear_dist(float dist);

            [DllImport(DLL, EntryPoint = " heatmap_stamp_gen", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr stamp_gen(uint r);

            [DllImport(DLL, EntryPoint = "  heatmap_stamp_gen_nonlinear", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr stamp_gen_nonlinear(uint r, IntPtr distshape);

            [DllImport(DLL, EntryPoint = " heatmap_stamp_free", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr stamp_free(IntPtr s);

            [DllImport(DLL, EntryPoint = "heatmap_colorscheme_load", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Colorscheme_load(string in_colors, uint ncolors);

            [DllImport(DLL, EntryPoint = "heatmap_colorscheme_free", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Colorscheme_free(IntPtr cs);
        }
    }

    
}
