using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor
{
    public class EntityHelper
    {
        public void UpdateProjectTree()
        {
            App.MainViewModel.TreeRoot = null;
            if (ErlangEditor.Core.SolutionUtil.Solution == null)
            {
                return;
            }
            MakeTreeLoop(null,ErlangEditor.Core.SolutionUtil.Solution);
        }
        private void MakeTreeLoop(ViewModel.PrjTreeItemVM aParentVM, object aNode)
        {
            var node = new ViewModel.PrjTreeItemVM(aNode);
            if (aNode is ErlangEditor.Core.Entity.SolutionEntity)
            {
                var sln = aNode as ErlangEditor.Core.Entity.SolutionEntity;
                node.DisplayText = sln.Name;
                node.Children = new System.Collections.ObjectModel.ObservableCollection<ViewModel.PrjTreeItemVM>();
            }
            if (aNode is ErlangEditor.Core.Entity.ApplicationEntity)
            {
                var app = aNode as ErlangEditor.Core.Entity.ApplicationEntity;
                node.DisplayText = app.Name;
                node.Children = new System.Collections.ObjectModel.ObservableCollection<ViewModel.PrjTreeItemVM>();
            }
            if (aNode is ErlangEditor.Core.Entity.FolderEntity)
            {
                var fld = aNode as ErlangEditor.Core.Entity.FolderEntity;
                node.DisplayText = fld.Name;
                node.Children = new System.Collections.ObjectModel.ObservableCollection<ViewModel.PrjTreeItemVM>();
            }
            if (aNode is ErlangEditor.Core.Entity.FileEntity)
            {
                var fle = aNode as ErlangEditor.Core.Entity.FileEntity;
                node.DisplayText = fle.DisplayName;
            }
            node.Entity = aNode;
            if (aParentVM == null)
            {
                App.MainViewModel.TreeRoot = new System.Collections.ObjectModel.ObservableCollection<ViewModel.PrjTreeItemVM>();
                App.MainViewModel.TreeRoot.Add(node);
            }
            else
            {
                aParentVM.Children.Add(node);
            }
            if (aNode is ErlangEditor.Core.Entity.SolutionEntity)
            {
                var sln = aNode as ErlangEditor.Core.Entity.SolutionEntity;
                foreach (var i in sln.Apps)
                {
                    MakeTreeLoop(node, i);
                }
            }
            if (aNode is ErlangEditor.Core.Entity.ApplicationEntity)
            {
                var app = aNode as ErlangEditor.Core.Entity.ApplicationEntity;
                foreach (var i in app.Folders)
                {
                    MakeTreeLoop(node, i);
                }
                foreach (var i in app.Files)
                {
                    MakeTreeLoop(node, i);
                }
            }
            if (aNode is ErlangEditor.Core.Entity.FolderEntity)
            {
                var fld = aNode as ErlangEditor.Core.Entity.FolderEntity;
                foreach (var i in fld.Files)
                {
                    MakeTreeLoop(node, i);
                }
            }
        }
    }
}
