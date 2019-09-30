using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [RegisterDependency(typeof(UnitHexHighlights), true)]
    public class UnitHexHighlights
    {
        private readonly IMouseManager MouseManager;
        private readonly IHexHighlighter HexHighlighter;



        public UnitHexHighlights(IMouseManager MouseManager, IHexHighlighter HexHighlighter)
        {
            this.MouseManager = MouseManager;
            this.HexHighlighter = HexHighlighter;

            MouseManager.MouseReleased += OnMouseReleased;
            MouseManager.SelectableSelected += OnSelectableClicked;
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
