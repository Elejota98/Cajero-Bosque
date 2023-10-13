using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.WinForm.FrontEnd.Log_Viewer
{
    public class BackwardReader
    {

        private string path;
        private FileStream fs = null;

        public BackwardReader(string path)
        {
            this.path = path;
            fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fs.Seek(0, SeekOrigin.End);
        }

        public string Readline()
        {

            byte[] line;
            byte[] text = new byte[1];
            long position = 0;
            int count;

            fs.Seek(0, SeekOrigin.Current);
            position = fs.Position;

            if (fs.Length > 1)
            {
                byte[] vagnretur = new byte[2];
                fs.Seek(-2, SeekOrigin.Current);
                fs.Read(vagnretur, 0, 2);

                if (Encoding.UTF8.GetString(vagnretur).Equals("\r\n"))
                {
                    //move it back
                    fs.Seek(-2, SeekOrigin.Current);
                    position = fs.Position;
                }
            }

            while (fs.Position > 0)
            {
                text.Initialize();
                //read one char
                fs.Read(text, 0, 1);
                string asciiText = Encoding.UTF8.GetString(text);

                //moveback to the charachter before
                fs.Seek(-2, SeekOrigin.Current);

                if (asciiText.Equals("\n"))
                {
                    fs.Read(text, 0, 1);
                    asciiText = Encoding.UTF8.GetString(text);
                    if (asciiText.Equals("\r"))
                    {
                        fs.Seek(1, SeekOrigin.Current);
                        break;
                    }
                }
            }

            count = int.Parse((position - fs.Position).ToString());
            line = new byte[count];
            fs.Read(line, 0, count);
            fs.Seek(-count, SeekOrigin.Current);

            return Encoding.UTF8.GetString(line);


        }

        public bool SOF
        {
            get
            {
                return fs.Position == 0;
            }
        }

        public void Close()
        {
            fs.Close();
        }
    }
}
