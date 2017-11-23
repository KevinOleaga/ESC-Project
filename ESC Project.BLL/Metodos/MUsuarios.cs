using ESC_Project.BLL.Interfaces;
using ESC_Project.DATA;

namespace ESC_Project.BLL.Metodos
{
    public class MUsuarios : IUsuarios
    {
        private DAL.Interfaces.IUsuarios _usu;
        public MUsuarios()
        {
            _usu = new DAL.Metodos.MUsuarios();
        }

        public bool Login_01(string Usuario)
        {
            return _usu.Login_01(Usuario);
        }

        public bool Login_02(string Usuario, string Password)
        {
            return _usu.Login_02(Usuario, Password);
        }

        public bool CuentaBloqueada(string Usuario)
        {
            return _usu.CuentaBloqueada(Usuario);
        }

        public bool CuentaDesactivada(string Usuario)
        {
            return _usu.CuentaDesactivada(Usuario);
        }

        public void EliminarIntentos(string Usuario)
        {
            _usu.EliminarIntentos(Usuario);
        }

        public void CrearIntentoFallido(string Usuario)
        {
            _usu.CrearIntentoFallido(Usuario);
        }

        public int ObtenerIntentosFallidos(string Usuario)
        {
            return _usu.ObtenerIntentosFallidos(Usuario);
        }

        public void BloquearCuenta(string Usuario)
        {
            _usu.BloquearCuenta(Usuario);
        }

        public bool Existe(string Usuario)
        {
            return _usu.Existe(Usuario);
        }

        public string ObtenerEmail(string Usuario)
        {
            return _usu.ObtenerEmail(Usuario);
        }

        public void RestaurarPassword(string Email, string Body, string Usuario)
        {
            _usu.RestaurarPassword(Email, Body, Usuario);
        }

        public string ActualizarCodigo(string Usuario)
        {
            return _usu.ActualizarCodigo(Usuario);
        }

        public string CrearCodigo()
        {
            return _usu.CrearCodigo();
        }

        public bool VerificarCodigo(string Usuario, string SecretCode)
        {
            return _usu.VerificarCodigo(Usuario, SecretCode);
        }

        public void ResetCodigo(string Usuario)
        {
            _usu.ResetCodigo(Usuario);
        }

        public void ActualizarPassword(string Usuario, string Password)
        {
            _usu.ActualizarPassword(Usuario, Password);
        }

        public void ActivarCuenta(string Usuario)
        {
            _usu.ActivarCuenta(Usuario);
        }

        public TB_Usuarios[] ObtenerInfo(string Usuario)
        {
            return _usu.ObtenerInfo(Usuario);
        }

        public TB_Roles[] ObtenerRoles()
        {
            return _usu.ObtenerRoles();
        }

        public TB_Usuarios[] ObtenerInfoGeneral()
        {
            return _usu.ObtenerInfoGeneral();
        }

        public int TotalUsuarios()
        {
            return _usu.TotalUsuarios();
        }

        public string ObtenerRol(int ID_ROL)
        {
            return _usu.ObtenerRol(ID_ROL);
        }

        public void CrearUsuario(TB_Usuarios usuario)
        {
            _usu.CrearUsuario(usuario);
        }
    }
}