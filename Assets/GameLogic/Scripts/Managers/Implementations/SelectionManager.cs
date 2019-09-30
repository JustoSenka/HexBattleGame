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
        private readonly IMouseManager MouseManager;
        private readonly IHexHighlighter HexHighlighter;

        public SelectionManager(IMouseManager MouseManager, IHexHighlighter HexHighlighter)
        {
            this.MouseManager = MouseManager;
            this.HexHighlighter = HexHighlighter;

            MouseManager.MouseReleased += OnMouseReleased;
            MouseManager.SelectableClicked += OnSelectableClicked;
        }

        private void OnMouseReleased()
        {
            //throw new NotImplementedException();
        }

        private void OnSelectableClicked(Selectable obj)
        {
            //throw new NotImplementedException();
        }

        public void Update()
        {

        }
    }
}
