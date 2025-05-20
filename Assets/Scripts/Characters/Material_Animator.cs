using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Animator : MonoBehaviour // By Samuel White // UNUSED SCRIPT
{
    //========================================
    // The Enemy Animator.
    // Used to animate the enemy material image.
    //========================================

    public Material Mat { private get; set; }

    [System.Serializable]
    public struct AnimationData
    {
        public string name;
        public string textureName;
        public float duration;
        public int startIndex;
        public int endIndex;
        public bool loop;

        public List<Texture2D> texture;
    }
    public AnimationData[] data;

    public enum AnimationType
    {
        Idle, Attack
    }
    public AnimationType animationType;

    public void Animate(AnimationType type)
    {
        switch (type)
        {
            case AnimationType.Idle:
                Mat.SetTexture(data[0].textureName, data[0].texture[0]);
                break;

            case AnimationType.Attack:
                StartCoroutine(AnimateMaterial(Mat, data[0]));
                break;
        }
    }

    IEnumerator AnimateMaterial(Material material, AnimationData data)
    {
        float t = 0;
        int index = data.startIndex;

        while (t < data.duration)
        {
            t += Global_Game_Speed.GetDeltaTime();

            float normalizedTime = t / data.duration;

            if (normalizedTime >= 1f)
            {
                if (data.loop)
                {
                    t = 0;
                    index = data.startIndex;
                }
                else
                {
                    break;
                }
            }

            material.SetTexture(data.textureName, data.texture[index]);
            index = Mathf.FloorToInt(Mathf.Lerp(data.startIndex, data.endIndex, normalizedTime));
            yield return null;
        }
    }

    public void SetSprite()
    {
        if (data.Length > 0)
        {
            Mat.SetTexture(data[0].textureName, data[0].texture[0]);
        }
        else
        {
            Debug.LogWarning("No animation data found");
        }
    }
}
