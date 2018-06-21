# LightShafts-Monogame
A port from the LightShafts Tutorial by Nicolas Menzel
http://www.mathematik.uni-marburg.de/~menzel/index.php?seite=tutorials&id=2

![My image](https://github.com/Stephanowicz/LightShafts-Monogame/blob/master/LightShafts.jpg) 
![My image](https://github.com/Stephanowicz/LightShafts-Monogame/blob/master/LightShaftsControl.jpg)

The folder [0 Initial release] contains all converted functions from the original.
There's lot of unused stuff - also the additive blend function is not working - I think there's something missing - so commented it out.

I replaced the original gearmodels as I couldn't load the c4d files in order to edit the normals etc. And I also had problemes with the cube texture masks - so I implemented a simple diffuse light &  specular shader. 
I also added a control-form for testing the lightshaft parameters.

The folder [1 Basic LightShafts] contains the cleared up project with only the necessary functions to get the lightshaft effect running.

The folder [1 Basic LightShafts v2] is an extended version of [1 Basic LightShafts] with some fancy stuff like drawing to desktop background (might not work on Win7). Also added the option to control the sample size of the lightshaft texture.
