//
// Shake - C# Make
//
// Simple TextWriter logger
// Basically for logging console output
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System.IO;
using System.Text;

namespace Shake.Helpers
{
    /// <summary>
    /// TextWriter logger class
    /// </summary>
    internal class TextWriterLogger : TextWriter
    {
        private readonly TextWriter _textWriter;
        readonly StringWriter _stringWriter = new StringWriter();

        public TextWriterLogger(TextWriter textWriterToLog)
        {
            _textWriter = textWriterToLog;
        }

        public override Encoding Encoding
        {
            get { return _textWriter.Encoding; }
        }

        public override void Write(char value)
        {
            _textWriter.Write(value);
            _stringWriter.Write(value);
        }

        internal void SaveToFile(string path)
        {
            File.WriteAllText(path, _stringWriter.ToString());
        }
    }
}