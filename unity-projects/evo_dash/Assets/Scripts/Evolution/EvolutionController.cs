using System.Collections;
using UnityEngine;
using Platformer.Mechanics;
using Platformer.Model;
using static Platformer.Core.Simulation;

namespace Platformer.Evolution
{
    [RequireComponent(typeof(PlayerController))]
    public class EvolutionController : MonoBehaviour
    {
        public EvolutionForm[] availableForms;
        public EvolutionForm currentForm { get; private set; }

        private PlayerController _player;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        void Awake()
        {
            _player = GetComponent<PlayerController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        public void EvolveRandom()
        {
            if (availableForms == null || availableForms.Length == 0) return;

            EvolutionForm next;
            if (availableForms.Length == 1)
            {
                next = availableForms[0];
            }
            else
            {
                // avoid picking the same form twice in a row
                EvolutionForm candidate;
                do { candidate = availableForms[Random.Range(0, availableForms.Length)]; }
                while (candidate == currentForm);
                next = candidate;
            }

            ApplyForm(next);
        }

        public void ApplyForm(EvolutionForm form)
        {
            currentForm = form;

            _player.maxSpeed = form.traits.maxSpeed;
            _player.jumpTakeOffSpeed = form.traits.jumpTakeOffSpeed;
            model.jumpModifier = form.traits.jumpModifier;

            if (form.sprite != null)
                _spriteRenderer.sprite = form.sprite;

            if (form.animatorController != null)
                _animator.runtimeAnimatorController = form.animatorController;

            StartCoroutine(FlashColor(form.tintColor));

            Debug.Log($"[Evolution] Evolved into: {form.formName}");
        }

        IEnumerator FlashColor(Color targetColor)
        {
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.05f);
            _spriteRenderer.color = targetColor;
            yield return new WaitForSeconds(0.05f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.05f);
            _spriteRenderer.color = targetColor;
        }
    }
}
