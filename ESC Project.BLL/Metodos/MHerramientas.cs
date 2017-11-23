using ESC_Project.BLL.Interfaces;

namespace ESC_Project.BLL.Metodos
{
    public class MHerramientas : IHerramientas
    {
        private DAL.Interfaces.IHerramientas _Herramientas;
        public MHerramientas()
        {
            _Herramientas = new DAL.Metodos.MHerramientas();
        }
        
        #region Encryption

        public string Encrypt(string data)
        {
            return _Herramientas.Encrypt(data);
        }

        public string Decrypt(string data)
        {
            return _Herramientas.Decrypt(data);
        }

        #endregion Encryption

        // Capitalize()
        public string Capitalize(string text)
        {
            return _Herramientas.Capitalize(text);
        }
    }
}
