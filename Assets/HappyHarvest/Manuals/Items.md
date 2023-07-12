# Writing new Items

You can write your own items.

For this, create a new script that inherit from Item. You will need to 
add a CreateAssetMenu to create it in your project folder (check the Hoe or
SeedBag script for an example).

This class have 2 abstract functions you need to override and complete :

- CanUse which give you the currently targeted cell and should return true if the 
object can be used on that cell, false otherwise.

- Use when the player press the Use button and CanUse return true. This should return
true if the use was successful and false otherwise. This will then "cancel" the use,
this is used e.g. by the Basket when trying to harvest a crop and the inventory have no
space for it. 