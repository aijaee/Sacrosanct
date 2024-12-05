using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleManagerScript : MonoBehaviour
{
    //This script is for the battle system that this game uses
    //The following variables are public for easy setting in the inspector
    //They can be set to private if you can pull them into the inspector
    public camShakeScript CSS;
    public gameManagerScript GMS;
   //This is used to check if the battle has been finished
    private bool battleStatus;

    //In: two 'unit' game Objects the initiator is the unit that initiated the attack and the recipient is the receiver
    //Out: void - units take damage or are destroyed if the hp threshold is <= 
    //Desc: This is usually called by another script which has access to the two units and then just sets the units as parameters for the function

    public void battle(GameObject initiator, GameObject recipient)
    {
        battleStatus = true;
        var initiatorUnit = initiator.GetComponent<UnitScript>();
        var recipientUnit = recipient.GetComponent<UnitScript>();
        int initiatorAtt = initiatorUnit.attackDamage;

        // Instantiate damage particle effect on the recipient
        GameObject tempParticle = Instantiate(recipientUnit.GetComponent<UnitScript>().damagedParticle, recipient.transform.position, recipient.transform.rotation);
        Destroy(tempParticle, 2f);

        // Deal damage to the recipient only
        recipientUnit.dealDamage(initiatorAtt);

        // Check if the recipient is dead
        if (checkIfDead(recipient))
        {
            recipient.transform.parent = null;
            recipientUnit.unitDie();
            battleStatus = false;
            GMS.checkIfUnitsRemain(initiator, recipient);
            return;
        }

        // If the initiator is not supposed to take damage, skip dealing damage to the initiator
        battleStatus = false;
    }


    //In: gameObject to check
    //Out: boolean - true if unit is dead, false otherwise
    //Desc: the health of the gameObject is checked (must be 'unit') or it'll break
    public bool checkIfDead(GameObject unitToCheck)
    {
        if (unitToCheck.GetComponent<UnitScript>().currentHealthPoints <= 0)
        {
            return true;
        }
        return false;
    }

    //In: gameObject to destroy
    //Out: void
    //Desc: the gameObject in the parameter is destroyed
    public void destroyObject(GameObject unitToDestroy)
    {
        Destroy(unitToDestroy);
    }

    //In: two unit gameObjects the attacker and the receiver
    //Out: this plays the animations for the battle
    //Desc: this function calls all the functions for the battle 
    public IEnumerator attack(GameObject unit, GameObject enemy)
    {
        battleStatus = true;
        float elapsedTime = 0;
        Vector3 startingPos = unit.transform.position;
        Vector3 endingPos = enemy.transform.position;
        unit.GetComponent<UnitScript>().setWalkingAnimation();
        while (elapsedTime < .25f)
        {
           
            unit.transform.position = Vector3.Lerp(startingPos, startingPos+((((endingPos - startingPos) / (endingPos - startingPos).magnitude)).normalized*.5f), (elapsedTime / .25f));
            elapsedTime += Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }
        
        
        
        while (battleStatus)
        {
           
            StartCoroutine(CSS.camShake(.2f,unit.GetComponent<UnitScript>().attackDamage,getDirection(unit,enemy)));
            if(unit.GetComponent<UnitScript>().attackRange == enemy.GetComponent<UnitScript>().attackRange && enemy.GetComponent<UnitScript>().currentHealthPoints - unit.GetComponent<UnitScript>().attackDamage > 0)
            {
                StartCoroutine(unit.GetComponent<UnitScript>().displayDamageEnum(enemy.GetComponent<UnitScript>().attackDamage));
                StartCoroutine(enemy.GetComponent<UnitScript>().displayDamageEnum(unit.GetComponent<UnitScript>().attackDamage));
            }
           
            else
            {
                StartCoroutine(enemy.GetComponent<UnitScript>().displayDamageEnum(unit.GetComponent<UnitScript>().attackDamage));
            }
            
            //unit.GetComponent<UnitScript>().displayDamage(enemy.GetComponent<UnitScript>().attackDamage);
            //enemy.GetComponent<UnitScript>().displayDamage(unit.GetComponent<UnitScript>().attackDamage);
            
            battle(unit, enemy);
            
            yield return new WaitForEndOfFrame();
        }
        
        if (unit != null)
        {
           StartCoroutine(returnAfterAttack(unit, startingPos));
          
        }
       
        

       
        //unit.GetComponent<UnitScript>().wait();

    }

    //In: unit that is returning to its position, the endPoint vector to return to
    //Out: The unit returns back to its location
    //Desc: the gameObject in the parameter is returned to the endPoint
    public IEnumerator returnAfterAttack(GameObject unit, Vector3 endPoint) {
        float elapsedTime = 0;
        

        while (elapsedTime < .30f)
        {
            unit.transform.position = Vector3.Lerp(unit.transform.position, endPoint, (elapsedTime / .25f));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        unit.GetComponent<UnitScript>().setWaitIdleAnimation();
        unit.GetComponent<UnitScript>().wait();
       
        
    }

    //In: two 'unit' gameObjects 
    //Out: vector3 the direction that the unit needs to moveTowards 
    //Desc: the vector3 which the unit needs to moveTowards is returned by this function
    public Vector3 getDirection(GameObject unit, GameObject enemy)
    {
        Vector3 startingPos = unit.transform.position;
        Vector3 endingPos = enemy.transform.position;
        return (((endingPos - startingPos) / (endingPos - startingPos).magnitude)).normalized;
    }
    




}
