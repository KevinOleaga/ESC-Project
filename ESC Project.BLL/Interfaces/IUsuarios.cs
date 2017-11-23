using ESC_Project.DATA;

namespace ESC_Project.BLL.Interfaces
{
    public interface IUsuarios
    {
        bool Login_01(string Usuario);

        bool Login_02(string Usuario, string Password);

        bool CuentaBloqueada(string Usuario);

        bool CuentaDesactivada(string Usuario);

        void EliminarIntentos(string Usuario);

        void CrearIntentoFallido(string Usuario);

        int ObtenerIntentosFallidos(string Usuario);

        void BloquearCuenta(string Usuario);

        bool Existe(string Usuario);

        string ObtenerEmail(string Usuario);

        void RestaurarPassword(string Email, string Body, string Usuario);

        string ActualizarCodigo(string Usuario);

        string CrearCodigo();

        bool VerificarCodigo(string Usuario, string SecretCode);

        void ResetCodigo(string Usuario);

        void ActualizarPassword(string Usuario, string Password);

        void ActivarCuenta(string Usuario);

        TB_Usuarios[] ObtenerInfo(string Usuario);

        TB_Roles[] ObtenerRoles();

        TB_Usuarios[] ObtenerInfoGeneral();

        int TotalUsuarios();

        string ObtenerRol(int ID_ROL);

        void CrearUsuario(TB_Usuarios usuario);
    }
}
