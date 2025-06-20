using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleManagerScript : MonoBehaviour
{
    public camShakeScript CSS;
    public gameManagerScript GMS;

    private bool battleStatus;
    public TileDisabler tileDisabler;

    public void battle(GameObject initiator, GameObject recipient)
    {
        battleStatus = true;
        var initiatorUnit = initiator.GetComponent<UnitScript>();
        var recipientUnit = recipient.GetComponent<UnitScript>();
        int initiatorAtt = initiatorUnit.attackDamage;

        // Trigger damaged animation for the recipient
        recipientUnit.SetDamaged(true);

        // Instantiate damage particle effect
        GameObject tempParticle = Instantiate(recipientUnit.damagedParticle, recipient.transform.position, recipient.transform.rotation);
        Destroy(tempParticle, 2f);

        // Deal damage to the recipient
        recipientUnit.dealDamage(initiatorAtt);

        if (checkIfDead(recipient))
        {
            recipient.transform.parent = null;
            recipientUnit.unitDie();
            battleStatus = false;
            GMS.checkIfUnitsRemain(initiator, recipient);

            if (tileDisabler != null)
            {
                tileDisabler.DisableTile();
            }
            return;
        }

        battleStatus = false;
    }

    public bool checkIfDead(GameObject unitToCheck)
    {
        return unitToCheck.GetComponent<UnitScript>().currentHealthPoints <= 0;
    }

    public void destroyObject(GameObject unitToDestroy)
    {
        Destroy(unitToDestroy);
    }

    public IEnumerator attack(GameObject unit, GameObject enemy)
    {
        battleStatus = true;
        float elapsedTime = 0;
        Vector3 startingPos = unit.transform.position;
        Vector3 endingPos = enemy.transform.position;

        // Trigger walking animation
        unit.GetComponent<UnitScript>().SetWalking(true);

        while (elapsedTime < .25f)
        {
            unit.transform.position = Vector3.Lerp(startingPos, startingPos + (((endingPos - startingPos) / (endingPos - startingPos).magnitude)).normalized * .5f, (elapsedTime / .25f));
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        // Stop walking animation and trigger attack animation
        unit.GetComponent<UnitScript>().SetWalking(false);
        unit.GetComponent<UnitScript>().SetAttacking(true);

        while (battleStatus)
        {
            StartCoroutine(CSS.camShake(.2f, unit.GetComponent<UnitScript>().attackDamage, getDirection(unit, enemy)));

            if (unit.GetComponent<UnitScript>().attackRange == enemy.GetComponent<UnitScript>().attackRange && enemy.GetComponent<UnitScript>().currentHealthPoints - unit.GetComponent<UnitScript>().attackDamage > 0)
            {
                StartCoroutine(unit.GetComponent<UnitScript>().displayDamageEnum(enemy.GetComponent<UnitScript>().attackDamage));
                StartCoroutine(enemy.GetComponent<UnitScript>().displayDamageEnum(unit.GetComponent<UnitScript>().attackDamage));
            }
            else
            {
                StartCoroutine(enemy.GetComponent<UnitScript>().displayDamageEnum(unit.GetComponent<UnitScript>().attackDamage));
            }

            battle(unit, enemy);
            yield return new WaitForEndOfFrame();
        }

        // Stop attack animation
        unit.GetComponent<UnitScript>().SetAttacking(false);

        if (unit != null)
        {
            StartCoroutine(returnAfterAttack(unit, startingPos));
        }
    }

    public IEnumerator returnAfterAttack(GameObject unit, Vector3 endPoint)
    {
        float elapsedTime = 0;

        while (elapsedTime < .30f)
        {
            unit.transform.position = Vector3.Lerp(unit.transform.position, endPoint, (elapsedTime / .25f));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Trigger idle animation after returning
        unit.GetComponent<UnitScript>().SetIdle(true);
        unit.GetComponent<UnitScript>().SetWalking(false);
        unit.GetComponent<UnitScript>().SetAttacking(false);
        unit.GetComponent<UnitScript>().SetDamaged(false);

        unit.GetComponent<UnitScript>().wait();
    }

    public Vector3 getDirection(GameObject unit, GameObject enemy)
    {
        Vector3 startingPos = unit.transform.position;
        Vector3 endingPos = enemy.transform.position;
        return (((endingPos - startingPos) / (endingPos - startingPos).magnitude)).normalized;
    }
}
