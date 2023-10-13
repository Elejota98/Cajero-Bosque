using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ds.BusinessObjects.DataTransferObject;
using Ds.ServiceProxy.Ds_ModuloComercialService;
using Ds.BusinessObjects.Enums;

namespace Ds.ServiceProxy.DataTransferObjectMapper
{
    internal static partial class Mapper
    {

        internal static List<DtoAutorizado> FromDataTransferObjects(IList<ServiceDtoAutorizado> oServiceAutorizado)
        {
            if (oServiceAutorizado == null)
                return null;

            return oServiceAutorizado.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoAutorizado FromDataTransferObject(ServiceDtoAutorizado oServiceAutorizado)
        {
            DtoAutorizado oDtoAutorizado = new DtoAutorizado();

            if (oServiceAutorizado != null)
            {
                oDtoAutorizado.Documento = oServiceAutorizado.Documento;
                oDtoAutorizado.Estado = oServiceAutorizado.Estado;
                oDtoAutorizado.EstadoAutorizacion = oServiceAutorizado.EstadoAutorizacion;
                oDtoAutorizado.IdAutorizacion = oServiceAutorizado.IdAutorizacion;
                oDtoAutorizado.IdTarjeta = oServiceAutorizado.IdTarjeta;
                oDtoAutorizado.NombresAutorizado = oServiceAutorizado.NombresAutorizado;
                oDtoAutorizado.FechaInicial = oServiceAutorizado.FechaInicial;
                oDtoAutorizado.FechaFinal = oServiceAutorizado.FechaFinal;
                oDtoAutorizado.IdEstacionamiento = oServiceAutorizado.IdEstacionamiento;
                oDtoAutorizado.NombreEmpresa = oServiceAutorizado.NombreEmpresa;
                oDtoAutorizado.NIT = oServiceAutorizado.NIT;
                oDtoAutorizado.NombreAutorizacion = oServiceAutorizado.NombreAutorizacion;
            }

            return oDtoAutorizado;
        }

        internal static List<DtoModulo> FromDataTransferObjects(IList<ServiceDtoModulo> oServiceModulo)
        {
            if (oServiceModulo == null)
                return null;

            return oServiceModulo.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoModulo FromDataTransferObject(ServiceDtoModulo oServiceModulo)
        {
            DtoModulo oDtoModulo = new DtoModulo();

            if (oServiceModulo != null)
            {
                oDtoModulo.IdModulo = oServiceModulo.IdModulo;
                oDtoModulo.Ciudad = oServiceModulo.Ciudad;
                oDtoModulo.Estado = oServiceModulo.Estado;
                oDtoModulo.Extension = oServiceModulo.Extension;
                oDtoModulo.IdCiudad = oServiceModulo.IdCiudad;
                oDtoModulo.IdPais = oServiceModulo.IdPais;
                oDtoModulo.IdSede = oServiceModulo.IdSede;
                oDtoModulo.IdTipoModulo = oServiceModulo.IdTipoModulo;
                oDtoModulo.Ip = oServiceModulo.Ip;
                oDtoModulo.Mac = oServiceModulo.Mac;
                oDtoModulo.Nombre = oServiceModulo.Nombre;
                oDtoModulo.Pais = oServiceModulo.Pais;
                oDtoModulo.Direccion = oServiceModulo.Direccion;
                oDtoModulo.Acciones = Mapper.FromDataTransferObjects(oServiceModulo.Acciones);
                oDtoModulo.Partes = Mapper.FromDataTransferObjects(oServiceModulo.Partes);
                oDtoModulo.Factura = Mapper.FromDataTransferObject(oServiceModulo.Factura);
            }

            return oDtoModulo;
        }

        internal static List<DtoFactura> FromDataTransferObjects(IList<ServiceDtoFactura> oServiceFactura)
        {
            if (oServiceFactura == null)
                return null;

            return oServiceFactura.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoFactura FromDataTransferObject(ServiceDtoFactura oServiceFactura)
        {
            DtoFactura oDtoFactura = new DtoFactura();

            if (oServiceFactura != null)
            {
                oDtoFactura.Estado = oServiceFactura.Estado;
                oDtoFactura.FacturaActual = oServiceFactura.FacturaActual;
                oDtoFactura.FacturaFinal = oServiceFactura.FacturaFinal;
                oDtoFactura.FacturaInicial = oServiceFactura.FacturaInicial;
                oDtoFactura.FechaFinResolucion = oServiceFactura.FechaFinResolucion;
                oDtoFactura.FechaResolucion = oServiceFactura.FechaResolucion;
                oDtoFactura.IdFacturacion = oServiceFactura.IdFacturacion;
                oDtoFactura.IdModulo = oServiceFactura.IdModulo;
                oDtoFactura.IdSede = oServiceFactura.IdSede;
                oDtoFactura.NumeroResolucion = oServiceFactura.NumeroResolucion;
                oDtoFactura.Prefijo = oServiceFactura.Prefijo;
                
            }

            return oDtoFactura;
        }

        internal static List<DtoPIN> FromDataTransferObjects(IList<ServiceDtoPIN> oServicePIN)
        {
            if (oServicePIN == null)
                return null;

            return oServicePIN.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoPIN FromDataTransferObject(ServiceDtoPIN oServicePIN)
        {
            DtoPIN oDtoPIN = new DtoPIN();

            if (oServicePIN != null)
            {
                oDtoPIN.FechaValidacion = oServicePIN.FechaValidacion;
                oDtoPIN.PIN = oServicePIN.PIN;
                oDtoPIN.TipoVehiculo = oServicePIN.TipoVehiculo;
            }

            return oDtoPIN;
        }

        internal static List<DtoAccion> FromDataTransferObjects(IList<ServiceDtoAccion> oServiceAccion)
        {
            if (oServiceAccion == null)
                return null;

            return oServiceAccion.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoAccion FromDataTransferObject(ServiceDtoAccion oServiceAccion)
        {
            DtoAccion oDtoAccion = new DtoAccion();

            if (oServiceAccion != null)
            {
                oDtoAccion.NombreAccion = oServiceAccion.NombreAccion;
            }

            return oDtoAccion;
        }

        //internal static List<DtoSede> FromDataTransferObjects(IList<ServiceDtoSede> oServiceDtoSede)
        //{
        //    if (oServiceDtoSede == null)
        //        return null;

        //    return oServiceDtoSede.Select(c => FromDataTransferObject(c)).ToList();
        //}
        //internal static DtoSede FromDataTransferObject(ServiceDtoSede oServiceDtoSede)
        //{
        //    DtoSede oDtoSede = new DtoSede();

        //    if (oServiceDtoSede != null)
        //    {
        //        oDtoSede.Ciudad = oServiceDtoSede.Ciudad;
        //        oDtoSede.Departamento = oServiceDtoSede.Departamento;
        //        oDtoSede.Descripcion = oServiceDtoSede.Descripcion;
        //        oDtoSede.Direccion = oServiceDtoSede.Direccion;
        //        oDtoSede.ID_Sede = oServiceDtoSede.ID_Sede;
        //        oDtoSede.Nombre_Sede = oServiceDtoSede.Nombre_Sede;
        //    }

        //    return oDtoSede;
        //}

        internal static List<DtoParteModulo> FromDataTransferObjects(IList<ServiceDtoParteModulo> oServiceDtoParteModulo)
        {
            if (oServiceDtoParteModulo == null)
                return null;

            return oServiceDtoParteModulo.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoParteModulo FromDataTransferObject(ServiceDtoParteModulo oServiceDtoParteModulo)
        {
            DtoParteModulo oDtoParteModulo = new DtoParteModulo();

            if (oServiceDtoParteModulo != null)
            {
                oDtoParteModulo.IdParte = oServiceDtoParteModulo.IdParte;
                oDtoParteModulo.IdModulo = oServiceDtoParteModulo.IdModulo;
                oDtoParteModulo.TipoParte = oServiceDtoParteModulo.TipoParte;
                oDtoParteModulo.Nombre = oServiceDtoParteModulo.Nombre;
                oDtoParteModulo.Denominacion = oServiceDtoParteModulo.Denominacion;
                oDtoParteModulo.CantidadMin = oServiceDtoParteModulo.CantidadMin;
                oDtoParteModulo.CantidadAlarma = oServiceDtoParteModulo.CantidadAlarma;
                oDtoParteModulo.Estado = oServiceDtoParteModulo.Estado;
                oDtoParteModulo.DineroActual = oServiceDtoParteModulo.DineroActual;
                oDtoParteModulo.CantidadActual = oServiceDtoParteModulo.CantidadActual;
                oDtoParteModulo.Prioridad = oServiceDtoParteModulo.Prioridad;
                oDtoParteModulo.NumParte = oServiceDtoParteModulo.NumParte;
                
            }

            return oDtoParteModulo;
        }

        internal static List<DtoParteModuloF56> FromDataTransferObjects(IList<ServiceDtoParteModuloF56> oServiceDtoParteModuloF56)
        {
            if (oServiceDtoParteModuloF56 == null)
                return null;

            return oServiceDtoParteModuloF56.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoParteModuloF56 FromDataTransferObject(ServiceDtoParteModuloF56 oServiceDtoParteModuloF56)
        {
            DtoParteModuloF56 oDtoParteModulo = new DtoParteModuloF56();

            if (oServiceDtoParteModuloF56 != null)
            {
                oDtoParteModulo.Denominacion = oServiceDtoParteModuloF56.Denominacion;
                oDtoParteModulo.Dinero_Actual = oServiceDtoParteModuloF56.Dinero_Actual;
                oDtoParteModulo.Nombre_Parte = oServiceDtoParteModuloF56.Nombre_Parte;
                oDtoParteModulo.Num_Parte = oServiceDtoParteModuloF56.Num_Parte;
                oDtoParteModulo.Prioridad = oServiceDtoParteModuloF56.Prioridad;
                oDtoParteModulo.Qty_Actual = oServiceDtoParteModuloF56.Qty_Actual;
                oDtoParteModulo.Qty_Alarma = oServiceDtoParteModuloF56.Qty_Alarma;
                oDtoParteModulo.Qty_Min = oServiceDtoParteModuloF56.Qty_Min;
                oDtoParteModulo.Tipo_Parte = oServiceDtoParteModuloF56.Tipo_Parte;
                oDtoParteModulo.Qty_Max = oServiceDtoParteModuloF56.Qty_Max;
            }

            return oDtoParteModulo;
        }

        internal static List<DtoSaldos> FromDataTransferObjects(IList<ServiceDtoSaldos> oServiceDtoSaldos)
        {
            if (oServiceDtoSaldos == null)
                return null;

            return oServiceDtoSaldos.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoSaldos FromDataTransferObject(ServiceDtoSaldos oServiceDtoSaldos)
        {
            DtoSaldos oDtoSaldos = new DtoSaldos();

            if (oServiceDtoSaldos != null)
            {
                oDtoSaldos.DtoPartes = Mapper.FromDataTransferObjects(oServiceDtoSaldos.DtoPartes);
            }

            return oDtoSaldos;
        }

        internal static List<DtoOperacion> FromDataTransferObjects(IList<ServiceDtoOperacion> oServiceDtoOperacion)
        {
            if (oServiceDtoOperacion == null)
                return null;

            return oServiceDtoOperacion.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoOperacion FromDataTransferObject(ServiceDtoOperacion oServiceDtoOperacion)
        {
            DtoOperacion oDtoOperacion = new DtoOperacion();

            if (oServiceDtoOperacion != null)
            {
                oDtoOperacion.DtoArqueo = Mapper.FromDataTransferObject(oServiceDtoOperacion.DtoArqueo);
                oDtoOperacion.DtoPago = Mapper.FromDataTransferObject(oServiceDtoOperacion.DtoPago);
                oDtoOperacion.ID_Modulo = oServiceDtoOperacion.ID_Modulo;
                oDtoOperacion.ID_Operacion = oServiceDtoOperacion.ID_Operacion;
                oDtoOperacion.ID_Transaccion = oServiceDtoOperacion.ID_Transaccion;
                oDtoOperacion.ID_Usuario = oServiceDtoOperacion.ID_Usuario;
                oDtoOperacion.ID_Fake_Operacion = oServiceDtoOperacion.ID_Fake_Operacion;

                if (oServiceDtoOperacion.TipoOperacion == Ds_ModuloComercialService.TipoOperacion.Pago)
                {
                    oDtoOperacion.TipoOperacion = BusinessObjects.Enums.TipoOperacion.Pago;
                }
                else if (oServiceDtoOperacion.TipoOperacion == Ds_ModuloComercialService.TipoOperacion.ArqueoParcial)
                {
                    oDtoOperacion.TipoOperacion = BusinessObjects.Enums.TipoOperacion.ArqueoParcial;
                }
                else if (oServiceDtoOperacion.TipoOperacion == Ds_ModuloComercialService.TipoOperacion.ArqueoTotal)
                {
                    oDtoOperacion.TipoOperacion = BusinessObjects.Enums.TipoOperacion.ArqueoTotal;
                }
                else if (oServiceDtoOperacion.TipoOperacion == Ds_ModuloComercialService.TipoOperacion.Carga)
                {
                    oDtoOperacion.TipoOperacion = BusinessObjects.Enums.TipoOperacion.Carga;
                }
                else if (oServiceDtoOperacion.TipoOperacion == Ds_ModuloComercialService.TipoOperacion.CerrarAplicacion)
                {
                    oDtoOperacion.TipoOperacion = BusinessObjects.Enums.TipoOperacion.CerrarAplicacion;
                }
                else if (oServiceDtoOperacion.TipoOperacion == Ds_ModuloComercialService.TipoOperacion.Mantenimiento)
                {
                    oDtoOperacion.TipoOperacion = BusinessObjects.Enums.TipoOperacion.Mantenimiento;
                }
                else if (oServiceDtoOperacion.TipoOperacion == Ds_ModuloComercialService.TipoOperacion.Pago)
                {
                    oDtoOperacion.TipoOperacion = BusinessObjects.Enums.TipoOperacion.Pago;
                }
            }

            return oDtoOperacion;
        }

        internal static List<DtoArqueo> FromDataTransferObjects(IList<ServiceDtoArqueo> oServiceDtoArqueo)
        {
            if (oServiceDtoArqueo == null)
                return null;

            return oServiceDtoArqueo.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoArqueo FromDataTransferObject(ServiceDtoArqueo oServiceDtoArqueo)
        {
            DtoArqueo oDtoArqueo = new DtoArqueo();

            if (oServiceDtoArqueo != null)
            {
                oDtoArqueo.ID_Arqueo = Convert.ToInt64(oServiceDtoArqueo.IdArqueo);
                oDtoArqueo.Producido = Convert.ToInt32(oServiceDtoArqueo.Producido);
                oDtoArqueo.Valor = Convert.ToInt32(oServiceDtoArqueo.Valor);
                oDtoArqueo.Denominacion = oServiceDtoArqueo.Denominacion;
                oDtoArqueo.Cantidad = oServiceDtoArqueo.Cantidad;
                oDtoArqueo.Parte = oServiceDtoArqueo.Parte;
            }

            return oDtoArqueo;
        }

        internal static List<DtoPago> FromDataTransferObjects(IList<ServiceDtoPago> oServiceDtoPago)
        {
            if (oServiceDtoPago == null)
                return null;

            return oServiceDtoPago.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoPago FromDataTransferObject(ServiceDtoPago oServiceDtoPago)
        {
            DtoPago oDtoPago = new DtoPago();

            if (oServiceDtoPago != null)
            {
                if (oServiceDtoPago.EstadoPago == Ds_ModuloComercialService.TipoEstadoPago.Aprobado)
                {
                    oDtoPago.EstadoPago = BusinessObjects.Enums.TipoEstadoPago.Aprobado;
                }
                else if (oServiceDtoPago.EstadoPago == Ds_ModuloComercialService.TipoEstadoPago.Cancelado)
                {
                    oDtoPago.EstadoPago = BusinessObjects.Enums.TipoEstadoPago.Cancelado;
                }
                else if (oServiceDtoPago.EstadoPago == Ds_ModuloComercialService.TipoEstadoPago.Error_Dispositivo)
                {
                    oDtoPago.EstadoPago = BusinessObjects.Enums.TipoEstadoPago.Error_Dispositivo;
                }
                else if (oServiceDtoPago.EstadoPago == Ds_ModuloComercialService.TipoEstadoPago.Error_WebService)
                {
                    oDtoPago.EstadoPago = BusinessObjects.Enums.TipoEstadoPago.Error_WebService;
                }
                else if (oServiceDtoPago.EstadoPago == Ds_ModuloComercialService.TipoEstadoPago.NoAplica)
                {
                    oDtoPago.EstadoPago = BusinessObjects.Enums.TipoEstadoPago.NoAplica;
                }
                else if (oServiceDtoPago.EstadoPago == Ds_ModuloComercialService.TipoEstadoPago.ReconteoExitoso)
                {
                    oDtoPago.EstadoPago = BusinessObjects.Enums.TipoEstadoPago.ReconteoExitoso;
                }
                oDtoPago.Factura = oServiceDtoPago.Factura;
                oDtoPago.ID_Pago = oServiceDtoPago.ID_Pago;
                oDtoPago.Referencia = oServiceDtoPago.Referencia;
                oDtoPago.CodigoBarras = oServiceDtoPago.CodigoBarras;
                oDtoPago.Total = oServiceDtoPago.Total;
                oDtoPago.Comision = oServiceDtoPago.Comision;
                oDtoPago.Redondeo = oServiceDtoPago.Redondeo;
                oDtoPago.Iva = oServiceDtoPago.Iva;




                if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.Credito)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.Credito;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.Ahorros)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.Ahorros;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.Efectivo)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.Efectivo;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.BolsilloCredito)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.BolsilloCredito;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.BolsilloDebito)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.BolsilloDebito;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.BonoDescuento)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.BonoDescuento;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.BonoEfectivo)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.BonoEfectivo;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.BonoRegalo)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.BonoRegalo;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.Corriente)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.Corriente;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.CreditoRotativo)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.CreditoRotativo;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.CuotaMonetaria)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.CuotaMonetaria;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.CupoCredito)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.CupoCredito;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.Lealtad)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.Lealtad;
                }
                else if (oServiceDtoPago.TipoPago == Ds_ModuloComercialService.TipoPago.SuperCupo)
                {
                    oDtoPago.TipoPago = BusinessObjects.Enums.TipoPago.SuperCupo;
                }

            }

            return oDtoPago;
        }

        internal static List<DtoUsuario> FromDataTransferObjects(IList<ServiceDtoUsuario> oServiceDtoUsuario)
        {
            if (oServiceDtoUsuario == null)
                return null;

            return oServiceDtoUsuario.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoUsuario FromDataTransferObject(ServiceDtoUsuario oServiceDtoUsuario)
        {
            DtoUsuario oDtoUsuario = new DtoUsuario();

            if (oServiceDtoUsuario != null)
            {
                oDtoUsuario.Apellido = oServiceDtoUsuario.Apellido;
                oDtoUsuario.Cargo = oServiceDtoUsuario.Cargo;
                oDtoUsuario.Empresa = oServiceDtoUsuario.Empresa;
                oDtoUsuario.IdUsuario = oServiceDtoUsuario.IdUsuario;
                oDtoUsuario.Nombres = oServiceDtoUsuario.Nombres;
                oDtoUsuario.Password = oServiceDtoUsuario.Password;
                oDtoUsuario.Perfil = oServiceDtoUsuario.Perfil;
                oDtoUsuario.lstDtoPerfil = Mapper.FromDataTransferObjects(oServiceDtoUsuario.lstDtoPerfil);
            }

            return oDtoUsuario;
        }

        internal static List<DtoPerfil> FromDataTransferObjects(IList<ServiceDtoPerfil> oServiceDtoPerfil)
        {
            if (oServiceDtoPerfil == null)
                return null;

            return oServiceDtoPerfil.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoPerfil FromDataTransferObject(ServiceDtoPerfil oServiceDtoPerfil)
        {
            DtoPerfil oDtoPerfil = new DtoPerfil();

            if (oServiceDtoPerfil != null)
            {
                oDtoPerfil.NombreControl = oServiceDtoPerfil.NombreControl;
            }

            return oDtoPerfil;
        }

        internal static TipoValidarSaldosMinimos FromDataTransferObject(string oServiceTipoValidarSaldosMinimos)
        {
            TipoValidarSaldosMinimos ValidarSaldosMinimos = new TipoValidarSaldosMinimos();

            if (oServiceTipoValidarSaldosMinimos != null && oServiceTipoValidarSaldosMinimos != string.Empty)
            {
                if (oServiceTipoValidarSaldosMinimos == "True")
                {
                    ValidarSaldosMinimos = TipoValidarSaldosMinimos.True;
                }
                else if (oServiceTipoValidarSaldosMinimos == "False")
                {
                    ValidarSaldosMinimos = TipoValidarSaldosMinimos.False;
                }
            }

            return ValidarSaldosMinimos;
        }


        internal static List<DtoParametro> FromDataTransferObjects(IList<ServiceDtoParametro> oServiceDtoParametro)
        {
            if (oServiceDtoParametro == null)
                return null;

            return oServiceDtoParametro.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoParametro FromDataTransferObject(ServiceDtoParametro oServiceDtoParametro)
        {
            DtoParametro oDtoParametro = new DtoParametro();

            if (oServiceDtoParametro != null)
            {
                oDtoParametro.Activo = oServiceDtoParametro.Estado;
                oDtoParametro.Codigo = oServiceDtoParametro.Codigo;
                oDtoParametro.Descripcion = oServiceDtoParametro.Descripcion;
                oDtoParametro.IdCajero = oServiceDtoParametro.IdModulo;
                oDtoParametro.Valor = oServiceDtoParametro.Valor;
            }

            return oDtoParametro;
        }

        internal static List<DtoIngresos> FromDataTransferObjects(IList<ServiceDtoIngresos> oServiceDtoIngresos)
        {
            if (oServiceDtoIngresos == null)
                return null;

            return oServiceDtoIngresos.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoIngresos FromDataTransferObject(ServiceDtoIngresos oServiceDtoIngresos)
        {
            DtoIngresos oDtoIngresos = new DtoIngresos();

            if (oServiceDtoIngresos != null)
            {
                oDtoIngresos.Status = oServiceDtoIngresos.Status;
                oDtoIngresos.Codigo = oServiceDtoIngresos.Codigo;
            }

            return oDtoIngresos;
        }

        internal static List<DtoLogMovimiento> FromDataTransferObjects(IList<ServiceDtoLogMovimiento> oServiceDtoLogMovimiento)
        {
            if (oServiceDtoLogMovimiento == null)
                return null;

            return oServiceDtoLogMovimiento.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoLogMovimiento FromDataTransferObject(ServiceDtoLogMovimiento oServiceDtoLogMovimiento)
        {
            DtoLogMovimiento oDtoLogMovimiento = new DtoLogMovimiento();

            if (oServiceDtoLogMovimiento != null)
            {
                oDtoLogMovimiento.Accion = oServiceDtoLogMovimiento.Accion;
                oDtoLogMovimiento.Acumulado = oServiceDtoLogMovimiento.Acumulado;
                oDtoLogMovimiento.Cantidad = oServiceDtoLogMovimiento.Cantidad;
                oDtoLogMovimiento.Denominacion = oServiceDtoLogMovimiento.Denominacion;
                oDtoLogMovimiento.Id = oServiceDtoLogMovimiento.Id;
                oDtoLogMovimiento.IdCajero = oServiceDtoLogMovimiento.IdCajero;
                oDtoLogMovimiento.IdMovimiento = oServiceDtoLogMovimiento.IdMovimiento;
                oDtoLogMovimiento.Parte = oServiceDtoLogMovimiento.Parte;
                oDtoLogMovimiento.Valor = oServiceDtoLogMovimiento.Valor;
            }

            return oDtoLogMovimiento;
        }

        internal static List<DtoTransacciones> FromDataTransferObjects(IList<ServiceDtoTransacciones> oServiceDtoTransacciones)
        {
            if (oServiceDtoTransacciones == null)
                return null;

            return oServiceDtoTransacciones.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoTransacciones FromDataTransferObject(ServiceDtoTransacciones oServiceDtoTransacciones)
        {
            DtoTransacciones oDtoTransacciones = new DtoTransacciones();

            if (oServiceDtoTransacciones != null)
            {
                oDtoTransacciones.IdTransaccion = oServiceDtoTransacciones.IdTransaccion;
            }

            return oDtoTransacciones;
        }

        internal static List<DtoTarjetas> FromDataTransferObjects(IList<ServiceDtoTarjetas> oServiceDtoTarjetas)
        {
            if (oServiceDtoTarjetas == null)
                return null;

            return oServiceDtoTarjetas.Select(c => FromDataTransferObject(c)).ToList();
        }
        internal static DtoTarjetas FromDataTransferObject(ServiceDtoTarjetas oServiceDtoTarjetas)
        {
            DtoTarjetas oDtoTarjetas = new DtoTarjetas();

            if (oServiceDtoTarjetas != null)
            {
                oDtoTarjetas.Estado = oServiceDtoTarjetas.Estado;
                oDtoTarjetas.IdEstacionamiento = oServiceDtoTarjetas.IdEstacionamiento;
                oDtoTarjetas.IdTarjeta = oServiceDtoTarjetas.IdTarjeta;
            }

            return oDtoTarjetas;
        }
    }
}
