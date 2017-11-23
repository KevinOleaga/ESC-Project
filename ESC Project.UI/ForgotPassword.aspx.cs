using ESC_Project.BLL.Interfaces;
using ESC_Project.BLL.Metodos;
using System;
using System.IO;

namespace ESC_Project.UI
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private static string _Usuario;
        private IUsuarios _usuarios;
        private IHerramientas _herramientas;

        public ForgotPassword()
        {
            _usuarios = new MUsuarios();
            _herramientas = new MHerramientas();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lbError.Visible = false;
        }

        protected void btnRestoreAccount_Click(object sender, EventArgs e)
        {
            _Usuario = txtUser.Text.ToUpper();

            if (_Usuario == string.Empty)
            {
                lbError.Visible = true;
                lbError.Text = "Debe digitar un usuario";
            }
            else
            {
                _Usuario = _herramientas.Encrypt(_Usuario);
                switch (_usuarios.Existe(_Usuario))
                {
                    case true:
                        lbError.Visible = false;

                        _usuarios.RestaurarPassword(_usuarios.ObtenerEmail(_Usuario), CreateBody(), _Usuario);
                        Response.Redirect("Login.aspx");
                        break;
                    case false:
                        lbError.Visible = true;
                        lbError.Text = "Usuario inválido";
                        break;
                }
            }
        }

        private string CreateBody()
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(MapPath("EmailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }
    }
}