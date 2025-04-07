
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    int index = 0;
    ShipEnemyShootScreen currentPattern;
    public ShipEnemyShootScreen timeToDie;
    public List<ShipEnemyShootScreen> patterns;
    bool enableNext = false;
    bool waiting = true;
    float timeSinceLastPattern = 0f;
    public float shootDelay = 3f;

    public Vector2 startPos;
    public Transform shipTransform;
    Vector2? currentDestination = null;
    MovementType movementType = MovementType.CIRCLE;
    private const float scale = 15;
    const float quarterPI = Mathf.PI/4f;
    bool hold = false;
    float holdTime = 0f;
    float holdDelay = .5f;
    int destinationIndex = 0;
    bool transitionOne = false;
    bool transitionTwo = false;
    Vector2[] destinations = new Vector2[] {
        new Vector2(Mathf.Cos(3*quarterPI), Mathf.Sin(3*quarterPI)) * scale,
        new Vector2(Mathf.Cos(quarterPI), Mathf.Sin(quarterPI)) * scale,
        new Vector2(Mathf.Cos(5*quarterPI), Mathf.Sin(5*quarterPI)) * scale,
        new Vector2(Mathf.Cos(7*quarterPI), Mathf.Sin(7*quarterPI)) * scale
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPattern = patterns[0];
        startPos = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(waiting){
            timeSinceLastPattern += Time.deltaTime;
            if(timeSinceLastPattern >= shootDelay){
                currentPattern.enabled = true;
            }
        }
        if(currentPattern.shootingDone){
            timeSinceLastPattern = 0f;
            currentPattern.enabled = false;

            index++;
            if(index > patterns.Count - 1) index = 0;
            currentPattern = patterns[index];
            currentPattern.shootingDone = false;
            waiting = true;
        }

        if(GetComponent<EnemyHealth>().life < 50 && !transitionOne){
            movementType = MovementType.Z;
            currentDestination = destinations[0];
            transitionOne = true;
        }

        if(GetComponent<EnemyHealth>().life < 25 && !transitionTwo){
            timeToDie.enabled = true;
        }

        if(movementType == MovementType.CIRCLE){
            float moveTime = 5;
            
            var x = Mathf.Cos(Time.time / moveTime * 2 * Mathf.PI) * scale;
            var y = Mathf.Sin(Time.time / moveTime * 2 * Mathf.PI) * scale;

            transform.position = new Vector3(startPos.x + x, startPos.y + y, shipTransform.position.z + 50f);
        } else if(movementType == MovementType.Z){
            if(hold){
                holdTime += Time.deltaTime;
                if(holdTime >= holdDelay){
                    hold = false;
                    destinationIndex++;
                    if(destinationIndex > destinations.Length - 1) destinationIndex = 0;
                    currentDestination = destinations[destinationIndex];
                }
            } else {
                var speed = 25f; 
                Vector3 destination = new Vector3(currentDestination.Value.x, currentDestination.Value.y, shipTransform.position.z + 50f);
                Vector3 diff = destination - transform.position;
                if(diff.magnitude <= .1f) {
                    transform.position = destination;
                    hold = true;
                    holdTime = 0f;
                } else {
                    transform.position += diff.normalized * speed * Time.deltaTime;
                }
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, shipTransform.position.z + 50f);
        }
    }
}

public enum MovementType{
    CIRCLE, Z
}
