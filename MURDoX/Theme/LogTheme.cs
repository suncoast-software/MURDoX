using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Theme
{
   public sealed class LogTheme : ConsoleTheme
    {
        public override bool CanBuffer => false;

        protected override int ResetCharCount => 0;

        public override void Reset(TextWriter output)
        {
            Console.ResetColor();
        }

        public override int Set(TextWriter output, ConsoleThemeStyle style)
        {
            (ConsoleColor foreground, ConsoleColor background) = style switch
            {
                ConsoleThemeStyle.Scalar => (ConsoleColor.Blue, ConsoleColor.Black), 
                ConsoleThemeStyle.Number => (ConsoleColor.DarkBlue, ConsoleColor.Black),
                ConsoleThemeStyle.LevelDebug => (ConsoleColor.Green, ConsoleColor.Black),
                ConsoleThemeStyle.LevelError => (ConsoleColor.Red, ConsoleColor.Black),
                ConsoleThemeStyle.LevelFatal => (ConsoleColor.Red, ConsoleColor.Black),
                ConsoleThemeStyle.LevelVerbose => (ConsoleColor.Magenta, ConsoleColor.Black),
                ConsoleThemeStyle.LevelWarning => (ConsoleColor.Yellow, ConsoleColor.Black),
                ConsoleThemeStyle.SecondaryText => (ConsoleColor.DarkBlue, ConsoleColor.Black),
                ConsoleThemeStyle.LevelInformation => (ConsoleColor.White, ConsoleColor.Black),
                _ => (ConsoleColor.Yellow, ConsoleColor.Black)
            };
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            return 0;
        }
    }
}
