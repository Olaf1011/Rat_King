using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;


using Debuffs = DebuffManager.Debuffs;
public class King : MonoBehaviour
{
    // Start is called before the first frame update

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    [SerializeField] private GameObject debuffManagerObject;
    [Tooltip("In seconds")]
    [SerializeField] private float debuffThreshold = 30.0f;

    private DebuffManager debuffManager;

    [SerializeField] private float timer = 0.0f;
    [SerializeField] private bool debuffIsActive = false;



    //Animation stuff

    private Animator2D animator;
    enum AnimationStates
    {
        IDLE        = 0,
        ACTION      = 1
    }

    AnimationStates animState;
    AnimationStates newAnimState;


    void Start()
    {
        Debug.Assert(debuffManagerObject != null, "Missing debuffManager on king", this);
        debuffManager = debuffManagerObject.GetComponent<DebuffManager>();

        animator = gameObject.GetComponent<Animator2D>();
        animState = AnimationStates.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.sGameState == GameManager.GameState.PLAYING && GameManager.Instance.sSubState == GameManager.SubState.MAIN_GAME)
        {
            if (!debuffIsActive)
            {
                timer += Time.deltaTime;
            }

            if (timer > debuffThreshold && !debuffIsActive)
            {
                SetDebuff();
                animState = AnimationStates.ACTION;
                animator.ChangeAnimation((int)animState);
            }
        }
        
    }

    void LateUpdate()
    {
        if(animState == AnimationStates.ACTION && animator.lastFrameFlag)
        {
            animState = AnimationStates.IDLE;
            animator.ChangeAnimation((int)animState);
        }
        
    }

    public static Debuffs GetRandomDebuff(Debuffs prevDebuffs)
    {
        Debuffs chosenDebuff = (Debuffs)(Random.Range(1, (int)(Debuffs.MAX_SIZE)));
        if (chosenDebuff == prevDebuffs)
        {
            ++chosenDebuff;
            if (chosenDebuff == Debuffs.MAX_SIZE)
                chosenDebuff -= 2;
        }

        return chosenDebuff;
    }

    private void SetDebuff()
    {
        debuffIsActive = true;
        timer = 0.0f;
        debuffManager.SetKingsDebuff();
    }


    public void DebuffDone()
    {
        debuffIsActive = false;
    }
}
