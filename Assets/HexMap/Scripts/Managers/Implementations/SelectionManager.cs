using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [RegisterDependency(typeof(SelectionManager), true)]
    public class SelectionManager
    {
        private readonly IInputManager InputManager;

        public SelectionManager(IInputManager InputManager)
        {
            this.InputManager = InputManager;
        }
    }
}
