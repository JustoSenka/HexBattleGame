﻿using System;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Controls how skills are selected and performed for LOCAL player.
    /// This will work independently for each user on network and is not used by enemy AIs.
    /// Visual feedback on local clients is depending mostly on this class.
    /// </summary>
    [RegisterDependency(typeof(ISkillSelectionManager), true)]
    public class SkillSelectionManager : ISkillSelectionManager
    {
        public event Action<SkillType> SkillClicked;
        public event Action<SkillType> SkillSelectionChanged;
        public event Action<SkillType, HexCell> SkillPerformed;

        public SkillType SelectedSkill { get; private set; }

        private readonly IHexDatabase HexDatabase;
        private readonly IUnitSelectionManager UnitSelectionManager;
        private readonly ISelectionManager SelectionManager;
        private readonly ICrossPlayerController CrossPlayerController;
        public SkillSelectionManager(IHexDatabase HexDatabase, IUnitSelectionManager UnitSelectionManager, ISelectionManager SelectionManager, ICrossPlayerController CrossPlayerController)
        {
            this.HexDatabase = HexDatabase;
            this.UnitSelectionManager = UnitSelectionManager;
            this.CrossPlayerController = CrossPlayerController;
            
            SelectionManager.HexClicked += OnHexClicked;
        }
        
        public void ClickAndSelectSkill(SkillType skill)
        {
            Debug.Log($"Clicked skill {skill}");

            SkillClicked?.Invoke(skill);
            SelectSkill(skill);
        }

        public void SelectSkill(SkillType skill)
        {
            if (SelectedSkill != skill)
            {
                SelectedSkill = skill;
                SkillSelectionChanged?.Invoke(skill);
            }
        }


        // --- Callbacks

        private void OnHexClicked(HexCell hex)
        {
            if (SelectedSkill == SkillType.None || UnitSelectionManager.SelectedUnit == null)
                return;

            // Skill range ?
            // Unit correctness with skill ?

            CrossPlayerController.PerformSkill(UnitSelectionManager.SelectedUnit, hex.Position, SelectedSkill);

            SkillPerformed?.Invoke(SelectedSkill, hex);
        }
    }
}
