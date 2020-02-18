﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Competitor : MonoBehaviour
{
    //Name and scoring Variables
    public string Name;
    public int Score;
    private bool scoredGoal;

    //Powerup constructor intakes
    public Transform origin;
    public bool navMeshOff;

    //Variables for power up effects
    public bool chillOut;
    public bool gottaGoFast;
    public bool untouchable;
    public bool inivisible;
    public bool ballOfSteel;
    public bool superBounce;
    public bool calmDown;
    public bool isTerrain;
    private Color originalColor;



    public Material startMaterial;
    public Material invisMaterial;
    private IEnumerator coruotine;
    GameObject particlesGTF;
    GameObject particlesCTT;
    GameObject particlesCD;

    //trail is the trail render on the ball
    TrailRenderer trails;
    Material trailMaterial;
    Rigidbody myRB;
    public Color lowSpeed;
    public Color midSpeed;
    public Color maxSpeed;


    //Called when a ball hits the thanoswall collider

    //AudioManager
    AudioManager am;

    private void Awake()
    {
        StartCoroutine(FindAudioManager(0.1f));
        this.transform.localScale = new Vector3(.025f, .025f, .025f);
        origin = this.transform;
        navMeshOff = false;
        chillOut = false;
        gottaGoFast = false;
        untouchable = false;
        inivisible = false;
        ballOfSteel = false;
        superBounce = false;
        calmDown = false;
        myRB = GetComponent<Rigidbody>();
        trails = GetComponent<TrailRenderer>();

        if (trails != null)
        {
            trailMaterial = trails.materials[0];
        }


    }


    public void Start()
    {
        //if (transform.childCount > 0)
        //{
        //    Transform t = transform.GetChild(1);
        //    if (t.childCount > 0)
        //    {
        //        startMaterial = t.GetComponentInChildren<Renderer>().material;

        //        return;
        //    }
        //}

        if (transform.CompareTag("Player"))
        {
           
            if (transform.GetChild(1).GetComponentInChildren<MeshRenderer>().material)
            {
                startMaterial = transform.GetChild(1).GetComponentInChildren<MeshRenderer>().material;
                Debug.Log("Player Material: " + startMaterial);
            }

        }
        else
        {
            startMaterial = transform.GetChild(1).GetComponentInChildren<MeshRenderer>().material;
        }
    }

    public IEnumerator FindAudioManager(float duration)
    {

        yield return new WaitForSeconds(duration);
        am = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (trails != null)
        {
            trails.time = Mathf.Clamp(myRB.velocity.magnitude * .008f, 0, .3f);
            if (myRB.velocity.magnitude > 30)
            {
                trails.material.color = Color.Lerp(trails.material.color, maxSpeed, .05f);
            }
            else if (myRB.velocity.magnitude > 20)
            {
                trails.material.color = Color.Lerp(trails.material.color, midSpeed, .05f);
            }
            else if (myRB.velocity.magnitude > 10)
            {
                trails.material.color = Color.Lerp(trails.material.color, lowSpeed, .05f);
            }
            else
            {
                trails.material.color = Color.Lerp(trails.material.color, Color.black, .05f);
            }
        }

    }

    public bool ScoredGoal
    {
        get { return scoredGoal; }

        set { scoredGoal = value; }
    }


    public void Blast(Transform transform, float duration)
    {
        StartCoroutine(BeenBlasted(transform, duration));
    }

    public IEnumerator BeenBlasted(Transform transform, float duration)
    {
        if (am != null)
        {
            am.Play("BlastZone");
        }
        yield return new WaitForSeconds(duration);

        transform.GetComponent<Competitor>().navMeshOff = false;
        transform.GetComponent<AIStateMachine>().enabled = true;
    }

    public void BeenChilled(Competitor competitor, float duration)
    {
        if (chillOut == true)
        {
            if (coruotine != null)
            {
                StopCoroutine(coruotine);
            }
        }
        coruotine = TurnMovementControlBackOn(competitor, duration);
        StartCoroutine(coruotine);

    }

    private IEnumerator TurnMovementControlBackOn(Competitor competitor, float duration)
    {
        if (am != null)
        {
            am.Play("Chillout");
        }
        yield return new WaitForSeconds(duration);
        if (competitor.GetComponent<RigidBodyControl>())
        {
            competitor.GetComponent<Rigidbody>().freezeRotation = false;
            competitor.GetComponent<RigidBodyControl>().enabled = true;
            competitor.chillOut = false;
        }
        else
        {
            competitor.GetComponent<Rigidbody>().freezeRotation = false;
            competitor.GetComponent<AIStateMachine>().enabled = true;
            competitor.chillOut = false;
        }

    }

    public void WentFast(Transform origin, float duration, float speedMultiplier)
    {

        if (gottaGoFast == true)
        {
            if (coruotine != null)
            {
                RemoveParticleEffect(particlesGTF);
                StopCoroutine(coruotine);
            }
        }
        coruotine = ResetSpeed(origin, duration, speedMultiplier);
        StartCoroutine(coruotine);

    }

    private IEnumerator ResetSpeed(Transform origin, float duration, float speedMultiplier)
    {
        //Disable the current trail.

        origin.GetComponent<TrailRenderer>().enabled = false;
        particlesGTF = InstantiateParticleEffect("PE_GottaGoFast");

        gottaGoFast = true;

        //AudioManager am = FindObjectOfType<AudioManager>();
        if (am != null)
        {
            am.Play("GottaGoFast");
        }
        yield return new WaitForSeconds(duration);
        origin.GetComponent<TrailRenderer>().enabled = true;
        RemoveParticleEffect(particlesGTF);
        

        if (origin.GetComponent<RigidBodyControl>())
        {
            origin.GetComponent<RigidBodyControl>().speed /= speedMultiplier;
        }
        else
        {
            origin.GetComponent<AIStateMachine>().Velocity /= speedMultiplier;
        }

        gottaGoFast = false;

    }

    public void CantTouchMe(float duration)
    {
        if (untouchable == true)
        {
            if (coruotine != null)
            {
                RemoveParticleEffect(particlesCTT);
                StopCoroutine(coruotine);
            }
        }
        coruotine = Untouchable(duration);
        StartCoroutine(coruotine);
    }

    private IEnumerator Untouchable(float duration)
    {
        untouchable = true;
        particlesCTT = InstantiateParticleEffect("PE_CantTouchThis");
        if (am != null)
        {
            am.Play("CantTouchThis");
        }
        yield return new WaitForSeconds(duration);
        RemoveParticleEffect(particlesCTT);
        untouchable = false;
    }

    public void CantFindMe(Transform origin, float duration)
    {
        if (inivisible == true)
        {
            if (coruotine != null)
            {
                StopCoroutine(coruotine);
            }
        }
        coruotine = Invis(origin, duration);
        StartCoroutine(coruotine);
    }

    IEnumerator Invis(Transform origin, float duration)
    {

        var playerM = origin.GetChild(1).GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
        Material[] playerH = new Material[0];
        if (origin.GetChild(0).GetChild(1).gameObject.GetComponentInChildren<MeshRenderer>())
        {
            playerH = origin.GetChild(0).GetChild(1).gameObject.GetComponentInChildren<MeshRenderer>().materials;
        }
        if (origin.tag == "Player")
        {
            originalColor = new Color(playerM.GetColor("_ColorBlend").r, playerM.GetColor("_ColorBlend").g, playerM.GetColor("_ColorBlend").b, 1f);
        }


        inivisible = true;

        if (am != null)
        {
            am.Play("SneakySnake");
        }

        if (ballOfSteel == true)
        {

            Color playerColor = new Color(playerM.GetColor("_ColorBlend").r, playerM.GetColor("_ColorBlend").g, playerM.GetColor("_ColorBlend").b, 0.3f);
            playerM = startMaterial;
            playerM.DisableKeyword("_ALPHATEST_ON");
            playerM.DisableKeyword("_ALPHABLEND_ON");
            playerM.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            playerM.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            playerM.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            playerM.SetInt("_Zwrite", 0);
            playerM.shader = Shader.Find("Nasty/CelShading - Transparent");
            playerM.SetColor("_ColorBlend", playerColor);
            playerM.renderQueue = 3000;
            playerM.SetFloat("_Mode", 3);
        }

        yield return new WaitForSeconds(duration);
        if (origin.tag == "Player")
        {
            playerM.DisableKeyword("_ALPHATEST_ON");
            playerM.DisableKeyword("_ALPHABLEND_ON");
            playerM.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            playerM.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            playerM.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            playerM.SetInt("_Zwrite", 1);
            playerM.shader = Shader.Find("Nasty/CelShading");
            playerM.SetColor("_ColorBlend", originalColor);
            playerM.renderQueue = -1;
            playerM.SetFloat("_Mode", 0);



            if (playerH.Length > 0)
            {
                for (int i = 0; i < playerH.Length; i++)
                {
                    Color originalHColor = new Color(playerH[i].GetColor("_ColorBlend").r, playerH[i].GetColor("_ColorBlend").g, playerH[i].GetColor("_ColorBlend").b, 1f);
                    playerH[i].DisableKeyword("_ALPHATEST_ON");
                    playerH[i].DisableKeyword("_ALPHABLEND_ON");
                    playerH[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    playerH[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    playerH[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    playerH[i].SetInt("_Zwrite", 1);
                    playerH[i].shader = Shader.Find("Nasty/CelShading");
                    playerH[i].SetColor("_ColorBlend", originalHColor);
                    playerH[i].renderQueue = -1;
                    playerH[i].SetFloat("_Mode", 0);
                }
            }
        }
        else
        {
            origin.GetChild(1).GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
            origin.GetComponent<TrailRenderer>().enabled = true;
            if (origin.GetChild(0).GetChild(1).gameObject.GetComponentInChildren<MeshRenderer>())
            {
                origin.GetChild(0).GetChild(1).gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
            }

        }

        inivisible = false;
    }

    public void BallOfSteel(Transform origin, float duration)
    {

        if (ballOfSteel == true)
        {
            if (coruotine != null)
            {
                StopCoroutine(coruotine);
            }
        }
        coruotine = Unbouncable(origin, duration);
        StartCoroutine(coruotine);
    }

    private IEnumerator Unbouncable(Transform origin, float duration)
    {
        ballOfSteel = true;

        //AudioManager am = FindObjectOfType<AudioManager>();
        if (am != null)
        {
            am.Play("BallsOfSteel");
        }

        yield return new WaitForSeconds(duration);

        if (inivisible == true && origin.tag == "Player")
        {
            origin.GetComponent<MeshRenderer>().material = invisMaterial;
        }
        else
        {
            origin.GetComponent<MeshRenderer>().material = startMaterial;
        }

        origin.GetComponentInParent<Bounce>().enabled = true;
        ballOfSteel = false;


    }

    public void NormalBounce(Transform origin, float duration, float bounceMultiplier)
    {
        if (superBounce == true)
        {
            if (coruotine != null)
            {
                StopCoroutine(coruotine);
            }
        }
        coruotine = ReverseBounceMultiplier(origin, duration, bounceMultiplier);
        StartCoroutine(coruotine);
    }

    private IEnumerator ReverseBounceMultiplier(Transform origin, float duration, float bounceMultiplier)
    {
        superBounce = true;
        if (am != null)
        {
            am.Play("SuperBounce");
        }
        yield return new WaitForSeconds(duration);
        origin.GetComponent<Bounce>().SetMaxmiumBounce();
        superBounce = false;
    }

    public void BeenSlowed(Competitor competitor, float duration, float speedMultiplier)
    {
        if (calmDown == true)
        {
            if (coruotine != null)
            {
                RemoveParticleEffect(particlesCD);
                StopCoroutine(coruotine);
            }
        }
        coruotine = ReverseMovementSpeed(competitor, duration, speedMultiplier);
        StartCoroutine(coruotine);
    }

    private IEnumerator ReverseMovementSpeed(Competitor competitor, float duration, float speedMultiplier)
    {
        calmDown = true;
        particlesCD = InstantiateParticleEffect("PE_CalmDown");
        if (am != null)
        {
            am.Play("CalmDown");
        }
        yield return new WaitForSeconds(duration);
        RemoveParticleEffect(particlesCD);

        if (competitor.GetComponent<RigidBodyControl>())
        {
            competitor.GetComponent<RigidBodyControl>().speed /= speedMultiplier;
        }
        else
        {
            competitor.GetComponent<AIStateMachine>().Velocity /= speedMultiplier;
        }

        calmDown = false;
    }

    public void DisMine(float duration, GameObject disMine, Vector3 position, Quaternion rotation)
    {
        StartCoroutine(MineDelayActivation(duration, disMine, position, rotation));
    }

    private IEnumerator MineDelayActivation(float duration, GameObject disMine, Vector3 position, Quaternion rotation)
    {
        if (am != null)
        {
            am.Play("DisMine");
        }
        yield return new WaitForSeconds(duration);
        Instantiate(disMine, position, rotation);
    }

    private GameObject InstantiateParticleEffect(string effect)
    {
        if(effect == "PE_GottaGoFast")
        {
            GameObject option = Instantiate(
            (GameObject)Resources.Load($"Prefabs/Particle Effects/{effect}"),
            transform.GetChild(1) //Place it on the Hat component in order for it to not rotate.
            );
            option.SetActive(true);
            return option;
        }
        else
        {
            GameObject option = Instantiate(
            (GameObject)Resources.Load($"Prefabs/Particle Effects/{effect}"),
            transform.GetChild(0) //Place it on the Hat component in order for it to not rotate.
            );
            option.SetActive(true);
            return option;
        }

    }

    private void RemoveParticleEffect(GameObject option)
    {
        //option.SetActive(false);
        Destroy(option);
    }

}
