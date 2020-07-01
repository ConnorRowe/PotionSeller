using Godot;
using System;

public class PotionCircle : Sprite
{
    public enum FillStates
    {
        Empty,
        Filling,
        Full
    }

    private const float _FillSpeed = 2f;
    private const float _MaxPower = 50f;

    private FillStates _fillState = FillStates.Empty;
    private float _fillGoal = 0f;
    private float _fillAmount = 0f;
    private Sprite _fillSprite;
    private TextureProgress _potionBar;
    private float _potionBarFillMax = 300f;
    private float _potionBarFillCurrent = 0f;
    private float _potionBarAdd = 0;

    public FillStates FillState
    {
        get { return _fillState; }
        set
        {
            _fillState = value;
            FillStateUpdated(value);
        }
    }

    public override void _Ready()
    {
        _fillSprite = GetNode<Sprite>("PotionFill");
        _potionBar = GetParent().GetNode<TextureProgress>("PotionProgress");

        FillState = FillStates.Empty;
    }

    public override void _Process(float delta)
    {
        if (_fillState == FillStates.Filling)
        {
            if (_fillAmount >= _fillGoal)
            {
                _fillAmount = _fillGoal;
                FillState = FillStates.Full;
            }
            _fillAmount = Mathf.Lerp(_fillAmount, _fillGoal, _FillSpeed * delta);

            float spriteScale = Mathf.Clamp(_fillAmount / _MaxPower, 0f, 1f);
            _fillSprite.Scale = new Vector2(spriteScale, spriteScale);
        }

        if (_potionBarAdd > 0)
        {
            var barProgress = _potionBarAdd * delta;
            _potionBarAdd -= barProgress;
            _potionBarFillCurrent += barProgress;

            _potionBar.Value = ((double)_potionBarFillCurrent / (double)_potionBarFillMax) * 120.0;
        }
    }

    public void InitiateCircleFill(float fillGoal)
    {
        FillState = FillStates.Filling;
        _fillGoal = fillGoal;
        _potionBarAdd += fillGoal;
    }

    private void FillStateUpdated(FillStates state)
    {
        switch (state)
        {
            case FillStates.Empty:
                _fillSprite.Scale = Vector2.Zero;
                _fillAmount = 0f;
                break;
            case FillStates.Filling:
                _fillSprite.Scale = Vector2.Zero;
                _fillAmount = 0f;
                break;
            case FillStates.Full:
                break;
        }
    }
}
