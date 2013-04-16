using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErlangEditor.Core.Entity;

namespace ErlangEditor.ViewModel
{
    public class SolutionVM
    {
        private SolutionEntity entity_;
        public SolutionVM() { }
        public SolutionVM(SolutionEntity aEntity) { entity_ = aEntity; }
        public SolutionEntity Entity
        {
            get { return entity_; }
            private set { entity_ = value; }
        }
    }
}
