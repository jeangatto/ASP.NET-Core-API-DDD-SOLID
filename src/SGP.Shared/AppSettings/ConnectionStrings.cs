namespace SGP.Shared.AppSettings
{
    public sealed class ConnectionStrings
    {
        /// <summary>
        /// String da conexão padrão.
        /// </summary>
        public string DefaultConnection { get; private set; }

        public static ConnectionStrings Create(string defaultConnection)
        {
            return new ConnectionStrings { DefaultConnection = defaultConnection };
        }
    }
}