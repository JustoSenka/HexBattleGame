using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class TurnOrderIndicator : MonoBehaviour
    {
        [HideInInspector]
        public RectTransform rect;

        public int turnOrder;

        public Image image;
        public Text text;

        public void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        public void OnMouseEnter()
        {
            
        }

        public void OnMouseLeave()
        {

        }
    }
}
