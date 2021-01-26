using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.RunPoinsSytem
{
    [CreateAssetMenu(fileName = "EnemyList", menuName = "RpsCustom/EnemyList", order = 0)]
    public class EnemiesStageList : ScriptableObject
    {
        public List<GenerableData> enemies;
    }
}