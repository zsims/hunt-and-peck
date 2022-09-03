# hunt-n-peck
[![Build status](https://ci.appveyor.com/api/projects/status/jet85wsdqn10grhk/branch/master?svg=true)](https://ci.appveyor.com/project/zsims/hunt-and-peck/branch/master)

Simple vimium/vimperator style navigation for Windows applications based on the UI Automation framework. In essence, it works the same as screen readers or accessibility programs but with the goal of making any Windows program faster to use.

It works for any Windows program (excluding Modern UI apps :))

NOTE: hunt-n-peck is no longer maintained, please consider one of the various forks.

# Download

https://github.com/zsims/hunt-and-peck/releases/download/release%2F1.6/HuntAndPeck-1.6.zip

# Screenshots

![ScreenShot](https://raw.github.com/zsims/hunt-n-peck/master/screenshots/explorer.png)
![ScreenShot](https://raw.github.com/zsims/hunt-n-peck/master/screenshots/visual-studio.png)

## To use

1. Launch the executable.
2. With any window focused, press `Alt + ;`
    - The tray can be highlighted with `Ctrl + ;`
3. An overlay window will be displayed, type any of the hint characters you see.

Alternatively, Hunt and Peck can be launched via the command-line or AutoHotKey by specifying `/hint`:
```
hap.exe /hint
```

Or in tray mode with
```
hap.exe /tray
```

## To customize

Hotkeys can be found at and modified at %AppData%\\Local\\HuntAndPeck\\[VERSION NUMBER]\\user.config.xml

Please supply a [key code](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=windowsdesktop-6.0) and one or more modifier keys Alt, Control, Shift, Windows.  Seperate them with a comma as seen here.  

# Supported Elements
Only UI Automation elements with "Invoke" patterns are supported (and displayed).
