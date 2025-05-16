using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class Player_Controller_Rumble : MonoBehaviour //  By Samuel White (Scrapped since it requires me to rework the Main Menu)
{
    private class RumbleData
    {
        public float defualtLowFreq = .25f;
        public float lowFreq;
        public float defualtHighFreq = .75f;
        public float highFreq;
        public float endTime;
        public bool isRumbling;
        public float currentTime;
    }

    private Gamepad gamepad;
    private RumbleData rumbleData = new();
    private PlayerInput playerInput;
    private InputDevice inputDevice;

    private Coroutine rumbleRoutine;
    [SerializeField] bool isGamepad;

    private void Start() 
    {
        playerInput = GetComponent<PlayerInput>();
        inputDevice = playerInput.devices[0];
        if (inputDevice is Gamepad gPad)
        {
            gamepad = gPad;
            isGamepad = true;
        }
        else
        {
            gamepad = null;
            isGamepad = false;
        }
        Debug.Log($"{name} Input Device is: {inputDevice.name}");
    }

    private IEnumerator RumbleRoutine(float intensity, float duration, float targetIntensity)
    {
        float currentIntensity = 0;
        while (!rumbleData.isRumbling && rumbleData.currentTime < rumbleData.endTime)
        {
            rumbleData.currentTime -= Global_Game_Speed.GeUnscaledtDeltaTime();
            if(intensity == targetIntensity) yield return null;
            else
            {
                currentIntensity = Mathf.InverseLerp(intensity, targetIntensity, rumbleData.currentTime);

                rumbleData.lowFreq = Mathf.Clamp01(rumbleData.defualtLowFreq / currentIntensity);
                rumbleData.highFreq = Mathf.Clamp01(rumbleData.defualtHighFreq / currentIntensity);

                gamepad.SetMotorSpeeds(rumbleData.lowFreq, rumbleData.highFreq);
            }
            yield return null;
        }
        StopRumble();
        yield break;
    }

    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Gamepad.html
    #region Start Rumble
    public void StartRumble(float intensity, float duration, float targetIntensity)
    {
        if(!isGamepad || !Settings_Manager.controllerVibration) return;

        rumbleData.lowFreq = Mathf.Clamp01(rumbleData.defualtLowFreq / intensity);
        rumbleData.highFreq = Mathf.Clamp01(rumbleData.defualtHighFreq / intensity);

        rumbleData.endTime = Mathf.Max(rumbleData.endTime, Time.unscaledTime + duration);
        rumbleData.isRumbling = true;

        gamepad.SetMotorSpeeds(rumbleData.lowFreq, rumbleData.highFreq);

        rumbleRoutine = StartCoroutine(RumbleRoutine(intensity, duration, targetIntensity));
    }
    #endregion

    #region Stop Rumble
    public void StopRumble()
    {
        if(!isGamepad) return;
        gamepad.SetMotorSpeeds(0f, 0f);
        rumbleData.isRumbling = false;
        rumbleData.currentTime = 0f;
        rumbleData.endTime = 0f;
    }
    #endregion

    #region Contingencies
    // private void OnDisable()
    // {
    //     StopRumble();
    // }

    // private void OnApplicationQuit()
    // {
    //     StopRumble();
    // }
    #endregion
}
