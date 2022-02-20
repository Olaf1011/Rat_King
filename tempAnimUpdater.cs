using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempAnimUpdater : MonoBehaviour
{

    enum Anims
    {
        FORWARD = 0,
        LEFT = 1,
        RIGHT = 2,
        BACK  = 3,
        TOTAL_ANIMS = 4
    }

    public float updateSpeed = 5.0f;
    private float updateTime;

    private Animator2D animator;

    private int currentAnim;
    // Start is called before the first frame update
    void Start()
    {
        updateTime = 0.0f;
        currentAnim = 0;

        animator = gameObject.GetComponent<Animator2D>();
    }

    // Update is called once per frame
    void Update()
    {
        updateTime += Time.deltaTime;
        if(updateTime > updateSpeed)
        {
            updateTime = 0.0f;
            ++currentAnim;
            if(currentAnim == (uint)Anims.TOTAL_ANIMS)
            {
                currentAnim = 0;
            }
            animator.ChangeAnimation(currentAnim);
        }
    }
}
