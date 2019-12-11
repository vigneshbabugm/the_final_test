using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public AudioClip hitAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        // Recognition Related
        //public Text recogniseMessage;
        public Transform lineRenderPrefab;
        //public InputField templateName;


        private List<Point> strokePoints = new List<Point>();
        private List<Gesture> classificationGestures = new List<Gesture>();
        private List<LineRenderer> setOfGestures = new List<LineRenderer>();
        private int strokeId = -1;

        private Vector3 currentPosition = Vector2.zero;
        private int pointCount = 0;
        private LineRenderer currentLineRenderer;

        private bool isClassified;

        public string myTemplatePath = "D:\\Files\\Sketch Recognition\\Project\\SketchRecognition\\Assets\\Scripts\\templates";
        
        public int myHealth = 100;

        public LayerMask isEnemy;
        public Transform attackPosition;
        public Transform pos;
        public float attackRange;

        public Slider healthBar;

        public static int score=0;
        public Text scoreCounter;

        void Awake()
        {
            score = 0;
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            //Load your templates
            string[] filePaths = Directory.GetFiles(myTemplatePath, "*.xml");
            foreach (string file in filePaths)
                classificationGestures.Add(GestureIO.ReadGestureFromFile(file));
        }

        protected override void Update()
        {
            
            if (controlEnabled)
            {
                //Debug.Log(myHealth);
                healthBar.value = myHealth;
                scoreCounter.text = score.ToString();
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                if (Input.GetMouseButton(0))
                {
                    currentPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    ++strokeId;

                    Transform stroke = Instantiate(lineRenderPrefab, transform.position, transform.rotation) as Transform;
                    currentLineRenderer = stroke.GetComponent<LineRenderer>();
                    setOfGestures.Add(currentLineRenderer);
                    pointCount = 0;
                }

                if (Input.GetMouseButton(0))
                {
                    strokePoints.Add(new Point(currentPosition.x, -currentPosition.y, strokeId));
                    ++pointCount;
                    currentLineRenderer.positionCount = pointCount;
                    currentLineRenderer.SetPosition(pointCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(currentPosition.x, currentPosition.y, 6)));
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Gesture classifyArray = new Gesture(strokePoints.ToArray());
                    Result classificationResult = PointCloudRecognizer.Classify(classifyArray, classificationGestures.ToArray());
                    //recogniseMessage.text = classificationResult.GestureClass;
                    string nameOfclass = classificationResult.GestureClass;
                    
                        switch (nameOfclass)
                        {
                            case "heavypoke":
                                animator.SetTrigger("heavypoke");
                            Collider2D[] enemiesDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, isEnemy);
                            Collider2D[] enemyDamage1 = Physics2D.OverlapCircleAll(pos.position, attackRange, isEnemy);
                            for (int i = 0; i < enemiesDamage.Length; i++)
                            {
                                enemiesDamage[i].GetComponent<EnemyController>().takeDamage(30);
                                audioSource.clip = hitAudio;
                                audioSource.Play();
                            }
                            for (int i = 0; i < enemyDamage1.Length; i++)
                            {
                                enemyDamage1[i].GetComponent<EnemyController>().takeDamage(30);
                                audioSource.clip = hitAudio;
                                audioSource.Play();
                            }
                            break;
                            case "poke":
                                animator.SetTrigger("lightpoke");
                            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, isEnemy);
                            Collider2D[] enemies1 = Physics2D.OverlapCircleAll(pos.position, attackRange, isEnemy);
                            for (int i = 0; i < enemies.Length; i++)
                            {
                                enemies[i].GetComponent<EnemyController>().takeDamage(10);
                                audioSource.clip = hitAudio;
                                audioSource.Play();
                            }
                            for (int i = 0; i < enemies1.Length; i++)
                            {
                                enemies1[i].GetComponent<EnemyController>().takeDamage(10);
                                audioSource.clip = hitAudio;
                                audioSource.Play();
                            }
                            break;
                            case "slash":
                                animator.SetTrigger("slash");
                            Collider2D[] enemiesSlash = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, isEnemy);
                            Collider2D[] enemySlash1 = Physics2D.OverlapCircleAll(pos.position, attackRange, isEnemy);
                            for (int i = 0; i < enemiesSlash.Length; i++)
                            {
                                enemiesSlash[i].GetComponent<EnemyController>().takeDamage(20);
                                audioSource.clip = hitAudio;
                                audioSource.Play();
                            }
                            for (int i = 0; i < enemySlash1.Length; i++)
                            {
                                enemySlash1[i].GetComponent<EnemyController>().takeDamage(20);
                                audioSource.clip = hitAudio;
                                audioSource.Play();
                            }
                            break;
                        }
     
                    strokeId = -1;

                    strokePoints.Clear();
                    foreach (LineRenderer line in setOfGestures)
                    {

                        line.positionCount = 0;
                        Destroy(line.gameObject);
                    }

                    setOfGestures.Clear();
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
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
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
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
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
       
        public void takeDamage(int damage)
        {
            myHealth = myHealth - damage;
            if (myHealth <= 0)
            {
                audioSource.clip=respawnAudio;
                audioSource.Play();
                //write gameOver
                PlayerPrefs.SetInt("Score", score);
                SceneManager.LoadScene(2, LoadSceneMode.Single);
            }
            else
            {
                audioSource.clip = ouchAudio;
                audioSource.Play();
            }
           
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition.position, attackRange);
            Gizmos.DrawWireSphere(pos.position, attackRange);
        }
    }
}