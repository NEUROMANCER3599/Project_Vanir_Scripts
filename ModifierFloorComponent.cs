using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierFloorComponent : MonoBehaviour
{
    [SerializeField] private float JumpForceModifier, MoveSpeedModifier, PassiveHPModifier;
    [SerializeField] private bool IsDestroyProps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.GetComponent<PlayerBehavior>() != null)
        {
            PlayerBehavior player = other.gameObject.GetComponent<PlayerBehavior>();
            player.moveSpeed = player.GetDefaultMoveSpeed() * MoveSpeedModifier;
            player.JumpForce = player.GetDefaultJumpForce() * JumpForceModifier;
            player.HP += PassiveHPModifier * Time.deltaTime;
        }

        if(other.gameObject.GetComponent<EnemyBehavior>() != null)
        {
            EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();
            enemy.movespeed = enemy.movespeed * MoveSpeedModifier;
            enemy.HP += PassiveHPModifier * Time.deltaTime;
        }

        if(other.gameObject.GetComponent<ObjectPickup>() != null)
        {

            ObjectPickup PhysicObject = other.gameObject.GetComponent<ObjectPickup>();

            if (IsDestroyProps)
            {
                PhysicObject.HP = 0;
            }
           
        }

        if(other.gameObject.GetComponent<ProjectileBehavior>() != null)
        {
            
            ProjectileBehavior projectile = other.gameObject.GetComponent<ProjectileBehavior>();
            projectile.OnHit();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerBehavior>() != null)
        {
            PlayerBehavior player = other.gameObject.GetComponent<PlayerBehavior>();
            player.moveSpeed = player.GetDefaultMoveSpeed();
            player.JumpForce = player.GetDefaultJumpForce();
            
        }

        if (other.gameObject.GetComponent<EnemyBehavior>() != null)
        {
            EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();
            enemy.movespeed = enemy.ReturnDefaultMoveSpeed();
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBehavior>() != null)
        {
            PlayerBehavior player = other.gameObject.GetComponent<PlayerBehavior>();
            player.moveSpeed = player.GetDefaultMoveSpeed() * MoveSpeedModifier;
            player.JumpForce = player.GetDefaultJumpForce() * JumpForceModifier;
            player.HP += PassiveHPModifier * Time.deltaTime;
        }

        if (other.gameObject.GetComponent<EnemyBehavior>() != null)
        {
            EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();
            enemy.movespeed = enemy.movespeed * MoveSpeedModifier;
            enemy.HP += PassiveHPModifier * Time.deltaTime;
        }

        if (other.gameObject.GetComponent<ObjectPickup>() != null)
        {

            ObjectPickup PhysicObject = other.gameObject.GetComponent<ObjectPickup>();

            if (IsDestroyProps)
            {
                PhysicObject.HP = 0;
            }

        }

        if (other.gameObject.GetComponent<ProjectileBehavior>() != null)
        {

            ProjectileBehavior projectile = other.gameObject.GetComponent<ProjectileBehavior>();
            projectile.OnHit();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBehavior>() != null)
        {
            PlayerBehavior player = other.gameObject.GetComponent<PlayerBehavior>();
            player.moveSpeed = player.GetDefaultMoveSpeed();
            player.JumpForce = player.GetDefaultJumpForce();

        }

        if (other.gameObject.GetComponent<EnemyBehavior>() != null)
        {
            EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();
            enemy.movespeed = enemy.ReturnDefaultMoveSpeed();

        }
    }
}
