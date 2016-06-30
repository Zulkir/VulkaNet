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