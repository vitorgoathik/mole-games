using UnityEngine;

namespace Platformer.Evolution
{
    [RequireComponent(typeof(Collider2D))]
    public class EvolutionItem : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var evo = other.GetComponent<EvolutionController>();
            if (evo == null) return;
            evo.EvolveRandom();
            Destroy(gameObject);
        }
    }
}
