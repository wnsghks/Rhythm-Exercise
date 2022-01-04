using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OptionType { Button, Slider, Text }

public interface IOption
{
    public OptionType type { get; }
}

public interface IOptionButton : IOption
{
    void Process();
}

public interface IOptionSlider : IOption
{
    void Process( int _value );
}

public interface IOptionA
{
    public OptionType type { get; }
    public void Process();
}