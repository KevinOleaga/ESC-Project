namespace ESC_Project.BLL.Interfaces
{
    public interface IHerramientas
    {
        #region Encryption

        // Encrypt()
        string Encrypt(string data);

        // Decrypt()
        string Decrypt(string data);

        #endregion Encryption

        // Capitalize()
        string Capitalize(string text);
    }
}
