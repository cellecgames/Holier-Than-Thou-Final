﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointTracker : MonoBehaviour

{
    private int basePoints = 1;
    private int multPoints = 1;
    private int totalPoints = 1;
    private float bounceVal = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        basePoints = 1 + GetComponentInParent<Crown>().getCrownVal();
        totalPoints = basePoints * multPoints;
    }

     private void OnCollisionExit(Collision collision)
    {
        var vel = this.GetComponentInParent<Rigidbody>().velocity;
        var _competitor = collision.gameObject.GetComponent<Competitor>();
        if (_competitor != null)
        {
            if (vel.magnitude > 8)
            {
                FindObjectOfType<AudioManager>().Play("HitBlop"); // "HitBlop"
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("Blop");
            }
        }
        //Checking if you are colliding with a competitor
        if (collision.collider.GetComponentInParent<Competitor>() != null)
        {
            //Updates the bounce debugging UI field if a player is colliding
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                bounceVal = GetComponentInParent<Rigidbody>().velocity.magnitude;
            }

            //Checks if person A is going faster than person B for multiplier
            if (GetComponentInParent<Rigidbody>().velocity.magnitude < collision.collider.GetComponentInParent<Rigidbody>().velocity.magnitude && collision.collider.GetComponentInParent<Rigidbody>().velocity.magnitude > 10 && GetComponentInParent<Rigidbody>().velocity.magnitude > 10)
            {
                GetComponentInParent<PointTracker>().multPoints = GetComponentInParent<PointTracker>().multPoints + (multCalc(bounceVal));

            }

            //Checks if I won the bump
            if (GetComponentInParent<Rigidbody>().velocity.magnitude <= collision.collider.GetComponentInParent<Rigidbody>().velocity.magnitude && collision.collider.GetComponentInParent<Rigidbody>().velocity.magnitude > 10 && GetComponentInParent<Rigidbody>().velocity.magnitude > 10)
            {
                //Consumes others crown because they have a crown and I got yeeted less faster
                GetComponentInParent<Crown>().setCrownVal(collision.collider.GetComponentInParent<Crown>().getCrownVal());
                collision.collider.GetComponentInParent<Crown>().resetCrown();
                if(gameObject.name == "Player")
                {
                    GetComponent<PlayerAchievementTracker>().playerCrownsStolen += 1;
                }
            }
            else
            {
                return;
            }
        }
    }

    private int multCalc(float bounce)
    {
        int number = (int)(bounce);
        if (number <= 6)
        {
            number = 1;
        }
        else if (number > 6 && number <= 25)
        {
            number = 3;
        }
        else if (number > 25 && number < 35)
        {
            number = 5;
        }
        else if (number >= 35 && number < 40)
        {
            number = 10;
        }
        else if (number >= 40)
        {
            number = 25;
        }
        else
        {
            return number;
        }
        return number;
    }

    private void OnTriggerEnter(Collider other)
    {
    
        if (other.name.StartsWith("Crown"))
        {
            GetComponentInParent<Crown>().addCrownVal();
            other.gameObject.SetActive(false);
            if(gameObject.name == "Player")
            {
                GetComponent<PlayerAchievementTracker>().playerCrownsCollected += 1;
            }
        }
    }
    public int PointVal()
    {
        return totalPoints;
    }

    public int MultVal()
    {
        return multPoints;
    }

    public void ResetMult()
    {
        multPoints = 1;
    }

    public void ResetBasePoints()
    {
        basePoints = 1;
    }

    public int baseVal()
    {
        return basePoints;
    }

    public float GetBounceVal()
    {
        return bounceVal;
    }
}
