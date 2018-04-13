using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Sftp
{
    public class Sftp : IDisposable
    {
        SftpClient _Sftp;

        #region Properties
        public bool IsConnected
        {
            get { return _Sftp.IsConnected; }
        }
        #endregion
        public Sftp(string ip, string account, string password) : this(ip, 22, account, password)
        {
            
        }
        public Sftp(string ip, int port, string account, string password)
        {
            _Sftp = new SftpClient(ip, port, account, password);
            try
            {
                _Sftp.Connect();
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            if(_Sftp != null)
            {
                _Sftp.Disconnect();
                _Sftp.Dispose();
                _Sftp = null;
            }
        }

        /// <summary>
        /// ls given path 
        /// </summary>
        /// <param name="lsPath"></param>
        /// <returns></returns>
        public IEnumerable<SftpFile> Show_ls(string lsPath)
        {
            IEnumerable<SftpFile> list = null;
            try
            {
                list = _Sftp.ListDirectory(lsPath);
            }
            catch (Exception)
            {

                throw;
            }
            return list;
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="srcFile">本地端路徑，含檔名</param>
        /// <param name="dstFile">伺服端路徑，含檔名</param>
        public void UploadFile(string srcFile, string dstFile)
        {
            FileStream file = null;
            try
            {
                file = File.OpenRead(srcFile);
                _Sftp.UploadFile(file, dstFile);
            }
            catch (Exception)
            {
                CloseOpenFile(file);
                throw;
            }

            CloseOpenFile(file);
        }

        /// <summary>
        /// 下載檔案
        /// </summary>
        /// <param name="srcFile">伺服端路徑，含檔名</param>
        /// <param name="dstFile">本地端路徑，含檔名</param>
        public void DownloadFile(string srcFile, string dstFile)
        {
            if(Directory.Exists(Path.GetDirectoryName(dstFile)) == false)
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(dstFile));
                }
                catch (Exception)
                {
                    throw;
                }
            }

            FileStream file = null;
            try
            {
                file = File.OpenWrite(dstFile);
                _Sftp.DownloadFile(srcFile, file);
            }
            catch (Exception)
            {
                CloseOpenFile(file);
                throw;
            }
            CloseOpenFile(file);
        }

        /// <summary>
        /// 關閉已開啟的檔案
        /// </summary>
        /// <param name="file"></param>
        private void CloseOpenFile(FileStream file)
        {
            if(file != null)
            {
                file.Close();
                file.Dispose();
                file = null;
            }
        }

    }
}
