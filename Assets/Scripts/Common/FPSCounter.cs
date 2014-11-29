using UnityEngine;
using System.Collections;

public class FPSCounter
{
    private float m_fUpdateInterval = 1.0f;

    private float m_fLastInterval;

    private int m_iFrames = 0;

    private float m_fFps;

    public float FPS
    {
        get { return m_fFps; }
    }

    public FPSCounter()
    {

    }

    public void Start()
    {
        m_fLastInterval = Time.realtimeSinceStartup;

        m_iFrames = 0;
    }

    public void Update()
    {
        ++m_iFrames;

        if (Time.realtimeSinceStartup > m_fLastInterval + m_fUpdateInterval)
        {
            m_fFps = m_iFrames / (Time.realtimeSinceStartup - m_fLastInterval);

            m_iFrames = 0;

            m_fLastInterval = Time.realtimeSinceStartup;
        }
    }
}