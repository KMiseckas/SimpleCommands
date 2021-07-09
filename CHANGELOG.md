#### Fixes
#70: Fixed import and method errors due to legacy input.
#75: Replaced incorrect input key checking for `Tilde` with `BackQuote` to show the console when using legacy input.
#76: When not visible, the console no longer processes certain input for legacy and new input system.

[0.2.0] - Internal Release

#### Additions
#24, #10, #7: Added ability to add modular input and output displays with a default implementation that comes with basic features.  
#13: Added ability to select which monobehaviour instance to call the command on by using tags or instance ID's.  
#13: Added ability to call commands on monobehaviour instances.  
#6: Commands are now defined using a [SCCommand] attribute.  
#18: Added comments to all existing classes.  
#31: Added a new class of Display.  
#8: Added support for command suggestions within a suggestion UI panel.  
#15: Added ability to define wether a command should be built for different build configurations when adding as an attribute.  
#16: Added colour coding to input and output.  
#29: Added default commands.  
#36: Added support for legacy and new input system.  
#38, #65: Added support for defining own type and target parsers using attributes.  
#48: SCCommand attribute now contains more constructors to allow easier use.  
#48: SCCommand attribute that does not contain a name definition will attempt to use the method name as a command name.  
#46: Console now prints out the command that has been attempted for execution.  
#43: Added default type and target parsers.  

#### Changes
#31: Refactored how the console displays are set to be visible or hidden.  
#31: Refactored some properties, fields and methods in the SCBase class. #21: Refactored Type parser and Target parser logic.  
#34: Console now uses darker colour shaders for UI display.  
#9: Refactored package structure to allow the project to be imported as a Unity package.  

# [0.1.0]  - Internal Release

- Project setup.