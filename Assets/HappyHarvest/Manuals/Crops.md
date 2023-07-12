# Creating new Crops

To create a new type of crops your will need 3 elements :

- a **Product** which is the end product the crop produce (Apple, Corn Cob etc.)
- a **Crop** that is the thing that grow from the ground
- a **SeedBag** which is the item that the player use to plant the Crop

## Creating the Product

To create a new product, right click in your project folder and choose 
_2D Farming >  Item > Product_

This will have 3 settings to set :
- The sprite to use for that product in the inventory
- How many of those products can stack in one inventory slot
- If the Product is Consumable, meaning removed from inventory when used.
(you probably want to leave that to true)

## Creating the Crop

Right click and choose _2D Farming > Crop_

The crop has 5 settings 

- Growth Stage Tiles : the tiles for each stage of growth of the Crop. Need at least 1.
- Produce : the Product that this crop will produce when harvested.
- Growth Time : how long, in second, the plant take to grow to maturity
- Number of Harvest : how many time can the crop be harvested
- Product Per Harvest : How many Products is given everytime you harvest

## Creating the Seed bag

Right click and choose _2D Farming > Items > SeedBag_

The seed bag have a setting to say which crop it will plant and normal item settings like
how many can stack and if consumable (which you probably want to keep true for seed so they
get used when planted)
