using UnityEngine;
using UnityEngine.InputSystem;
using Platformer.Gameplay;
using Platformer.Model;
using Platformer.Evolution;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        public Collider2D collider2d;
        public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        bool _doubleJumping;
        bool _hasAirJump;
        bool _isDashing;
        float _dashTimer;
        float _dashCooldownTimer;
        float _dashDirection = 1f;
        const float DashDuration = 0.15f;

        bool stopJump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        EvolutionController _evo;
        InputAction m_MoveAction;
        InputAction m_JumpAction;

        bool CanDoubleJump => _evo?.currentForm?.traits.canDoubleJump ?? false;
        bool CanDash => _evo?.currentForm?.traits.canDash ?? false;
        float DashSpeed => _evo?.currentForm?.traits.dashSpeed ?? 0f;
        float DashCooldown => _evo?.currentForm?.traits.dashCooldown ?? 1f;

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            _evo = GetComponent<EvolutionController>();

            m_MoveAction = InputSystem.actions.FindAction("Player/Move");
            m_JumpAction = InputSystem.actions.FindAction("Player/Jump");
            m_MoveAction.Enable();
            m_JumpAction.Enable();
        }

        protected override void Update()
        {
            if (_dashCooldownTimer > 0)
                _dashCooldownTimer -= Time.deltaTime;

            if (_isDashing)
            {
                _dashTimer -= Time.deltaTime;
                if (_dashTimer <= 0)
                    _isDashing = false;
            }

            if (controlEnabled)
            {
                move.x = m_MoveAction.ReadValue<Vector2>().x;

                if (move.x > 0.01f) _dashDirection = 1f;
                else if (move.x < -0.01f) _dashDirection = -1f;

                if (jumpState == JumpState.Grounded && m_JumpAction.WasPressedThisFrame())
                    jumpState = JumpState.PrepareToJump;
                else if (m_JumpAction.WasReleasedThisFrame())
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }

                if (CanDash && !_isDashing && _dashCooldownTimer <= 0
                    && Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame)
                {
                    _isDashing = true;
                    _dashTimer = DashDuration;
                    _dashCooldownTimer = DashCooldown;
                }
            }
            else
            {
                move.x = 0;
            }

            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    else if (_hasAirJump && CanDoubleJump && m_JumpAction.WasPressedThisFrame())
                    {
                        _hasAirJump = false;
                        _doubleJumping = true;
                        stopJump = false;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    _hasAirJump = true;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (_doubleJumping)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                _doubleJumping = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                    velocity.y *= model.jumpDeceleration;
            }

            if (_isDashing)
            {
                targetVelocity = new Vector2(_dashDirection * DashSpeed, 0);
                return;
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}
