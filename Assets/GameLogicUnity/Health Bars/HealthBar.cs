using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(UnitBehaviour))]
    public class HealthBar : MonoBehaviour
    {
        [System.NonSerialized]
        public bool ShowBar = true;

        public Canvas Canvas;
        public RectTransform HealthImage;
        public RectTransform ManaImage;

        private UnitBehaviour m_Unit;

        private bool m_LastShowBar;
        private int m_LastHp;
        private int m_LastMp;

        void Start()
        {
            m_Unit = transform.parent.GetComponent<UnitBehaviour>();
            Canvas.transform.localPosition = new Vector3(0, m_Unit.Height, 0);
            Canvas.enabled = m_LastShowBar;
        }

        void Update()
        {
            if (HealthImage && m_Unit.Unit.Health != m_LastHp)
            {
                var scale = HealthImage.localScale;
                scale.x = m_Unit.Unit.Health / m_Unit.Unit.MaxHealth;
                if (scale.x < 0) scale.x = 0;
                HealthImage.localScale = scale;
                m_LastHp = m_Unit.Unit.Health;
            }
            if (ManaImage && m_Unit.Unit.Magic != m_LastMp)
            {
                var scale = ManaImage.localScale;
                scale.x = m_Unit.Unit.Magic / m_Unit.Unit.MaxMagic;
                if (scale.x < 0) scale.x = 0;
                ManaImage.localScale = scale;
                m_LastMp = m_Unit.Unit.Magic;
            }
            if (m_LastShowBar != ShowBar)
            {
                Canvas.enabled = ShowBar;
                m_LastShowBar = ShowBar;
            }
        }
    }
}
