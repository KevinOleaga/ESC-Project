namespace ESC_Project.DATA
{
    public class TB_Usuarios
    {
        public string USUARIO { get; set; }

        public string PASS { get; set; }

        public int INTENTOS_FALLIDOS { get; set; }

        public bool CUENTA_BLOQUEADA { get; set; }

        public bool CUENTA_DESACTIVADA { get; set; }

        public string CODIGO { get; set; }

        public string NOMBRE { get; set; }

        public string APELLIDOS { get; set; }

        public string TELEFONO { get; set; }

        public int ID_ROL { get; set; }

        public string EMAIL { get; set; }

        public string CEDULA { get; set; }

        public string FOTO { get; set; }

        public string COMENTARIOS { get; set; }

        public string FRASE { get; set; }
    }
}
