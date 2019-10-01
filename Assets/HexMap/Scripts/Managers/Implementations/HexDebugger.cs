using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    [RegisterDependency(typeof(IHexDebugger), true)]
    public class HexDebugger : IHexDebugger
    {
        public PublicReferences PublicReferences;

        public HexDebugger(PublicReferences PublicReferences)
        {
            this.PublicReferences = PublicReferences;
        }

        public void Start()
        {
            var parent = new GameObject("Debug Text").transform;

            for (int i = -20; i <= 20; i++)
            {
                for (int j = -20; j <= 20; j++)
                {
                    var pos = new int2(i, j);
                    var hex = new HexCell(pos);

                    var go = GameObject.Instantiate(PublicReferences.DebugCellIndexText,
                        hex.WorldPosition + new Vector3(0, 0.1f, 0),
                        PublicReferences.DebugCellIndexText.transform.rotation, parent);

                    go.GetComponentInChildren<Text>().text = pos.x + "." + pos.y;
                }
            }
        }
    }
}
