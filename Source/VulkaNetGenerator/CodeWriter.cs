#region License
/*
Copyright (c) 2016 VulkaNet Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System;
using System.IO;

namespace VulkaNetGenerator
{
    public class CodeWriter : IDisposable
    {
        private readonly StreamWriter writer;
        private int indent;

        public CodeWriter(Stream stream)
        {
            writer = new StreamWriter(stream);
            indent = 0;
        }

        public void Tab(int spaces = 4) => indent += spaces;
        public void UnTab(int spaces = 4) => indent -= spaces;

        public void WriteLine() =>
            writer.WriteLine();

        public void WriteLine(string line)
        {
            for (int i = 0; i < indent; i++)
                writer.Write(' ');
            writer.WriteLine(line);
        }

        public CodeWriterCurlyBrackets Curly() => 
            new CodeWriterCurlyBrackets(this);

        public void Dispose()
        {
            writer.Dispose();
        }
    }

    public class CodeWriterCurlyBrackets : IDisposable
    {
        private readonly CodeWriter writer;
        private bool isDisposedAlready;

        public CodeWriterCurlyBrackets(CodeWriter writer)
        {
            this.writer = writer;
            writer.WriteLine("{");
            writer.Tab();
        }

        public void Dispose()
        {
            if (isDisposedAlready)
                return;
            isDisposedAlready = true;
            writer.UnTab();
            writer.WriteLine("}");
        }
    }
}