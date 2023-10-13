using Ds.BusinessService.DataTransferObject;
using Ds.BusinessService.Entities;
using Ds.BusinessService.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.DataService
{
    public partial class DataService : IDataService
    {
        
        public ResultadoOperacion ObtenerInformacionModulo(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoModulo oDtoModulo = new DtoModulo();


            ModuloDataSet.P_ObtenerInfoModuloDataTable _InfoModuloTable = new ModuloDataSet.P_ObtenerInfoModuloDataTable();
            ModuloDataSetTableAdapters.P_ObtenerInfoModuloTableAdapter _InfoModuloAdapter = new ModuloDataSetTableAdapters.P_ObtenerInfoModuloTableAdapter();

            try
            {
                _InfoModuloTable.Constraints.Clear();

                if (_InfoModuloAdapter.Fill(_InfoModuloTable, oModulo.ID_Modulo) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Modulo OK";

                    for (int i = 0; i < _InfoModuloTable.Rows.Count; i++)
                    {

                        oDtoModulo.IdModulo = _InfoModuloTable.Rows[i][0].ToString();
                        oDtoModulo.IdSede = Convert.ToInt64(_InfoModuloTable.Rows[i][1]);
                        oDtoModulo.Nombre = _InfoModuloTable.Rows[i][2].ToString();
                        oDtoModulo.Extension = _InfoModuloTable.Rows[i][8].ToString();
                        oDtoModulo.Estado = Convert.ToBoolean(_InfoModuloTable.Rows[i][9]);
                    }

                    oResultadoOperacion.EntidadDatos = oDtoModulo;
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
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerInformacionFactura(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoFactura oDtoFactura = new DtoFactura();


            ModuloDataSet.P_ObtenerInfoFacturaDataTable _InfoModuloTable = new ModuloDataSet.P_ObtenerInfoFacturaDataTable();
            ModuloDataSetTableAdapters.P_ObtenerInfoFacturaTableAdapter _InfoModuloAdapter = new ModuloDataSetTableAdapters.P_ObtenerInfoFacturaTableAdapter();

            try
            {
                _InfoModuloTable.Constraints.Clear();

                if (_InfoModuloAdapter.Fill(_InfoModuloTable, oModulo.ID_Modulo) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Modulo OK";

                    for (int i = 0; i < _InfoModuloTable.Rows.Count; i++)
                    {

                        oDtoFactura.IdFacturacion = Convert.ToInt64(_InfoModuloTable.Rows[i][0]);
                        oDtoFactura.IdModulo = _InfoModuloTable.Rows[i][1].ToString();
                        
                        oDtoFactura.Prefijo = _InfoModuloTable.Rows[i][3].ToString();
                        oDtoFactura.FacturaInicial = Convert.ToInt32(_InfoModuloTable.Rows[i][4]);
                        oDtoFactura.FacturaFinal = Convert.ToInt32(_InfoModuloTable.Rows[i][5]);
                        oDtoFactura.FacturaActual = Convert.ToInt32(_InfoModuloTable.Rows[i][6]);
                        oDtoFactura.NumeroResolucion = Convert.ToInt64(_InfoModuloTable.Rows[i][7]);
                        oDtoFactura.FechaResolucion = _InfoModuloTable.Rows[i][8].ToString();
                        oDtoFactura.Estado = Convert.ToBoolean(_InfoModuloTable.Rows[i][9]);
                        oDtoFactura.FechaFinResolucion = _InfoModuloTable.Rows[i][10].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = oDtoFactura;
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
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerInformacionPartesModulo(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoParteModulo> oListaDtoParteModulo = new List<DtoParteModulo>();

            ModuloDataSet.P_ObtenerPartesDataTable _InfoParteTable = new ModuloDataSet.P_ObtenerPartesDataTable();
            ModuloDataSetTableAdapters.P_ObtenerPartesTableAdapter _InfoParteAdapter = new ModuloDataSetTableAdapters.P_ObtenerPartesTableAdapter();

            try
            {
                _InfoParteTable.Constraints.Clear();

                if (_InfoParteAdapter.Fill(_InfoParteTable, oModulo.ID_Modulo) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _InfoParteTable.Rows.Count; i++)
                    {
                        DtoParteModulo ParteModulo = new DtoParteModulo();
                        
                        ParteModulo.TipoParte = _InfoParteTable.Rows[i][3].ToString();
                        ParteModulo.Nombre = _InfoParteTable.Rows[i][4].ToString();
                        ParteModulo.Denominacion = _InfoParteTable.Rows[i][5].ToString();
                        ParteModulo.CantidadMin = _InfoParteTable.Rows[i][6].ToString();
                        ParteModulo.CantidadAlarma = _InfoParteTable.Rows[i][10].ToString();
                        ParteModulo.Prioridad = Convert.ToBoolean(_InfoParteTable.Rows[i][13]);
                        ParteModulo.DineroActual = _InfoParteTable.Rows[i][11].ToString();
                        ParteModulo.CantidadActual = _InfoParteTable.Rows[i][12].ToString();
                        ParteModulo.NumParte = _InfoParteTable.Rows[i][8].ToString();

                        oListaDtoParteModulo.Add(ParteModulo);
                    }

                    oResultadoOperacion.ListaEntidadDatos = oListaDtoParteModulo;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin partes registradas en base de datos.";
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

        public ResultadoOperacion ObtenerInformacionPartesF56(Modulo oModulo, int Tipo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoParteModuloF56> oListaDtoParteModulo = new List<DtoParteModuloF56>();

            ModuloDataSet.P_ListarPartesF56DataTable _InfoParteTable = new ModuloDataSet.P_ListarPartesF56DataTable();
            ModuloDataSetTableAdapters.P_ListarPartesF56TableAdapter _InfoParteAdapter = new ModuloDataSetTableAdapters.P_ListarPartesF56TableAdapter();

            try
            {
                _InfoParteTable.Constraints.Clear();

                if (_InfoParteAdapter.Fill(_InfoParteTable, oModulo.ID_Modulo, Tipo) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info PartesF56 OK";

                    for (int i = 0; i < _InfoParteTable.Rows.Count; i++)
                    {
                        DtoParteModuloF56 parte = new DtoParteModuloF56();
                        parte.Num_Parte = Convert.ToInt32(_InfoParteTable.Rows[i][0].ToString());
                        parte.Denominacion = Convert.ToInt32(_InfoParteTable.Rows[i][1].ToString());
                        parte.Qty_Actual = Convert.ToInt32(_InfoParteTable.Rows[i][2].ToString());

                        oListaDtoParteModulo.Add(parte);
                    }

                    oResultadoOperacion.ListaEntidadDatos = oListaDtoParteModulo;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin partes registradas en base de datos.";
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

        public ResultadoOperacion ObtenerParametrosModulo(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoParametro> oListaDtoParametros = new List<DtoParametro>();

            ModuloDataSet.P_ObtenerParametrosDataTable _InfoParteTable = new ModuloDataSet.P_ObtenerParametrosDataTable();
            ModuloDataSetTableAdapters.P_ObtenerParametrosTableAdapter _InfoParteAdapter = new ModuloDataSetTableAdapters.P_ObtenerParametrosTableAdapter();

            try
            {
                _InfoParteTable.Constraints.Clear();

                int idestacionamiento = 9;

                if (_InfoParteAdapter.Fill(_InfoParteTable, Convert.ToInt64(idestacionamiento)) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _InfoParteTable.Rows.Count; i++)
                    {
                        DtoParametro parametro = new DtoParametro();
                        parametro.Codigo = _InfoParteTable.Rows[i][1].ToString();
                        parametro.Valor = _InfoParteTable.Rows[i][2].ToString();
                        parametro.Estado = Convert.ToBoolean(_InfoParteTable.Rows[i][4]);
                        parametro.Descripcion = _InfoParteTable.Rows[i][3].ToString();
                        oListaDtoParametros.Add(parametro);
                    }

                    oResultadoOperacion.ListaEntidadDatos = oListaDtoParametros;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin partes registradas en base de datos.";
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

        public ResultadoOperacion ValidarSaldosMinimos(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoSaldos> olstDtoSaldos = new List<DtoSaldos>();

            ModuloDataSet.P_ValidarSaldosDataTable _SaldosMinimosTable = new ModuloDataSet.P_ValidarSaldosDataTable();
            ModuloDataSetTableAdapters.P_ValidarSaldosTableAdapter _SaldosMinimosAdapter = new ModuloDataSetTableAdapters.P_ValidarSaldosTableAdapter();

            try
            {
                _SaldosMinimosTable.Constraints.Clear();

                if (_SaldosMinimosAdapter.Fill(_SaldosMinimosTable, oModulo.ID_Modulo) > 0)
                {
                    for (int i = 0; i < _SaldosMinimosTable.Rows.Count; i++)
                    {
                        DtoSaldos oDtoSaldos = new DtoSaldos();

                        oDtoSaldos.NombreParte = _SaldosMinimosTable.Rows[i][0].ToString();
                        oDtoSaldos.CantMin = Convert.ToInt32(_SaldosMinimosTable.Rows[i][1]);
                        oDtoSaldos.CantActual = Convert.ToInt32(_SaldosMinimosTable.Rows[i][2]);

                        olstDtoSaldos.Add(oDtoSaldos);
                    }

                    for (int i = 0; i < olstDtoSaldos.Count; i++)
                    {
                        if (olstDtoSaldos[i].CantActual <= olstDtoSaldos[i].CantMin)
                        {
                            oResultadoOperacion.Mensaje = "Saldos Mínimos Insuficientes en la parte " + olstDtoSaldos[i].NombreParte;
                            oResultadoOperacion.EntidadDatos = TipoValidarSaldosMinimos.True;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            break;
                        }
                    }

                    if (oResultadoOperacion.Mensaje == string.Empty)
                    {
                        oResultadoOperacion.EntidadDatos = TipoValidarSaldosMinimos.False;
                        oResultadoOperacion.Mensaje = "Saldos Mínimos OK";
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    }

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

        // ESTE NO SE USA AHORA
        public ResultadoOperacion RegistrarOperacion(Operacion oOperacion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //ModuloDataSet.P_RegistrarOperacionDataTable _OperacionTable = new ModuloDataSet.P_RegistrarOperacionDataTable();
            //ModuloDataSetTableAdapters.P_RegistrarOperacionTableAdapter _OperacionAdapter = new ModuloDataSetTableAdapters.P_RegistrarOperacionTableAdapter();

            DtoOperacion oDtoOperacion = new DtoOperacion();

            try
            {
                //_OperacionTable.Constraints.Clear();

                //int id_user = 0;

                //if (oOperacion.ID_Usuario != string.Empty)
                //{
                //    id_user = Convert.ToInt32(oOperacion.ID_Usuario);
                //}

                //if (_OperacionAdapter.Fill(_OperacionTable, id_user, oOperacion.ID_Modulo, oOperacion.ID_Transaccion, (int)oOperacion.TipoOperacion) > 0)
                //{
                //    for (int i = 0; i < _OperacionTable.Rows.Count; i++)
                //    {
                //        string status = _OperacionTable.Rows[i][(int)Operacion_DB.Status].ToString();
                //        long code = Convert.ToInt32(_OperacionTable.Rows[i][(int)Operacion_DB.Code].ToString());
                //        //string fakeCode = _OperacionTable.Rows[i][2].ToString();

                //        if (status == Operacion_Result.OK.ToString())
                //        {
                //            oResultadoOperacion.oEstado = TipoRespuesta.Exito;

                //            oDtoOperacion.ID_Operacion = code;
                //            //oDtoOperacion.ID_Fake_Operacion = fakeCode;

                //            oResultadoOperacion.EntidadDatos = oDtoOperacion;

                //            if (oOperacion.TipoOperacion == TipoOperacion.Carga)
                //                oResultadoOperacion.Mensaje = "Registro Carga OK";
                //            else if (oOperacion.TipoOperacion == TipoOperacion.ArqueoParcial)
                //                oResultadoOperacion.Mensaje = "Registro Arqueo Parcial OK";
                //            else if (oOperacion.TipoOperacion == TipoOperacion.ArqueoTotal)
                //                oResultadoOperacion.Mensaje = "Registro Arqueo Total OK";
                //            else if (oOperacion.TipoOperacion == TipoOperacion.Pago)
                //                oResultadoOperacion.Mensaje = "Registro Pago OK";

                //        }
                //        else if (status == Operacion_Result.ERROR.ToString())
                //        {
                //            oResultadoOperacion.oEstado = TipoRespuesta.Error;
                //            oResultadoOperacion.EntidadDatos = code;

                //            if (oOperacion.TipoOperacion == TipoOperacion.Carga)
                //                oResultadoOperacion.Mensaje = "Registro Carga Error";
                //            else if (oOperacion.TipoOperacion == TipoOperacion.ArqueoParcial)
                //                oResultadoOperacion.Mensaje = "Registro Arqueo Parcial Error";
                //            else if (oOperacion.TipoOperacion == TipoOperacion.ArqueoTotal)
                //                oResultadoOperacion.Mensaje = "Registro Arqueo Total Error";
                //            else if (oOperacion.TipoOperacion == TipoOperacion.Pago)
                //                oResultadoOperacion.Mensaje = "Registro Pago Error";
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarTransaccion(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarTransaccionDataTable _OperacionTable = new ModuloDataSet.P_RegistrarTransaccionDataTable();
            ModuloDataSetTableAdapters.P_RegistrarTransaccionTableAdapter _OperacionAdapter = new ModuloDataSetTableAdapters.P_RegistrarTransaccionTableAdapter();
            
            DtoOperacion oDtoOperacion = new DtoOperacion();

            try
            {
                _OperacionTable.Constraints.Clear();

                if (_OperacionAdapter.Fill(_OperacionTable, oTransaccion.IdTransaccion, oTransaccion.IdModulo, oTransaccion.IdSede) > 0)
                {
                    for (int i = 0; i < _OperacionTable.Rows.Count; i++)
                    {
                        string status = _OperacionTable.Rows[i][1].ToString();
                        //long code = Convert.ToInt64(_OperacionTable.Rows[i][1]);

                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Registro transaccion OK";
                        oResultadoOperacion.EntidadDatos = status;
                    }
                }
                else 
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Registro transaccion ERROR";
                    oResultadoOperacion.EntidadDatos = string.Empty;
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

        public ResultadoOperacion RegistrarArqueo(Arqueo oArqueo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarArqueoDataTable _OperacionTable = new ModuloDataSet.P_RegistrarArqueoDataTable();
            ModuloDataSetTableAdapters.P_RegistrarArqueoTableAdapter _OperacionAdapter = new ModuloDataSetTableAdapters.P_RegistrarArqueoTableAdapter();

            DtoOperacion oDtoOperacion = new DtoOperacion();

            try
            {
                _OperacionTable.Constraints.Clear();

                if (_OperacionAdapter.Fill(_OperacionTable, oArqueo.IdUsuario, oArqueo.IdModulo, oArqueo.Tipo,oArqueo.IdSede) > 0)
                {
                    for (int i = 0; i < _OperacionTable.Rows.Count; i++)
                    {
                        long status = Convert.ToInt64(_OperacionTable.Rows[i][0]);
                        //long code = Convert.ToInt64(_OperacionTable.Rows[i][1]);

                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Registro arqueo OK";
                        oResultadoOperacion.EntidadDatos = status;
                    }
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

        public ResultadoOperacion RegistrarCarga(Carga oCarga)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarCargasDataTable _OperacionTable = new ModuloDataSet.P_RegistrarCargasDataTable();
            ModuloDataSetTableAdapters.P_RegistrarCargasTableAdapter _OperacionAdapter = new ModuloDataSetTableAdapters.P_RegistrarCargasTableAdapter();

            DtoOperacion oDtoOperacion = new DtoOperacion();

            try
            {
                _OperacionTable.Constraints.Clear();

                if (_OperacionAdapter.Fill(_OperacionTable, oCarga.IdUsuario, oCarga.IdModulo, oCarga.IdSede) > 0)
                {
                    for (int i = 0; i < _OperacionTable.Rows.Count; i++)
                    {
                        long status = Convert.ToInt64(_OperacionTable.Rows[i][0]);
                        //long code = Convert.ToInt64(_OperacionTable.Rows[i][1]);

                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Registro Carga OK";
                        oResultadoOperacion.EntidadDatos = status;
                    }
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

        public ResultadoOperacion RegistrarMovimiento(Movimiento oMovimiento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarMovimientoDataTable _MovimientoTable = new ModuloDataSet.P_RegistrarMovimientoDataTable();
            ModuloDataSetTableAdapters.P_RegistrarMovimientoTableAdapter _MovimientoAdapter = new ModuloDataSetTableAdapters.P_RegistrarMovimientoTableAdapter();

            try
            {
                _MovimientoTable.Constraints.Clear();

                if (_MovimientoAdapter.Fill(_MovimientoTable, oMovimiento.IdTransaccion, oMovimiento.IdSede, oMovimiento.IdCarga, oMovimiento.IdArqueo, oMovimiento.ID_Modulo, oMovimiento.Parte,0, oMovimiento.TipoAccionMovimiento.ToString(), oMovimiento.Denominacion,oMovimiento.Cantidad) > 0)
                {
                    for (int i = 0; i < _MovimientoTable.Rows.Count; i++)
                    {
                        string status = _MovimientoTable.Rows[i][(int)Operacion_DB.Status].ToString();
                        long code = Convert.ToInt32(_MovimientoTable.Rows[i][(int)Operacion_DB.Code].ToString());

                        if (status == Operacion_Result.OK.ToString())
                        {
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "Registro Movimiento OK";
                            oResultadoOperacion.EntidadDatos = code;
                        }
                        else if (status == Operacion_Result.ERROR.ToString())
                        {
                            oResultadoOperacion.oEstado = TipoRespuesta.Error;
                            oResultadoOperacion.Mensaje = "Registro Movimiento Error";
                            oResultadoOperacion.EntidadDatos = code;
                        }
                    }
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

        public ResultadoOperacion ObtenerSaldoPartes(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //ModuloDataSet.P_ListarSaldosParteDataTable _SaldoParteTable = new ModuloDataSet.P_ListarSaldosParteDataTable();
            //ModuloDataSetTableAdapters.P_ListarSaldosParteTableAdapter _SaldoParteAdapter = new ModuloDataSetTableAdapters.P_ListarSaldosParteTableAdapter();

            try
            {
                //_SaldoParteTable.Constraints.Clear();

                //if (_SaldoParteAdapter.Fill(_SaldoParteTable, oModulo.ID_Modulo) > 0)
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                //    oResultadoOperacion.Mensaje = "Info Saldos OK";

                //    DtoSaldos oDtoSaldos = new DtoSaldos();

                //    for (int i = 0; i < _SaldoParteTable.Rows.Count; i++)
                //    {
                //        DtoParteModulo parte = new DtoParteModulo();
                //        parte.TipoParte = _SaldoParteTable.Rows[i][(int)SaldoParte_DB.Tipo_Parte].ToString();
                //        parte.CantidadActual = _SaldoParteTable.Rows[i][(int)SaldoParte_DB.Cantidad].ToString();
                //        //oDtoSaldos.DtoPartes.Add(parte);
                //    }

                //    oResultadoOperacion.EntidadDatos = oDtoSaldos;
                //}
                //else
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                //    oResultadoOperacion.Mensaje = "Módulo sin saldos ni partes registradas en base de datos.";
                //}
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ConfirmarOperacion(Operacion oOperacion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoOperacion oDtoOperacion = new DtoOperacion();

            oDtoOperacion.TipoOperacion = oOperacion.TipoOperacion;

            if ((oOperacion.TipoOperacion == TipoOperacion.ArqueoParcial) || (oOperacion.TipoOperacion == TipoOperacion.ArqueoTotal))
            {
                ModuloDataSet.P_ConfirmarArqueoDataTable _ArqueoTable = new ModuloDataSet.P_ConfirmarArqueoDataTable();
                ModuloDataSetTableAdapters.P_ConfirmarArqueoTableAdapter _ArqueoAdapter = new ModuloDataSetTableAdapters.P_ConfirmarArqueoTableAdapter();

                try
                {
                    _ArqueoTable.Constraints.Clear();

                    if (_ArqueoAdapter.Fill(_ArqueoTable, oOperacion.ID_Operacion) > 0)
                    {
                        for (int i = 0; i < _ArqueoTable.Rows.Count; i++)
                        {
                            string status = _ArqueoTable.Rows[i][(int)InfoArqueo_DB.Status].ToString();

                            oDtoOperacion.DtoArqueo.IdArqueo = Convert.ToInt32(_ArqueoTable.Rows[i][(int)InfoArqueo_DB.IdArqueo].ToString());
                            oDtoOperacion.DtoArqueo.Producido = Convert.ToInt32(_ArqueoTable.Rows[i][(int)InfoArqueo_DB.Producido].ToString());
                            oDtoOperacion.DtoArqueo.Valor = Convert.ToInt32(_ArqueoTable.Rows[i][(int)InfoArqueo_DB.Valor].ToString());

                            if (status == Operacion_Result.OK.ToString())
                            {
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Confirmación Arqueo OK";
                                oResultadoOperacion.EntidadDatos = oDtoOperacion;
                            }
                            else if (status == Operacion_Result.ERROR.ToString())
                            {
                                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                                oResultadoOperacion.Mensaje = "Confirmación Arqueo Error";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Generar LOG DataBase Exception
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    string exMessage = ex.ToString();
                }
            }
            else if (oOperacion.TipoOperacion == TipoOperacion.Pago || oOperacion.TipoOperacion == TipoOperacion.Datafono || oOperacion.TipoOperacion == TipoOperacion.Casco || oOperacion.TipoOperacion == TipoOperacion.Evento || oOperacion.TipoOperacion == TipoOperacion.Mensualidad || oOperacion.TipoOperacion == TipoOperacion.Reposicion || oOperacion.TipoOperacion == TipoOperacion.CobroTarjetaMensual)
            {
                ModuloDataSet.P_ConfirmarPagoDataTable _PagoTable = new ModuloDataSet.P_ConfirmarPagoDataTable();
                ModuloDataSetTableAdapters.P_ConfirmarPagoTableAdapter _PagoAdapter = new ModuloDataSetTableAdapters.P_ConfirmarPagoTableAdapter();


                int TipoPago = (int)oOperacion.TipoOperacion;

                try
                {

                    _PagoTable.Constraints.Clear();

                    if (TipoPago == 3)
                    {
                        ModuloDataSet.P_RegistrarTransaccionREPODataTable _PagoREPOTable = new ModuloDataSet.P_RegistrarTransaccionREPODataTable();
                        ModuloDataSetTableAdapters.P_RegistrarTransaccionREPOTableAdapter _PagoREPOAdapter = new ModuloDataSetTableAdapters.P_RegistrarTransaccionREPOTableAdapter();

                        try
                        {
                            _PagoREPOTable.Constraints.Clear();

                            if (_PagoREPOAdapter.Fill(_PagoREPOTable, oOperacion.ID_Transaccion) > 0)
                            {
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Confirmación Pago REPO OK";
                            }
                        }
                        catch (Exception ex)
                        {
                            // Generar LOG DataBase Exception
                            oResultadoOperacion.oEstado = TipoRespuesta.Error;
                            string exMessage = ex.ToString();
                        }
                    }
                    //else if (TipoPago == 7)
                    //{
                    //    ModuloDataSet.P_RegistrarTransaccioncobroTarjetaDataTable _PagoREPOTable = new ModuloDataSet.P_RegistrarTransaccioncobroTarjetaDataTable();
                    //    ModuloDataSetTableAdapters.P_RegistrarTransaccioncobroTarjetaTableAdapter _PagoREPOAdapter = new ModuloDataSetTableAdapters.P_RegistrarTransaccioncobroTarjetaTableAdapter();

                    //    try
                    //    {
                    //        _PagoREPOTable.Constraints.Clear();

                    //        if (_PagoREPOAdapter.Fill(_PagoREPOTable, oOperacion.ID_Transaccion) > 0)
                    //        {
                    //            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    //            oResultadoOperacion.Mensaje = "Confirmación Pago Cobro Tarjeta OK";
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        // Generar LOG DataBase Exception
                    //        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    //        string exMessage = ex.ToString();
                    //    }
                    //}
                    else if (TipoPago == 6)
                    {
                        ModuloDataSet.P_RegistrarTransaccionCascoDataTable _PagoREPOTable = new ModuloDataSet.P_RegistrarTransaccionCascoDataTable();
                        ModuloDataSetTableAdapters.P_RegistrarTransaccionCascoTableAdapter _PagoREPOAdapter = new ModuloDataSetTableAdapters.P_RegistrarTransaccionCascoTableAdapter();

                        try
                        {
                            _PagoREPOTable.Constraints.Clear();

                            if (_PagoREPOAdapter.Fill(_PagoREPOTable, oOperacion.ID_Transaccion) > 0)
                            {
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Confirmación Pago Cobro Casco OK";
                            }
                        }
                        catch (Exception ex)
                        {
                            // Generar LOG DataBase Exception
                            oResultadoOperacion.oEstado = TipoRespuesta.Error;
                            string exMessage = ex.ToString();
                        }
                    }

                    if (TipoPago == 7)
                    {

                        ModuloDataSet.P_RegistrarTransaccionDatafonoDataTable _PagoDataTable = new ModuloDataSet.P_RegistrarTransaccionDatafonoDataTable();
                        ModuloDataSetTableAdapters.P_RegistrarTransaccionDatafonoTableAdapter _PagoDataAdapter = new ModuloDataSetTableAdapters.P_RegistrarTransaccionDatafonoTableAdapter();

                        try
                        {
                            _PagoDataTable.Constraints.Clear();

                            //oOperacion.Pago.Factura = "0";
                            oOperacion.Pago.Referencia = "0";
                            if (_PagoDataAdapter.Fill(_PagoDataTable, oOperacion.ID_Transaccion, Convert.ToInt32(oOperacion.Pago.Factura), Convert.ToInt32(oOperacion.Pago.NoAutorizacion), oOperacion.Pago.Franquicia, oOperacion.TotalPagado, oOperacion.Pago.CodigoBarras, Convert.ToInt32(oOperacion.Pago.NoTarjeta), oOperacion.Pago.Referencia) > 0)
                            {
                                for (int i = 0; i < _PagoDataTable.Count; i++)
                                {

                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "Confirmación Pago OK";
                                }
                            }
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            // Generar LOG DataBase Exception
                            oResultadoOperacion.oEstado = TipoRespuesta.Error;
                            string exMessage = ex.ToString();
                        }
                    }
                    else if (TipoPago == 5 && (int)oOperacion.Pago.EstadoPago != 2)
                    {
                        ModuloDataSet.P_RegistrarTransaccionEventoDataTable _PagoREPOTable = new ModuloDataSet.P_RegistrarTransaccionEventoDataTable();
                        ModuloDataSetTableAdapters.P_RegistrarTransaccionEventoTableAdapter _PagoREPOAdapter = new ModuloDataSetTableAdapters.P_RegistrarTransaccionEventoTableAdapter();

                        try
                        {
                            _PagoREPOTable.Constraints.Clear();

                            if (_PagoREPOAdapter.Fill(_PagoREPOTable, oOperacion.ID_Transaccion) > 0)
                            {
                                for (int i = 0; i < _PagoREPOTable.Count; i++)
                                {
                                    string Factura = _PagoREPOTable.Rows[i][0].ToString();
                                    //string CodigoBarras = _PagoTable.Rows[i][1].ToString();


                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "Confirmación Pago OK";
                                    oDtoOperacion.DtoPago.Factura = Factura;
                                    //oDtoOperacion.DtoPago.CodigoBarras = CodigoBarras;
                                    oResultadoOperacion.EntidadDatos = oDtoOperacion;




                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Generar LOG DataBase Exception
                            oResultadoOperacion.oEstado = TipoRespuesta.Error;
                            string exMessage = ex.ToString();
                        }
                    }
                    else
                    {

                        if (oOperacion.ValidacionCobro == 7)
                        {
                            TipoPago = 7;
                        }

                        if (oOperacion.Descripcion != "0" && oOperacion.Total == 0)
                        {
                            TipoPago = 4;
                        }

                        if (_PagoAdapter.Fill(_PagoTable, oOperacion.ID_Transaccion, (int)oOperacion.Pago.EstadoPago, 0, TipoPago, oOperacion.Total, oOperacion.Iva, oOperacion.TotalPagado, oOperacion.Comision) > 0)
                        {
                            for (int i = 0; i < _PagoTable.Count; i++)
                            {
                                string Factura = _PagoTable.Rows[i][0].ToString();
                                //string CodigoBarras = _PagoTable.Rows[i][1].ToString();


                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Confirmación Pago OK";
                                oDtoOperacion.DtoPago.Factura = Factura;
                                //oDtoOperacion.DtoPago.CodigoBarras = CodigoBarras;
                                oResultadoOperacion.EntidadDatos = oDtoOperacion;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Generar LOG DataBase Exception
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    string exMessage = ex.ToString();
                }
            }
            else if (oOperacion.TipoOperacion == TipoOperacion.Carga)
            {
                ModuloDataSet.P_ConfirmarCargaDataTable _CargaTable = new ModuloDataSet.P_ConfirmarCargaDataTable();
                ModuloDataSetTableAdapters.P_ConfirmarCargaTableAdapter _CargaAdapter = new ModuloDataSetTableAdapters.P_ConfirmarCargaTableAdapter();

                try
                {
                    _CargaTable.Constraints.Clear();

                    if (_CargaAdapter.Fill(_CargaTable, oOperacion.ID_Operacion) > 0)
                    {
                        for (int i = 0; i < _CargaTable.Rows.Count; i++)
                        {
                            string status = _CargaTable.Rows[i][(int)InfoCarga_DB.Status].ToString();


                            if (status == Operacion_Result.OK.ToString())
                            {
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Confirmación Carga OK";
                                oResultadoOperacion.EntidadDatos = oDtoOperacion;
                            }
                            else if (status == Operacion_Result.ERROR.ToString())
                            {
                                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                                oResultadoOperacion.Mensaje = "Confirmación Carga Error";
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    // Generar LOG DataBase Exception
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    string exMessage = ex.ToString();
                }
            }

            return oResultadoOperacion;
        }       

        public ResultadoOperacion ObtenerUsuario(Usuario oUsuario)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoUsuario oDtoDtoUsuario = new DtoUsuario();
            //List<DtoPerfil> lstPerfiles = new List<DtoPerfil>();

            //EGlobalTAdminDataSet.P_Admin_ValidarUsuDataTable _InfoUsuarioTable = new EGlobalTAdminDataSet.P_Admin_ValidarUsuDataTable();
            //EGlobalTAdminDataSetTableAdapters.P_Admin_ValidarUsuTableAdapter _InfoUsuarioAdapter = new EGlobalTAdminDataSetTableAdapters.P_Admin_ValidarUsuTableAdapter();

            //try
            //{
            //    _InfoUsuarioTable.Constraints.Clear();

            //    if (_InfoUsuarioAdapter.Fill(_InfoUsuarioTable, oUsuario.IdCriptUsuario) > 0)
            //    {
            //        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
            //        oResultadoOperacion.Mensaje = "Info Partes OK";

            //        for (int i = 0; i < _InfoUsuarioTable.Rows.Count; i++)
            //        {
            //            DtoPerfil oDtoPerfil = new DtoPerfil();
            //            oDtoDtoUsuario.Password = _InfoUsuarioTable.Rows[i][(int)InfoUsuario_DB.Password].ToString();
            //        }

            //        //if (lstPerfiles.Count > 0)
            //        //{
            //        //    oDtoDtoUsuario.lstDtoPerfil = lstPerfiles;
            //        //}

            //        oResultadoOperacion.EntidadDatos = oDtoDtoUsuario;
            //    }
            //    else
            //    {
            //        oResultadoOperacion.oEstado = TipoRespuesta.Error;
            //        oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Generar LOG DataBase Exception
            //    oResultadoOperacion.oEstado = TipoRespuesta.Error;
            //    string exMessage = ex.ToString();
            //}

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarClave(long Identificacion, string clave)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_ValidarClaveDataTable _ValidarClaveTable = new ModuloDataSet.P_ValidarClaveDataTable();
            ModuloDataSetTableAdapters.P_ValidarClaveTableAdapter _ValidarClaveAdapter = new ModuloDataSetTableAdapters.P_ValidarClaveTableAdapter();

            string resultado = string.Empty;

            try
            {
                _ValidarClaveTable.Constraints.Clear();

                if (_ValidarClaveAdapter.Fill(_ValidarClaveTable, Identificacion,clave) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _ValidarClaveTable.Rows.Count; i++)
                    {
                        resultado = _ValidarClaveTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion GenerarClave(Modulo oModulo, Usuario oUsuario)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string result = string.Empty;

            ModuloDataSet.P_GenerarClaveDataTable _InfoGenerarClave = new ModuloDataSet.P_GenerarClaveDataTable();
            ModuloDataSetTableAdapters.P_GenerarClaveTableAdapter _GenerarClaveAdapter = new ModuloDataSetTableAdapters.P_GenerarClaveTableAdapter();

            try
            {
                _InfoGenerarClave.Constraints.Clear();

                if (_GenerarClaveAdapter.Fill(_InfoGenerarClave, oModulo.ID_Modulo) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Generar Clave OK";

                    oResultadoOperacion.EntidadDatos = result;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion ValidarSegundaClave(Modulo oModulo, string clave)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //ModuloDataSet.P_ValidarSegundaClaveDataTable _ValidarSegundaClaveTable = new ModuloDataSet.P_ValidarSegundaClaveDataTable();
            //ModuloDataSetTableAdapters.P_ValidarSegundaClaveTableAdapter _ValidarSegundaClaveAdapter = new ModuloDataSetTableAdapters.P_ValidarSegundaClaveTableAdapter();

            string resultado = string.Empty;

            try
            {
                //_ValidarSegundaClaveTable.Constraints.Clear();

                //if (_ValidarSegundaClaveAdapter.Fill(_ValidarSegundaClaveTable, oModulo.ID_Modulo, clave) > 0)
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                //    oResultadoOperacion.Mensaje = "Info Segunda Clave OK";

                //    for (int i = 0; i < _ValidarSegundaClaveTable.Rows.Count; i++)
                //    {
                //        resultado = _ValidarSegundaClaveTable.Rows[i][0].ToString();
                //    }

                //    oResultadoOperacion.EntidadDatos = resultado;
                //}
                //else
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                //    oResultadoOperacion.Mensaje = "Segunda clave erronea";
                //}
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }      

        public ResultadoOperacion CrearAlarma(Alarma oAlarma)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int idcajero = 0;

            ModuloDataSet.P_RegistrarAlarmaDataTable _CrearAlarmaTable = new ModuloDataSet.P_RegistrarAlarmaDataTable();
            ModuloDataSetTableAdapters.P_RegistrarAlarmaTableAdapter _CrearAlarmaAdapter = new ModuloDataSetTableAdapters.P_RegistrarAlarmaTableAdapter();

            try
            {
                _CrearAlarmaTable.Constraints.Clear();

                if (_CrearAlarmaAdapter.Fill(_CrearAlarmaTable,1,oAlarma.NivelError,oAlarma.TipoError,oAlarma.NombreParte,oAlarma.Descripcion,oAlarma.IdCajero) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info crear alarma OK";

                    oResultadoOperacion.EntidadDatos = idcajero;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion SolucionarTodasAlarmas(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int idcajero = 0;

            ModuloDataSet.P_SolucionarAlarmasModuloDataTable _SolucionarTodasAlarmasTable = new ModuloDataSet.P_SolucionarAlarmasModuloDataTable();
            ModuloDataSetTableAdapters.P_SolucionarAlarmasModuloTableAdapter _SolucionarTodasAlarmasAdapter = new ModuloDataSetTableAdapters.P_SolucionarAlarmasModuloTableAdapter();

            try
            {
                _SolucionarTodasAlarmasTable.Constraints.Clear();

                if (_SolucionarTodasAlarmasAdapter.Fill(_SolucionarTodasAlarmasTable, oModulo.ID_Modulo) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    oResultadoOperacion.EntidadDatos = idcajero;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion InsertarLogWS(LogWS oLogWS)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int idLog = 0;

            //ModuloDataSet.P_InsertarLogWSDataTable _InsertarLogWSTable = new ModuloDataSet.P_InsertarLogWSDataTable();
            //ModuloDataSetTableAdapters.P_InsertarLogWSTableAdapter _InsertarLogWSAdapter = new ModuloDataSetTableAdapters.P_InsertarLogWSTableAdapter();

            try
            {
                //_InsertarLogWSTable.Constraints.Clear();

                //if (_InsertarLogWSAdapter.Fill(_InsertarLogWSTable, oLogWS.IdCajero, oLogWS.IdTransaccion, oLogWS.Metodo, oLogWS.Entrada, oLogWS.Salida) > 0)
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                //    oResultadoOperacion.Mensaje = "Info Partes OK";

                //    for (int i = 0; i < _InsertarLogWSTable.Rows.Count; i++)
                //    {
                //        idLog = Convert.ToInt32(_InsertarLogWSTable.Rows[i][1]);
                //    }

                //    oResultadoOperacion.EntidadDatos = idLog;
                //}
                //else
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                //    oResultadoOperacion.Mensaje = "No Inserto Log";
                //}
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerIdTransacciones(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();


            List<DtoIngresos> lstDtoIngresos = new List<DtoIngresos>();

            //ModuloDataSet.P_RegistrarTransaccionDataTable _IdTransaccionesTable = new ModuloDataSet.P_RegistrarTransaccionDataTable();
            //ModuloDataSetTableAdapters.P_RegistrarTransaccionTableAdapter _TransaccionesAdapter = new ModuloDataSetTableAdapters.P_RegistrarTransaccionTableAdapter();

            try
            {
                //_IdTransaccionesTable.Constraints.Clear();
                //if (_TransaccionesAdapter.Fill(_IdTransaccionesTable, oTransaccion.IdModulo, oTransaccion.NumeroDocumentoOrigen) > 0)
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                //    oResultadoOperacion.Mensaje = "Listar Registrar Transaccion OK";

                //    for (int i = 0; i < _IdTransaccionesTable.Rows.Count; i++)
                //    {
                //        DtoIngresos oDtoIngresos = new DtoIngresos();

                //        oDtoIngresos.Status = _IdTransaccionesTable.Rows[i][0].ToString();
                //        oDtoIngresos.Codigo = _IdTransaccionesTable.Rows[i][1].ToString();

                //        lstDtoIngresos.Add(oDtoIngresos);
                //    }

                //    oResultadoOperacion.ListaEntidadDatos = lstDtoIngresos;
                //}
                //else
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                //    oResultadoOperacion.Mensaje = "error en base de datos.";
                //}
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerTransaccionesOfflineModulo(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoTransacciones> oListaDtoTransacciones = new List<DtoTransacciones>();

            //ModuloDataSet.P_ListarTransacOfflineDataTable _InfoTransaccionesTable = new ModuloDataSet.P_ListarTransacOfflineDataTable();
            //ModuloDataSetTableAdapters.P_ListarTransacOfflineTableAdapter _InfoTransaccionesAdapter = new ModuloDataSetTableAdapters.P_ListarTransacOfflineTableAdapter();

            try
            {
                //_InfoTransaccionesTable.Constraints.Clear();

                //if (_InfoTransaccionesAdapter.Fill(_InfoTransaccionesTable) > 0)
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                //    oResultadoOperacion.Mensaje = "Info TransaccionesOffline OK";

                //    for (int i = 0; i < _InfoTransaccionesTable.Rows.Count; i++)
                //    {
                //        DtoTransacciones Transacciones = new DtoTransacciones();
                //        Transacciones.IdTransaccion = Convert.ToInt32(_InfoTransaccionesTable.Rows[i][0].ToString());
                //        Transacciones.IdModulo = _InfoTransaccionesTable.Rows[i][1].ToString();
                //        Transacciones.IdDocumento = _InfoTransaccionesTable.Rows[i][2].ToString();
                //        Transacciones.FechaTransaccion = Convert.ToDateTime(_InfoTransaccionesTable.Rows[i][3].ToString());
                //        oListaDtoTransacciones.Add(Transacciones);
                //    }

                //    oResultadoOperacion.ListaEntidadDatos = oListaDtoTransacciones;
                //}
                //else
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                //    oResultadoOperacion.Mensaje = "Módulo sin partes registradas en base de datos.";
                //}
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerPagosOffline(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();


            List<DtoPago> lstDtoPago = new List<DtoPago>();

            //ModuloDataSet.P_ListarPagosOfflineDataTable _PagosOfflineTable = new ModuloDataSet.P_ListarPagosOfflineDataTable();
            //ModuloDataSetTableAdapters.P_ListarPagosOfflineTableAdapter _PagosOfflineAdapter = new ModuloDataSetTableAdapters.P_ListarPagosOfflineTableAdapter();

            try
            {
                //_PagosOfflineTable.Constraints.Clear();
                //if (_PagosOfflineAdapter.Fill(_PagosOfflineTable, oTransaccion.IdTransaccion) > 0)
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                //    oResultadoOperacion.Mensaje = "Listar Pagos Offline OK";

                //    for (int i = 0; i < _PagosOfflineTable.Rows.Count; i++)
                //    {
                //        DtoPago oDtoPago = new DtoPago();

                //        oDtoPago.ID_Pago = Convert.ToInt32(_PagosOfflineTable.Rows[i][0].ToString());
                //        oDtoPago.CodigoBarras = _PagosOfflineTable.Rows[i][13].ToString();
                //        oDtoPago.Factura = _PagosOfflineTable.Rows[i][7].ToString();
                //        oDtoPago.Total = Convert.ToDouble(_PagosOfflineTable.Rows[i][3].ToString());
                //        oDtoPago.Comision = Convert.ToDouble(_PagosOfflineTable.Rows[i][4].ToString());
                //        oDtoPago.Redondeo = Convert.ToDouble(_PagosOfflineTable.Rows[i][5].ToString());
                //        oDtoPago.Iva = Convert.ToDouble(_PagosOfflineTable.Rows[i][6].ToString());
                //        oDtoPago.EstadoPago = (TipoEstadoPago)Convert.ToInt32(_PagosOfflineTable.Rows[i][11].ToString());

                //        lstDtoPago.Add(oDtoPago);
                //    }

                //    oResultadoOperacion.ListaEntidadDatos = lstDtoPago;
                //}
                //else
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                //    oResultadoOperacion.Mensaje = "error en base de datos.";
                //}
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerMovimientoOffline(Pago oPago)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoLogMovimiento> oListaDtoLogMovimiento = new List<DtoLogMovimiento>();

            //ModuloDataSet.P_ListarMovimientosOfflineDataTable _InfoMovimientosTable = new ModuloDataSet.P_ListarMovimientosOfflineDataTable();
            //ModuloDataSetTableAdapters.P_ListarMovimientosOfflineTableAdapter _InfoMovimientosAdapter = new ModuloDataSetTableAdapters.P_ListarMovimientosOfflineTableAdapter();

            try
            {
                //_InfoMovimientosTable.Constraints.Clear();

                //if (_InfoMovimientosAdapter.Fill(_InfoMovimientosTable,oPago.ID_Pago) > 0)
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                //    oResultadoOperacion.Mensaje = "Info MovimientosOffline OK";

                //    for (int i = 0; i < _InfoMovimientosTable.Rows.Count; i++)
                //    {
                //        DtoLogMovimiento Movimiento = new DtoLogMovimiento();
                //        Movimiento.IdMovimiento = Convert.ToInt32(_InfoMovimientosTable.Rows[i][0].ToString());
                //        Movimiento.IdCajero = _InfoMovimientosTable.Rows[i][1].ToString();
                //        Movimiento.Id = Convert.ToInt32(_InfoMovimientosTable.Rows[i][2].ToString());
                //        Movimiento.Parte = _InfoMovimientosTable.Rows[i][5].ToString();
                //        Movimiento.Accion = _InfoMovimientosTable.Rows[i][6].ToString();
                //        Movimiento.Denominacion = Convert.ToInt32(_InfoMovimientosTable.Rows[i][7].ToString());
                //        Movimiento.Cantidad = Convert.ToInt32(_InfoMovimientosTable.Rows[i][8].ToString());
                //        Movimiento.Valor = Convert.ToInt32(_InfoMovimientosTable.Rows[i][9].ToString());
                //        Movimiento.Acumulado = Convert.ToInt32(_InfoMovimientosTable.Rows[i][10].ToString());

                //        oListaDtoLogMovimiento.Add(Movimiento);
                //    }

                //    oResultadoOperacion.ListaEntidadDatos = oListaDtoLogMovimiento;
                //}
                //else
                //{
                //    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                //    oResultadoOperacion.Mensaje = "Módulo sin partes registradas en base de datos.";
                //}
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ActualizarRegistro(Pago oPago)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //ModuloDataSet.P_ActualizarRegistroDataTable _ActualizarRegistroTable = new ModuloDataSet.P_ActualizarRegistroDataTable();
            //ModuloDataSetTableAdapters.P_ActualizarRegistroTableAdapter _ActualizarRegistroAdapter = new ModuloDataSetTableAdapters.P_ActualizarRegistroTableAdapter();

            try
            {
                //_ActualizarRegistroTable.Constraints.Clear();

                //if (_ActualizarRegistroAdapter.Fill(_ActualizarRegistroTable,oPago.ID_Pago) > 0)
                //{
                //    for (int i = 0; i < _ActualizarRegistroTable.Rows.Count; i++)
                //    {
                //        string status = _ActualizarRegistroTable.Rows[i][(int)Operacion_DB.Status].ToString();
                //        //long code = Convert.ToInt32(_ActualizarRegistroTable.Rows[i][(int)Operacion_DB.Code].ToString());

                //        if (status == Operacion_Result.OK.ToString())
                //        {
                //            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                //            oResultadoOperacion.Mensaje = "Registro Movimiento OK";
                //            oResultadoOperacion.EntidadDatos = 1;
                //        }
                //        else if (status == Operacion_Result.ERROR.ToString())
                //        {
                //            oResultadoOperacion.oEstado = TipoRespuesta.Error;
                //            oResultadoOperacion.Mensaje = "Registro Movimiento Error";
                //            oResultadoOperacion.EntidadDatos = "";
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                // Generar LOG DataBase Exception
                string exMessage = ex.ToString();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerDetalleArqueo(Arqueo oArqueo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoArqueo> oListaDtoDetalleArqueo = new List<DtoArqueo>();

            ModuloDataSet.P_DetalleArqueoDataTable _InfoDetalleArqueoTable = new ModuloDataSet.P_DetalleArqueoDataTable();
            ModuloDataSetTableAdapters.P_DetalleArqueoTableAdapter _InfoDetalleArqueoAdapter = new ModuloDataSetTableAdapters.P_DetalleArqueoTableAdapter();

            try
            {
                _InfoDetalleArqueoTable.Constraints.Clear();

                if (_InfoDetalleArqueoAdapter.Fill(_InfoDetalleArqueoTable, oArqueo.IdArqueo) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info DetalleArqueo OK";

                    for (int i = 0; i < _InfoDetalleArqueoTable.Rows.Count; i++)
                    {
                        DtoArqueo Arqueo = new DtoArqueo();
                        Arqueo.Parte = _InfoDetalleArqueoTable.Rows[i][0].ToString();
                        Arqueo.Denominacion = Convert.ToInt32(_InfoDetalleArqueoTable.Rows[i][1].ToString());
                        Arqueo.Cantidad = Convert.ToInt32(_InfoDetalleArqueoTable.Rows[i][2].ToString());
                        Arqueo.Valor = Convert.ToInt32(_InfoDetalleArqueoTable.Rows[i][3].ToString());

                        oListaDtoDetalleArqueo.Add(Arqueo);
                    }

                    oResultadoOperacion.ListaEntidadDatos = oListaDtoDetalleArqueo;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Info DetalleArqueo error";
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

        public ResultadoOperacion GenerarToken()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_GenerarTokenDataTable _GenerarTokenTable = new ModuloDataSet.P_GenerarTokenDataTable();
            ModuloDataSetTableAdapters.P_GenerarTokenTableAdapter _GenerarTokenAdapter = new ModuloDataSetTableAdapters.P_GenerarTokenTableAdapter();

            _GenerarTokenAdapter.Connection = new System.Data.SqlClient.SqlConnection("Data Source=107.180.70.70;Initial Catalog=SmartCoinCentral;User ID=sa;Password=Sm4rtC0in$");

            string resultado = string.Empty;

            try
            {
                _GenerarTokenTable.Constraints.Clear();

                if (_GenerarTokenAdapter.Fill(_GenerarTokenTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _GenerarTokenTable.Rows.Count; i++)
                    {
                        resultado = _GenerarTokenTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion Monitoreo(string IdModulo,string Estado)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarMonitoreoDataTable _GenerarTokenTable = new ModuloDataSet.P_RegistrarMonitoreoDataTable();
            ModuloDataSetTableAdapters.P_RegistrarMonitoreoTableAdapter _GenerarTokenAdapter = new ModuloDataSetTableAdapters.P_RegistrarMonitoreoTableAdapter();

            _GenerarTokenAdapter.Connection = new System.Data.SqlClient.SqlConnection("Data Source=107.180.70.70;Initial Catalog=SmartCoinCentral;User ID=sa;Password=Sm4rtC0in$");

            string resultado = string.Empty;

            try
            {
                _GenerarTokenTable.Constraints.Clear();

                if (_GenerarTokenAdapter.Fill(_GenerarTokenTable,IdModulo,Estado) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _GenerarTokenTable.Rows.Count; i++)
                    {
                        resultado = _GenerarTokenTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion RegistrarCliente(string Celular, string Email)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarClienteDataTable _RegistrarClienteTable = new ModuloDataSet.P_RegistrarClienteDataTable();
            ModuloDataSetTableAdapters.P_RegistrarClienteTableAdapter _RegistrarClienteAdapter = new ModuloDataSetTableAdapters.P_RegistrarClienteTableAdapter();

            string resultado = string.Empty;

            try
            {
                _RegistrarClienteTable.Constraints.Clear();

                if (_RegistrarClienteAdapter.Fill(_RegistrarClienteTable, Celular, Email) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _RegistrarClienteTable.Rows.Count; i++)
                    {
                        resultado = _RegistrarClienteTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion ValidarCliente(string Celular)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_ValidarClienteDataTable _ValidarClienteTable = new ModuloDataSet.P_ValidarClienteDataTable();
            ModuloDataSetTableAdapters.P_ValidarClienteTableAdapter _ValidarClienteAdapter = new ModuloDataSetTableAdapters.P_ValidarClienteTableAdapter();

            string resultado = string.Empty;

            try
            {
                _ValidarClienteTable.Constraints.Clear();

                if (_ValidarClienteAdapter.Fill(_ValidarClienteTable,Celular) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _ValidarClienteTable.Rows.Count; i++)
                    {
                        resultado = _ValidarClienteTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion ObtenerDatosConvenio(long IdConvenio)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string NombreConvenio = string.Empty;

            ModuloDataSet.P_ObtenerDatosConvenioDataTable _InfoParteTable = new ModuloDataSet.P_ObtenerDatosConvenioDataTable();
            ModuloDataSetTableAdapters.P_ObtenerDatosConvenioTableAdapter _InfoParteAdapter = new ModuloDataSetTableAdapters.P_ObtenerDatosConvenioTableAdapter();

            try
            {
                _InfoParteTable.Constraints.Clear();

                if (_InfoParteAdapter.Fill(_InfoParteTable, IdConvenio) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _InfoParteTable.Rows.Count; i++)
                    {
                        NombreConvenio = _InfoParteTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = NombreConvenio;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin partes registradas en base de datos.";
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

        public ResultadoOperacion RegistrarConvenioValidado(string Consecutivo, string CodigoCompleto, string IdModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarConvenioValidadoDataTable _RegistrarConvenioValidadoTable = new ModuloDataSet.P_RegistrarConvenioValidadoDataTable();
            ModuloDataSetTableAdapters.P_RegistrarConvenioValidadoTableAdapter _RegistrarConvenioValidadoAdapter = new ModuloDataSetTableAdapters.P_RegistrarConvenioValidadoTableAdapter();

            string resultado = string.Empty;

            try
            {
                _RegistrarConvenioValidadoTable.Constraints.Clear();

                if (_RegistrarConvenioValidadoAdapter.Fill(_RegistrarConvenioValidadoTable, Consecutivo, CodigoCompleto,IdModulo) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _RegistrarConvenioValidadoTable.Rows.Count; i++)
                    {
                        resultado = _RegistrarConvenioValidadoTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion ValidarConvenio(string Codigo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_ValidarConvenioDataTable _ValidarConvenioTable = new ModuloDataSet.P_ValidarConvenioDataTable();
            ModuloDataSetTableAdapters.P_ValidarConvenioTableAdapter _ValidarConvenioAdapter = new ModuloDataSetTableAdapters.P_ValidarConvenioTableAdapter();

            string resultado = string.Empty;

            try
            {
                _ValidarConvenioTable.Constraints.Clear();

                if (_ValidarConvenioAdapter.Fill(_ValidarConvenioTable, Codigo) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _ValidarConvenioTable.Rows.Count; i++)
                    {
                        resultado = _ValidarConvenioTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
                    oResultadoOperacion.EntidadDatos = string.Empty;
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

        public ResultadoOperacion ObtenerInfoAutorizado(Autorizado oAutorizado)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoAutorizado> olstDtoAutorizado = new List<DtoAutorizado>();

            ModuloDataSet.P_ValidarAutorizadoTarjetaYPlacaDataTable _InfoAutorizadoTable = new ModuloDataSet.P_ValidarAutorizadoTarjetaYPlacaDataTable();
            ModuloDataSetTableAdapters.P_ValidarAutorizadoTarjetaYPlacaTableAdapter _InfoAutorizadoAdapter = new ModuloDataSetTableAdapters.P_ValidarAutorizadoTarjetaYPlacaTableAdapter();

            

            try
            {
                _InfoAutorizadoTable.Constraints.Clear();

                if (_InfoAutorizadoAdapter.Fill(_InfoAutorizadoTable, oAutorizado.IdTarjeta,oAutorizado.Placa1) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Autorizado OK";

                    for (int i = 0; i < _InfoAutorizadoTable.Rows.Count; i++)
                    {
                        DtoAutorizado oDtoAutorizado = new DtoAutorizado();

                        oDtoAutorizado.Documento = _InfoAutorizadoTable.Rows[i][0].ToString();
                        oDtoAutorizado.IdAutorizacion = Convert.ToInt64(_InfoAutorizadoTable.Rows[i][1]);
                        oDtoAutorizado.Estado = Convert.ToBoolean(_InfoAutorizadoTable.Rows[i][2]);
                        oDtoAutorizado.EstadoAutorizacion = Convert.ToBoolean(_InfoAutorizadoTable.Rows[i][3]);
                        oDtoAutorizado.IdTarjeta = _InfoAutorizadoTable.Rows[i][4].ToString();
                        oDtoAutorizado.NombresAutorizado = _InfoAutorizadoTable.Rows[i][5].ToString();

                        string fechini = _InfoAutorizadoTable.Rows[i][6].ToString();
                        string fechfin = _InfoAutorizadoTable.Rows[i][7].ToString();

                        if (fechini != string.Empty)
                        {
                            oDtoAutorizado.FechaInicial = Convert.ToDateTime(fechini);
                        }
                        else
                        {
                            oDtoAutorizado.FechaInicial = null;
                        }

                        if (fechfin != string.Empty)
                        {
                            oDtoAutorizado.FechaFinal = Convert.ToDateTime(fechfin);
                        }
                        else 
                        {
                            oDtoAutorizado.FechaFinal = null;
                        }
                        oDtoAutorizado.IdEstacionamiento = Convert.ToInt64(_InfoAutorizadoTable.Rows[i][8]);
                        oDtoAutorizado.NombreAutorizacion = _InfoAutorizadoTable.Rows[i][9].ToString();
                        //oDtoAutorizado.NombreEmpresa = _InfoAutorizadoTable.Rows[i][10].ToString();
                        //oDtoAutorizado.NIT = _InfoAutorizadoTable.Rows[i][11].ToString();


                        olstDtoAutorizado.Add(oDtoAutorizado);
                    }

                    oResultadoOperacion.ListaEntidadDatos = olstDtoAutorizado;
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

        public ResultadoOperacion ValidarTransaccion(string IdTarjeta, DateTime FechaEntrada)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_ValidarTransaccionDataTable _ValidarTransaccionTable = new ModuloDataSet.P_ValidarTransaccionDataTable();
            ModuloDataSetTableAdapters.P_ValidarTransaccionTableAdapter _ValidarConvenioAdapter = new ModuloDataSetTableAdapters.P_ValidarTransaccionTableAdapter();

            string resultado = string.Empty;

            try
            {
                _ValidarTransaccionTable.Constraints.Clear();

                if (_ValidarConvenioAdapter.Fill(_ValidarTransaccionTable,IdTarjeta,FechaEntrada) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _ValidarTransaccionTable.Rows.Count; i++)
                    {
                        resultado = _ValidarTransaccionTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Registro no encontrado en la DB";
                    oResultadoOperacion.EntidadDatos = string.Empty;
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

        public ResultadoOperacion RegistrarTransaccionEntrada(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarIngresoDataTable _TransaccionTable = new ModuloDataSet.P_RegistrarIngresoDataTable();
            ModuloDataSetTableAdapters.P_RegistrarIngresoTableAdapter _TransaccionAdapter = new ModuloDataSetTableAdapters.P_RegistrarIngresoTableAdapter();

            try
            {
                _TransaccionTable.Constraints.Clear();

                if (_TransaccionAdapter.Fill(_TransaccionTable, oTransaccion.IdTransaccion, oTransaccion.CarrilEntrada, oTransaccion.ModuloEntrada, oTransaccion.IdEstacionamiento, 0, oTransaccion.IdTarjeta, oTransaccion.PlacaEntrada, oTransaccion.IdTipoVehiculo) > 0)
                {
                    for (int i = 0; i < _TransaccionTable.Rows.Count; i++)
                    {

                        long code = Convert.ToInt64(_TransaccionTable.Rows[i][0].ToString());
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.EntidadDatos = code;
                    }
                }
            }
            catch (Exception ex)
            {
                //Generar LOG DataBase Exception
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerFecha()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DateTime FechaServer = new DateTime();

            ModuloDataSet.P_ObtenerFechaHoraDataTable _InfoFechaTable = new ModuloDataSet.P_ObtenerFechaHoraDataTable();
            ModuloDataSetTableAdapters.P_ObtenerFechaHoraTableAdapter _InfoFechaAdapter = new ModuloDataSetTableAdapters.P_ObtenerFechaHoraTableAdapter();

            try
            {
                _InfoFechaTable.Constraints.Clear();

                if (_InfoFechaAdapter.Fill(_InfoFechaTable) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info parametros OK";

                    for (int i = 0; i < _InfoFechaTable.Rows.Count; i++)
                    {
                        FechaServer = Convert.ToDateTime(_InfoFechaTable.Rows[i][0]);

                    }

                    oResultadoOperacion.EntidadDatos = FechaServer;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error al consultar registros en base de datos.";
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

        public ResultadoOperacion RegistrarConvenioAplicado(long idTransaccion, int Convenio)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarConvenioAplicadoDataTable _RegistrarConvenioTable = new ModuloDataSet.P_RegistrarConvenioAplicadoDataTable();
            ModuloDataSetTableAdapters.P_RegistrarConvenioAplicadoTableAdapter _RegistrarConvenioAdapter = new ModuloDataSetTableAdapters.P_RegistrarConvenioAplicadoTableAdapter();

            string resultado = string.Empty;

            try
            {
                _RegistrarConvenioTable.Constraints.Clear();

                if (_RegistrarConvenioAdapter.Fill(_RegistrarConvenioTable, idTransaccion, Convenio) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _RegistrarConvenioTable.Rows.Count; i++)
                    {
                        resultado = _RegistrarConvenioTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion RegistrarTiempoGracia(long idTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarTiempoGraciaDataTable _RegistrarTiempoGraciaTable = new ModuloDataSet.P_RegistrarTiempoGraciaDataTable();
            ModuloDataSetTableAdapters.P_RegistrarTiempoGraciaTableAdapter _RegistrarTiempoGraciaAdapter = new ModuloDataSetTableAdapters.P_RegistrarTiempoGraciaTableAdapter();

            string resultado = string.Empty;

            try
            {
                _RegistrarTiempoGraciaTable.Constraints.Clear();

                if (_RegistrarTiempoGraciaAdapter.Fill(_RegistrarTiempoGraciaTable, idTransaccion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _RegistrarTiempoGraciaTable.Rows.Count; i++)
                    {
                        resultado = _RegistrarTiempoGraciaTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion ObtenerFechaConvenio(long idTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DateTime FechaServer = new DateTime();

            ModuloDataSet.P_ValidarFechaConvenioDataTable _InfoFechaTable = new ModuloDataSet.P_ValidarFechaConvenioDataTable();
            ModuloDataSetTableAdapters.P_ValidarFechaConvenioTableAdapter _InfoFechaAdapter = new ModuloDataSetTableAdapters.P_ValidarFechaConvenioTableAdapter();

            try
            {
                _InfoFechaTable.Constraints.Clear();

                if (_InfoFechaAdapter.Fill(_InfoFechaTable,idTransaccion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info parametros OK";

                    for (int i = 0; i < _InfoFechaTable.Rows.Count; i++)
                    {
                        FechaServer = Convert.ToDateTime(_InfoFechaTable.Rows[i][0]);

                    }

                    oResultadoOperacion.EntidadDatos = FechaServer;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error al consultar registros en base de datos.";
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

        public ResultadoOperacion ObtenerTarjetas(long IdEstacionamiento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoTarjetas> olstDtoTarjetas = new List<DtoTarjetas>();

            ModuloDataSet.P_ListarTarjetasDataTable _InfoTarjetasTable = new ModuloDataSet.P_ListarTarjetasDataTable();
            ModuloDataSetTableAdapters.P_ListarTarjetasTableAdapter _InfoTarjetasAdapter = new ModuloDataSetTableAdapters.P_ListarTarjetasTableAdapter();

            try
            {
                _InfoTarjetasTable.Constraints.Clear();

                if (_InfoTarjetasAdapter.Fill(_InfoTarjetasTable, IdEstacionamiento) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Tarjetas OK";

                    for (int i = 0; i < _InfoTarjetasTable.Rows.Count; i++)
                    {
                        DtoTarjetas oDtoTarjetas = new DtoTarjetas();

                        oDtoTarjetas.IdEstacionamiento = Convert.ToInt64(_InfoTarjetasTable.Rows[i][0]);
                        oDtoTarjetas.IdTarjeta = _InfoTarjetasTable.Rows[i][1].ToString();
                        oDtoTarjetas.Estado = Convert.ToBoolean(_InfoTarjetasTable.Rows[i][4]);

                        olstDtoTarjetas.Add(oDtoTarjetas);
                    }

                    oResultadoOperacion.ListaEntidadDatos = olstDtoTarjetas;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Módulo sin tarjetas en base de datos.";
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

        public ResultadoOperacion ObtenerResultadoPlaca(string Letra1, string Letra2)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoTransacciones> olstDtoTransacciones = new List<DtoTransacciones>();

            ModuloDataSet.P_ObtenerResultadoPlacaDataTable _InfoTable = new ModuloDataSet.P_ObtenerResultadoPlacaDataTable();
            ModuloDataSetTableAdapters.P_ObtenerResultadoPlacaTableAdapter _InfoAdapter = new ModuloDataSetTableAdapters.P_ObtenerResultadoPlacaTableAdapter();

            try
            {
                _InfoTable.Constraints.Clear();

                if (_InfoAdapter.Fill(_InfoTable, Letra1,Letra2) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Tarjetas OK";

                    for (int i = 0; i < _InfoTable.Rows.Count; i++)
                    {
                        DtoTransacciones oDtoTransacciones = new DtoTransacciones();

                        oDtoTransacciones.IdTransaccion = Convert.ToInt64(_InfoTable.Rows[i][0]);
                        oDtoTransacciones.PlacaEntrada = _InfoTable.Rows[i][5].ToString();
                        

                        olstDtoTransacciones.Add(oDtoTransacciones);
                    }

                    oResultadoOperacion.ListaEntidadDatos = olstDtoTransacciones;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Sin Transacciones en base de datos.";
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

        public ResultadoOperacion RegistrarPIN(string PIN, int TipoVehiculo, string IdModulo, long IdEstacionamiento, long IdTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarPinDataTable _RegistrarConvenioTable = new ModuloDataSet.P_RegistrarPinDataTable();
            ModuloDataSetTableAdapters.P_RegistrarPinTableAdapter _RegistrarConvenioAdapter = new ModuloDataSetTableAdapters.P_RegistrarPinTableAdapter();

            string resultado = string.Empty;

            try
            {
                _RegistrarConvenioTable.Constraints.Clear();

                if (_RegistrarConvenioAdapter.Fill(_RegistrarConvenioTable, PIN,TipoVehiculo,IdModulo,IdEstacionamiento,IdTransaccion) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _RegistrarConvenioTable.Rows.Count; i++)
                    {
                        resultado = _RegistrarConvenioTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion ValidarPIN(long PIN)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_ValidarDatosDataTable _ValidarClaveTable = new ModuloDataSet.P_ValidarDatosDataTable();
            ModuloDataSetTableAdapters.P_ValidarDatosTableAdapter _ValidarClaveAdapter = new ModuloDataSetTableAdapters.P_ValidarDatosTableAdapter();

            DtoPIN oDtoPIN = new DtoPIN();

            try
            {
                _ValidarClaveTable.Constraints.Clear();

                if (_ValidarClaveAdapter.Fill(_ValidarClaveTable, PIN) > 0)
                {
                   

                    for (int i = 0; i < _ValidarClaveTable.Rows.Count; i++)
                    {
                        oDtoPIN.PIN = _ValidarClaveTable.Rows[i][0].ToString();
                        oDtoPIN.FechaValidacion = Convert.ToDateTime(_ValidarClaveTable.Rows[i][1]);
                        oDtoPIN.TipoVehiculo = Convert.ToInt32(_ValidarClaveTable.Rows[i][2]);
                    }

                    oResultadoOperacion.EntidadDatos = oDtoPIN;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion RegistrarPAGOPIN(string IdTransaccion, string PIN, long Secuencia)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarPagoPinDataTable _RegistrarConvenioTable = new ModuloDataSet.P_RegistrarPagoPinDataTable();
            ModuloDataSetTableAdapters.P_RegistrarPagoPinTableAdapter _RegistrarConvenioAdapter = new ModuloDataSetTableAdapters.P_RegistrarPagoPinTableAdapter();

            string resultado = string.Empty;

            try
            {
                _RegistrarConvenioTable.Constraints.Clear();

                if (_RegistrarConvenioAdapter.Fill(_RegistrarConvenioTable, IdTransaccion,PIN, Secuencia) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _RegistrarConvenioTable.Rows.Count; i++)
                    {
                        resultado = _RegistrarConvenioTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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

        public ResultadoOperacion RegistrarTransaccionEvento(string IdTransaccion, string IdTarjeta, long IdEvento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ModuloDataSet.P_RegistrarEventoTransaccionCajeroDataTable _RegistrarConvenioTable = new ModuloDataSet.P_RegistrarEventoTransaccionCajeroDataTable();
            ModuloDataSetTableAdapters.P_RegistrarEventoTransaccionCajeroTableAdapter _RegistrarConvenioAdapter = new ModuloDataSetTableAdapters.P_RegistrarEventoTransaccionCajeroTableAdapter();

            string resultado = string.Empty;

            try
            {
                _RegistrarConvenioTable.Constraints.Clear();

                if (_RegistrarConvenioAdapter.Fill(_RegistrarConvenioTable, Convert.ToInt64(IdTransaccion), IdTarjeta, IdEvento) > 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Info Partes OK";

                    for (int i = 0; i < _RegistrarConvenioTable.Rows.Count; i++)
                    {
                        resultado = _RegistrarConvenioTable.Rows[i][0].ToString();
                    }

                    oResultadoOperacion.EntidadDatos = resultado;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Usuario no registrado en base de datos";
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
