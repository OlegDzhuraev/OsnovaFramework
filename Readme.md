## Osnova Framework

### About
Osnova Framework is an ECS-like and DOD pattern implementation. It exists just because any other ECS doesn't fully fit my needs. 

### Disclaimer
Actual framework version is very raw, so there can be a lot of changes in API and namings in the future, absolutely not recommended for production use now.

### Pros and cons

#### Pros
- Should be more intuitive than others ECS for Unity developers, who previously used classic MonoBehaviour approach.
- Direct access from code to the any Unity features like UI, Physics, and all GameObject fields and methods.
- Easy to debug in runtime: you can see and modify any fields in components using Unity inspector.
- Speedups iterative development because there no recompile delays when changing systems amount and order, components added to the entity, etc. Because you do it in the Unity inspector, not from code.
- Based on MonoBehaviour, minimum boilerplate code, no code generators.

#### Cons
- It is made to cover my needs, so nobody guarantees updates and any support.
- It is not about multi-thread now.
- And even not about optimization and performance boosts.

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
It is recommended to place in component only runtime logic variables used by systems and if something requires setup via drag-n-drop from the scene (prefab bones, etc.).

Not recommended to place there any links to the assets or pre-defined settings, better use one ScriptableObject for it.  It will keep clean your prefabs and make easier to debug runtime values.

### Systems
System is a ScriptableObject-based class, which will run your logic.

To create a new system, make a new class and derive it from **BaseSystem**.

Override **Update** method and put your logic into it.

```c#
using OsnovaFramework;

public class MoveSystem : BaseSystem
{
    readonly Filter<MoveComponent> filter = Components.Filter<MoveComponent>();

    public override void Update()
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

After creating a new system, you need to use **Top Menu -> Osnova Framework -> Generate Systems** button. It will generate ScriptableObject assets of systems, which you will able to place in the **Layer** list using drag-n-drop. 

#### Systems run order
Sometimes it is needed to run systems in a specific order. You can define your Systems run order in the **Layer** settings.

### Layers
Layer is a MonoBehaviour-based class, which you can add to any object on your scene. Layer allows to setup all systems, which should run in this *layer*. For example, you can separate systems used by different features by the different Layers.

#### Layers run order
Actually **Layers have no specific run order support**, so it is preferred to use one Layer for all systems, if your systems are order-sensitive.

### Filters
Filters allow you to get all type-specific components. For example, in MoveSystem you need to gather all MoveComponents from entities. There you use the filter.

Basic of usage you can see in the Systems partition.

Filters can be more complex:
```c#
// returns all MoveComponents from entities, which also have PlayerComponent
var filter = Components.Filter<MoveComponent>().With<PlayerComponent>();

// returns all EnemyComponents from entities, which also have GunComponent and have NO IdleStateComponent
var anotherFilter = Components.Filter<EnemyComponent>().With<GunComponent>().Without<IdleStateComponent>();
```

### Examples
Someday it will appear here.

### License
MIT License.