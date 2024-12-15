namespace MainProgram.Interfaces
{
    public interface IAuthSettings
    {
        public string Issuer { get; }
        public string Audience { get; }
        public string Key { get; }
        public int TokenExpiresAfterHours { get; }
    }
}
