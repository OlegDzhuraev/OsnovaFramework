## Osnova Framework
This repository is archived since I don't plan to update it anymore.

### About
Osnova Framework is an ECS-like framework. Primary idea is to move as much of settings as possible to the Unity Inspector to make better connectivity of ECS with Unity.

I made this project to check is it a good idea. And finally I can say: this is not the best idea. :) So use this framework at your own risk.

### Disclaimer, updates and roadmap
Actual framework version is very raw, so there can be a lot of changes in API and namings in the future, absolutely not recommended for production use now.
Also there is a very small chance that framework will be actively updated, "About" section describes why.

### Pros and cons

#### Pros
- Should be more intuitive than others ECS for Unity developers, who previously used classic MonoBehaviour approach.
- Direct access from code to the any Unity features like UI, Physics, and all GameObject fields and methods.
- Easy to debug in runtime: you can see and modify any fields in components using Unity inspector.
- Speedups iterative development because there no recompile delays when changing systems amount and order, components added to the entity, etc. Because you do it in the Unity inspector, not from code.
- Based on MonoBehaviour, minimum boilerplate code, no code generators.

#### Cons
- It is made to cover my needs, so nobody guarantees updates and any support.
- It is not about multi-thread.
- And even not about optimization and performance boosts. It can be really slow on 1000+ entities.

### Requirements and install
Unity version with C# 9.0 features support.

There is two installation options:
1. Install framework using Unity Package Manager button "Add package from git URL...". There you'll need to insert a git clone url.
2. Clone repository and place it to the Assets folder as it is.

### Entities
Entity is a simple Unity component, which you need add to the any object, which will be handled by Osnova Framework (and which require at least one component, other objects not necessarily should be entities).

### Components
Component is a MonoBehaviour-based class, which you can add to your entities by the single drag-n-drop. It will store entity data, used by a Systems. No logic should be placed in components.

To create a new component, make a new class and derive it from **BaseComponent**.

```c#
using OsnovaFramework;

public class MoveComponent : BaseComponent
{
    public Vector3 Direction;
    public float Speed;
}
```
It is recommended to place in component only runtime logic variables used by systems and things which requires setup via drag-n-drop from the scene (prefab bones, etc.).

Not recommended to place there any links to the assets or pre-defined settings, better use one ScriptableObject for it. It will keep clean your prefabs and make easier to debug runtime values.

### Systems
System is also a MonoBehaviour-based class, which will run your logic.

To create a new system, make a new class and derive it from **BaseSystem**.

Override **Init** method to put some initialization logic.
Override **Run** method and put your update/tick logic into it.

```c#
using OsnovaFramework;

public class MoveSystem : BaseSystem
{
    readonly Filter<MoveComponent> filter = new (); // C# 9.0 feature

    public override void Init()
    {
        // you can place some init or cache logic there.
    }
    
    public override void Run()
    {
        foreach (var moveComponent in filter)
        {
            var direction = moveComponent.Direction.normalized;
            var moveOffset = direction * (moveComponent.Speed * Time.deltaTime);
            
            moveComponent.transform.position += moveOffset;
        }
    }
}
```

After creating a new system, you need to add your system to your Layer object and drag'n'drop its component into the **Layer** systems list. 
It can be look like a good idea to place some variables into system code to edit from inspector, but it is not recommended to do - prefer loading any data into Entities components in System Start method or any other way.

#### Systems run order
Sometimes it is needed to run systems in a specific order. You can define your Systems run order in the **Layer** settings.

### Layers
Layer is a MonoBehaviour-based class, which you can add to any object on your scene. Layer allows to setup all systems, which should run in this *layer*. For example, you can separate systems used by different features by the different Layers.

You can specify how to run Layer, there is 3 options:
- Update (most of game logic usually run there, so this is default value)
- FixedUpdate (physics or networking is better to run there)
- Custom (manually from any of your code)

This value can be changed from inspector.

#### Layers run order
Actually **Layers have no specific run order support**, so it is preferred to use one Layer for all systems, if your systems are order-sensitive.

### Filters
Filters allow you to get all type-specific components. For example, in MoveSystem you need to gather all MoveComponents from entities. There you use the filter.

Basic of usage you can see in the Systems partition.

Filters can be more complex:
```c#
// returns all MoveComponents from entities, which also have PlayerComponent
var filter = new Filter<MoveComponent>().With<PlayerComponent>();

// returns all EnemyComponents from entities, which also have GunComponent and have NO IdleStateComponent
var anotherFilter = new Filter<EnemyComponent>().With<GunComponent>().Without<IdleStateComponent>();
```

You can take first element of the filter. It can be used for unique components:
```c#
var input = new Filter<PlayerInput>().First(); // if there no PlayerInput component, value will be null
```

### Signals
Signal is like a component, but live only one frame and works faster than usual components because don't use any Unity things like MonoBehaviour. It can be used like events. 
It is allowed to have only one instance for each signal type, so you cant send two similar signals in one frame.

Making a new signal:
```c#
using OsnovaFramework;

public class GunShotSignal : Signal
{
    // it can contain any data, or no data at all
    public GameObject ShootedBy;
}
```

You can signal to specific entity, or you can send it globally, which allows check it in all systems.

```c#
// Sending an Entity signal
Entity.AddSignal<GunShotSignal>();
// or you can pass it with some initial data
Entity.AddSignal(new GunShotSignal() { ShootedBy = gameObject });

// Reading an Entity signal
var signal = Entity.GetSignal<GunShotSignal>();

// Sending a Global signal
GlobalSignal.Add<GunShotSignal>();
// or you can pass it with some initial data
GlobalSignal.Add(new GunShotSignal() { ShootedBy = gameObject });

// Reading Global Signal
var globalSignal = GlobalSignal.Get<GunShotSignal>();
```

Signals is sensitive to the systems order.

### Settings
You can easily add any settings for your components and systems using a custom ScriptableObject asset. You can create new type of SO, where you can place any of your data assets with settings.

To create an instance of your SO asset, use context menu: **Right Click => Your SO Name** in the Project Window. Now, you can drag'n'drop it into any Layer Settings list.
And now you can access  this asset from any of your systems just using code like this:
```c#
using OsnovaFramework;

public class SomeSystem : BaseSystem
{
    public override void Run()
    {
        // Layer property declared in the Base system and auto-initialized by framework code
        var yourSettings = Layer.GetSettings<YourSettingsType>();
    }
}
```
You can cache it to a variable in the Start method to optimize performance for a bit.

In these SO you can store gameplay parameters, for example. Keep it simple - not recommended to store there any big resources like links to the textures or huge prefabs.

### Examples
Someday it will appear here.

### License
MIT License.
