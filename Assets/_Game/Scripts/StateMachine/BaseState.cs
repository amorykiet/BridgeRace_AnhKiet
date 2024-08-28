using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T>
{
    public abstract void OnEnter(T owner);

    public abstract void OnExcute(T owner);

    public abstract void OnExit(T owner);
}
