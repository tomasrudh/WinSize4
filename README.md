WinSize4 is an application that moves and resizes windows according to your settings. I want more control over where windows are located, and sized, that what Windows can offer.
I have since a long time used WinSize2, but that application is missing a few features and is not maintained since long.<br>
https://winsize2.sourceforge.net/en/index.html <br>
Later a WinSize3 has emerged, but that is also lacking some functionality, in my opinion.<br>
https://github.com/Hamster128/WinSize3 <br>
<br>
## WinSize4.
<br>
WinSize4 runs in the background consuming very little resources. Every time a window gets focus does it check its database if a window with the same criteria exists, and if so places and resizes it. The criteria is which executable that runs it and / or the title of the window, and which screen the window is on.
![image](https://user-images.githubusercontent.com/24943208/227801558-157a4e7b-0a1b-465e-b031-b96e093dde33.png)
# The list shows which windows WinSize4 is monitoring, name of the entry, size of the screen it should be on, and if that screen is primary or not. Lines in grey are for a screen that is not currently present. There is a check mark in the bottom that select if only the windows belonging to present screens should be shown, or all.

# Name
The name of the saved Window

# Window Class
The class of window the application is registered with, can be selected if this shuld be considered when the windows is evaluated for move.

# Window Title

The title of the window, might not be visible. Can be selected if this shuld be considered when the windows is evaluated for move. If considered, it is possible to select wether the full title should be considered, exact match, if the text should be somewhere in the title, or if the title should begin with the text.

# Executable

The name of the exe file that has opened the window. Can be selected if this shuld be considered when the windows is evaluated for move.

# Left and Top

To what distance from the left and top edge of the screen the window should be moved. If 'Custom' is selected will these always be '0'.

# Width and Height

The width and height the window shall have. If 'Custom' is checked will these be the 'Custom working area' values for the screen the window is on.

# Full Screen

The window will be set to 'full screen'.

# Remove

This button removes the selected window from the list, pressing the 'Delete' button on the keyboard does the same thing.

# Ignore Child Windows

When for example 'Save As' is pressed in an application will that window spawn a new window with the 'save as' functions. This newly spawned window is a 'child window'. If this selection is checked will child windows not be resized or moved.

Note that some programs show a window with for example log in information before the main window openes. This is not a child window, and in those cases this check might be better not checked. By saving this application as two windows with different 'Title', can both the first window, and the main window be handles properly.

Also note that File Explorer actually is a child process of Explorer.exe, so this check should probably be un-checked. This also goes for Edge, two windows running Edge share the same instance of the process 'msedge', so each window will be considered a child to the others.

'Ignore Child Windows' is checked by default, except for the executables 'explorer' and 'msedge'.

# Reset moved if new screen is detected

If WinSize4 should detect that a screen has been removed, or a new one has been connceted, will all windows be eligable for move, if this selection is checked.

# Reset Moved

This button resets the moved status of all windows and make them eligable for move.






# Detection of windows

Two times per second does WinSize4 check if the window that currently has focus is where it is supposed to be, otherwise it is moved there, and possible resized.

# Moved

Once a window has been moved and / or resized, will it be marked so that it will not be moved again.