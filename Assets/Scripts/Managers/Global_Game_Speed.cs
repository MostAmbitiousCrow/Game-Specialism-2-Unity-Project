using UnityEngine;

public static class Global_Game_Speed // By Samuel White
{
    //==========================================
    // The Global Game Speed used for scripts to obtain deltatimes multiplied by the game speed.
    // =========================================

    public static float GetDeltaTime()
    {
        return Time.deltaTime * Settings_Manager.gameSpeed;
    }

    public static float GetFixedDeltaTime()
    {
        return Time.fixedDeltaTime * Settings_Manager.gameSpeed;
    }

    public static float GetUnscaledDeltaTime()
    {
        return Time.unscaledDeltaTime * Settings_Manager.gameSpeed;
    }
}
