using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIParticleScale : MonoBehaviour
{
    private List<ScaleData> scaleDatas = null;
    void Awake()
    {
        scaleDatas = new List<ScaleData>();
        foreach (ParticleSystem p in transform.GetComponentsInChildren<ParticleSystem>(true))
        {
            scaleDatas.Add(new ScaleData() { transform = p.transform, beginScale = p.transform.localScale });
        }
    }

    void Start()
    {
        float designWidth = 1080;//����ʱ�ֱ��ʿ�
        float designHeight = 1920;//����ʱ�ֱ��ʸ�
        float designScale = designWidth / designHeight;
        float scaleRate = (float)Screen.width / (float)Screen.height;

        foreach (ScaleData scale in scaleDatas)
        {
            if (scale.transform != null)
            {
                if (scaleRate < designScale)
                {
                    float scaleFactor = scaleRate / designScale;
                    scale.transform.localScale = scale.beginScale * scaleFactor;
                }
                else
                {
                    scale.transform.localScale = scale.beginScale;
                }
            }
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        Start(); //Editor���޸���Ļ�Ĵ�СʵʱԤ������Ч��
    }
#endif
    class ScaleData
    {
        public Transform transform;
        public Vector3 beginScale = Vector3.one;
    }
}