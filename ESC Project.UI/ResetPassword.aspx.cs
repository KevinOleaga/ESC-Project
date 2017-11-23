using ESC_Project.BLL.Interfaces;
using ESC_Project.BLL.Metodos;
using System;

namespace ESC_Project.UI
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private static string _Usuario;
        private IUsuarios _usuarios;
        private IHerramientas _herramientas;

        public ResetPassword()
        {
            _usuarios = new MUsuarios();
            _herramientas = new MHerramientas();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            divPassword.Visible = false;
            divPassword2.Visible = false;
            btnResetPassword.Visible = false;
            lbError.Visible = false;
        }

        protected void btnVerificar_Click(object sender, EventArgs e)
        {
            _Usuario = Request.QueryString["U"];
            var SecretCode = _herramientas.Encrypt(txtCode.Text);

            if (SecretCode == string.Empty)
            {
                lbError.Visible = true;
                lbError.Text = "Debe digitar un código";
            }
            else
            {
                switch (_usuarios.VerificarCodigo(_Usuario, SecretCode))
                {
                    case true:
                        _usuarios.ResetCodigo(_Usuario);

                        divPassword.Visible = true;
                        divPassword2.Visible = true;
                        btnResetPassword.Visible = true;

                        divSecretCode.Visible = false;
                        btnVerificar.Visible = false;

                        txtCode.Text = string.Empty;
                        lbError.Visible = false;
                        break;
                    case false:
                        _usuarios.ResetCodigo(_Usuario);

                        lbError.Text = "Código Incorrecto";
                        lbError.Visible = true;
                        break;
                }
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            var Password = txtPassword.Text.ToUpper();
            var Password2 = txtPassword2.Text.ToUpper();

            if (Password == string.Empty || Password2 == string.Empty)
            {
                lbError.Visible = true;
                lbError.Text = "Verifique que los campos no se encuentran vacios.";

                divPassword.Visible = true;
                divPassword2.Visible = true;
                btnResetPassword.Visible = true;
            }
            else
            {
                if (Password == Password2)
                {
                    Password = _herramientas.Encrypt(Password);
                    _usuarios.ActualizarPassword(_Usuario, Password);
                    _usuarios.EliminarIntentos(_Usuario);
                    _usuarios.ActivarCuenta(_Usuario);

                    Response.Redirect("Login.aspx");
                }
                else
                {
                    divPassword.Visible = true;
                    divPassword2.Visible = true;
                    btnResetPassword.Visible = true;

                    lbError.Visible = true;
                    lbError.Text = "Las contraseñas no coinciden";
                }
            }
        }
    }
}