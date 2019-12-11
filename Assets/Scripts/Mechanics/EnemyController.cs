using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;

        public Bounds Bounds => _collider.bounds;

        public int health = 100;

        public LayerMask isPlayer;
        public Transform attackPosition;
        public float attackRange;

        public Transform pos;

        private Animator anim;
        public GameObject tokenPrefab;

        

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
        }

        // void OnCollisionEnter2D(Collision2D collision)
        //{
        //var player = collision.gameObject.GetComponent<PlayerController>();
        // if (player != null)
        // {
        // var ev = Schedule<PlayerEnemyCollision>();
        // ev.player = player;
        ///ev.enemy = this;
        //}
        // }

        void Update()
        {
            
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }
         
        }

        public void takeDamage(int damage)
        {
            health = health - damage;
            if (health <= 0)
            {
                Instantiate(tokenPrefab, pos.position,Quaternion.identity);
                Destroy(this.gameObject);
                PlayerController.score+=1;
                Debug.Log(PlayerController.score);
                
            }

            Debug.Log(health);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision.gameObject.CompareTag("Player"));
            if (collision.gameObject.CompareTag("Player"))
            {
                int val = Random.Range(1, 3);
               // Collider2D[] playerDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, isPlayer);
                //Collider2D[] playerDamage1 = Physics2D.OverlapCircleAll(pos.position, attackRange, isPlayer);
                //for (int i = 0; i < playerDamage.Length; i++)
                //{
                    switch (val)
                    {
                        case 1:
                            anim.SetTrigger("punch");
                            collision.gameObject.GetComponent<PlayerController>().takeDamage(20);
                            //playerDamage1[i].GetComponent<PlayerController>().takeDamage(10);


                            break;
                        case 2:
                            anim.SetTrigger("slash");
                            collision.gameObject.GetComponent<PlayerController>().takeDamage(15);
                            //playerDamage1[i].GetComponent<PlayerController>().takeDamage(15);
                            break;
                    }

               // }


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
