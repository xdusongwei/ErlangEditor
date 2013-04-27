﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ErlangEditor.ViewModel;
using ErlangEditor.CompilerProxy;

namespace ErlangEditor
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static MainViewModel mainViewModel_ = new MainViewModel();
        public static MainViewModel ViewModel
        {
            get { return mainViewModel_; }
        }

        private static SolutionCompiler solutionCompiler_ = new SolutionCompiler();
        public static SolutionCompiler CompilerProxy
        {
            get { return solutionCompiler_; }
        }
    }
}
