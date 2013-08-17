using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ErlangEditor.Core.UnitTest
{
    public static class TemplateTest
    {
        public static void Test()
        {
            //add .app file
            FileUtil.AddFile("hello", string.Empty, new Entity.FileEntity { Name = "hello.app" },
                ErlangEditor.Template.TemplateUtil.MakeAppFile("1.0.0", "hello", "this is the app description", new string[] { "x_y", "a_b" }, new string[] { "a_b" }, new string[] { "kernel", "stdlib" }, "a_b", "[]"));
            //add application code file
            FileUtil.AddFile("hello", "src", new Entity.FileEntity { Name = "a_b.erl", DisplayName = "a_b" },
                ErlangEditor.Template.TemplateUtil.MakeApplicationCode("a_b", "test_sup:startk_link()"));
            //add normal erlang code file
            FileUtil.AddFile("hello", "src", new Entity.FileEntity { Name = "x_y.erl", DisplayName = "x_y" },
                ErlangEditor.Template.TemplateUtil.MakeErlangCode("x_y"));
            //add normal header code file
            FileUtil.AddFile("hello", "include", new Entity.FileEntity { Name = "h.hrl", DisplayName = "h" },
                ErlangEditor.Template.TemplateUtil.MakeHeaderCode());
        }
    }
}
