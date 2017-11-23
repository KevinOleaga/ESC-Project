using ESC_Project.DAL.Interfaces;
using ESC_Project.DATA;
using ServiceStack.OrmLite;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ESC_Project.DAL.Metodos
{
    public class MUsuarios : IUsuarios
    {
        private OrmLiteConnectionFactory _conexion;
        private IDbConnection _db;
        private IHerramientas _herramientas;

        public MUsuarios()
        {
            _herramientas = new MHerramientas();
            _conexion = new OrmLiteConnectionFactory(BD.Default.conexion, SqlServerDialect.Provider);
            _db = _conexion.Open();
        }

        public bool Login_01(string Usuario)
        {
            var res = false;
            try
            {
                if (_db.Select<TB_Usuarios>(x => x.USUARIO == Usuario).Count() == 1)
                {
                    res = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> Login_01(). \nDescripción: " + ex.Message);
            }
            return res;
        }

        public bool Login_02(string Usuario, string Password)
        {
            var res = false;
            try
            {
                if (_db.Select<TB_Usuarios>(x => x.USUARIO == Usuario && x.PASS == Password).Count() == 1)
                {
                    res = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> Login_02(). \nDescripción: " + ex.Message);
            }
            return res;
        }

        public bool CuentaBloqueada(string Usuario)
        {
            var estado = true;
            TB_Usuarios[] res = { };

            try
            {
                res = _db.Select<TB_Usuarios>(x => x.USUARIO == Usuario).ToArray();
                estado = res[0].CUENTA_BLOQUEADA;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> CuentaBloqueada(). \nDescripción: " + ex.Message);
            }
            return estado;
        }

        public bool CuentaDesactivada(string Usuario)
        {
            var estado = true;
            TB_Usuarios[] res = { };

            try
            {
                res = _db.Select<TB_Usuarios>(x => x.USUARIO == Usuario).ToArray();
                estado = res[0].CUENTA_DESACTIVADA;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> CuentaDesactivada(). \nDescripción: " + ex.Message);
            }
            return estado;
        }

        public void EliminarIntentos(string Usuario)
        {
            try
            {
                var SQL = string.Format("UPDATE TB_USUARIOS SET INTENTOS_FALLIDOS = 0 WHERE USUARIO = '{0}'", Usuario);
                _db.ExecuteSql(SQL);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> EliminarIntentos(). \nDescripción: " + ex.Message);
            }
        }

        public void CrearIntentoFallido(string Usuario)
        {
            try
            {
                var SQL = string.Format("UPDATE TB_USUARIOS SET INTENTOS_FALLIDOS = INTENTOS_FALLIDOS + 1 WHERE USUARIO = '{0}'", Usuario);
                _db.ExecuteSql(SQL);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> CrearIntentoFallido(). \nDescripción: " + ex.Message);
            }
        }

        public int ObtenerIntentosFallidos(string Usuario)
        {
            TB_Usuarios[] res = { };
            var intentos = 0;

            try
            {
                res = _db.Select<TB_Usuarios>(x => x.USUARIO == Usuario).ToArray();
                intentos = res[0].INTENTOS_FALLIDOS;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> ObtenerIntentosFallidos(). \nDescripción: " + ex.Message);
            }
            return intentos;
        }

        public void BloquearCuenta(string Usuario)
        {
            try
            {
                var SQL = string.Format("UPDATE TB_USUARIOS SET CUENTA_BLOQUEADA = 1 WHERE USUARIO = '{0}'", Usuario);
                _db.ExecuteSql(SQL);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> BloquearCuenta(). \nDescripción: " + ex.Message);
            }
        }

        public bool Existe(string Usuario)
        {
            var res = false;
            try
            {
                if (_db.Select<TB_Usuarios>(x => x.USUARIO == Usuario).Count() == 1)
                {
                    res = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> Existe(). \nDescripción: " + ex.Message);
            }
            return res;
        }

        public string ObtenerEmail(string Usuario)
        {
            TB_Usuarios[] res = { };
            var email = string.Empty;

            try
            {
                res = _db.Select<TB_Usuarios>(x => x.USUARIO == Usuario).ToArray();
                email = res[0].EMAIL;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> ObtenerEmail(). \nDescripción: " + ex.Message);
            }
            return email;
        }

        public void RestaurarPassword(string Email, string Body, string Usuario)
        {
            var NewLink = "http://localhost:52635/ResetPassword.aspx?U=" + Usuario;
            var NewSecretCode = ActualizarCodigo(Usuario);

            var SystemEmail = "PruebaOleaga@outlook.com";
            var SystemPassword = "Oleaga1234";

            try
            {
                SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
                MailMessage email = new MailMessage();

                email.From = new MailAddress(SystemEmail, "Colegio Vocacional Monseñor Sanabria");
                email.Body = "Se ha solicitado la restauración de su contraseña. \nSi presenta problemas para iniciar sesión comuniquese con el administrador.";
                email.Subject = "Restauración de Contraseña";
                email.To.Add(Email);
                Body = Body.Replace("{NewLink}", NewLink);
                Body = Body.Replace("{NewCode}", NewSecretCode);
                email.Body = Body;
                email.IsBodyHtml = true;

                client.Credentials = new NetworkCredential(SystemEmail, SystemPassword);
                client.EnableSsl = true;
                client.Send(email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuarios -> RestaurarPassword(). \nDescripción: " + ex.Message);
            }
        }

        public string ActualizarCodigo(string Usuario)
        {
            var NuevoCodSecreto = CrearCodigo();

            try
            {
                var SQL = string.Format("UPDATE TB_USUARIOS SET CODIGO = '{0}' WHERE USUARIO = '{1}'", _herramientas.Encrypt(NuevoCodSecreto), Usuario);
                _db.ExecuteSql(SQL);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> ActualizarCodigo(). \nDescripción: " + ex.Message);
            }
            return NuevoCodSecreto;
        }

        public string CrearCodigo()
        {
            // Password Size
            int length = 8;
            var res = "";

            try
            {
                const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                StringBuilder SB = new StringBuilder();
                Random rnd = new Random();

                while (0 < length--)
                {
                    SB.Append(valid[rnd.Next(valid.Length)]);
                }
                res = SB.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuarios -> CrearPassword(). \nDescripción: " + ex.Message);
            }
            return res;
        }

        public bool VerificarCodigo(string Usuario, string SecretCode)
        {
            var res = false;

            try
            {

                if (_db.Select<TB_Usuarios>(x => x.USUARIO == Usuario && x.CODIGO == SecretCode).Count() == 1)
                {
                    res = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuarios -> VerificarCodigo(). \nDescripción: " + ex.Message);
            }
            return res;
        }

        public void ResetCodigo(string Usuario)
        {
            var reset = CrearCodigo();

            try
            {
                var SQL = string.Format("UPDATE TB_USUARIOS SET CODIGO = '{0}' WHERE USUARIO = '{1}'", _herramientas.Encrypt(reset), Usuario);
                _db.ExecuteSql(SQL);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuarios -> ResetCodigo(). \nDescripción: " + ex.Message);
            }
        }

        public void ActualizarPassword(string Usuario, string Password)
        {
            try
            {
                var SQL = string.Format("UPDATE TB_USUARIOS SET PASS = '{0}' WHERE USUARIO = '{1}'", Password, Usuario);
                _db.ExecuteSql(SQL);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> ActualizarPassword(). \nDescripción: " + ex.Message);
            }
        }

        public void ActivarCuenta(string Usuario)
        {
            try
            {
                var SQL = string.Format("UPDATE TB_USUARIOS SET CUENTA_BLOQUEADA = 0 WHERE USUARIO = '{0}'", Usuario);
                _db.ExecuteSql(SQL);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> ActivarCuenta(). \nDescripción: " + ex.Message);
            }
        }

        // METODO DE EJEMPLO
        public TB_Usuarios[] ObtenerInfo(string Usuario)
        {
            TB_Usuarios[] res = { };

            try
            {
                res = _db.Select<TB_Usuarios>(x => x.USUARIO == Usuario).ToArray();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> ObtenerInfo(). \nDescripción: " + ex.Message);
            }
            return res;
        }

        public TB_Roles[] ObtenerRoles()
        {
            TB_Roles[] res = { };

            try
            {
                res = _db.Select<TB_Roles>().ToArray();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> ObtenerRoles(). \nDescripción: " + ex.Message);
            }
            return res;
        }

        public TB_Usuarios[] ObtenerInfoGeneral()
        {
            TB_Usuarios[] res = { };

            try
            {
                res = _db.Select<TB_Usuarios>().ToArray();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> ObtenerInfoGeneral(). \nDescripción: " + ex.Message);
            }
            return res;
        }

        public int TotalUsuarios()
        {
            var res = 0;
            try
            {
                res = _db.Select<TB_Usuarios>().Count();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> TotalUsuarios(). \nDescripción: " + ex.Message);
            }
            return res;
        }

        public string ObtenerRol(int ID_ROL)
        {
            TB_Roles[] res = { };
            var rol = string.Empty;

            try
            {
                res = _db.Select<TB_Roles>(x => x.ID_ROL == ID_ROL).ToArray();
                rol = res[0].NOMBRE;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> ObtenerRol(). \nDescripción: " + ex.Message);
            }
            return rol;
        }
        
        public void CrearUsuario(TB_Usuarios usuario)
        {
            try
            {
                _db.Insert(usuario);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nError \nUbicación: Capa DAL -> MUsuario -> ActualizarPassword(). \nDescripción: " + ex.Message);
            }
        }
    }
}
