using Ds.BusinessService.DataTransferObject;
using Ds.BusinessService.Entities;
using Ds.BusinessService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.DataService
{
    public partial class DataService : IDataService
    {
        public ResultadoOperacion ObtenerSincronizacionTransaccion()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoTransacciones oDtoTransaccion = new DtoTransacciones();

            ModuloDataSet.P_DatosTransaccionSincronizacionDataTable _InfoTransaccionTable = new ModuloDataSet.P_DatosTransaccionSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_DatosTransaccionSincronizacionTableAdapter _InfoTransaccionAdapter = new ModuloDataSetTableAdapters.P_DatosTransaccionSincronizacionTableAdapter();


            try
            {
                _InfoTransaccionTable.Constraints.Clear();

                if (_InfoTransaccionAdapter.Fill(_InfoTransaccionTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info transaccion OK";

                    for (int i = 0; i < _InfoTransaccionTable.Rows.Count; i++)
                    {
                        oDtoTransaccion.IdTransaccion = Convert.ToInt64(_InfoTransaccionTable.Rows[i][0]);
                        oDtoTransaccion.IdModulo = _InfoTransaccionTable.Rows[i][1].ToString();
                        oDtoTransaccion.IdTipoTransaccion = Convert.ToInt64(_InfoTransaccionTable.Rows[i][2]);
                        oDtoTransaccion.IdSede = Convert.ToInt64(_InfoTransaccionTable.Rows[i][3]);
                        oDtoTransaccion.FechaTransaccion = Convert.ToDateTime(_InfoTransaccionTable.Rows[i][4]);

                        string ValorRecibido = _InfoTransaccionTable.Rows[i][5].ToString();

                        if (ValorRecibido != string.Empty)
                        {
                            oDtoTransaccion.ValorRecibido = Convert.ToDouble(ValorRecibido);
                        }
                        else 
                        {
                            oDtoTransaccion.ValorRecibido = 0;
                        }

                        string Iva = _InfoTransaccionTable.Rows[i][6].ToString();
                        if (Iva != string.Empty)
                        {
                            oDtoTransaccion.Iva = Convert.ToDouble(Iva);
                        }
                        else 
                        {
                            oDtoTransaccion.Iva = 0;
                        }

                        string Comision = _InfoTransaccionTable.Rows[i][7].ToString();
                        if (Comision != string.Empty)
                        {
                            oDtoTransaccion.Comision = Convert.ToDouble(Comision);
                        }
                        else
                        {
                            oDtoTransaccion.Comision = 0;
                        }

                        string Redondeo = _InfoTransaccionTable.Rows[i][8].ToString();
                        if (Redondeo != string.Empty)
                        {
                            oDtoTransaccion.Redondeo = Convert.ToDouble(Redondeo);
                        }
                        else
                        {
                            oDtoTransaccion.Redondeo = 0;
                        }

                        string TotalPagado = _InfoTransaccionTable.Rows[i][9].ToString();
                        if (TotalPagado != string.Empty)
                        {
                            oDtoTransaccion.TotalPagado = Convert.ToDouble(TotalPagado);
                        }
                        else
                        {
                            oDtoTransaccion.TotalPagado = 0;
                        }

                        
                        oDtoTransaccion.CodigoBarras = _InfoTransaccionTable.Rows[i][10].ToString();
                        oDtoTransaccion.NumeroFactura = _InfoTransaccionTable.Rows[i][11].ToString();
                        oDtoTransaccion.Anulada = Convert.ToBoolean(_InfoTransaccionTable.Rows[i][12]);
                        oDtoTransaccion.Sincronizacion = Convert.ToBoolean(_InfoTransaccionTable.Rows[i][13]);

                        string EstadoTransac = _InfoTransaccionTable.Rows[i][14].ToString();
                        if (EstadoTransac != string.Empty)
                        {
                            oDtoTransaccion.EstadoTransaccion = Convert.ToInt32(EstadoTransac);
                        }
                        else 
                        {
                            oDtoTransaccion.EstadoTransaccion = 0;
                        }
                        oDtoTransaccion.SincronizacionPago = Convert.ToBoolean(_InfoTransaccionTable.Rows[i][15]);

                    }

                    oResultadoOperacion.EntidadDatos = oDtoTransaccion;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin registro en base de datos.";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                oResultadoOperacion.Mensaje = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ObtenerSincronizacionPagoTransaccion()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoTransacciones oDtoTransaccion = new DtoTransacciones();

            ModuloDataSet.P_DatosTransaccionSincronizacionPagoDataTable _InfoTransaccionTable = new ModuloDataSet.P_DatosTransaccionSincronizacionPagoDataTable();
            ModuloDataSetTableAdapters.P_DatosTransaccionSincronizacionPagoTableAdapter _InfoTransaccionAdapter = new ModuloDataSetTableAdapters.P_DatosTransaccionSincronizacionPagoTableAdapter();

            try
            {
                _InfoTransaccionTable.Constraints.Clear();

                if (_InfoTransaccionAdapter.Fill(_InfoTransaccionTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info transaccion OK";

                    for (int i = 0; i < _InfoTransaccionTable.Rows.Count; i++)
                    {
                        oDtoTransaccion.IdTransaccion = Convert.ToInt64(_InfoTransaccionTable.Rows[i][0]);
                        oDtoTransaccion.IdModulo = _InfoTransaccionTable.Rows[i][1].ToString();
                        oDtoTransaccion.IdTipoTransaccion = Convert.ToInt64(_InfoTransaccionTable.Rows[i][2]);
                        oDtoTransaccion.IdSede = Convert.ToInt64(_InfoTransaccionTable.Rows[i][3]);
                        oDtoTransaccion.FechaTransaccion = Convert.ToDateTime(_InfoTransaccionTable.Rows[i][4]);

                        string ValorRecibido = _InfoTransaccionTable.Rows[i][5].ToString();

                        if (ValorRecibido != string.Empty)
                        {
                            oDtoTransaccion.ValorRecibido = Convert.ToDouble(ValorRecibido);
                        }
                        else
                        {
                            oDtoTransaccion.ValorRecibido = 0;
                        }

                        string Iva = _InfoTransaccionTable.Rows[i][6].ToString();
                        if (Iva != string.Empty)
                        {
                            oDtoTransaccion.Iva = Convert.ToDouble(Iva);
                        }
                        else
                        {
                            oDtoTransaccion.Iva = 0;
                        }

                        string Comision = _InfoTransaccionTable.Rows[i][7].ToString();
                        if (Comision != string.Empty)
                        {
                            oDtoTransaccion.Comision = Convert.ToDouble(Comision);
                        }
                        else
                        {
                            oDtoTransaccion.Comision = 0;
                        }

                        string Redondeo = _InfoTransaccionTable.Rows[i][8].ToString();
                        if (Redondeo != string.Empty)
                        {
                            oDtoTransaccion.Redondeo = Convert.ToDouble(Redondeo);
                        }
                        else
                        {
                            oDtoTransaccion.Redondeo = 0;
                        }

                        string TotalPagado = _InfoTransaccionTable.Rows[i][9].ToString();
                        if (TotalPagado != string.Empty)
                        {
                            oDtoTransaccion.TotalPagado = Convert.ToDouble(TotalPagado);
                        }
                        else
                        {
                            oDtoTransaccion.TotalPagado = 0;
                        }


                        oDtoTransaccion.CodigoBarras = _InfoTransaccionTable.Rows[i][10].ToString();
                        oDtoTransaccion.NumeroFactura = _InfoTransaccionTable.Rows[i][11].ToString();
                        oDtoTransaccion.Anulada = Convert.ToBoolean(_InfoTransaccionTable.Rows[i][12]);
                        oDtoTransaccion.Sincronizacion = Convert.ToBoolean(_InfoTransaccionTable.Rows[i][13]);

                        string EstadoTransac = _InfoTransaccionTable.Rows[i][14].ToString();
                        if (EstadoTransac != string.Empty)
                        {
                            oDtoTransaccion.EstadoTransaccion = Convert.ToInt32(EstadoTransac);
                        }
                        else
                        {
                            oDtoTransaccion.EstadoTransaccion = 0;
                        }
                        oDtoTransaccion.SincronizacionPago = Convert.ToBoolean(_InfoTransaccionTable.Rows[i][15]);
                    }

                    oResultadoOperacion.EntidadDatos = oDtoTransaccion;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin registros en base de datos.";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                oResultadoOperacion.Mensaje = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarSincronizacionTransaccion(Transaccion oTransaccion, string Conexion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_RegistrarTransaccionSincronizacionDataTable _RegistroTransaccionesTable = new ModuloDataSet.P_RegistrarTransaccionSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_RegistrarTransaccionSincronizacionTableAdapter _RegistroTransaccionesAdapter = new ModuloDataSetTableAdapters.P_RegistrarTransaccionSincronizacionTableAdapter();

            _RegistroTransaccionesAdapter.Connection = new System.Data.SqlClient.SqlConnection(Conexion);


            try
            {
                _RegistroTransaccionesTable.Constraints.Clear();

                if (_RegistroTransaccionesAdapter.Fill(_RegistroTransaccionesTable, oTransaccion.IdTransaccion, oTransaccion.IdModulo, oTransaccion.IdTipoTransaccion, oTransaccion.IdSede, oTransaccion.FechaTransaccion, oTransaccion.ValorRecibido, oTransaccion.Iva, oTransaccion.Comision, oTransaccion.Redondeo, oTransaccion.TotalPagado, oTransaccion.CodigoBarras, oTransaccion.NumeroFactura, oTransaccion.Anulada, oTransaccion.EstadoTransaccion, oTransaccion.Sincronizacion, oTransaccion.SincronizacionPago) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro transaccion OK";

                    for (int i = 0; i < _RegistroTransaccionesTable.Rows.Count; i++)
                    {
                        resultado = _RegistroTransaccionesTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando prestamo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarSincronizacionPagoTransaccion(Transaccion oTransaccion, string Conexion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_RegistrarTransaccionSincronizacionPagoDataTable _RegistroTransaccionesTable = new ModuloDataSet.P_RegistrarTransaccionSincronizacionPagoDataTable();
            ModuloDataSetTableAdapters.P_RegistrarTransaccionSincronizacionPagoTableAdapter _RegistroTransaccionesAdapter = new ModuloDataSetTableAdapters.P_RegistrarTransaccionSincronizacionPagoTableAdapter();

            _RegistroTransaccionesAdapter.Connection = new System.Data.SqlClient.SqlConnection(Conexion);


            try
            {
                _RegistroTransaccionesTable.Constraints.Clear();

                if (_RegistroTransaccionesAdapter.Fill(_RegistroTransaccionesTable, oTransaccion.IdTransaccion, oTransaccion.IdModulo, oTransaccion.ValorRecibido, oTransaccion.Iva, oTransaccion.Comision, oTransaccion.Redondeo, oTransaccion.TotalPagado, oTransaccion.CodigoBarras, oTransaccion.NumeroFactura, oTransaccion.EstadoTransaccion, oTransaccion.SincronizacionPago) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro transaccion OK";

                    for (int i = 0; i < _RegistroTransaccionesTable.Rows.Count; i++)
                    {
                        resultado = _RegistroTransaccionesTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando prestamo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ActualizaSincronizacionTransaccion(long Transaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_ActualizaSincronizacionTransaccionDataTable _UpdateTransaccionesTable = new ModuloDataSet.P_ActualizaSincronizacionTransaccionDataTable();
            ModuloDataSetTableAdapters.P_ActualizaSincronizacionTransaccionTableAdapter _UpdateTransaccionesAdapter = new ModuloDataSetTableAdapters.P_ActualizaSincronizacionTransaccionTableAdapter();


            try
            {
                _UpdateTransaccionesTable.Constraints.Clear();

                if (_UpdateTransaccionesAdapter.Fill(_UpdateTransaccionesTable, Transaccion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro transaccion OK";

                    for (int i = 0; i < _UpdateTransaccionesTable.Rows.Count; i++)
                    {
                        resultado = _UpdateTransaccionesTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando prestamo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ActualizaSincronizacionPagoTransaccion(long Transaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_ActualizaSincronizacionPagoTransaccionDataTable _UpdateTransaccionesTable = new ModuloDataSet.P_ActualizaSincronizacionPagoTransaccionDataTable();
            ModuloDataSetTableAdapters.P_ActualizaSincronizacionPagoTransaccionTableAdapter _UpdateTransaccionesAdapter = new ModuloDataSetTableAdapters.P_ActualizaSincronizacionPagoTransaccionTableAdapter();


            try
            {
                _UpdateTransaccionesTable.Constraints.Clear();

                if (_UpdateTransaccionesAdapter.Fill(_UpdateTransaccionesTable, Transaccion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro transaccion OK";

                    for (int i = 0; i < _UpdateTransaccionesTable.Rows.Count; i++)
                    {
                        resultado = _UpdateTransaccionesTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando prestamo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        /////////OPERACIONES

        public ResultadoOperacion ObtenerCambioSincronizacion()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoTransacciones oDtoTransaccion = new DtoTransacciones();

            ModuloDataSet.P_DatosCambioSincronizacionDataTable _InfoCambioTable = new ModuloDataSet.P_DatosCambioSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_DatosCambioSincronizacionTableAdapter _InfoCambioAdapter = new ModuloDataSetTableAdapters.P_DatosCambioSincronizacionTableAdapter();


            try
            {
                _InfoCambioTable.Constraints.Clear();

                if (_InfoCambioAdapter.Fill(_InfoCambioTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Cambio OK";

                    for (int i = 0; i < _InfoCambioTable.Rows.Count; i++)
                    {
                        oDtoTransaccion.IdTransaccion = Convert.ToInt64(_InfoCambioTable.Rows[i][0]);
                        oDtoTransaccion.IdModulo = _InfoCambioTable.Rows[i][1].ToString();
                        oDtoTransaccion.IdTipoTransaccion = Convert.ToInt64(_InfoCambioTable.Rows[i][2]);
                        oDtoTransaccion.IdSede = Convert.ToInt64(_InfoCambioTable.Rows[i][3]);
                        oDtoTransaccion.FechaTransaccion = Convert.ToDateTime(_InfoCambioTable.Rows[i][4]);

                        string ValorRecibido = _InfoCambioTable.Rows[i][5].ToString();

                        if (ValorRecibido != string.Empty)
                        {
                            oDtoTransaccion.ValorRecibido = Convert.ToDouble(ValorRecibido);
                        }
                        else
                        {
                            oDtoTransaccion.ValorRecibido = 0;
                        }

                        string Iva = _InfoCambioTable.Rows[i][6].ToString();
                        if (Iva != string.Empty)
                        {
                            oDtoTransaccion.Iva = Convert.ToDouble(Iva);
                        }
                        else
                        {
                            oDtoTransaccion.Iva = 0;
                        }

                        string Comision = _InfoCambioTable.Rows[i][7].ToString();
                        if (Comision != string.Empty)
                        {
                            oDtoTransaccion.Comision = Convert.ToDouble(Comision);
                        }
                        else
                        {
                            oDtoTransaccion.Comision = 0;
                        }

                        string Redondeo = _InfoCambioTable.Rows[i][8].ToString();
                        if (Redondeo != string.Empty)
                        {
                            oDtoTransaccion.Redondeo = Convert.ToDouble(Redondeo);
                        }
                        else
                        {
                            oDtoTransaccion.Redondeo = 0;
                        }

                        oDtoTransaccion.CodigoBarras = _InfoCambioTable.Rows[i][9].ToString();
                        oDtoTransaccion.NumeroFactura = _InfoCambioTable.Rows[i][10].ToString();
                        oDtoTransaccion.Anulada = Convert.ToBoolean(_InfoCambioTable.Rows[i][11]);
                        string EstadoTransac = _InfoCambioTable.Rows[i][12].ToString();
                        if (EstadoTransac != string.Empty)
                        {
                            oDtoTransaccion.EstadoTransaccion = Convert.ToInt32(EstadoTransac);
                        }
                        else
                        {
                            oDtoTransaccion.EstadoTransaccion = 0;
                        }
                        oDtoTransaccion.Sincronizacion = Convert.ToBoolean(_InfoCambioTable.Rows[i][13]);

                    }

                    oResultadoOperacion.EntidadDatos = oDtoTransaccion;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin registro en base de datos.";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                oResultadoOperacion.Mensaje = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ObtenerRecargasSincronizacion()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoTransacciones oDtoTransaccion = new DtoTransacciones();

            ModuloDataSet.P_DatosRecargasSincronizacionDataTable _InfoRecargasTable = new ModuloDataSet.P_DatosRecargasSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_DatosRecargasSincronizacionTableAdapter _InfoRecargasAdapter = new ModuloDataSetTableAdapters.P_DatosRecargasSincronizacionTableAdapter();


            try
            {
                _InfoRecargasTable.Constraints.Clear();

                if (_InfoRecargasAdapter.Fill(_InfoRecargasTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info transaccion OK";

                    for (int i = 0; i < _InfoRecargasTable.Rows.Count; i++)
                    {
                        oDtoTransaccion.IdTransaccion = Convert.ToInt64(_InfoRecargasTable.Rows[i][0]);
                        oDtoTransaccion.IdModulo = _InfoRecargasTable.Rows[i][1].ToString();
                        oDtoTransaccion.IdTipoTransaccion = Convert.ToInt64(_InfoRecargasTable.Rows[i][2]);
                        oDtoTransaccion.IdSede = Convert.ToInt64(_InfoRecargasTable.Rows[i][3]);
                        oDtoTransaccion.FechaTransaccion = Convert.ToDateTime(_InfoRecargasTable.Rows[i][4]);

                        string ValorRecibido = _InfoRecargasTable.Rows[i][5].ToString();

                        if (ValorRecibido != string.Empty)
                        {
                            oDtoTransaccion.ValorRecibido = Convert.ToDouble(ValorRecibido);
                        }
                        else
                        {
                            oDtoTransaccion.ValorRecibido = 0;
                        }

                        oDtoTransaccion.Operador = _InfoRecargasTable.Rows[i][6].ToString();
                        oDtoTransaccion.Linea = _InfoRecargasTable.Rows[i][7].ToString();
                        oDtoTransaccion.Descripcion = _InfoRecargasTable.Rows[i][8].ToString();

                        oDtoTransaccion.NumeroFactura = _InfoRecargasTable.Rows[i][9].ToString();
                        string EstadoTransac = _InfoRecargasTable.Rows[i][10].ToString();
                        if (EstadoTransac != string.Empty)
                        {
                            oDtoTransaccion.EstadoTransaccion = Convert.ToInt32(EstadoTransac);
                        }
                        else
                        {
                            oDtoTransaccion.EstadoTransaccion = 0;
                        }
                        oDtoTransaccion.Sincronizacion = Convert.ToBoolean(_InfoRecargasTable.Rows[i][11]);

                    }

                    oResultadoOperacion.EntidadDatos = oDtoTransaccion;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin registro en base de datos.";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                oResultadoOperacion.Mensaje = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ObtenerDonacionSincronizacion()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoTransacciones oDtoTransaccion = new DtoTransacciones();

            ModuloDataSet.P_DatosDonacionSincronizacionDataTable _InfoDonacionTable = new ModuloDataSet.P_DatosDonacionSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_DatosDonacionSincronizacionTableAdapter _InfoDonacionAdapter = new ModuloDataSetTableAdapters.P_DatosDonacionSincronizacionTableAdapter();


            try
            {
                _InfoDonacionTable.Constraints.Clear();

                if (_InfoDonacionAdapter.Fill(_InfoDonacionTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info transaccion OK";

                    for (int i = 0; i < _InfoDonacionTable.Rows.Count; i++)
                    {
                        oDtoTransaccion.IdTransaccion = Convert.ToInt64(_InfoDonacionTable.Rows[i][0]);
                        oDtoTransaccion.IdModulo = _InfoDonacionTable.Rows[i][1].ToString();
                        oDtoTransaccion.IdTipoTransaccion = Convert.ToInt64(_InfoDonacionTable.Rows[i][2]);
                        oDtoTransaccion.IdSede = Convert.ToInt64(_InfoDonacionTable.Rows[i][3]);
                        oDtoTransaccion.FechaTransaccion = Convert.ToDateTime(_InfoDonacionTable.Rows[i][4]);

                        string ValorRecibido = _InfoDonacionTable.Rows[i][5].ToString();

                        if (ValorRecibido != string.Empty)
                        {
                            oDtoTransaccion.ValorRecibido = Convert.ToDouble(ValorRecibido);
                        }
                        else
                        {
                            oDtoTransaccion.ValorRecibido = 0;
                        }

                        oDtoTransaccion.Fundacion = _InfoDonacionTable.Rows[i][6].ToString();
                        oDtoTransaccion.Sincronizacion = Convert.ToBoolean(_InfoDonacionTable.Rows[i][7]);
                    }

                    oResultadoOperacion.EntidadDatos = oDtoTransaccion;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin registro en base de datos.";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                oResultadoOperacion.Mensaje = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarSincronizacionCambio(Transaccion oTransaccion, string Conexion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_RegistrarCambioSincronizacionDataTable _RegistroCambioTable = new ModuloDataSet.P_RegistrarCambioSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_RegistrarCambioSincronizacionTableAdapter _RegistroCambioAdapter = new ModuloDataSetTableAdapters.P_RegistrarCambioSincronizacionTableAdapter();

            _RegistroCambioAdapter.Connection = new System.Data.SqlClient.SqlConnection(Conexion);


            try
            {
                _RegistroCambioTable.Constraints.Clear();

                if (_RegistroCambioAdapter.Fill(_RegistroCambioTable, oTransaccion.IdTransaccion, oTransaccion.IdModulo, oTransaccion.IdTipoTransaccion, oTransaccion.IdSede, oTransaccion.FechaTransaccion, oTransaccion.ValorRecibido, oTransaccion.Iva, oTransaccion.Comision, oTransaccion.Redondeo, oTransaccion.CodigoBarras, oTransaccion.NumeroFactura, oTransaccion.Anulada, oTransaccion.EstadoTransaccion, oTransaccion.Sincronizacion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro transaccion OK";

                    for (int i = 0; i < _RegistroCambioTable.Rows.Count; i++)
                    {
                        resultado = _RegistroCambioTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando prestamo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarSincronizacionRecargas(Transaccion oTransaccion, string Conexion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_RegistrarRecargasSincronizacionDataTable _RegistroRecargasTable = new ModuloDataSet.P_RegistrarRecargasSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_RegistrarRecargasSincronizacionTableAdapter _RegistroRecargasAdapter = new ModuloDataSetTableAdapters.P_RegistrarRecargasSincronizacionTableAdapter();

            _RegistroRecargasAdapter.Connection = new System.Data.SqlClient.SqlConnection(Conexion);


            try
            {
                _RegistroRecargasTable.Constraints.Clear();

                if (_RegistroRecargasAdapter.Fill(_RegistroRecargasTable, oTransaccion.IdTransaccion, oTransaccion.IdModulo, oTransaccion.IdTipoTransaccion, oTransaccion.IdSede, oTransaccion.FechaTransaccion, oTransaccion.ValorRecibido, oTransaccion.Operador, oTransaccion.Linea, oTransaccion.Descripcion, oTransaccion.NumeroFactura, oTransaccion.EstadoTransaccion, oTransaccion.Sincronizacion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro transaccion OK";

                    for (int i = 0; i < _RegistroRecargasTable.Rows.Count; i++)
                    {
                        resultado = _RegistroRecargasTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando prestamo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarSincronizacionDonacion(Transaccion oTransaccion, string Conexion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_RegistrarDonacionSincronizacionDataTable _RegistroDonacionTable = new ModuloDataSet.P_RegistrarDonacionSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_RegistrarDonacionSincronizacionTableAdapter _RegistroDonacionAdapter = new ModuloDataSetTableAdapters.P_RegistrarDonacionSincronizacionTableAdapter();

            _RegistroDonacionAdapter.Connection = new System.Data.SqlClient.SqlConnection(Conexion);


            try
            {
                _RegistroDonacionTable.Constraints.Clear();

                if (_RegistroDonacionAdapter.Fill(_RegistroDonacionTable, oTransaccion.IdTransaccion, oTransaccion.IdModulo, oTransaccion.IdTipoTransaccion, oTransaccion.IdSede, oTransaccion.FechaTransaccion, oTransaccion.ValorRecibido, oTransaccion.Fundacion, oTransaccion.SincronizacionPago) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro transaccion OK";

                    for (int i = 0; i < _RegistroDonacionTable.Rows.Count; i++)
                    {
                        resultado = _RegistroDonacionTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando prestamo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ActualizaSincronizacionCambio(long Transaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_ActualizaSincronizacionCambioDataTable _UpdateCambioTable = new ModuloDataSet.P_ActualizaSincronizacionCambioDataTable();
            ModuloDataSetTableAdapters.P_ActualizaSincronizacionCambioTableAdapter _UpdateCambioAdapter = new ModuloDataSetTableAdapters.P_ActualizaSincronizacionCambioTableAdapter();


            try
            {
                _UpdateCambioTable.Constraints.Clear();

                if (_UpdateCambioAdapter.Fill(_UpdateCambioTable, Transaccion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro transaccion OK";

                    for (int i = 0; i < _UpdateCambioTable.Rows.Count; i++)
                    {
                        resultado = _UpdateCambioTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando prestamo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ActualizaSincronizacionRecargas(long Transaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_ActualizaSincronizacionRecargasDataTable _UpdateRecargasTable = new ModuloDataSet.P_ActualizaSincronizacionRecargasDataTable();
            ModuloDataSetTableAdapters.P_ActualizaSincronizacionRecargasTableAdapter _UpdateRecargasAdapter = new ModuloDataSetTableAdapters.P_ActualizaSincronizacionRecargasTableAdapter();


            try
            {
                _UpdateRecargasTable.Constraints.Clear();

                if (_UpdateRecargasAdapter.Fill(_UpdateRecargasTable, Transaccion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro transaccion OK";

                    for (int i = 0; i < _UpdateRecargasTable.Rows.Count; i++)
                    {
                        resultado = _UpdateRecargasTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando prestamo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ActualizaSincronizacionDonacion(long Transaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_ActualizaSincronizacionDonacionDataTable _UpdateDonacionTable = new ModuloDataSet.P_ActualizaSincronizacionDonacionDataTable();
            ModuloDataSetTableAdapters.P_ActualizaSincronizacionDonacionTableAdapter _UpdateDonacionAdapter = new ModuloDataSetTableAdapters.P_ActualizaSincronizacionDonacionTableAdapter();


            try
            {
                _UpdateDonacionTable.Constraints.Clear();

                if (_UpdateDonacionAdapter.Fill(_UpdateDonacionTable, Transaccion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro transaccion OK";

                    for (int i = 0; i < _UpdateDonacionTable.Rows.Count; i++)
                    {
                        resultado = _UpdateDonacionTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando prestamo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        /////////ARQUEOS

        public ResultadoOperacion ObtenerArqueoSincronizacion()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoArqueo oDtoArqueos = new DtoArqueo();

            ModuloDataSet.P_DatosArqueoSincronizacionDataTable _InfoArqueoTable = new ModuloDataSet.P_DatosArqueoSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_DatosArqueoSincronizacionTableAdapter _InfoArqueoAdapter = new ModuloDataSetTableAdapters.P_DatosArqueoSincronizacionTableAdapter();


            try
            {
                _InfoArqueoTable.Constraints.Clear();

                if (_InfoArqueoAdapter.Fill(_InfoArqueoTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Arqueo OK";

                    for (int i = 0; i < _InfoArqueoTable.Rows.Count; i++)
                    {
                        oDtoArqueos.IdArqueo = Convert.ToInt64(_InfoArqueoTable.Rows[i][0]);
                        oDtoArqueos.FechaInicio = Convert.ToDateTime(_InfoArqueoTable.Rows[i][1]);

                        string FECHAFIN = _InfoArqueoTable.Rows[i][2].ToString();

                        if (FECHAFIN != string.Empty)
                        {
                            oDtoArqueos.FechaFin = Convert.ToDateTime(FECHAFIN);
                        }
                        else
                        {
                            oDtoArqueos.FechaFin = null;
                        }

                        string VALOR = _InfoArqueoTable.Rows[i][3].ToString();
                        if (VALOR != string.Empty)
                        {
                            oDtoArqueos.Valor = Convert.ToDouble(VALOR);
                        }
                        else
                        {
                            oDtoArqueos.Valor = 0;
                        }
                        oDtoArqueos.IdUsuario = Convert.ToInt64(_InfoArqueoTable.Rows[i][4]);
                        oDtoArqueos.IdModulo = _InfoArqueoTable.Rows[i][5].ToString();
                        oDtoArqueos.IdSede = Convert.ToInt64(_InfoArqueoTable.Rows[i][6]);

                        string CNTTrans = _InfoArqueoTable.Rows[i][7].ToString();

                        if (CNTTrans != string.Empty)
                        {
                            oDtoArqueos.CantTransacciones = Convert.ToInt32(CNTTrans);
                        }
                        else
                        {
                            oDtoArqueos.CantTransacciones = 0;
                        }

                        string Produ = _InfoArqueoTable.Rows[i][8].ToString();

                        if (Produ != string.Empty)
                        {
                            oDtoArqueos.Producido = Convert.ToDouble(Produ);
                        }
                        else
                        {
                            oDtoArqueos.Producido = 0;
                        }
                        oDtoArqueos.Tipo = _InfoArqueoTable.Rows[i][9].ToString();

                        string CONTEO = _InfoArqueoTable.Rows[i][10].ToString();
                        if (CONTEO != string.Empty)
                        {
                            oDtoArqueos.Conteo = Convert.ToDouble(CONTEO);
                        }
                        else
                        {
                            oDtoArqueos.Conteo = 0;
                        }

                        oDtoArqueos.Sincronizacion = Convert.ToBoolean(_InfoArqueoTable.Rows[i][11]);

                    }

                    oResultadoOperacion.EntidadDatos = oDtoArqueos;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin registro en base de datos.";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                oResultadoOperacion.Mensaje = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarSincronizacionArqueo(Arqueo oArqueos, string Conexion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_RegistrarArqueoSincronizacionDataTable _RegistroArqueoTable = new ModuloDataSet.P_RegistrarArqueoSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_RegistrarArqueoSincronizacionTableAdapter _RegistroArqueoAdapter = new ModuloDataSetTableAdapters.P_RegistrarArqueoSincronizacionTableAdapter();

            _RegistroArqueoAdapter.Connection = new System.Data.SqlClient.SqlConnection(Conexion);


            try
            {
                _RegistroArqueoTable.Constraints.Clear();

                if (_RegistroArqueoAdapter.Fill(_RegistroArqueoTable, oArqueos.IdArqueo, oArqueos.FechaInicio, oArqueos.FechaFin, oArqueos.Valor, oArqueos.IdUsuario, oArqueos.IdModulo, oArqueos.IdSede, oArqueos.CantTransacciones, oArqueos.Producido, oArqueos.Tipo, oArqueos.Conteo, oArqueos.Sincronizacion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro Arqueo OK";

                    for (int i = 0; i < _RegistroArqueoTable.Rows.Count; i++)
                    {
                        resultado = _RegistroArqueoTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando Arqueo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ActualizaSincronizacionArqueo(long Arqueo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_ActualizaSincronizacionArqueoDataTable _UpdateArqueoTable = new ModuloDataSet.P_ActualizaSincronizacionArqueoDataTable();
            ModuloDataSetTableAdapters.P_ActualizaSincronizacionArqueoTableAdapter _UpdateArqueoAdapter = new ModuloDataSetTableAdapters.P_ActualizaSincronizacionArqueoTableAdapter();


            try
            {
                _UpdateArqueoTable.Constraints.Clear();

                if (_UpdateArqueoAdapter.Fill(_UpdateArqueoTable, Arqueo) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro Arqueo OK";

                    for (int i = 0; i < _UpdateArqueoTable.Rows.Count; i++)
                    {
                        resultado = _UpdateArqueoTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando Arqueo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }


        /////////CARGAS

        public ResultadoOperacion ObtenerCargaSincronizacion()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoCarga oDtoCarga = new DtoCarga();

            ModuloDataSet.P_DatosCargueSincronizacionDataTable _InfoCargaTable = new ModuloDataSet.P_DatosCargueSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_DatosCargueSincronizacionTableAdapter _InfoCargaAdapter = new ModuloDataSetTableAdapters.P_DatosCargueSincronizacionTableAdapter();


            try
            {
                _InfoCargaTable.Constraints.Clear();

                if (_InfoCargaAdapter.Fill(_InfoCargaTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Carga OK";

                    for (int i = 0; i < _InfoCargaTable.Rows.Count; i++)
                    {
                        oDtoCarga.IdCarga = Convert.ToInt64(_InfoCargaTable.Rows[i][0]);
                        oDtoCarga.FechaInicio = Convert.ToDateTime(_InfoCargaTable.Rows[i][1]);

                        string FECHAFIN = _InfoCargaTable.Rows[i][2].ToString();

                        if (FECHAFIN != string.Empty)
                        {
                            oDtoCarga.FechaFin = Convert.ToDateTime(FECHAFIN);
                        }
                        else
                        {
                            oDtoCarga.FechaFin = null;
                        }

                        string VALOR = _InfoCargaTable.Rows[i][3].ToString();
                        if (VALOR != string.Empty)
                        {
                            oDtoCarga.Valor = Convert.ToDouble(VALOR);
                        }
                        else
                        {
                            oDtoCarga.Valor = 0;
                        }
                        oDtoCarga.IdUsuario = Convert.ToInt64(_InfoCargaTable.Rows[i][4]);
                        oDtoCarga.IdModulo = _InfoCargaTable.Rows[i][5].ToString();
                        oDtoCarga.IdSede = Convert.ToInt64(_InfoCargaTable.Rows[i][6]);

                        oDtoCarga.Sincronizacion = Convert.ToBoolean(_InfoCargaTable.Rows[i][7]);

                    }

                    oResultadoOperacion.EntidadDatos = oDtoCarga;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin registro en base de datos.";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                oResultadoOperacion.Mensaje = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarSincronizacionCarga(Carga oCargas, string Conexion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_RegistrarCargaSincronizacionDataTable _RegistroArqueoTable = new ModuloDataSet.P_RegistrarCargaSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_RegistrarCargaSincronizacionTableAdapter _RegistroArqueoAdapter = new ModuloDataSetTableAdapters.P_RegistrarCargaSincronizacionTableAdapter();

            _RegistroArqueoAdapter.Connection = new System.Data.SqlClient.SqlConnection(Conexion);


            try
            {
                _RegistroArqueoTable.Constraints.Clear();

                if (_RegistroArqueoAdapter.Fill(_RegistroArqueoTable, oCargas.IdCarga, oCargas.FechaInicio, oCargas.FechaFin, oCargas.Valor, oCargas.IdUsuario, oCargas.IdModulo, oCargas.IdSede, oCargas.Sincronizacion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro Carga OK";

                    for (int i = 0; i < _RegistroArqueoTable.Rows.Count; i++)
                    {
                        resultado = _RegistroArqueoTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando Carga";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ActualizaSincronizacionCarga(long Carga)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_ActualizaSincronizacionCargaDataTable _UpdateCargaTable = new ModuloDataSet.P_ActualizaSincronizacionCargaDataTable();
            ModuloDataSetTableAdapters.P_ActualizaSincronizacionCargaTableAdapter _UpdateCargaAdapter = new ModuloDataSetTableAdapters.P_ActualizaSincronizacionCargaTableAdapter();


            try
            {
                _UpdateCargaTable.Constraints.Clear();

                if (_UpdateCargaAdapter.Fill(_UpdateCargaTable, Carga) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro Arqueo OK";

                    for (int i = 0; i < _UpdateCargaTable.Rows.Count; i++)
                    {
                        resultado = _UpdateCargaTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando Arqueo";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }


        /////////PARTES

        public ResultadoOperacion ObtenerPartesSincronizacion()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoParteModulo oDtoPartes = new DtoParteModulo();

            ModuloDataSet.P_DatosPartesSincronizacionDataTable _InfoParteTable = new ModuloDataSet.P_DatosPartesSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_DatosPartesSincronizacionTableAdapter _InfoParteAdapter = new ModuloDataSetTableAdapters.P_DatosPartesSincronizacionTableAdapter();


            try
            {
                _InfoParteTable.Constraints.Clear();

                if (_InfoParteAdapter.Fill(_InfoParteTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info partes OK";

                    for (int i = 0; i < _InfoParteTable.Rows.Count; i++)
                    {
                        oDtoPartes.IdParte = Convert.ToInt64(_InfoParteTable.Rows[i][0]);
                        oDtoPartes.IdModulo = _InfoParteTable.Rows[i][1].ToString();
                        oDtoPartes.IdSede = Convert.ToInt64(_InfoParteTable.Rows[i][2]);
                        oDtoPartes.TipoParte = _InfoParteTable.Rows[i][3].ToString();
                        oDtoPartes.Nombre = _InfoParteTable.Rows[i][4].ToString();
                        oDtoPartes.Denominacion = _InfoParteTable.Rows[i][5].ToString();
                        oDtoPartes.CantidadMin = _InfoParteTable.Rows[i][6].ToString();
                        oDtoPartes.CantidadAlarma = _InfoParteTable.Rows[i][10].ToString();
                        oDtoPartes.Prioridad = Convert.ToBoolean(_InfoParteTable.Rows[i][13]);
                        oDtoPartes.DineroActual = _InfoParteTable.Rows[i][11].ToString();
                        oDtoPartes.CantidadActual = _InfoParteTable.Rows[i][12].ToString();
                        oDtoPartes.NumParte = _InfoParteTable.Rows[i][8].ToString();

                    }

                    oResultadoOperacion.EntidadDatos = oDtoPartes;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin partes en base de datos.";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                oResultadoOperacion.Mensaje = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ActualizaSincronizacionParte(string IdModulo, long IdSede, string NombreParte,int Denominacion, double DineroActual, int CantidadActual, string Conexion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_ActualizaSincronizacionPartesDataTable _UpdatePartesTable = new ModuloDataSet.P_ActualizaSincronizacionPartesDataTable();
            ModuloDataSetTableAdapters.P_ActualizaSincronizacionPartesTableAdapter _UpdatePartesAdapter = new ModuloDataSetTableAdapters.P_ActualizaSincronizacionPartesTableAdapter();

            _UpdatePartesAdapter.Connection = new System.Data.SqlClient.SqlConnection(Conexion);

            try
            {
                _UpdatePartesTable.Constraints.Clear();

                if (_UpdatePartesAdapter.Fill(_UpdatePartesTable,IdModulo,IdSede,NombreParte,DineroActual,CantidadActual,Denominacion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro partes OK";

                    for (int i = 0; i < _UpdatePartesTable.Rows.Count; i++)
                    {
                        resultado = _UpdatePartesTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando partes";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ActualizaSincronizacionParteLocal(string IdModulo, long IdSede, string NombreParte, int Denominacion, double DineroActual, int CantidadActual)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_ActualizaSincronizacionPartesLocalDataTable _UpdatePartesTable = new ModuloDataSet.P_ActualizaSincronizacionPartesLocalDataTable();
            ModuloDataSetTableAdapters.P_ActualizaSincronizacionPartesLocalTableAdapter _UpdatePartesAdapter = new ModuloDataSetTableAdapters.P_ActualizaSincronizacionPartesLocalTableAdapter();


            try
            {
                _UpdatePartesTable.Constraints.Clear();

                if (_UpdatePartesAdapter.Fill(_UpdatePartesTable, IdModulo, IdSede, NombreParte, DineroActual, CantidadActual,Denominacion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro partes OK";

                    for (int i = 0; i < _UpdatePartesTable.Rows.Count; i++)
                    {
                        resultado = _UpdatePartesTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando partes";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        /////////MOVIMIENTOS

        public ResultadoOperacion ObtenerMovimientoSincronizacion()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoMovimiento oDtoMovimientos = new DtoMovimiento();

            ModuloDataSet.P_DatosMovimientoSincronizacionDataTable _InfoMovimientoTable = new ModuloDataSet.P_DatosMovimientoSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_DatosMovimientoSincronizacionTableAdapter _InfoMovimientoAdapter = new ModuloDataSetTableAdapters.P_DatosMovimientoSincronizacionTableAdapter();


            try
            {
                _InfoMovimientoTable.Constraints.Clear();

                if (_InfoMovimientoAdapter.Fill(_InfoMovimientoTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info movimiento OK";

                    for (int i = 0; i < _InfoMovimientoTable.Rows.Count; i++)
                    {
                        oDtoMovimientos.IdMovimiento = Convert.ToInt64(_InfoMovimientoTable.Rows[i][0]);

                        string IdTransaccion = _InfoMovimientoTable.Rows[i][1].ToString();
                        if (IdTransaccion != string.Empty)
                        {
                            oDtoMovimientos.IdTransaccion = Convert.ToInt64(IdTransaccion);
                        }
                        else
                        {
                            oDtoMovimientos.IdTransaccion = null;
                        }

                        oDtoMovimientos.IdSede = Convert.ToInt64(_InfoMovimientoTable.Rows[i][2]);


                        string IdCarga = _InfoMovimientoTable.Rows[i][3].ToString();
                        if (IdCarga != string.Empty)
                        {
                            oDtoMovimientos.IdCarga = Convert.ToInt64(IdCarga);
                        }
                        else
                        {
                            oDtoMovimientos.IdCarga = null;
                        }


                        string IdArqueo = _InfoMovimientoTable.Rows[i][4].ToString();
                        if (IdArqueo != string.Empty)
                        {
                            oDtoMovimientos.IdArqueo = Convert.ToInt64(IdArqueo);
                        }
                        else
                        {
                            oDtoMovimientos.IdArqueo = null;
                        }


                        oDtoMovimientos.IdCajero = _InfoMovimientoTable.Rows[i][5].ToString();
                        oDtoMovimientos.Parte = _InfoMovimientoTable.Rows[i][6].ToString();
                        oDtoMovimientos.Accion = _InfoMovimientoTable.Rows[i][7].ToString();
                        oDtoMovimientos.Denominacion = Convert.ToInt32(_InfoMovimientoTable.Rows[i][8]);
                        oDtoMovimientos.Cantidad = Convert.ToInt32(_InfoMovimientoTable.Rows[i][9]);
                        oDtoMovimientos.Valor = Convert.ToInt32(_InfoMovimientoTable.Rows[i][10]);
                        oDtoMovimientos.FechaMovimiento = Convert.ToDateTime(_InfoMovimientoTable.Rows[i][11]);
                        oDtoMovimientos.Sincronizacion = Convert.ToBoolean(_InfoMovimientoTable.Rows[i][12]);

                    }

                    oResultadoOperacion.EntidadDatos = oDtoMovimientos;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin registro en base de datos.";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                oResultadoOperacion.Mensaje = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarSincronizacionMovimiento(Movimiento oMovimiento, string Conexion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_RegistrarMovimientosSincronizacionDataTable _RegistroMovimientoTable = new ModuloDataSet.P_RegistrarMovimientosSincronizacionDataTable();
            ModuloDataSetTableAdapters.P_RegistrarMovimientosSincronizacionTableAdapter _RegistroMovimientoAdapter = new ModuloDataSetTableAdapters.P_RegistrarMovimientosSincronizacionTableAdapter();

            _RegistroMovimientoAdapter.Connection = new System.Data.SqlClient.SqlConnection(Conexion);


            try
            {
                _RegistroMovimientoTable.Constraints.Clear();

                if (_RegistroMovimientoAdapter.Fill(_RegistroMovimientoTable, oMovimiento.ID_Movimiento, oMovimiento.IdTransaccion, oMovimiento.IdSede, oMovimiento.IdCarga, oMovimiento.IdArqueo, oMovimiento.ID_Modulo, oMovimiento.Parte, oMovimiento.Accion, oMovimiento.Denominacion, oMovimiento.Cantidad, oMovimiento.Valor, oMovimiento.FechaMovimiento, oMovimiento.Sincronizacion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro Movimiento OK";

                    for (int i = 0; i < _RegistroMovimientoTable.Rows.Count; i++)
                    {
                        resultado = _RegistroMovimientoTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando movimiento";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ActualizaSincronizacionMovimiento(long Movimiento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string resultado = string.Empty;



            ModuloDataSet.P_ActualizaSincronizacionMovimientoDataTable _UpdateMovimientoTable = new ModuloDataSet.P_ActualizaSincronizacionMovimientoDataTable();
            ModuloDataSetTableAdapters.P_ActualizaSincronizacionMovimientoTableAdapter _UpdateMovimientoAdapter = new ModuloDataSetTableAdapters.P_ActualizaSincronizacionMovimientoTableAdapter();


            try
            {
                _UpdateMovimientoTable.Constraints.Clear();

                if (_UpdateMovimientoAdapter.Fill(_UpdateMovimientoTable, Movimiento) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro movimiento OK";

                    for (int i = 0; i < _UpdateMovimientoTable.Rows.Count; i++)
                    {
                        resultado = _UpdateMovimientoTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {

                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error registrando movimiento";
                }
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }
    }
}
