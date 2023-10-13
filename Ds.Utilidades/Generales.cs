using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ds.Utilidades
{
    public class Generales
    {

        /// <summary>
        /// Verifica si el archivo está en uso
        /// </summary>
        /// <param name="sRuta"></param>
        /// <param name="sNombreArchivo"></param>
        /// <returns></returns>
        public static bool VerificaArchivoEnUso(string sRuta, string sNombreArchivo)
        {
            try
            {
                using (var stream = new FileStream(sRuta + "\\" + sNombreArchivo, FileMode.Open, FileAccess.Read)) { }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

        public static bool WebSiteIsAvailable(string Url)
        {
            bool IsAvailable = false;

            string Message = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Url);

            // Set the credentials to the current user account
            request.Credentials = new NetworkCredential("admin", "admin");
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Do nothing; we're only testing to see if we can get the response
                }
            }
            catch (WebException ex)
            {
                Message += ((Message.Length > 0) ? "\n" : "") + ex.Message;
            }

            IsAvailable = (Message.Length == 0);

            return IsAvailable;
        }

        public static string CadenaLista<T>(List<T> lstLista, string fieldName, string sSeparador)
        {
            string sCadena = "";
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            if (fieldName != null)
            {
                if (lstLista.Count > 0)
                {
                    string[] Datos = new string[lstLista.Count];
                    int n = 0;
                    foreach (T item in lstLista)
                    {
                        int i = 0;
                        string[] sCampos = fieldName.Split(delimiterChars);
                        string[] Datos1 = new string[sCampos.Length];
                        foreach (string sfieldName in sCampos)
                        {
                            PropertyInfo pi = item.GetType().GetProperty(sfieldName);
                            Datos1[i++] = pi.GetValue(item, null).ToString();
                        }
                        Datos[n++] = string.Join("-", Datos1);
                    }
                    sCadena = string.Join(sSeparador, Datos);

                }
            }
            return sCadena;
        }

        public DataTable convert2Table<T>(List<T> list) //Este metodo convierte un List en un Datatable
        {
            DataTable table = new DataTable();
            T target = Activator.CreateInstance<T>();
            PropertyInfo[] properties = target.GetType().GetProperties();
            List<string> columns = new List<string>();
            if (list.Count > 0)
            {
                foreach (PropertyInfo pi in properties)
                {
                    table.Columns.Add(pi.Name);
                    columns.Add(pi.Name);
                }
                foreach (T item in list)
                {
                    object[] cells = getValues(columns, item);
                    table.Rows.Add(cells);
                }
            }
            else
            {

                foreach (PropertyInfo pi in properties)
                {
                    table.Columns.Add(pi.Name);
                    columns.Add(pi.Name);
                }
            }
            return table;
        }

        private static object[] getValues(List<string> columns, object instance) //Este metodo es usado por el metodo convert2Table
        {
            object[] ret = new object[columns.Count];
            for (int n = 0; n < ret.Length; n++)
            {
                PropertyInfo pi = instance.GetType().GetProperty(columns[n]);
                object value = pi.GetValue(instance, null);
                ret[n] = value;
            }
            return ret;
        }

        public static byte[] ImageToByteArray(Image imageIn, ImageFormat oFormato)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, oFormato);
            return ms.ToArray();
        }

        public static Stream RetornaStreamByte(byte[] byteImagen)
        {

            Stream StreamFile = null;

            MemoryStream ms = new MemoryStream();
            ms.Write(byteImagen, 0, byteImagen.Length);
            ms.Flush();
            ms.Close();

            StreamFile = (Stream)ms;

            ms.Dispose();
            ms = null;

            return StreamFile;
        }

        public static byte[] RetornaByteStream(Stream StreamImagen)
        {
            byte[] barrImg = new byte[1];

            MemoryStream ms = new MemoryStream();
            ms = (MemoryStream)StreamImagen;
            barrImg = ms.ToArray();
            ms.Close();
            ms.Dispose();
            ms = null;


            return barrImg;
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
