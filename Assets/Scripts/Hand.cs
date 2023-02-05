using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Transform player1;
    private bool wait = false;
    private bool movingTowardsPlayer;
    private Vector3 playerPosition;

    private Vector3 startingPosition = new Vector3(0f,200f,0f);
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startingPosition;
        playerPosition = player1.position;
        movingTowardsPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3.MoveTowards(startingPosition, player1.position,10f * Time.deltaTime);
        if(!wait) {
            Scratch();
        }
    }

    IEnumerator StartDelay() {
            wait = true;
            yield return new WaitForSeconds(2f);
            if(movingTowardsPlayer) {
                playerPosition = player1.position;
            }
            wait = false;
    }

    private void Scratch() {
        if(movingTowardsPlayer) {
            transform.position = Vector3.MoveTowards(transform.position,playerPosition, 60 * Time.deltaTime);
            if(transform.position == playerPosition) {
                movingTowardsPlayer = false;
                StartCoroutine(StartDelay());
            }
        }
        if(!movingTowardsPlayer) {
            transform.position = Vector3.MoveTowards(transform.position,startingPosition, 60 * Time.deltaTime);
            if(transform.position == startingPosition) {
                movingTowardsPlayer = true;
                StartCoroutine(StartDelay());
            }
        }
    }
}
