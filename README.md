# WinSize4
WinSize4 is an application that moves and resizes windows according to your settings. It can set windows position and size over multiple screens, and reset them if the setup changes, for example if a screen is attached or removed.
# History
I want more control over where windows are located, and sized, than what Windows can offer.
I have since a long time used WinSize2, but that application is missing a few features and is not maintained since long.<br>
https://winsize2.sourceforge.net/en/index.html <br>
Later a WinSize3 has emerged, but that is also lacking some functionality, in my opinion.<br>
https://github.com/Hamster128/WinSize3 <br>
# My take
WinSize4 runs in the background consuming very little resources. Every time a window gets focus it checks its database if a window with the same criteria exists, and if so places and resizes it. The criteria is which executable that runs it and / or the title of the window, and which screen the window is on.

![main 1.1.3](https://github.com/user-attachments/assets/bc878d8d-49c9-4b3c-9bb7-b23af5ac10f8)

The list shows which windows WinSize4 is monitoring, name of the entry, size of the screen it should be on, and if that screen is primary or not. Lines in grey are for a screen that is not currently present. There is a check mark in the bottom that select if only the windows belonging to present screens should be shown, or all.
## Name
The name of the saved Window
## Window Class
The class of window the application is registered with, can be selected if this shuld be considered when the windows is evaluated for move.

## Window Title

The title of the window, might not be visible. Can be selected if this shuld be considered when the windows is evaluated for move. If considered, it is possible to select wether the full title should be considered, exact match, if the text should be somewhere in the title, or if the title should begin with the text.

### Include and Exclude

You can include and / or exclude a part of, or entire, title. You can add several texts divided with a pipe character (|). Exclude takes precedence over Include, if the same text is added to both Include and Exclude, will the matched window be excluded.

## Executable

The name of the exe file that has opened the window. Can be selected if this shuld be considered when the windows is evaluated for move.

## Left and Top

To what distance from the left and top edge of the screen the window should be moved. If 'Custom' is selected will these always be '0'.

## Width and Height

The width and height the window shall have. If 'Custom' is checked will these be the 'Custom working area' values for the screen the window is on.

If the checbox is unticked will Winsize4 not change the windows size.

## Full Screen

The window will be set to 'full screen'.

## Remove

This button removes the selected window from the list, pressing the 'Delete' button on the keyboard does the same thing.

## Ignore Child Windows

When for example 'Save As' is pressed in an application will that window spawn a new window with the 'save as' functions. This newly spawned window is a 'child window'. If this selection is checked will child windows not be resized or moved.

Note that some programs show a window with for example log in information before the main window openes. This is not a child window, and in those cases this check might be better not checked. By saving this application as two windows with different 'Title', can both the first window, and the main window be handles properly.

Also note that File Explorer actually is a child process of Explorer.exe, so this check should probably be un-checked. This also goes for Edge, two windows running Edge share the same instance of the process 'msedge', so each window will be considered a child to the others.

'Ignore Child Windows' is checked by default, except for the executables 'explorer' and 'msedge'.

## Always move
The window will be moved whenever it gets focus, even if it has already been moved. This also goes for a browser where the tab is set as title of the window, when the tab gets focus.

## Reset moved if new screen is detected

If WinSize4 should detect that a screen has been removed, or a new one has been connected, will all windows be eligable for move, if this selection is checked.

## Reset Moved

This button resets the moved status of all windows and make them eligable for move.

## Pause
When selected will WinSize4 not affect any windows.

# What screen?
How does WinSize4 know what screen to place the window on? The screens are identified by the resolution they have and whether they are the primary screen or not.

When a window gets focus does WinSize4 check if that window has a line in the configuration that matches the screen it is currently on, the same dimensions and if it is the primary screen. If so, it resizes the window on that screen. If not, it moves the window to the first screen that matches.

# Moved

Once a window has been moved and / or resized, will it be marked so that it will not be moved again. That is, after a window has been moved can you resize it and it stays. If you close the application and restart it, it will again be placed and resized.

# Screens
![screens](https://github.com/tomasrudh/WinSize4/assets/24943208/c6a41d69-c880-425b-9f55-9a07c9faebe5)


'Edit screens' on the main window takes you to a window where you can set some properties for the screens.

The list shows all screens WinSize4 has seen, greyed out ones are not available right now.

## Full area
This is the full size, out to the edges of the screen, this is the dimensions WinSize4 uses to select what screen to place the windows on.

## Working area
This is what Windows considers the usable area, excluding the task bar area.

## Custom working area
You can set your own working area, for example exclude an area to the right for gadgets that you don't want covered by windows.
