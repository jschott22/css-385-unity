using UnityEngine;
using System.Collections.Generic;

public interface IControllable
{
    public void handleInputs(List<KeyCode> inputs);
}
