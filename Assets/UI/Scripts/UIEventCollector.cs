using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class UIEventCollector : MonoBehaviour
    {
#pragma warning disable CS0649
        [Dependency(typeof(ISkillSelectionManager))]
        private ISkillSelectionManager SkillSelectionManager;
#pragma warning restore CS0649

        public Button GuardSkillButton;

        void Start()
        {
            
        }

        void Update()
        {

        }

        private void PerformSkill(SkillType skill)
        {

        }

        // Called by unity when UI button is pressed
        public void PerformSkill(int skillID)
        {
            SkillSelectionManager.ClickAndSelectSkill((SkillType) Enum.Parse(typeof(SkillType), skillID.ToString()));
        }
    }
}
