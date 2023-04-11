using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester.Presentation
{
    public class ConsoleManager
    {
        public virtual void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
