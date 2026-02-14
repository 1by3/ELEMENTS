# ELEMENTS

A code-first UI framework for Unity.

https://elements.1by3.co

## Installation

ELEMENTS can be easily added to any Unity project, running **Unity 6** or above. Unity 6.3+ is recommended to take full advantage of UI Toolkit's shader support and in world UI support.

### Install R3

ELEMENTS depends on [R3](https://github.com/Cysharp/R3). Install it from NuGet using [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity) — search for "R3" and install the latest stable version.

### Install ELEMENTS

Add ELEMENTS via the Unity Package Manager using the following git URL:

```
https://github.com/1by3/ELEMENTS.git?path=Assets/ELEMENTS
```

You can pin a specific version by appending a tag:

```
https://github.com/1by3/ELEMENTS.git?path=Assets/ELEMENTS#2.0.0
```

## Your first Component

In ELEMENTS, your UI is made up of a series of "Components". A Component is a single class that contains both your state and your UI definition.

### Building a Component

Start by making a class that inherits from `Component`. Override the `Render` method to return the element tree that defines your UI...

```csharp
public class ExampleComponent : Component
{
    protected override IElement Render()
    {
        return new VerticalGroup(
            new Label("The button has been clicked 0 times."),
            new Button(new Label("Click Me!"))
        );
    }
}
```

<Example title="ExampleComponent">
  <div>
    <Label>The button has been clicked 0 times.</Label>
    <Button variant="outline">Click Me!</Button>
  </div>
</Example>

We'll talk more about the elements used (`VerticalGroup`, `Label`, and `Button`) shortly. For now, let's talk about how we can make this static UI interactive with state.

### Adding State

State lives directly on your Component as fields. Use `ReactiveProperty` to make state that automatically updates the UI when it changes...

```csharp
public class ExampleComponent : Component
{
    public readonly ReactiveProperty<int> Count = new(0);

    public void Increment()
    {
        Count.Value += 1;
    }

    protected override IElement Render()
    {
        return new VerticalGroup(
            new Label()
                .BindText(Count.Select(count => $"The button has been clicked {count} times.")),
            new Button(new Label("Click Me!"))
                .OnClick(_ => Increment())
        );
    }
}
```

We've done a couple of things here. First, we've bound the `Text` property of our `Label` to a transformed version of the `Count` property. This will make the text of the label update each time `Count` is changed. We've also mapped the `Increment` method to be called when the `Button` is clicked.

You now have a dynamic and interactive UI built with ELEMENTS!

## Rendering your UI

The simplest way to render your UI is from a `MonoBehaviour` using the `RenderElement` extension method on a `UIDocument`...

```csharp
public class GameUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private void OnEnable()
    {
        uiDocument.AddStyleSheet("ELEMENTS/DefaultStyles");
        uiDocument.RenderElement(new ExampleComponent());
    }
}
```

A couple of notes...

- We loaded the default ELEMENTS stylesheet onto the UIDocument using the `AddStyleSheet` helper. This is optional, but highly recommended for a good starting point with styles.
- We used the `OnEnable` method. This is an important detail that allows your UI to update when Unity reloads your game if you make code changes. If your UI is disappearing on reload, this might be why.

If you start your game, you should see your UI on the screen!

## Continue learning...

https://elements.1by3.co/docs
