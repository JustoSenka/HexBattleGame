using System;

namespace Assets
{
    public interface ISkillSelectionManager
    {
        SkillType SelectedSkill { get; }

        event Action<SkillType> SkillClicked;
        event Action<SkillType> SkillSelectionChanged;
        event Action<SkillType, HexCell> SkillPerformed;

        void ClickAndSelectSkill(SkillType skill);
        void SelectSkill(SkillType skill);
    }
}
