namespace WebApiSignalR
{
    public class FileHelper
    {
        public static void Write(double data)
        {
            File.WriteAllText("data.txt", data.ToString());
        }

        public static void Write(string room,double data)
        {
            File.WriteAllText(room+".txt", data.ToString());
        }

        public static double Read()
        {
            return double.Parse(File.ReadAllText("data.txt"));
        }
        public static double Read(string room)
        {
            return double.Parse(File.ReadAllText(room+".txt"));
        }
    }
}
