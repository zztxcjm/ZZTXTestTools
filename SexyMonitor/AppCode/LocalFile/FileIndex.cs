using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyMonitor
{
    public class FileIndex : IDisposable
    {

        private const string IndexFileName = "index.dbf";

        private string _currentDirName = null;
        private string _currentIndexFullPath = null;
        private Dictionary<string, int> _hash = new Dictionary<string, int>();

        private Stream _sr = null;
        private StreamReader _reader = null;
        private StreamWriter _writer = null;

        private object _lock1 = new object();

        public FileIndex(string dirName)
        {

            if (String.IsNullOrEmpty(dirName))
                throw new Exception("索引目录为空，初始化索引失败");
            if (!System.IO.Directory.Exists(dirName))
                throw new Exception("索引目录不存在(" + dirName + ")");

            _currentDirName = dirName;
            if (_currentDirName.EndsWith("\\"))
                _currentIndexFullPath = _currentDirName + IndexFileName;
            else
                _currentIndexFullPath = _currentDirName + "\\" + IndexFileName;

            _sr = new FileStream(_currentIndexFullPath, FileMode.OpenOrCreate);
            _reader = new StreamReader(_sr, Encoding.UTF8);
            _writer = new StreamWriter(_sr, Encoding.UTF8);

            //初始化索引到内存
            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                if (!String.IsNullOrWhiteSpace(line))
                    _hash.Add(line, 0);
            }

        }

        public void AddFile(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                return;

            if (_hash.ContainsKey(filename))
                return;

            lock (_lock1)
            {
                if (!_hash.ContainsKey(filename))
                {
                    _hash.Add(filename, 0);
                    //写到文件
                    _writer.WriteLine(filename);
                    _writer.Flush();

                }
            }

        }

        public bool Exist(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                return false;

            return _hash.ContainsKey(filename);

        }

        public void Close()
        {
            try
            {
                if (_sr != null)
                    _sr.Close();
                if (_reader != null)
                    _reader.Close();
                if (_writer != null)
                    _writer.Close();

                _hash = null;

            }
            catch
            {
            }
        }

        public void Dispose()
        {
            Close();
        }
    }

}
