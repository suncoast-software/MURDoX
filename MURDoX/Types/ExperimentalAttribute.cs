using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Types
{
    /// <summary>
    ///     Denotes this command is experimental, and may not work properly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExperimentalAttribute : Attribute { }

    public static class ExperimentalAttributeExtensions
    {
        public static bool IsExperimental(this Command c)
        {
            return c.CustomAttributes.OfType<ExperimentalAttribute>().Any();
        }
    }
}
