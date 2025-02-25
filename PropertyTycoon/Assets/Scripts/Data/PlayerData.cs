namespace Data
{
    public class PlayerData
    {
        public string Name;
        public bool IsAI;
        public Token Token;

        public PlayerData(string name, Token token, bool isAI = false)
        {
            Name = name;
            Token = token;
            IsAI = isAI;
        }
    }
}