namespace OrderOperations.WebApi.Services;

public class ConsoleLogger : ILoggerService
{
    public void Write(params object[] messageParams)
    {
        string localLogMsg = "";
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write("[Console Logger] ");
        Console.Write(DateTime.UtcNow);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" | ");
        for (int i = 0; i < messageParams.Length; i += 2)
        {
#pragma warning disable CS8600 // Null sabit değeri veya olası null değeri, boş değer atanamaz türe dönüştürülüyor.
            string text = messageParams[i + 1].ToString();
#pragma warning restore CS8600 // Null sabit değeri veya olası null değeri, boş değer atanamaz türe dönüştürülüyor.
            localLogMsg += text + " ";
            Console.ForegroundColor = (ConsoleColor)messageParams[i];
            Console.Write(text);
        }

        Console.ResetColor();
        Console.WriteLine();
    }
}
