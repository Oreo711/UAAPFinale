using System;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Stages;
using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using UnityEngine;
using UnityEngine.Serialization;


namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Levels
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Levels/NewLevelConfig", fileName = "LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<StageConfig> _stageConfigs;
        [SerializeField] private int               _winGoldReward;
        [SerializeField] private int               _winDiamondReward;
        [SerializeField] private TowerConfig       _towerConfig;

        public IReadOnlyList<StageConfig> StageConfigs     => _stageConfigs;
        public int                        WinGoldReward    => _winGoldReward;
        public int                        WinDiamondReward => _winDiamondReward;
        public TowerConfig                TowerConfig      => _towerConfig;
    }

    [Serializable]
    public class TowerConfig
    {
        [SerializeField] private float _health;
        [SerializeField] private float _damage;
        [SerializeField] private float _radius;

        public float Health => _health;
        public float Damage => _damage;
        public float Radius => _radius;
    }
}
