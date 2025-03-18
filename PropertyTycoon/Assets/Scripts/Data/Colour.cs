namespace Data
{
    public class Colour
    {
        public static readonly Colour Brown = new Colour("Brown", new UnityEngine.Color(0.5f, 0.25f, 0f, 0f));
        public static readonly Colour LightBlue = new Colour("Light Blue", UnityEngine.Color.cyan);
        public static readonly Colour Magenta = new Colour("Magenta", UnityEngine.Color.magenta);
        public static readonly Colour Orange = new Colour("Orange", new UnityEngine.Color(1f, 0.5f, 0f, 0f));
        public static readonly Colour Red = new Colour("Red", UnityEngine.Color.red);
        public static readonly Colour Yellow = new Colour("Yellow", UnityEngine.Color.yellow);
        public static readonly Colour Green = new Colour("Green", UnityEngine.Color.green);
        public static readonly Colour DarkBlue = new Colour("Dark Blue", UnityEngine.Color.blue);
            
        public string Name { get; }
        public UnityEngine.Color UnityColour { get; }

        private Colour(string name, UnityEngine.Color unityColour)
        {
            Name = name;
            UnityColour = unityColour;
        }

        public override string ToString() { return Name; }
    }
}