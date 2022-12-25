using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BooleanOption { Off, On, Count }

public enum Alignment { Left, Center, Right, Count, }

public enum PitchType { None, Normalize, Nightcore, Count, }

public enum GameRandom
{
    None,
    Mirror,
    Basic_Random,
    Half_Random,
    Max_Random,
    Count,
}

[Flags]
public enum GameMode
{
    None     = 0,
    AutoPlay = 1 << 0,
    NoFail   = 1 << 1,
    NoSlider = 1 << 2,

    All      = int.MaxValue,
}

[Flags]
public enum GameVisualFlag
{
    None        = 0,
    TouchEffect = 1 << 0,
    LaneEffect  = 1 << 1,
    ShowMeasure = 1 << 2,
    ShowGearKey = 1 << 3,

    All         = int.MaxValue,
}

public class GameSetting
{
    // Mode
    public static GameVisualFlag CurrentVisualFlag    = GameVisualFlag.All;
    public static GameMode       CurrentGameMode      = GameMode.AutoPlay | GameMode.NoFail;
    public static GameRandom     CurrentRandom        = GameRandom.None;
    public static Alignment      CurrentGearAlignment = Alignment.Center;
    public static PitchType      CurrentPitchType     = PitchType.None;

    // Speed
    private static double OriginScrollSpeed = 7.3d; 
    public static double ScrollSpeed
    {

        get => OriginScrollSpeed;
        set
        {
            if ( value < 1d ) return;
            OriginScrollSpeed = value;
        }
    }

    public static double Weight => ScrollSpeed * 320d;
    public static double PreLoadTime => 1200d / Weight;

    // Sound
    public static readonly int DefaultSoundOffset = 50;
    public static int SoundOffset = 0;

    // Opacity Percentage ( 0 ~ 100 )
    public static float BGAOpacity   = 100f;
    public static float PanelOpacity = 100f;

    // IO
    public static readonly string SoundDirectoryPath = System.IO.Path.Combine( Application.streamingAssetsPath, "Songs" );
    public static readonly string FailedPath         = System.IO.Path.Combine( Application.streamingAssetsPath, "Failed" );

    // Measure
    public static float MeasureHeight = 2f;

    // Jugdement
    private static float DefaultJudgePos = -340f;
    public static float JudgePos => DefaultJudgePos + JudgeOffset;
    public static float JudgeOffset = 0f;
    public static float JudgeHeight = 50f;

    // note
    public static float NoteSizeMultiplier = 1f;
    public static float NoteWidth  => 112f * NoteSizeMultiplier;
    public static float NoteBodyWidth => 107f * NoteSizeMultiplier;
    public static float NoteHeight => 62f * NoteSizeMultiplier;
    public static float NoteBlank  = 0f;
    public static float NoteStartPos => -( ( NoteWidth * ( NowPlaying.CurrentSong.keyCount - 1 ) ) + ( NoteBlank * ( NowPlaying.CurrentSong.keyCount + 1 ) ) ) * .5f;

    // Gear
    public static float GearStartPos => ( -( ( NoteWidth * NowPlaying.CurrentSong.keyCount ) + ( NoteBlank * ( NowPlaying.CurrentSong.keyCount + 1 ) ) ) * .5f );
    public static float GearWidth    => ( ( NoteWidth * NowPlaying.CurrentSong.keyCount ) + ( NoteBlank * ( NowPlaying.CurrentSong.keyCount + 1 ) ) );

    // Pitch
    private static int pitch = 100;
    public static float CurrentPitch { get { return pitch * .01f; } set { pitch = ( int )value; } }

    // PPU
    public static int PPU = 100; // pixel per unit

    // Debug
    public static bool IsAutoRandom = false;
}
