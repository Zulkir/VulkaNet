namespace VulkaNetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var inStructGenerator = new InStructGenerator();
            inStructGenerator.Generate<GenApplicationInfo>();
            inStructGenerator.Generate<GenInstanceCreateInfo>();
        }
    }
}
