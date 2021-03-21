# SimpleCommands - Unity

Developed on `Unity` `2020.2.4f1` for a project that required quick and simple commands and in-game console display for prototyping. This became an interesting topic and so will continue
further development on this project.

Simple yet effective display window:
![Simple Console](ReadMeAssets/Type%20example.gif)

Resizable from editor, scroll view included:
![Resizable & scroll view](ReadMeAssets/Resize.gif)

Commands can be created in classes that extend from `CommandDefinitions.cs`, these support parameters using generics (future work planned to improve this):
![Command Creation](ReadMeAssets/Command%20Creation.png)

Requires the new Unity Input System.

Still in `DEVELOPMENT` but usable for simple commands and in game console message display.

# Planned features:
- [Command] attribute for easy command assignment.
- Component based to allow user made consoles to be attached for output/input.
- Added basic type support for commands with complex parameters.
- Auto-complete for commands.
