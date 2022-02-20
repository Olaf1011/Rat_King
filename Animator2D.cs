using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Animator2D : MonoBehaviour
{
    //Reference to the sprite renderer components
    public SpriteRenderer spriteRenderer;

    public float updateSpeed;
    
    //Array of sprites to render
    public Sprite[] sprites;

    public int[] startFrames;

    private List<int> m_endFrames = new List<int>();

    public int defaultAnimIndex = 0;

    private int m_currentAnimIndex;

    private int m_currentFrame;
    //Animation update values
    private float m_frameTime;

    private bool m_animChanged;

    public bool lastFrameFlag;
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        m_animChanged = false;
        m_frameTime = 0.0f;

        m_currentAnimIndex = defaultAnimIndex;
        
        for(int i = 0; i < startFrames.Length; ++i)
        {
            if(i == startFrames.Length - 1)
            {
                m_endFrames.Add(sprites.Length - 1);
            }
            else
            {
                m_endFrames.Add(startFrames[i + 1] - 1);
            }
        }

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(paused)
        {
            return;
        }
        m_frameTime += Time.deltaTime;
        if(m_animChanged)
        {
            m_frameTime = 0.0f;
            m_currentFrame = startFrames[m_currentAnimIndex];
            m_animChanged = false;
        }

        if(m_frameTime > updateSpeed)
        {
            m_frameTime = 0.0f;
            ++m_currentFrame;
            
            if(m_currentFrame > m_endFrames[m_currentAnimIndex])
            {
                m_currentFrame = startFrames[m_currentAnimIndex];
            }
        }
        
        spriteRenderer.sprite = sprites[m_currentFrame];
        lastFrameFlag = (m_currentFrame == m_endFrames[m_currentAnimIndex]);
    }

    public void ChangeAnimation(int AnimIndex)
    {
        if(AnimIndex == m_currentAnimIndex)
        {
            return;
        }

        m_currentAnimIndex = AnimIndex;
        m_animChanged = true;
    }

    public void Flip(bool flipOn)
    {
        spriteRenderer.flipX = flipOn;
    }

    public void SetPaused(bool pause)
    {
        paused = pause;
    }

}
