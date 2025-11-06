namespace BeepTerminal.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");



            Console.WriteLine("");
            Console.WriteLine("-------------------------");

            // loop
            // check if enter from 1 to 9, make a sound - neep with console.readline(beepInAscii)
            // return
            while (true)
            {
                string input = Console.ReadLine();
                if (!(input is null || input is ""))
                {
                    int temp = -1;
                    int.TryParse(input, out temp);
                    if (temp >= 1 && temp <= 9)
                    {
                        for (int i = 0; i < temp; i++)
                        {
                            // make a sound
                            Console.Beep();
                        }
                    }
                    else
                    {
                        Console.WriteLine(input);
                    }
                }              
            }
        }
    }
}
