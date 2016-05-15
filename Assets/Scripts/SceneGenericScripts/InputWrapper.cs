using UnityEngine;
using System.Collections;

/// <summary>
/// Wrapped user input.
/// Only used to gain control over user input and restrict it in some cases.
/// </summary>
public static class InputWrapper {

    public static bool inputEnabled = false;

    public static float GetAxisRaw(string designation)
    {
        float retval = 0;

        if (inputEnabled)
            retval = Input.GetAxisRaw(designation);

        return retval;
    }

    public static bool GetButtonDown(string designation)
    {
        bool retval = false;

		if (inputEnabled || designation.Equals("Target") || designation.Equals("Pause"))
            retval = Input.GetButtonDown(designation);

        return retval;
    }
}
