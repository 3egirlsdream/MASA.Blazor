using Masa.Blazor.Components.Slider;
using Masa.Blazor.Utils;

namespace Masa.Blazor;

#if NET6_0
public class SliderInteropHandle<TValue, TNumeric>
#else
public class SliderInteropHandle<TValue, TNumeric> where TNumeric : struct, IComparable<TNumeric>
#endif
{
    private readonly ThrottleTask _throttleTask = new(16);
    private readonly MSliderBase<TValue, TNumeric> _slider;

    public SliderInteropHandle(MSliderBase<TValue, TNumeric> slider)
    {
        _slider = slider;
    }

    [JSInvokable]
    public Task OnMouseDownInternal(ExMouseEventArgs args)
        => _slider.HandleOnSliderMouseDownAsync(args);

    [JSInvokable]
    public Task OnTouchStartInternal(ExTouchEventArgs args)
        => _slider.HandleOnTouchStartAsync(args);

    [JSInvokable]
    public Task OnMouseUpInternal(bool isTouch)
    {
        Console.Out.WriteLine("[SliderInteropHandle] " + (isTouch ? "touchend" : "mouseup"));
        return _slider.HandleOnSliderEndSwiping(isTouch);
    }

    [JSInvokable]
    public Task OnClickInternal(MouseEventArgs args)
        => _slider.HandleOnSliderClickAsync(args);

    [JSInvokable]
    public Task OnMouseMoveInternal(MouseEventArgs args)
        => _throttleTask.RunAsync(() => _slider.HandleOnMouseMoveAsync(args));

    [JSInvokable]
    public Task OnThumbContainerFocusInternal(int index)
        => _slider.HandleOnFocusAsync(index, new FocusEventArgs());
}
