using Godot;

namespace MyCraft.Scripts;

public partial class DebugLabel : Label
{
    [Export]
    private float _updateFrequencySec = 1f;

    private double _time = 0;

    private bool _keyPressed;

    public override void _Ready()
    {
        _time = _updateFrequencySec;

        DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
        Engine.MaxFps = 60;
    }

    public override void _Input(InputEvent _event)
    {
        InputEventKey keyEvent = _event as InputEventKey;
        if (keyEvent != null)
        {
            if (keyEvent.Keycode == Key.F12)
            {

                _keyPressed = keyEvent.Pressed;

            }
        }
    }

    public override void _Process(double delta)
    {

        if (_keyPressed)
        {
            if (DisplayServer.WindowGetVsyncMode() == DisplayServer.VSyncMode.Enabled)
            {
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);

                Engine.MaxFps = 999;
            }
            else
            {
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);

                Engine.MaxFps = 60;
            }
           
        }
        
        if (_time < _updateFrequencySec)
        {
            _time += delta;
            return;
        }

        _time = 0;
        
        base.Text = $"FPS: {((int)Engine.GetFramesPerSecond()).ToString()}\n" +
                    $"MaxFps: {Engine.MaxFps}\n" +
                    $"Vsync: {DisplayServer.WindowGetVsyncMode()} (F12 to toggle)\n";
    }
}