using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test_Bullet_Pattern_Controller : MonoBehaviour // By Samuel White
{
    [System.Serializable]
    public class BulletPattern
    {
        [Header("Pattern Settings")]

        [Header("Animate Pattern?")] // Should the pattern rotate?
        public bool animate;
        public enum AnimationType { Boomerang, Constant }
        public AnimationType patternType;
        public AnimationCurve lerpCurve;

        [Header("Pattern Segment Data")]

        public UnityEvent patternTrigger;

        [System.Serializable]
        public class PatternSegment
        {
            public string segmentName;
            public Bul_Pat_Adv_SO bulletData;
            [Space(10)]
            public int projectileAmount = 1;
            [Space(10)]
            public int repeats = 1;
            public float repeatRate = 1;
            [Space(10)]
            public bool useLocalSpace = false;
            public Vector3 offset;
            [Space(10)]
            [Range(0, 360)] public float direction;
            [Space(10)]
            public float cooldown = 1;
            [Space(10)]
            [Header("Debug Options")]
            public Color debugColor = Color.white;
            public bool showDebug = true;

            // [Header("Type")]
            public enum PatternType { Wall, Spray, Circle, Square, Triangle, }
            public PatternType patternType;
        }
        public PatternSegment[] patternSegments;

        public List<Test_Enemy_Basic_Projectile> availableProjectiles;
        public List<Test_Enemy_Basic_Projectile> activeProjectiles;
    }
    public List<BulletPattern> bulletPattern;

    public void StartPattern(int patternID)
    {
        bulletPattern[patternID].patternTrigger.Invoke();

        // StartCoroutine(Process());
    }

    IEnumerator Process()
    {

        yield break;
    }

    void OnDrawGizmosSelected()
    {
        
    }
}
