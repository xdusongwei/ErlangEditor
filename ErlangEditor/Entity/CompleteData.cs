using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Entity
{
    public class CompletionData : ICompletionData
    {
        public CompletionData(bool aAcAgain)
        {
            autocompleteAgain_ = aAcAgain;
        }

        private bool autocompleteAgain_;

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public string Text { get; set; }

        public object Content
        {
            get;
            set;
        }

        public double Priority { get { return 1.0; } }

        public object Description
        {
            get;
            set;
        }

        public void Complete(TextArea textArea, ISegment completionSegment,
             EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
            if (autocompleteAgain_)
            {
                var completionWindow = new CompletionWindow(textArea);
                completionWindow.Width = 256;
                IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;

                var functions = App.MainViewModel.AutoCompleteCache.GetFunctions(Content as string);
                (functions as List<ErlangEditor.AutoComplete.AcEntity>).Sort(new Tools.Reverser<ErlangEditor.AutoComplete.AcEntity>(new AutoComplete.AcEntity().GetType(), "FunctionName", Tools.ReverserInfo.Direction.ASC));
                foreach (var i in functions)
                {
                    data.Add(new CompletionData(false) { Text = "\'" + i.FunctionName + "\'(", Content = i.FunctionName + "/" + i.Arity, Description = i.Desc });
                }
                completionWindow.Show();
                completionWindow.Closed += delegate
                {
                    completionWindow = null;
                };
            }
        }
    }
}
