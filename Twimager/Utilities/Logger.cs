﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace Twimager.Utilities
{
    public class Logger
    {
        private readonly string _path;
        private StreamWriter _writer;

        public Logger(string path = null)
        {
            _path = path;

            if (_path == null) return;
            _writer = File.AppendText(_path);
            _writer.AutoFlush = true;
        }

        ~Logger()
        {
            _writer.Close();
        }

        public async Task LogAsync(string message)
        {
            message = AppendDateTime(message);
            Console.WriteLine(message);

            if (_writer == null) return;
            await _writer.WriteLineAsync(message);
        }

        public async Task LogExceptionAsync(Exception e)
        {
            var inner = e;
            while (inner != null)
            {
                await LogAsync(
                    $"{(inner == e ? "" : "Caused by: ")}{e.GetType().FullName}: {e.Message}"
                );

                await LogAsync(e.StackTrace);
            }
        }
        
        private string AppendDateTime(string message)
        {
            return $"{DateTime.Now.ToString()}: {message}";
        }
    }
}
