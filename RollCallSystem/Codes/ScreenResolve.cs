using System.Runtime.InteropServices;

namespace RollCallSystem.Codes
{
    // 更改屏幕分辨率的类
    static class ScreenResolve
    {
        public enum DMDO 
        { 
            DEFAULT = 0, 
            D90 = 1, 
            D180 = 2, 
            D270 = 3 
        } 
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)] 
        struct DEVMODE 
        { 
            public const int DM_DISPLAYFREQUENCY = 0x400000; 
            public const int DM_PELSWIDTH = 0x80000; 
            public const int DM_PELSHEIGHT = 0x100000; 
            private const int CCHDEVICENAME = 32; 
            private const int CCHFORMNAME = 32; 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)] 
            public string dmDeviceName; 
            public short dmSpecVersion; 
            public short dmDriverVersion; 
            public short dmSize; 
            public short dmDriverExtra; 
            public int dmFields; 
            public int dmPositionX; 
            public int dmPositionY; 
            public DMDO dmDisplayOrientation; 
            public int dmDisplayFixedOutput; 
            public short dmColor; 
            public short dmDuplex; 
            public short dmYResolution; 
            public short dmTTOption; 
            public short dmCollate; 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)] 
            public string dmFormName; 
            public short dmLogPixels; 
            public int dmBitsPerPel; 
            public int dmPelsWidth; 
            public int dmPelsHeight; 
            public int dmDisplayFlags; 
            public int dmDisplayFrequency; 
            public int dmICMMethod; 
            public int dmICMIntent; 
            public int dmMediaType; 
            public int dmDitherType; 
            public int dmReserved1; 
            public int dmReserved2; 
            public int dmPanningWidth; 
            public int dmPanningHeight; 
        } 

        [DllImport("user32.dll", CharSet = CharSet.Auto)] 
        static extern int ChangeDisplaySettings([In] ref DEVMODE lpDevMode, int dwFlags); 

        public static void ChangeScreen(int width,int height,int frequency)
        {
            long RetVal = 0; 
            DEVMODE dm = new DEVMODE(); 
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE)); 
            dm.dmPelsWidth = width; 
            dm.dmPelsHeight = height; 
            dm.dmDisplayFrequency = frequency; 
            dm.dmFields = DEVMODE.DM_PELSWIDTH | DEVMODE.DM_PELSHEIGHT | DEVMODE.DM_DISPLAYFREQUENCY; 
            RetVal = ChangeDisplaySettings(ref dm, 0); 
        }
    }
}