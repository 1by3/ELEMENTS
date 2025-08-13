# ELEMENTS

A code-first, MVVM based UI framework for Unity.

https://elements.1by3.co

## Installation

ELEMENTS can be easily added to almost any Unity project, so long as you are using **Unity 6** or above. Backwards compatibility is planned, but a low priority at this time.

### Install R3

1. Install [R3](https://github.com/Cysharp/R3) from NuGet using [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity). After installing NuGetForUnity, open NuGet > Manage NuGet Packages, search for "R3", and install the latest stable version.

![Installing R3 in NuGetForUnity](https://elements.1by3.co/_next/image?url=%2F_next%2Fstatic%2Fmedia%2Fr3-via-nuget.fb776318.png&w=1920&q=75&dpl=dpl_9sihqjQS56N5eEjwAgZyAXtKGVYE)

### Install ELEMENTS via UPM

2. Click on Window > Package Manager. Click the "+" button in the top left corner, and select "Add package from git URL...".

![Installing via Git URL](https://elements.1by3.co/_next/image?url=%2F_next%2Fstatic%2Fmedia%2Finstall-via-git-url.058675f9.png&w=1920&q=75&dpl=dpl_9sihqjQS56N5eEjwAgZyAXtKGVYE)

3. Enter the following Git URL for ELEMENTS...

```
https://github.com/1by3/ELEMENTS.git?path=Assets/ELEMENTS
```

You can specify a specific commit, branch, or release tag version to install by appending it with a `#` in between...

```
https://github.com/1by3/ELEMENTS.git?path=Assets/ELEMENTS#1.0.0
```

## Your first Component

In ELEMENTS, your UI is made up of a series of "Components". A Component is a pairing of a View and a View Model.

### Building a View

The View in your Component houses the code that actually defines your UI. Start by making a class that inherits from `View<T>` where `T` is the type of your View Model. We'll cover the View Model shortly, for now, here's some example code...

```csharp
public class ExampleView : View<ExampleViewModel>
{
  public IElement Render()
  {
    // Return your elements here
  }
}
```

Now, in the View's `Render` method, you can return the "markup" for your View. Here's a basic View that shows a Label and a Button stacked on top of each other...

```csharp
public class ExampleView : View<ExampleViewModel>
{
  public ExampleView(ExampleViewModel viewModel) : base(viewModel)
  {
  }

  protected override IElement Render()
  {
    return new VerticalGroup(
        new Label("The button has been clicked 0 times."),
        new Button(new Label("Click Me!"))
    );
  }
}
```

<Example title="ExampleView">
  <div>
    <Label>The button has been clicked 0 times.</Label>
    <Button variant="outline">Click Me!</Button>
  </div>
</Example>

We'll talk more about the elements used (`VerticalGroup`, `Label`, and `Button`) shortly. For now, let's talk about how we can make this static UI interactive with a View Model.

### Building a View Model

A View Model is paired with your View and provides the data and logic to power your UI. This decoupling ensures that your UI code stays organized and scalable. To create a View Model, define a class that inherits from `ViewModel`. Remember that our View from the last step inherited from `View<ExampleViewModel>`. Now, we're defining `ExampleViewModel`...

```csharp
public class ExampleViewModel : ViewModel
{

}
```

The View Model instance can be accessed from your View using the `View` property. This will be shown momentarily. First, let's define a `ReactiveProperty` to house a integer which will represent how many times the button has been clicked, as well as a method to increment it...

```csharp
public class ExampleViewModel : ViewModel
{
  public readonly ReactiveProperty<int> Count = new(0);

  public void Increment()
  {
    Count.Value += 1;
  }
}
```

Note that we made the `Count` property `readonly`. We do not want anything changing `Count` since the actual value is contained within `Count.Value`.

### Binding the View and View Model

Now that we have a View and a View Model, we can bind our View to the View Model. Here's what our View will look like once it's been bound...

```csharp
public class ExampleView : View<ExampleViewModel>
{
  public IElement Render()
  {
    return new VerticalGroup(
      new Label()
        .BindText(ViewModel.Count.Select(count => $"The button has been clicked {count} times.")),
      new Button(new Label("Click Me!"))
        .OnClick(_ => ViewModel.Increment())
    );
  }
}
```

We've done a couple of things here. First, we've bound the `Text` property of our `Label` to a transformed version of the `ViewModel.Count` property. This will make the text of the label update each time `ViewModel.Count` is changed. We've also mapped the `Increment` method of our View Model to be called when the `Button` is clicked.

You now have a dynamic and interactive UI built with ELEMENTS!

## Rendering your UI

The easiest way to render your UI is to create an Element Portal. An Element Portal is a wrapper around a UI Toolkit `UIDocument`. It sets up the container structure for ELEMENTS to function correctly, particularly elements like `Dialog` and `Alert`.

Start by creating your own Element Portal class...

```csharp
public class MyElementPortal : ElementPortal
{
}
```

The `ElementPortal` class dervies from `MonoBehaviour` so you can now drag this script onto an object in your scene. Also create a `UIDocument` on that same object, and connect it to the element portal component.

Next, render the component you built above inside of the portal...

```csharp
public class GameElementPortal : ElementPortal
{
  protected override void OnEnable()
  {
    base.OnEnable();
    StyleSheet("ELEMENTS/DefaultStyles");
    RenderComponent<ExampleViewModel, ExampleView>();
  }
}
```

A couple of notes...

- We called the `StyleSheet` method and passed it the resource path to the default ELEMENTS stylesheet. This is optional, but highly recommended for a good starting point with styles.
- We used the `OnEnable` method. This is an important detail that allows your UI to update when Unity reloads your game if you make code changes. If your UI is disappearing on reload, this might be why.

If you start your game, you should see your UI on the screen!

## Continue learning...

https://elements.1by3.co/docs
