using ESC_Project.BLL.Interfaces;
using ESC_Project.BLL.Metodos;
using System;
using System.Web.Security;

namespace ESC_Project.UI
{
    public partial class Login1 : System.Web.UI.Page
    {
        private static string _Usuario, _Password;
        private IUsuarios _usu;
        private IHerramientas _herra;

        public Login1()
        {
            _usu = new MUsuarios();
            _herra = new MHerramientas();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            divPassword.Visible = false;
            btnPassword.Visible = false;
            btnRestore.Visible = false;
            btnReturn.Visible = false;
            lbError.Visible = false;
        }

        protected void btnUser_Click(object sender, EventArgs e)
        {
            _Usuario = txtUser.Text.ToUpper();

            if (_Usuario == string.Empty)
            {
                lbError.Visible = true;
                lbError.Text = "Debe digitar un usuario";
            }
            else
            {
                _Usuario = _herra.Encrypt(_Usuario);

                switch (_usu.Login_01(_Usuario))
                {
                    case true:
                        var cuentaBloqueada = _usu.CuentaBloqueada(_Usuario);
                        var cuentaDesactivada = _usu.CuentaDesactivada(_Usuario);

                        if (cuentaDesactivada == true)
                        {
                            divUser.Visible = false;
                            btnUser.Visible = false;

                            divPassword.Visible = false;
                            btnPassword.Visible = false;

                            btnReturn.Visible = true;
                            btnRestore.Visible = false;

                            lbError.Visible = true;
                            lbError.Text = "Cuenta Desactivada";

                            link.Visible = false;
                        }
                        else if (cuentaBloqueada == true)
                        {
                            divUser.Visible = false;
                            btnUser.Visible = false;

                            divPassword.Visible = false;
                            btnPassword.Visible = false;

                            btnReturn.Visible = true;
                            btnRestore.Visible = true;

                            lbError.Visible = true;
                            lbError.Text = "Cuenta Bloqueada";

                            link.Visible = false;
                        }

                        else
                        {
                            divUser.Visible = false;
                            btnUser.Visible = false;

                            divPassword.Visible = true;
                            btnPassword.Visible = true;

                            btnReturn.Visible = false;
                            btnRestore.Visible = false;

                            lbError.Visible = false;
                        }
                        break;
                    case false:
                        txtUser.Text = string.Empty;

                        lbError.Visible = true;
                        lbError.Text = "Usuario Inválido";
                        break;
                }
            }
        }

        protected void btnPassword_Click(object sender, EventArgs e)
        {
            _Password = txtPassword.Text.ToUpper();

            if (_Password == string.Empty)
            {
                divPassword.Visible = true;
                btnPassword.Visible = true;

                lbError.Visible = true;
                lbError.Text = "Debe digitar una contraseña";

                link.Visible = true;
            }
            else
            {
                _Password = _herra.Encrypt(_Password);

                switch (_usu.Login_02(_Usuario, _Password))
                {
                    case true:
                        var cuentaBloqueada = _usu.CuentaBloqueada(_Usuario);

                        if (cuentaBloqueada == false)
                        {
                            _usu.EliminarIntentos(_Usuario);

                            FormsAuthentication.RedirectFromLoginPage(_Usuario, true);
                            Response.Redirect("Index.aspx");
                        }
                        else if (cuentaBloqueada == true)
                        {
                            divUser.Visible = false;
                            btnUser.Visible = false;

                            divPassword.Visible = false;
                            btnPassword.Visible = false;

                            btnReturn.Visible = true;
                            btnRestore.Visible = true;

                            lbError.Visible = true;
                            lbError.Text = "Cuenta Bloqueada";

                            link.Visible = false;
                        }
                        break;
                    case false:
                        cuentaBloqueada = _usu.CuentaBloqueada(_Usuario);
                        var intentos = 0;

                        if (cuentaBloqueada == false)
                        {
                            _usu.CrearIntentoFallido(_Usuario);
                            intentos = _usu.ObtenerIntentosFallidos(_Usuario);

                            if (intentos == 3)
                            {
                                divUser.Visible = false;
                                btnUser.Visible = false;

                                divPassword.Visible = false;
                                btnPassword.Visible = false;

                                btnReturn.Visible = true;
                                btnRestore.Visible = true;

                                lbError.Visible = true;
                                lbError.Text = string.Format("Cuenta bloqueada | Intento ({0}/3)", intentos);

                                link.Visible = false;

                                _usu.BloquearCuenta(_Usuario);
                            }
                            else
                            {
                                lbError.Visible = true;
                                lbError.Text = string.Format("Intento ({0}/3)", intentos);

                                divPassword.Visible = true;
                                btnPassword.Visible = true;
                            }
                        }
                        else if (cuentaBloqueada == true)
                        {
                            intentos = _usu.ObtenerIntentosFallidos(_Usuario);

                            divUser.Visible = false;
                            btnUser.Visible = false;

                            divPassword.Visible = false;
                            btnPassword.Visible = false;

                            btnReturn.Visible = true;
                            btnRestore.Visible = true;

                            lbError.Visible = true;
                            lbError.Text = string.Format("Usuario bloqueado | Intento ({0}/3)", intentos);

                            link.Visible = false;
                        }
                        break;
                }
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            txtUser.Text = string.Empty;

            divUser.Visible = true;
            btnUser.Visible = true;

            link.Visible = true;
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPassword.aspx");
        }
    }
}