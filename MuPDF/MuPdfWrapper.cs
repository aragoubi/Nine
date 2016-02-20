/*! MoonPdfLib - Provides a WPF user control to display PDF files
Copyright (C) 2013  (see AUTHORS file)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
!*/

/*
 * 2013 - Modified and extended version of W. Jordan's code (see AUTHORS file)
 */
using MuPdf.Helper;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace MuPdf
{

    public static class MuPdfWrapper
    {
        private static string generateOutputExtension = ".png";

        private static ImageFormat generateOutputFormat = ImageFormat.Png;

        /// <summary>
        /// Generates all PDF pages as JPEG files for a given pdf source and a destination folder.
        /// </summary>
        /// <param name="source">The PDF source</param>
        /// <param name="destination">The destination folder</param>
        /// <param name="size">Used to get a smaller or bigger JPEG file, depending on the specified value</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        public static void GeneratePagesAtSize(IPdfSource source, string destination, Size size, string password = null)
        {
            if (!Directory.Exists(destination))
            {
                throw new DirectoryNotFoundException("The directory \"" + destination + "\" does not exist !");
            }

            using (var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);
                int pageCount = CountPages(source);
                var currentDpi = DpiHelper.GetCurrentDpi();

                for (int i = 0; i < pageCount; i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    IntPtr p = NativeMethods.LoadPage(stream.Document, i); // loads the page
                    var bmp = RenderPageAtSize(stream.Context, stream.Document, p, size, currentDpi.HorizontalDpi, currentDpi.VerticalDpi);
                    NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page
                    bmp.Save(Path.Combine(destination,  i + generateOutputExtension), generateOutputFormat);
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Generates all PDF pages as JPEG files for a given pdf source and a destination folder.
        /// </summary>
        /// <param name="source">The PDF source</param>
        /// <param name="destination">The destination folder</param>
        /// <param name="width">Used to get a smaller or bigger JPEG file, depending on the specified value</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        public static void GeneratePagesAtWidth(IPdfSource source, string destination, int width, string password = null)
        {
            if (!Directory.Exists(destination))
            {
                throw new DirectoryNotFoundException("The directory \"" + destination + "\" does not exist !");
            }

            using (var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);
                int pageCount = CountPages(source);
                var currentDpi = DpiHelper.GetCurrentDpi();

                for (int i = 0; i < pageCount; i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    IntPtr p = NativeMethods.LoadPage(stream.Document, i); // loads the page
                    var bmp = RenderPageAtWidth(stream.Context, stream.Document, p, width, currentDpi.HorizontalDpi, currentDpi.VerticalDpi);
                    NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page
                    bmp.Save(Path.Combine(destination, i + generateOutputExtension), generateOutputFormat);
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Generates all PDF pages as JPEG files for a given pdf source and a destination folder.
        /// </summary>
        /// <param name="source">The PDF source</param>
        /// <param name="destination">The destination folder</param>
        /// <param name="height">Used to get a smaller or bigger JPEG file, depending on the specified value</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        public static void GeneratePagesAtHeight(IPdfSource source, string destination, int height, string password = null)
        {
            if (!Directory.Exists(destination))
            {
                throw new DirectoryNotFoundException("The directory \"" + destination + "\" does not exist !");
            }

            using (var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);
                int pageCount = CountPages(source);
                var currentDpi = DpiHelper.GetCurrentDpi();

                for (int i = 0; i < pageCount; i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    IntPtr p = NativeMethods.LoadPage(stream.Document, i); // loads the page
                    var bmp = RenderPageAtHeight(stream.Context, stream.Document, p, height, currentDpi.HorizontalDpi, currentDpi.VerticalDpi);
                    NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page
                    bmp.Save(Path.Combine(destination, i + generateOutputExtension), generateOutputFormat);
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Generates all PDF pages as JPEG files for a given pdf source and a destination folder.
        /// </summary>
        /// <param name="source">The PDF source</param>
        /// <param name="destination">The destination folder</param>
        /// <param name="zoomFactor">Used to get a smaller or bigger JPEG file, depending on the specified value</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        public static void GeneratePagesAtScale(IPdfSource source, string destination, float zoomFactor = 1.0f, string password = null)
        {
            if (!Directory.Exists(destination))
            {
                throw new DirectoryNotFoundException("The directory \"" + destination + "\" does not exist !");
            }

            using (var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);
                int pageCount = CountPages(source);
                var currentDpi = DpiHelper.GetCurrentDpi();

                for (int i = 0; i < pageCount; i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    IntPtr p = NativeMethods.LoadPage(stream.Document, i); // loads the page
                    var bmp = RenderPageAtScale(stream.Context, stream.Document, p, zoomFactor, currentDpi.HorizontalDpi, currentDpi.VerticalDpi);
                    NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page
                    bmp.Save(Path.Combine(destination, i + generateOutputExtension), generateOutputFormat);
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Extracts a PDF page as a Bitmap for a given pdf source and a page number.
        /// </summary>
        /// <param name="source">The PDF source</param>
        /// <param name="pageNumber">Page number, starting at 0</param>
        /// <param name="size">Used to get a smaller or bigger Bitmap, depending on the specified value</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        public static BitmapSource ExtractPageAtSize(IPdfSource source, int pageNumber, Size size, string password = null)
        {
            if (pageNumber < 0 || pageNumber >= CountPages(source))
            {
                throw new ArgumentOutOfRangeException("pageNumber", "The page \"" + pageNumber + "\" does not exist !");
            }

            using (var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);

                IntPtr p = NativeMethods.LoadPage(stream.Document, pageNumber); // loads the page
                var currentDpi = DpiHelper.GetCurrentDpi();
                var bmp = RenderPageAtSize(stream.Context, stream.Document, p, size, currentDpi.HorizontalDpi, currentDpi.VerticalDpi);
                NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page

                return bmp.ToBitmapSource();
            }
        }

        /// <summary>
        /// Extracts a PDF page as a Bitmap for a given pdf source and a page number.
        /// </summary>
        /// <param name="source">The PDF source</param>
        /// <param name="pageNumber">Page number, starting at 0</param>
        /// <param name="width">Used to get a smaller or bigger Bitmap, depending on the specified value</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        public static BitmapSource ExtractPageAtWidth(IPdfSource source, int pageNumber, int width, string password = null)
        {
            if (pageNumber < 0 || pageNumber >= CountPages(source))
            {
                throw new ArgumentOutOfRangeException("pageNumber", "The page \"" + pageNumber + "\" does not exist !");
            }

            using (var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);

                IntPtr p = NativeMethods.LoadPage(stream.Document, pageNumber); // loads the page
                var currentDpi = DpiHelper.GetCurrentDpi();
                var bmp = RenderPageAtWidth(stream.Context, stream.Document, p, width, currentDpi.HorizontalDpi, currentDpi.VerticalDpi);
                NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page

                return bmp.ToBitmapSource();
            }
        }

        /// <summary>
        /// Extracts a PDF page as a Bitmap for a given pdf source and a page number.
        /// </summary>
        /// <param name="source">The PDF source</param>
        /// <param name="pageNumber">Page number, starting at 0</param>
        /// <param name="height">Used to get a smaller or bigger Bitmap, depending on the specified value</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        public static BitmapSource ExtractPageAtHeight(IPdfSource source, int pageNumber, int height, string password = null)
        {
            if (pageNumber < 0 || pageNumber >= CountPages(source))
            {
                throw new ArgumentOutOfRangeException("pageNumber", "The page \"" + pageNumber + "\" does not exist !");
            }

            using (var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);

                IntPtr p = NativeMethods.LoadPage(stream.Document, pageNumber); // loads the page
                var currentDpi = DpiHelper.GetCurrentDpi();
                var bmp = RenderPageAtHeight(stream.Context, stream.Document, p, height, currentDpi.HorizontalDpi, currentDpi.VerticalDpi);
                NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page

                return bmp.ToBitmapSource();
            }
        }

        /// <summary>
        /// Extracts a PDF page as a Bitmap for a given pdf source and a page number.
        /// </summary>
        /// <param name="source">The PDF source</param>
        /// <param name="pageNumber">Page number, starting at 0</param>
        /// <param name="zoomFactor">Used to get a smaller or bigger Bitmap, depending on the specified value</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        public static BitmapSource ExtractPageAtScale(IPdfSource source, int pageNumber, float zoomFactor = 1.0f, string password = null)
        {
            if (pageNumber < 0 || pageNumber >= CountPages(source))
            {
                throw new ArgumentOutOfRangeException("pageNumber", "The page \"" + pageNumber + "\" does not exist !");
            }

            using (var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);

                IntPtr p = NativeMethods.LoadPage(stream.Document, pageNumber); // loads the page
                var currentDpi = DpiHelper.GetCurrentDpi();
                var bmp = RenderPageAtScale(stream.Context, stream.Document, p, zoomFactor, currentDpi.HorizontalDpi, currentDpi.VerticalDpi);
                NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page

                return bmp.ToBitmapSource();
            }
        }

        /// <summary>
        /// Gets the page bounds for a given pdf source and a page number.
        /// </summary>
        /// <param name="source">The PDF source</param>
        /// <param name="pageNumber">Page number, starting at 0</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        /// <returns></returns>
        public static System.Windows.Size GetPageBound(IPdfSource source, int pageNumber, string password = null)
        {
            if (pageNumber < 0 || pageNumber >= CountPages(source))
            {
                throw new ArgumentOutOfRangeException("pageNumber", "The page \"" + pageNumber + "\" does not exist !");
            }

            using (var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);

                IntPtr p = NativeMethods.LoadPage(stream.Document, pageNumber); // loads the page
                Rectangle pageBound = NativeMethods.BoundPage(stream.Document, p);
                NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page

                return new System.Windows.Size(pageBound.Width, pageBound.Height);
            }
        }

        /// <summary>
        /// Gets the page bounds for all pages of the given PDF. If a relevant rotation is supplied, the bounds will
        /// be rotated accordingly before returning.
        /// </summary>
        /// <param name="source">The PDF source</param>
        /// <param name="rotation">The rotation that should be applied</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        /// <returns></returns>
        public static System.Windows.Size[] GetPageBounds(IPdfSource source, Rotation rotation = Rotation.Rotate0, string password = null)
        {
            Func<double, double, System.Windows.Size> sizeCallback = (width, height) => new System.Windows.Size(width, height);

            if(rotation == Rotation.Rotate90 || rotation == Rotation.Rotate270)
                sizeCallback = (width, height) => new System.Windows.Size(height, width); // switch width and height

            using(var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);

                var pageCount = NativeMethods.CountPages(stream.Document); // gets the number of pages in the document
                var resultBounds = new System.Windows.Size[pageCount];

                for(int i = 0; i < pageCount; i++)
                {
                    IntPtr p = NativeMethods.LoadPage(stream.Document, i); // loads the page
                    Rectangle pageBound = NativeMethods.BoundPage(stream.Document, p);

                    resultBounds[i] = sizeCallback(pageBound.Width, pageBound.Height);

                    NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page
                }

                return resultBounds;
            }
        }

        /// <summary>
        /// Returns the total number of pages for a given PDF.
        /// <param name="source">The PDF source</param>
        /// <param name="password">The password for the pdf file (if required)</param>
        /// </summary>
        public static int CountPages(IPdfSource source, string password = null)
        {
            using(var stream = new PdfFileStream(source))
            {
                ValidatePassword(stream.Document, password);

                return NativeMethods.CountPages(stream.Document); // gets the number of pages in the document
            }
        }

        public static bool NeedsPassword(IPdfSource source)
        {
            using(var stream = new PdfFileStream(source))
            {
                return NeedsPassword(stream.Document);
            }
        }

        private static void ValidatePassword(IntPtr doc, string password)
        {
            if(NeedsPassword(doc) && NativeMethods.AuthenticatePassword(doc, password) == 0)
                throw new MissingOrInvalidPdfPasswordException();
        }

        private static bool NeedsPassword(IntPtr doc)
        {
            return NativeMethods.NeedsPassword(doc) != 0;
        }

        static Bitmap RenderPageAtSize(IntPtr context, IntPtr document, IntPtr page, Size size, float dpiX, float dpiY)
        {
            Rectangle pageBound = NativeMethods.BoundPage(document, page);
            Matrix ctm = new Matrix();

            var zoomFactorX = size.Width / pageBound.Width;
            var zoomFactorY = size.Height / pageBound.Height;
            var zoomX = zoomFactorX * (dpiX / DpiHelper.DEFAULT_DPI);
            var zoomY = zoomFactorY * (dpiY / DpiHelper.DEFAULT_DPI);

            // gets the size of the page and multiplies it with zoom factors
            int nativeWidth = (int)(pageBound.Width * zoomX);
            int nativeHeight = (int)(pageBound.Height * zoomY);

            // sets the matrix as a scaling matrix (zoomX,0,0,zoomY,0,0)
            ctm.A = zoomX;
            ctm.D = zoomY;

            return RenderPage(context, document, page, nativeWidth, nativeHeight, dpiX, dpiY, ctm);
        }

        static Bitmap RenderPageAtWidth(IntPtr context, IntPtr document, IntPtr page, int width, float dpiX, float dpiY)
        {
            Rectangle pageBound = NativeMethods.BoundPage(document, page);
            Matrix ctm = new Matrix();

            var zoomFactor = width / pageBound.Width;
            var zoomX = zoomFactor * (dpiX / DpiHelper.DEFAULT_DPI);
            var zoomY = zoomFactor * (dpiY / DpiHelper.DEFAULT_DPI);

            // gets the size of the page and multiplies it with zoom factors
            int nativeWidth = (int)(pageBound.Width * zoomX);
            int nativeHeight = (int)(pageBound.Height * zoomY);

            // sets the matrix as a scaling matrix (zoomX,0,0,zoomY,0,0)
            ctm.A = zoomX;
            ctm.D = zoomY;

            return RenderPage(context, document, page, nativeWidth, nativeHeight, dpiX, dpiY, ctm);
        }

        static Bitmap RenderPageAtHeight(IntPtr context, IntPtr document, IntPtr page, int height, float dpiX, float dpiY)
        {
            Rectangle pageBound = NativeMethods.BoundPage(document, page);
            Matrix ctm = new Matrix();

            var zoomFactor = height / pageBound.Height;
            var zoomX = zoomFactor * (dpiX / DpiHelper.DEFAULT_DPI);
            var zoomY = zoomFactor * (dpiY / DpiHelper.DEFAULT_DPI);

            // gets the size of the page and multiplies it with zoom factors
            int nativeWidth = (int)(pageBound.Width * zoomX);
            int nativeHeight = (int)(pageBound.Height * zoomY);

            // sets the matrix as a scaling matrix (zoomX,0,0,zoomY,0,0)
            ctm.A = zoomX;
            ctm.D = zoomY;

            return RenderPage(context, document, page, nativeWidth, nativeHeight, dpiX, dpiY, ctm);
        }

        static Bitmap RenderPageAtScale(IntPtr context, IntPtr document, IntPtr page, float zoomFactor, float dpiX, float dpiY)
        {
            Rectangle pageBound = NativeMethods.BoundPage(document, page);
            Matrix ctm = new Matrix();

            var zoomX = zoomFactor * (dpiX / DpiHelper.DEFAULT_DPI);
            var zoomY = zoomFactor * (dpiY / DpiHelper.DEFAULT_DPI);

            // gets the size of the page and multiplies it with zoom factors
            int nativeWidth = (int)(pageBound.Width * zoomX);
            int nativeHeight = (int)(pageBound.Height * zoomY);

            // sets the matrix as a scaling matrix (zoomX,0,0,zoomY,0,0)
            ctm.A = zoomX;
            ctm.D = zoomY;

            return RenderPage(context, document, page, nativeWidth, nativeHeight, dpiX, dpiY, ctm);
        }

        static Bitmap RenderPage(IntPtr context, IntPtr document, IntPtr page, int width, int height, float dpiX, float dpiY, Matrix ctm)
        {
            IntPtr pix = IntPtr.Zero;
            IntPtr dev = IntPtr.Zero;

            // creates a pixmap the same size as the width and height of the page
            pix = NativeMethods.NewPixmap(context, NativeMethods.FindDeviceColorSpace(context, "DeviceRGB"), width, height);
            // sets white color as the background color of the pixmap
            NativeMethods.ClearPixmap(context, pix, 0xFF);

            // creates a drawing device
            dev = NativeMethods.NewDrawDevice(context, pix);
            // draws the page on the device created from the pixmap
            NativeMethods.RunPage(document, page, dev, ctm, IntPtr.Zero);

            NativeMethods.FreeDevice(dev); // frees the resources consumed by the device
            dev = IntPtr.Zero;

            // creates a colorful bitmap of the same size of the pixmap
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            var imageData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            unsafe
            { // converts the pixmap data to Bitmap data
                byte* ptrSrc = (byte*)NativeMethods.GetSamples(context, pix); // gets the rendered data from the pixmap
                byte* ptrDest = (byte*)imageData.Scan0;
                for (int y = 0; y < height; y++)
                {
                    byte* pl = ptrDest;
                    byte* sl = ptrSrc;
                    for (int x = 0; x < width; x++)
                    {
                        //Swap these here instead of in MuPDF because most pdf images will be rgb or cmyk.
                        //Since we are going through the pixels one by one anyway swap here to save a conversion from rgb to bgr.
                        pl[2] = sl[0]; //b-r
                        pl[1] = sl[1]; //g-g
                        pl[0] = sl[2]; //r-b
                        //sl[3] is the alpha channel, we will skip it here
                        pl += 3;
                        sl += 4;
                    }
                    ptrDest += imageData.Stride;
                    ptrSrc += width * 4;
                }
            }
            bmp.UnlockBits(imageData);

            NativeMethods.DropPixmap(context, pix);
            bmp.SetResolution(dpiX, dpiY);

            return bmp;
        }

        /// <summary>
        /// Helper class for an easier disposing of unmanaged resources
        /// </summary>
        private sealed class PdfFileStream : IDisposable
        {
            const uint FZ_STORE_DEFAULT = 256 << 20;

            public IntPtr Context { get; private set; }
            public IntPtr Stream { get; private set; }
            public IntPtr Document { get; private set; }

            public PdfFileStream(IPdfSource source)
            {
                if(source is FileSource)
                {
                    var fs = (FileSource)source;
                    Context = NativeMethods.NewContext(IntPtr.Zero, IntPtr.Zero, FZ_STORE_DEFAULT); // Creates the context
                    Stream = NativeMethods.OpenFile(Context, fs.Filename); // opens file as a stream
                    Document = NativeMethods.OpenDocumentStream(Context, ".pdf", Stream); // opens the document
                }
                else if(source is MemorySource)
                {
                    var ms = (MemorySource)source;
                    Context = NativeMethods.NewContext(IntPtr.Zero, IntPtr.Zero, FZ_STORE_DEFAULT); // Creates the context
                    GCHandle pinnedArray = GCHandle.Alloc(ms.Bytes, GCHandleType.Pinned);
                    IntPtr pointer = pinnedArray.AddrOfPinnedObject();
                    Stream = NativeMethods.OpenStream(Context, pointer, ms.Bytes.Length); // opens file as a stream
                    Document = NativeMethods.OpenDocumentStream(Context, ".pdf", Stream); // opens the document
                    pinnedArray.Free();
                }
            }

            public void Dispose()
            {
                NativeMethods.CloseDocument(Document); // releases the resources
                NativeMethods.CloseStream(Stream);
                NativeMethods.FreeContext(Context);
            }
        }

        private static class NativeMethods
        {
            const string DLL = "MuPdf.dll";


            /**
             * mupdf/fitz/base_context.c
             */

            [DllImport(DLL, EntryPoint = "fz_new_context", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NewContext(IntPtr alloc, IntPtr locks, uint max_store);

            [DllImport(DLL, EntryPoint = "fz_free_context", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr FreeContext(IntPtr ctx);


            /**
             * mupdf/fitz/doc_document.c
             */

            [DllImport(DLL, EntryPoint = "fz_open_document_with_stream", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr OpenDocumentStream(IntPtr ctx, string magic, IntPtr stream);

            [DllImport(DLL, EntryPoint = "fz_close_document", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CloseDocument(IntPtr doc);

            [DllImport(DLL, EntryPoint = "fz_needs_password", CallingConvention = CallingConvention.Cdecl)]
            public static extern int NeedsPassword(IntPtr doc);

            [DllImport(DLL, EntryPoint = "fz_authenticate_password", CallingConvention = CallingConvention.Cdecl)]
            public static extern int AuthenticatePassword(IntPtr doc, string password);

            [DllImport(DLL, EntryPoint = "fz_count_pages", CallingConvention = CallingConvention.Cdecl)]
            public static extern int CountPages(IntPtr doc);

            [DllImport(DLL, EntryPoint = "fz_bound_page", CallingConvention = CallingConvention.Cdecl)]
            public static extern Rectangle BoundPage(IntPtr doc, IntPtr page);

            [DllImport(DLL, EntryPoint = "fz_free_page", CallingConvention = CallingConvention.Cdecl)]
            public static extern void FreePage(IntPtr doc, IntPtr page);

            [DllImport(DLL, EntryPoint = "fz_load_page", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr LoadPage(IntPtr doc, int pageNumber);

            [DllImport(DLL, EntryPoint = "fz_run_page", CallingConvention = CallingConvention.Cdecl)]
            public static extern void RunPage(IntPtr doc, IntPtr page, IntPtr device, Matrix transform, IntPtr cookie);


            /**
             * mupdf/fitz/stm_open.c
             */

            [DllImport(DLL, EntryPoint = "fz_open_file_w", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr OpenFile(IntPtr ctx, string filename);

            [DllImport(DLL, EntryPoint = "fz_close", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CloseStream(IntPtr stream);

            [DllImport(DLL, EntryPoint = "fz_open_memory", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr OpenStream(IntPtr ctx, IntPtr data, int length);


            /**
             * mupdf/fitz/res_pixmap.c
             */

            [DllImport(DLL, EntryPoint = "fz_clear_pixmap_with_value", CallingConvention = CallingConvention.Cdecl)]
            public static extern void ClearPixmap(IntPtr ctx, IntPtr pix, int byteValue);

            [DllImport(DLL, EntryPoint = "fz_new_pixmap", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NewPixmap(IntPtr ctx, IntPtr colorspace, int width, int height);

            [DllImport(DLL, EntryPoint = "fz_drop_pixmap", CallingConvention = CallingConvention.Cdecl)]
            public static extern void DropPixmap(IntPtr ctx, IntPtr pixels);


            /**
             * mupdf/fitz/res_colorspace.c
             */

            [DllImport(DLL, EntryPoint = "fz_find_device_colorspace", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr FindDeviceColorSpace(IntPtr ctx, string colorspace);


            /**
             * mupdf/fitz/res_bitmap.c
             */

            [DllImport(DLL, EntryPoint = "fz_pixmap_samples", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetSamples(IntPtr ctx, IntPtr pixels);


            /**
             * mupdf/fitz/draw_device.c
             */

            [DllImport(DLL, EntryPoint = "fz_new_draw_device", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NewDrawDevice(IntPtr ctx, IntPtr pixels);


            /**
             * mupdf/fitz/dev_null.c
             */

            [DllImport(DLL, EntryPoint = "fz_free_device", CallingConvention = CallingConvention.Cdecl)]
            public static extern void FreeDevice(IntPtr device);
        }
    }

    internal struct Rectangle
    {
        public float Left, Top, Right, Bottom;

        public float Width { get { return this.Right - this.Left; } }
        public float Height { get { return this.Bottom - this.Top; } }

    }

#pragma warning disable 0649
    internal struct BBox
    {
        public int Left, Top, Right, Bottom;
    }

    internal struct Matrix
    {
        public float A, B, C, D, E, F;
    }
#pragma warning restore 0649

    public class MissingOrInvalidPdfPasswordException : Exception
    {
        public MissingOrInvalidPdfPasswordException()
            : base("A password for the pdf document was either not provided or is invalid.")
        { }
    }

    public interface IPdfSource { }

    public class FileSource : IPdfSource
    {
        public string Filename { get; private set; }

        public FileSource(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("The file \"" + filename + "\" does not exist !", filename);
            }
            this.Filename = filename;
        }
    }

    public class MemorySource : IPdfSource
    {
        public byte[] Bytes { get; private set; }

        public MemorySource(byte[] bytes)
        {
            this.Bytes = bytes;
        }
    }
}
