namespace DiscordBot.Tools
{
    public static class Tools
    {
        public static string IntToEmoji(int number)
        {
            switch (number)
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                case 10:
                    return "ivan_lebof";
                case 11:
                    return "nico_lebg";
                case 12:
                    return "henri_lebg";
                case 13:
                    return "waouh";
                case 14:
                    return "mdr";
                case 15:
                    return "haha";
                default:
                    return "Nan";
            }
        }
    }
}