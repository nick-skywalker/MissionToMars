# MissionToMars

## WebApi aboard in a rover on Mars

This web api recivied a series command for moving rover from remote:
1) l = turn left
2) r = turn right
3) f = move forward
4) b = move backward

The rover have data contains obstacles coordinate and map of planet.
If the commands move the rover towards an obstacle, it stops and report the obstacle.

## Client on Earth

The client show map of red planet (Mars in 2d - taken by rover) and current position and direction rover, 
also show obstacles in the planet, taken by rover that with your image recognition 
it have make a json files with their coordinate (obstacles).

## Test web api

The tests test moving rover, wrapping planet, incident with an obstacle.
