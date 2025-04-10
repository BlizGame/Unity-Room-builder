# Unity-Room-builder
Simple Unity tool which allows you to easily connect rooms to each other

# How to use

1) First of all, create room prefabs. In them, you need to add Empty Object to the places where the rooms should be attached to each other. To the points (for each) we add the RoomPoint.cs script
   
2) Next, you need to add the RoomEmpty script to the room prefab.

3) Done! The scripts should simply be stored in your project directory. To use, simply place the room prefab and select it. A new tool will appear at the top left, allowing you to connect points to points when they are next to each other. (If you have downloaded the RoomConnectorAutoSelector script, the tool will be selected automatically when you select a room)
