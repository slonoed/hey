# Code style

Filters field named "somethingFilter"
Loops use "(x)i" var name for index where "(x)" contains first chars of words in filter name.
Example

```
EcsFilter<Player> myCoolPlayerFilter = null

...
foreach (var mcpi in myCoolPlayerFilter) {
  ...
}
```