Authors:
	Matt Grayston
	Christopher Nielson

CS 3500 - Software Practice
SpaceWars

Instructions:

1. How to Connetect:
	a. Enter server address
	b. Enter username
	c. Click connect

2. How to play:
	a. Left and right arrow buttons to stear ship
	b. Forward button to accelerate
	c. Spacebar to shoot

3. Game Rules/Objectives:
	a. Shoot as many opponents as possible without being shot/running into stars.
	b. Die after 5 hits.
	c. Score is shown to the right

4. Additional Mode
	An additional mode in which all stars move randomly was added. This mode can be turned on by setting the Mode tag in 
	the settings file to MovingStars. The normal, default mode, is Default.

Design Decisions:
	A timer was used to modulate the client framerate; when the timer ticks, the
	Panels are invalidated to redraw. Movement commands are captured by a loop running
	on its own thread. The thread captures currently pressed keys and sends them to the server.
	The scoreboard draws the ships' health bars in the correct color (although white looks a bit grey).
	The help button opens this file in a MessageBox. Timers are used for each ship to check if they are still
	connected; each timer starts with 2 seconds of time, and is reset each time the client receives data about it.
	If the timer reaches 0, the ship is considered to have been disconnected and removed (meaning its health bar and
	score will no longer be drawn).

	Unit Testing was performed on the Model section of this assignment.  Due to the separation of concerns in the
	structure of the code, this is a fairly effective means of testing the entire assignment. The modeling of the stars,
	ships, projectiles, and world are the structure of the assignment that contain a lot of the assignments fuctionality. 
	There are certain aspects that remaini untested, but would be more visible to the user when playing the game.

	Info is written to the a database to record player score, accuracy and game duration. 
	To close the server, thus ending a game, the word "close" must be entered.  We could not figure out how to get the
	closing of the console application to terminate the program correctly.