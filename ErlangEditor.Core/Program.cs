using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitTest.SolutionTest.Test();
            UnitTest.ApplicationTest.Test();
            UnitTest.FileTest.Test();
            UnitTest.TemplateTest.Test();
        }
    }
}
