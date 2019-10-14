using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class nutshell : MonoBehaviour
{
    /// <summary>
    /// I wanted a simple clean SLIDER VISUAL AID to show the player taking damage.
    /// Add this to an empty gameobject in the scene then in the Unity Inspector:
    /// Create two UI sliders, give them the same rect transform.
    /// Change the sliders fill to different colors, attach the objects to these public fields:
    /// </summary>
    public Slider mySliderFRONT;
    public Image frontFill;
    public Slider mySliderBack;
    public Text myHPDisplayText;

    // Animation Variables
    [Header("When to trigger blinking.")]
    public float hpDangerZone;
    [Header("Rate to reduce the  trail.")]
    public float reduceTrailPerTurn = 0.5f;
    [Header("Speed the trail should shrink.")]
    public float decrementTrailTime = 0.1f;
    [Header("How long to wait before starting trail anim.")]
    public float holdBeforeTrailAnim = 0.5f;

    public string theString { get; set; }
    public float theAmount { get; set; }

    private bool isDead;
    private float hp, hit;
    private float hpMin, hpMax;
    private readonly float sliderMax = 1.0f;
    private string textHPtodisplay = "";

    private Slider front, back;

    // blink
    Image myFrontBlink;
    bool blinking;
    Color bOne;
    Color bTwo;

    // TRAIL 
    float trail;


    private void Start()
    {
        isDead = false;
        hpMin = 0.001f;
        hpMax = 100.00f;
        blinking = false;
        hp = hpMax;
        ChangeScreenHP(hp);
        front = mySliderFRONT.GetComponent<Slider>();
        back = mySliderBack.GetComponent<Slider>();
        front.value = sliderMax;
        back.value = sliderMax;
        myFrontBlink = frontFill.GetComponent<Image>();
        bTwo = myFrontBlink.color;
        bOne = bTwo;
        bOne.a = 0.5f;
        myHPDisplayText.gameObject.SetActive(true);
        front.gameObject.SetActive(true);
        back.gameObject.SetActive(true);
    }

    void ChangeScreenHP(float input)
    {
        // TRIGGER ANIMATION "JUMP"
        int bigHP = 0;
        bigHP = Mathf.FloorToInt(hp * 1000);
        textHPtodisplay = bigHP.ToString();
        myHPDisplayText.text = textHPtodisplay;
    }

    void MyState(string var, float amount)
    {
        switch (var)
        {
            case "heal":
                // heal
                if (!isDead) // Still alive??
                {
                    if (hp < hpMax) // check HP before continuing: are we going to benefit from healing?
                    {
                        trail = hp;
                        hit = hp + amount;
                        hp = hit;
                        //Debug.Log(hp);
                        hit /= 100;
                        back.value = hit;
                        if (hp >= hpMax)
                        {
                            hp = hpMax;
                            Debug.Log("hp is already MAX");
                            //goto default;
                        }
                        ChangeScreenHP(hp);
                        StartCoroutine(PauseBeforeTrail(holdBeforeTrailAnim, -amount));
                    }
                    else {
                        if(hp >= hpMax)
                        {
                            Debug.LogError("casting heal on a fully healed player!");

                        }
                    }
                }
                break;
            case "damage":
                // wound
                trail = hp; // set the trailAmount to HP before we modify it.
                hit = hp - amount;
                hp = hit;
                hit /= 100f;
                front.value = hit;
                ChangeScreenHP(hp);
                // before moving check death variables
                if (hp <= hpMin) {
                    goto case "death";
                }
                StartCoroutine(PauseBeforeTrail(holdBeforeTrailAnim, amount));
                break;
            case "dangerZone":
                // blink
                if (!blinking)
                {
                    blinking = true;
                    StartCoroutine(LoopBlink());
                }
                goto case "damage";
            case "death":
                // dead
                Debug.LogError("death");
                hp = hpMin;
                ChangeScreenHP(hp);
                isDead = true;
                blinking = false;
                myHPDisplayText.gameObject.SetActive(false);
                front.gameObject.SetActive(false);
                back.gameObject.SetActive(false);
                break;
            default:
                // other
                break;
        }
    }

    //DEBUG METHODS
    public void BTN_STRING(string thing)
    {
        theString = thing;
    }
    public void BTN_FLOAT(float thing)
    {
        //thing = Random.Range(2.1f, 8.54f);
        theAmount = thing;
    }

    public void WrapperButton() {
        if(theAmount > float.Epsilon) // IF the DMG is a valid amount to apply
        {
            //ActionTrigger(theString, theAmount);
            if(hp <= hpDangerZone && theString == "damage")
            {
                theString = "dangerZone";
            }
            if (hp >= hpDangerZone)
            {
                blinking = false;
            }
            MyState(theString, theAmount);
        }
        else
        {
            Debug.LogError("invalid DMG amount");

        }
    }
    IEnumerator LoopBlink()
    {
        Debug.Log("start blink loop: " + blinking);

        if (blinking)
        {
            myFrontBlink.color = bOne;
            yield return new WaitForSeconds(0.1f);
            myFrontBlink.color = bTwo;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(LoopBlink());
        }


    }
    IEnumerator PauseBeforeTrail(float time, float amount)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(Trail(amount));
    }
    IEnumerator Trail(float val)
    {
        
        if(Mathf.Sign(val)==-1)
        {
            if (trail <= hp)
            {
                trail += reduceTrailPerTurn;
                front.value = trail / 100;
                Debug.Log("Blerp=" + trail);
                yield return new WaitForSeconds(decrementTrailTime);
                StartCoroutine(Trail(val));
            }
        }
        else
        {
            if (trail >= hp)
            {
                trail -= reduceTrailPerTurn;
                back.value = trail / 100;
                Debug.Log("lerp=" + trail);
                yield return new WaitForSeconds(decrementTrailTime);
                StartCoroutine(Trail(val));
            }

        }
        
        yield return new WaitForEndOfFrame();
    }

}
