using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class Ticket
    {
        //private Font _Fuente = new Font("Gulim", 6, GraphicsUnit.Point);
        //private Font _Fuente = new Font("GulimChe", 6, GraphicsUnit.Point);
        //private Font _Fuente = new Font("Dotum", 6, GraphicsUnit.Point);
        //private Font _Fuente = new Font("DotumChe", 6, GraphicsUnit.Point);

        //private Font _Fuente = new Font("Ms Gothic", 6, GraphicsUnit.Point); error en las lineas horizontales

        private Font _Fuente = new Font("Ms Gothic", 9, FontStyle.Regular);
        private Font _Fuente2 = new Font("Ms Gothic", 9, FontStyle.Regular);

        public Font Fuente2
        {
            get { return _Fuente2; }
            set { _Fuente2 = value; }
        }
        private Brush _ColorFuente = Brushes.Black;
        private int _Margen = 1;

        public int Margen
        {
            get { return _Margen; }
            set { _Margen = value; }
        }
        private int _Offset = 10;

        public int Offset
        {
            get { return _Offset; }
            set { _Offset = value; }
        }

        public Font Fuente
        {
            get { return _Fuente; }
            set { _Fuente = value; }
        }

        public Brush ColorFuente
        {
            get { return _ColorFuente; }
            set { _ColorFuente = value; }
        }



        string ticket = "";
        string parte1, parte2;

        string impresora = "Nanoptix - High Speed VL"; // nombre exacto de la impresora como esta en el panel de control
        int max, cort;
        public string LineasGuion()
        {
            ticket = "--------------------------------------------\n";   // agrega lineas separadoras -
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime linea
            return ticket;
        }
        public string LineasAsterisco()
        {
            ticket = "********************************************\n";   // agrega lineas separadoras *
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime linea
            return ticket;
        }
        public string LineasIgual()
        {
            ticket = "============================================\n";   // agrega lineas separadoras =
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime linea
            return ticket;
        }
        public string LineasTotales()
        {
            ticket = "                                  ----------\n"; ;   // agrega lineas de total
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime linea
            return ticket;
        }

        public string TextoIzquierda(string par1)                          // agrega texto a la izquierda
        {
            max = par1.Length;
            if (max > 44)                                 // **********
            {
                cort = max - 44;
                parte1 = par1.Remove(44, cort);        // si es mayor que 44 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1 + "\n";
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }
        public string TextoDerecha(string par1)
        {
            ticket = "";
            max = par1.Length;
            if (max > 44)                                 // **********
            {
                cort = max - 44;
                parte1 = par1.Remove(44, cort);           // si es mayor que 44 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            max = 44 - par1.Length;                     // obtiene la cantidad de espacios para llegar a 44
            for (int i = 0; i < max; i++)
            {
                ticket += " ";                          // agrega espacios para alinear a la derecha
            }
            ticket += parte1 + "\n";                    //Agrega el texto
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }
        public string TextoCentro(string par1)
        {
            ticket = "";
            max = par1.Length;
            if (max > 44)                                 // **********
            {
                cort = max - 44;
                parte1 = par1.Remove(44, cort);          // si es mayor que 44 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            max = (int)(44 - parte1.Length) / 2;         // saca la cantidad de espacios libres y divide entre dos
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios antes del texto a centrar
            }                                            // **********
            ticket += parte1 + "\n";
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }
        public string TextoExtremos(string par1, string par2)
        {
            max = par1.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte1 = par1.Remove(18, cort);          // si par1 es mayor que 18 lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1;                             // agrega el primer parametro
            max = par2.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte2 = par2.Remove(18, cort);          // si par2 es mayor que 18 lo corta
            }
            else { parte2 = par2; }
            max = 44 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                 // **********
            {
                ticket += " ";                            // Agrega espacios para poner par2 al final
            }                                             // **********
            ticket += parte2 + "\n";                     // agrega el segundo parametro al final
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }
        public string AgregaTotales(string par1, double total)
        {
            max = par1.Length;
            if (max > 25)                                 // **********
            {
                cort = max - 25;
                parte1 = par1.Remove(25, cort);          // si es mayor que 25 lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1;
            parte2 = total.ToString("#,###");
            max = 44 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios para poner el valor de moneda al final
            }                                            // **********
            ticket += parte2 + "\n";
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }

        #region preliquidacion
        public string TextoExtremosPre(string par1, string par2)
        {
            max = par1.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte1 = par1.Remove(18, cort);          // si par1 es mayor que 18 lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1;                             // agrega el primer parametro
            max = par2.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte2 = par2.Remove(18, cort);          // si par2 es mayor que 18 lo corta
            }
            else { parte2 = par2; }
            max = 55 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                 // **********
            {
                ticket += " ";                            // Agrega espacios para poner par2 al final
            }                                             // **********
            ticket += parte2 + "\n\n";                     // agrega el segundo parametro al final
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }
        public string AgregaTotalesPre(string par1, string total)
        {
            max = par1.Length;
            if (max > 25)                                 // **********
            {
                cort = max - 25;
                parte1 = par1.Remove(25, cort);          // si es mayor que 25 lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1;
            parte2 = total;
            max = 55 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios para poner el valor de moneda al final
            }                                            // **********
            ticket += parte2 + "\n\n";
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }
        public string LineasGuionPre()
        {
            ticket = "-------------------------------------------------------\n\n";   // agrega lineas separadoras -
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime linea
            return ticket;
        }
        public string LineasTotalesPre()
        {
            ticket = "                                               --------\n\n"; ;   // agrega lineas de total
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime linea
            return ticket;
        }
        public string EncabezadoPreliquidacion()
        {
            //ticket = "CANT. T.PRODUCTO DESCRIPCIÓN T.EDAD V.UNITARIO PORCENTAJE V.GRAVAMEN V.SEGURO SUBTOTAL\n\n\n";   // agrega lineas de  encabezados
            ticket = "T.PRODUCTO DESCRIPCIÓN T.EDAD CANT.            SUBTOTAL\n"; //9ESPACIOS
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }
        public string AgregaArticuloPreliquidacion(string tprod, string Desc, string tedad, string cant, string valor, string total)
        {

            try
            {
                max = tprod.Length;
                if (max > 11)                                 // **********
                {
                    cort = max - 11;
                    parte1 = tprod.Remove(11, cort);          // corta a 15 la descripcion del articulo
                }
                else { parte1 = tprod; }                      // **********
                ticket = "    " + parte1;                             // agrega articulo

                max = 12 - (Desc.ToString().Length) + 4;
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********
                ticket += Desc.ToString(); // agrega Desc

                max = 7 - (tedad.ToString().Length) - 2;
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********
                ticket += tedad.ToString(); // agrega Desc

                max = 6 - (cant.ToString().Length) + 0;
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********
                ticket += cant.ToString(); // agrega Desc

                max = 11 - (valor.ToString().Length) + 2;
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********
                ticket += valor.ToString(); // agrega Desc

                max = 9 - (total.ToString().Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********
                ticket += total + "\n\n"; // agrega Desc
                //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
                return ticket;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
        #endregion

        public string EncabezadoVenta()
        {
            ticket = "PARTE          DENOM     CANTIDAD     DINERO\n";   // agrega lineas de  encabezados
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }

        public string AgregaArticuloCarga(string par1, int denom, int cant, int total)
        {
            if (denom.ToString().Length <= 5 && cant.ToString().Length <= 10 && total.ToString().Length <= 11) // valida que denom cant y total esten dentro de rango
            {
                max = par1.Length;
                if (max > 15)                                 // **********
                {
                    cort = max - 15;
                    parte1 = par1.Remove(15, cort);          // corta a 15 la descripcion del articulo
                }
                else { parte1 = par1; }                      // **********
                ticket = parte1;                             // agrega articulo
                max = (5 - denom.ToString().Length) + (15 - parte1.Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios para poner el valor de denomidad
                }
                //if (par1 != "Box")
                //{
                ticket += denom.ToString();  // agrega denomidad
                //}
                //else
                //{
                //    ticket += "-"; // agrega denomidad
                //}

                max = 13 - (cant.ToString().Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********

                //if (par1 != "Box")
                //{
                ticket += cant.ToString(); // agrega cant
                //}
                //else
                //{
                //    ticket += "-"; // agrega cant
                //}

                max = 11 - (total.ToString().Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********
                ticket += total.ToString() + "\n"; // agrega cant
                //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
                return ticket;
            }
            else
            {
                //MessageBox.Show("Valores fuera de rango");
                //RawPrinterHelper.SendStringToPrinter(impresora, "Error, valor fuera de rango\n"); // imprime texto
                ticket = "";
                return ticket;
            }
        }

        public string EncabezadoVentaTicket()
        {
            ticket = "Cant	     Descripción	                  Valor\n";   // agrega lineas de  encabezados
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }

        public string TextoExtremosInterno(string par1, string par2)
        {
            max = par1.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte1 = par1.Remove(18, cort);          // si par1 es mayor que 18 lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1;                             // agrega el primer parametro
            max = par2.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte2 = par2.Remove(18, cort);          // si par2 es mayor que 18 lo corta
            }
            else { parte2 = par2; }
            max = 34 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                 // **********
            {
                ticket += " ";                            // Agrega espacios para poner par2 al final
            }                                             // **********
            ticket += parte2;                     // agrega el segundo parametro al final
            //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            return ticket;
        }

        public string AgregaArticuloTicket(string cant, string desc, string total)
        {
            if (cant.Length <= 4 && desc.Length <= 20 && total.Length <= 11) // valida que cant desc y total esten dentro de rango
            {
                max = cant.Length;
                if (max > 4)                                 // **********
                {
                    cort = max - 4;
                    parte1 = cant.Remove(4, cort);          // corta a 15 la descripcion del articulo
                }
                else { parte1 = cant; }                      // **********
                ticket = parte1;
                if (cant == "Cant")
                {
                    ticket += "      " + TextoExtremosInterno(desc, total);
                }
                else
                {
                    ticket += "         " + TextoExtremosInterno(desc, total);
                }

                // agrega articulo
                //max = (15 - desc.Length) + 9;
                //for (int i = 0; i < max; i++)                // **********
                //{
                //    ticket += " ";                           // Agrega espacios para poner el valor de cantidad
                //}
                //ticket += desc.ToString();                   // agrega cantidad          
                //max = 24 - (total.Length);
                //for (int i = 0; i < max; i++)                // **********
                //{
                //    ticket += " ";                           // Agrega espacios
                //}                                            // **********
                //ticket += total + "\n"; // agrega desc
                ////RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
                return ticket;
            }
            else
            {
                //MessageBox.Show("Valores fuera de rango");
                //RawPrinterHelper.SendStringToPrinter(impresora, "Error, valor fuera de rango\n"); // imprime texto
                ticket = "";
                return ticket;
            }
        }

        public string AgregaArticulo(string par1, int cant, int precio, int total)
        {
            if (cant.ToString().Length <= 3 && precio.ToString("c").Length <= 10 && total.ToString("c").Length <= 11) // valida que cant precio y total esten dentro de rango
            {
                max = par1.Length;
                if (max > 15)                                 // **********
                {
                    cort = max - 15;
                    parte1 = par1.Remove(15, cort);          // corta a 15 la descripcion del articulo
                }
                else { parte1 = par1; }                      // **********
                ticket = parte1;                             // agrega articulo
                max = (5 - cant.ToString().Length) + (15 - parte1.Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios para poner el valor de cantidad
                }
                ticket += cant.ToString();                   // agrega cantidad
                max = 13 - (precio.ToString("c").Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********
                ticket += precio.ToString("c"); // agrega precio
                max = 6 - (total.ToString().Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********
                ticket += total.ToString("c") + "\n"; // agrega precio
                //RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
                return ticket;
            }
            else
            {
                //MessageBox.Show("Valores fuera de rango");
                //RawPrinterHelper.SendStringToPrinter(impresora, "Error, valor fuera de rango\n"); // imprime texto
                ticket = "";
                return ticket;
            }
        }

        public string CortaTicket()
        {
            string corte = "\x1B" + "m";                  // caracteres de corte
            string avance = "\x1B" + "d" + "\x09";        // avanza 9 renglones
            //RawPrinterHelper.SendStringToPrinter(impresora, avance); // avanza
            //RawPrinterHelper.SendStringToPrinter(impresora, corte); // corta
            return corte;
        }

        public string AbreCajon()
        {
            string cajon0 = "\x1B" + "p" + "\x00" + "\x0F" + "\x96";                  // caracteres de apertura cajon 0
            string cajon1 = "\x1B" + "p" + "\x01" + "\x0F" + "\x96";                 // caracteres de apertura cajon 1
            //RawPrinterHelper.SendStringToPrinter(impresora, cajon0); // abre cajon0
            //RawPrinterHelper.SendStringToPrinter(impresora, cajon1); // abre cajon1
            return cajon0;
        }

    }
}
