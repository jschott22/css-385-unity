The interaction model in this commit consists of two parts:

1) The ability for the player to move around, collide with objects, move their camera, and use the right mouse to zoom in

2) The ability to look at controls and interact with them (as demonstrated by the impulse throttle)

The objective of the interaction is to be able to walk around, look at a control, and interact with it (eventually will be flying a spaceship)

Write-Up:

Ideally, this interaction model would be extended for multiple controls in different parts of the ship. The player would walk around, move from one control to another, interact with it, and then move on to the next. Currently there is not much to look at or do, but there is one control that can be increased and decreased (impulse throttle, which determines the speed of the ship). The player can interact with this control and will eventually see the results of the interaction displayed on the outside of the ship's window. Other controls may have different control schemes, so variability will be necessary (currently the control's script is implemented as an interface for easier abstraction in the future).