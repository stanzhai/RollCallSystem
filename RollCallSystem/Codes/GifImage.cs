using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Interop;
using System.Windows;
using System.Runtime.InteropServices;

namespace RollCallSystem.Codes
{
    public class GIFImage : System.Windows.Controls.Image
    {
        delegate void OnFrameChangedDelegate();
        private Bitmap m_Bitmap;
        public string Path { get; set; }
        BitmapSource bitmapSource;

        /// <summary>
        /// 设置要显示的Gif图像
        /// </summary>
        public Bitmap BitmapImage
        {
            get { return this.m_Bitmap; }
            set { this.m_Bitmap = value; AnimatedImageControl(); }
        }


        public void AnimatedImageControl()
        {
            Width = m_Bitmap.Width;
            Height = m_Bitmap.Height;
            
            ImageAnimator.Animate(m_Bitmap, OnFrameChanged);
            bitmapSource = GetBitmapSource();
            Source = bitmapSource;
        }


        private void OnFrameChanged(object sender, EventArgs e)
        {

            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                   new OnFrameChangedDelegate(OnFrameChangedInMainThread));

        }

        private void OnFrameChangedInMainThread()
        {
            ImageAnimator.UpdateFrames();
            if (bitmapSource != null)
                bitmapSource.Freeze();
            bitmapSource = GetBitmapSource();
            Source = bitmapSource;
            InvalidateVisual();
        }

        //private static bool loaded;
        private BitmapSource GetBitmapSource()
        {
            IntPtr inptr = m_Bitmap.GetHbitmap();
            bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                inptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(inptr);
            return bitmapSource;
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

    }

}
