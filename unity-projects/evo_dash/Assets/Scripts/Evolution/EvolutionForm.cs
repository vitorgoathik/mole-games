using UnityEngine;

namespace Platformer.Evolution
{
    [CreateAssetMenu(fileName = "NewForm", menuName = "evo_dash/Evolution Form")]
    public class EvolutionForm : ScriptableObject
    {
        public string formName;
        public Sprite sprite;
        public RuntimeAnimatorController animatorController;
        public Color tintColor = Color.white;
        public FormTraits traits;
    }
}
