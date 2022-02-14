### Base terrain generation
Procedural means generating data algorithmically, in world of terrains and landscapes we do this with procedural noises, almost all specialized terrain generation software world machine, world creator etc. use procedural noises to generate terrain.

Blender is not a specialized terrain creation software but it has built in support for generating different noises.

We can do this using blender displacement modifiers with procedural noise textures for displacement.

**_Before we begin production, it is always a good idea to do some post production so that we don't end up making decisions on the fly, with that said here is our design document._**

Terrain size : 1 km   
Type : typical grasslands with forest areas  
Time of day : clear evening   
Reference images / mood-board

![Image](images/moodBoard.jpg)

So now for production, since terrains are large make sure to set camera far clipping to a large value and scene SI units to metric.

![Image](images/01.png) ![Image](images/02.png)

Add a new plane of 1-km, rename it to **Terrain**, and add some subdivisions using subdivision modifier, I have raised poly count to 1-k to get nice large details.

![Image](images/03.png) ![Image](images/04.png)

So now far actual displacement, **_the way for creating terrain shapes is such that we begin by creating large shapes first than moving on to medium and finally adding small fine details, there are other ways to do this, but here I will use displacement modifiers with procedural textures_**

Select **terrain** game object and from modifiers tab add a new displacement modifier, rename it to **large shapes**.
Add a new texture for this modifier, rename it to large **large shapes_tex**.
You can play with settings of both the displacement modifier and it's corresponding texture, or just copy mine.

![Image](images/04a.png) ![Image](images/04b.png)

You may have notice by default we cannot randomize noise for some fixed values however there is a fix to it, add a new empty **Add > Empty > PlainAxis** rename it to **largeShapes_01_mod**, select **large shapes** displacement modifier, change coordinates to **object** and from **Object** drop down select **largeShapes_01_mod**.  
Now move, rotate, scale **largeShapes_01_mod** to randomize terrain shape.

![Image](images/04c.png) 

Now I have created 2 more displacement modifiers, to add some variation and to create medium terrain shapes, 

![Image](images/05.png) 

I have also added a average human size **(Dimensions x = 0.25m y = 0.25m z = 1.8m)** cube to get judgement about terrain.

![Image](images/06.png) 

As a final step for adding fine details I added another subdivisions modifier raising poly count to 2k and added a new displacement modifier with a perlin noise with subtle strengths, here is my final modifiers stack.

![Image](images/06a.png) 

And there you have it a procedural terrain.

![Image](images/07.png) 

### Creating a height map & importing in unity

**Creating a height map is easy, I will you through the process step by step.**

1. Open the blender file from last part or (final.blend) from project files, from output properties tab, set format settings as...

   ![Image](images/heightMap/00.png)

2. Create a new camera, set it's mode to orthographic and set the settings as follows...

   ![Image](images/heightMap/01.png)

3. Position the camera right above the terrain center, go to camera view (**view > camera > active camera**), make sure entire terrain is completly visible and there is no clipping and that terrain is entirly covered by camera.

   ![Image](images/heightMap/02.png)
   ![Image](images/heightMap/03.png)

4. Now we will create a height map, create a new shader, rename it to **heightMap** and replace default principled shader with emission shader.

    ![Image](images/heightMap/04.png)

   **_"A height map is a black and white image representation of terrain, where each pixel represents a position on terrain, black at value 0 being the lowest and white at value 1 the highest part of terrain."_**
 
5. We only need the z-component of the vertices I.e height value, so to get the position of vertices add a new **geometry node** and a **seperate xyz** node, connect output of position from **geometry node** to vector input of **seperate xyz** node.

   ![Image](images/heightMap/05.png)
 
6. Ideally when rendering out a height map, the lowest part of mesh should have a height value of 0 and top height value should be calculated relative to lowest value, however blender is not a specialized terrain creation software and we need a work around.

7. Add a new **Map to range** node, connect **z** output of **seperate xyz** to input value of **Map to range**.

   ![Image](images/heightMap/06.png)

8. Add a new **Add > Mesh > Plane** scale it up a bit, snap it to lowest most part of terrain and note its position's z value, and plug that into **Map to range** **from min value**, similarly snap the plane to top most vertex of terrain and plug position's z value to **from max value** of **Map to range**.
Here are my values.

   ![Image](images/heightMap/07.png)

9. To make sure image is black and white completly in range 0 to 1 connect output of **Map to range** to a **ColorRamp** node and finally plug color output of **ColorRamp** to color input of **Emission shader**.

   ![Image](images/heightMap/08.png)

10. Now we need to change some settings in order to render a proper height map
   * first switch to material view to view the current output of height map shader.
   * from scene settings change rendering engine to cycles and samples to 1.
   * Blender by default apply some view transformation to final image, however we only need the raw black and white image, so to get the raw image select **View Transform** to **Raw** in **Color Management** in render properties tab.

     ![Image](images/heightMap/09.png)
     ![Image](images/heightMap/10.png)

11. My final material view...

    ![Image](images/heightMap/11.png)

12. From Outliner hide all objects from both scene and render views, except terrain and camera, finally hit f-12 to bake the height map, save the heigh map as .png with following settings.

    ![Image](images/heightMap/12.png)

13. Unity terrain system can only accept **.raw** file formats, so to convert **.png** to raw use some online image format conversion, however here I have opened it in photoshop and saved it as **photoshop raw**.

14. 

### Creatign a splat-map and texturing
In last tutorial we created a height map and imported into unity for testing, In this part we will create a splat map in blender and use it to texture out terrain in unity.
This will not be a step by step tutorial rather I will give you an overview of the process.

So what is a splat map...? basically splat mapping is a procedural texturing technique for terrains, a splat map is just another image where each RGBA channel of image corresponds to a terrain texture on terrain, for example 
R(Red) channel can be = dirt texture on terrain
G(Green) channel can be = grass texture
B(Blue) channel can be = rock / cliff
A(Alpha) channel can be = snow 

So now you know what a splat map is, now I will walk you through the process on how to create a splat map.

Before proceeding just a disclaimer creating splat maps requires good understanding if some mathematical concepts and intermediate knowledge of blender cycles nodes, so if you just want a quick splat map shader use the one provided with project files.

Since our terrain has no rocks or cliffs instead we have forests, so in our splat map

red channel = dirt / ground texture
green channel = grass texture
blue = forests map
alpha = road overlay map

First for red and green channels, I choose all the surface that are flat beyond some threshold, (since buildings will be placed on flat surfaces) to be dirt / ground texture and everything else is green channel. 
Terrain surface steepness can be calculated using the formula 
and to control the blending between steep and flat surfaces I am using a logistics function formula

For forest map (blue) I am using a noise texture and over laying it on top of existing channels

and that's it for a now we will create road overlays map (alpha channel) in some next part.

Now to bake this splat map,  create new texture splat map 2048 * 2048 with alpha channel.

Add a new *image in terrain **splatMap** shader without connecting it to anything, make sure to select it.

