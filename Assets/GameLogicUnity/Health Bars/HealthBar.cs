using UnityEngine;

namespace Assets
{
    public class HealthBar : Billboard
    {
        [System.NonSerialized]
        public bool ShowBar = true;

        public Canvas Canvas;
        public RectTransform HealthImage;
        public RectTransform ManaImage;

        public Unit unit;

        private bool m_LastShowBar;
        private int m_LastHp;
        private int m_LastMp;

        public override void Start()
        {
            base.Start();

            var m_Unit = transform.parent.GetComponent<UnitBehaviour>();
            Canvas.transform.localPosition = new Vector3(0, m_Unit.Height, 0);
            Canvas.enabled = m_LastShowBar;
            Canvas.worldCamera = Camera.main;
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (unit == null)
                return;

            if (HealthImage && unit.Health != m_LastHp)
            {
                var scale = HealthImage.localScale;
                scale.x = unit.Health * 1.0f / unit.MaxHealth;
                if (scale.x < 0) scale.x = 0;
                HealthImage.localScale = scale;
                m_LastHp = unit.Health;
            }
            if (ManaImage && unit.Magic != m_LastMp)
            {
                var scale = ManaImage.localScale;
                scale.x = unit.Magic * 1.0f / unit.MaxMagic;
                if (scale.x < 0) scale.x = 0;
                ManaImage.localScale = scale;
                m_LastMp = unit.Magic;
            }
            if (m_LastShowBar != ShowBar)
            {
                Canvas.enabled = ShowBar;
                m_LastShowBar = ShowBar;
            }
            
        }
    }
}
