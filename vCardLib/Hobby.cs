namespace vCardLib
{
    public class Hobby
    {
        public string Activity { get; set; }

        public Level Level { get; set; }
    }


    public enum Level
    {
        High,
        Medium,
        Low
    }
}
