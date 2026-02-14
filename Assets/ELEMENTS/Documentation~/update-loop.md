# Update Loop

ELEMENTS is primarily reactive and event-driven — most UI should never need per-frame processing. For the cases that do (animations, real-time data, physics-rate logic), the framework provides an **opt-in** update loop tied to Unity's `MonoBehaviour.Update` and `FixedUpdate`.

Only elements and components that explicitly request updates are processed each frame. When nothing is registered, the loop early-returns with zero overhead.

## Two ways to opt in

### 1. Interface-based (Components)

Implement `IUpdatable` and/or `IFixedUpdatable` on a `Component`. Registration happens automatically when the component is built, and deregistration happens automatically on dispose.

```csharp
using ELEMENTS;
using ELEMENTS.Elements;

public class StopwatchHUD : Component, IUpdatable
{
    private float _elapsed;
    private readonly ReactiveProperty<string> _text = new("0.0s");

    protected override IElement Render()
    {
        return new Label(_text);
    }

    public void OnUpdate(float deltaTime)
    {
        _elapsed += deltaTime;
        _text.Value = $"{_elapsed:F1}s";
    }
}
```

You can implement both interfaces on the same component:

```csharp
public class PhysicsDebugHUD : Component, IUpdatable, IFixedUpdatable
{
    private readonly ReactiveProperty<string> _fps = new("");
    private readonly ReactiveProperty<string> _physicsRate = new("");

    protected override IElement Render()
    {
        return new VerticalGroup(
            new Label(_fps),
            new Label(_physicsRate)
        );
    }

    public void OnUpdate(float deltaTime)
    {
        _fps.Value = $"FPS: {1f / deltaTime:F0}";
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        _physicsRate.Value = $"Physics: {1f / fixedDeltaTime:F0} Hz";
    }
}
```

### 2. Fluent callback (any element)

Call `.OnUpdate()` or `.OnFixedUpdate()` in a builder chain. The callback is automatically deregistered when the element is disposed.

```csharp
protected override IElement Render()
{
    return new ProgressBar()
        .OnUpdate(dt =>
        {
            // animate progress each frame
        });
}
```

This follows the same `Disposables` pattern as other fluent methods like `BindVisible` or `BindClassName`.

## Interfaces

### IUpdatable

```csharp
public interface IUpdatable
{
    void OnUpdate(float deltaTime);
}
```

Called once per frame from `MonoBehaviour.Update`. The `deltaTime` parameter is `Time.deltaTime`.

### IFixedUpdatable

```csharp
public interface IFixedUpdatable
{
    void OnFixedUpdate(float fixedDeltaTime);
}
```

Called once per fixed timestep from `MonoBehaviour.FixedUpdate`. The `fixedDeltaTime` parameter is `Time.fixedDeltaTime`. Use this for anything that needs to stay in sync with the physics simulation.

## Registration API

For advanced scenarios, you can register directly with `ElementPortal`. Each method returns an `IDisposable` that deregisters the callback when disposed.

```csharp
IDisposable token = ElementPortal.Register(myUpdatable);        // IUpdatable
IDisposable token = ElementPortal.Register(myFixedUpdatable);   // IFixedUpdatable
IDisposable token = ElementPortal.RegisterUpdate(dt => { });    // Action<float> per frame
IDisposable token = ElementPortal.RegisterFixedUpdate(dt => {}); // Action<float> per fixed step
```

You are responsible for disposing the token when using this API directly. The `Component` and `BaseElement` integrations handle this for you.

## Lifecycle

- **Components**: Registered after `Render()` completes during `BuildVisualElement()`. Deregistered when `Dispose()` is called.
- **Fluent callbacks**: Registered immediately when `.OnUpdate()` / `.OnFixedUpdate()` is called. Deregistered when the element's `Dispose()` runs (which disposes all entries in `Disposables`).
- **Portal disabled/destroyed**: All registrations are cleared when `ElementPortal.OnDisable()` runs.

## Iteration safety

It is safe to dispose other registered elements during an update callback. The loop iterates a snapshot array and checks that each entry is still registered before invoking it. Disposing an element removes it from the set, and it is skipped on the current frame. Self-disposal during a callback works the same way.
