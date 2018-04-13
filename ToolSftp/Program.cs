using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Sftp;

namespace ToolSftp
{
    class Program
    {
        static void Main(string[] args)
        {
            Sftp sftp = null;
            try
            {
                sftp = new Sftp("127.0.0.1", "test", "test");
            }
            catch (Exception ex)
            {
                Console.WriteLine("create sftp obj fail: " + ex.Message);
                return;
            }
            if(sftp.IsConnected == false)
            {
                return;
            }
            string serverPath = "/home/sftp/";
            string localPath = @"D:\sftp\";

            try
            {
                var list = sftp.Show_ls(serverPath);
                foreach (var l in list)
                {
                    Console.WriteLine(l.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("show files fail: " + ex.Message);
            }

            try
            {
                sftp.DownloadFile(serverPath + "testa.txt", localPath + "testb.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("sftp download fail: " + ex.Message);
            }

            try
            {
                sftp.UploadFile(localPath + "testb.txt", serverPath + "testc.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("sftp upload fail: " + ex.Message);
            }
            sftp.Dispose();
            sftp = null;
        }
    }
}
