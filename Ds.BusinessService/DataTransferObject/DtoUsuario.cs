using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoUsuario", Namespace = "http://www.eglobalt.com/types/")]
    public class DtoUsuario
    {
        private int _IdUsuario;

        [DataMember]
        public int IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }

        private string _Nombres;

        [DataMember]
        public string Nombres
        {
            get { return _Nombres; }
            set { _Nombres = value; }
        }

        private string _Apellido;

        [DataMember]
        public string Apellido
        {
            get { return _Apellido; }
            set { _Apellido = value; }
        }

        private string _Password;

        [DataMember]
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private string _Cargo;

        [DataMember]
        public string Cargo
        {
            get { return _Cargo; }
            set { _Cargo = value; }
        }

        private int _Perfil;

        [DataMember]
        public int Perfil
        {
            get { return _Perfil; }
            set { _Perfil = value; }
        }

        private string _Empresa;

        [DataMember]
        public string Empresa
        {
            get { return _Empresa; }
            set { _Empresa = value; }
        }

        private List<DtoPerfil> _lstDtoPerfil = new List<DtoPerfil>();

        [DataMember]
        public List<DtoPerfil> lstDtoPerfil
        {
            get { return _lstDtoPerfil; }
            set { _lstDtoPerfil = value; }
        }

    }
}
