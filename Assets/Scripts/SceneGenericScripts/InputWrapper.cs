using UnityEngine;
using System.Collections;

public static class InputWrapper {

    public static bool inputEnabled = false;

    public static float GetAxisRaw(string designation)
    {
        float retval = 0;

        if (inputEnabled)
            retval = Input.GetAxisRaw(designation);

        return retval;
    }
}
