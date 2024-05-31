# Handheld-Hardware-Tools

This software is aimed to be a one stop shop for windows gaming handhelds. It is designed to be controller friendly and convenient to use and not instrusive while working with other software seamlessly. 

See my video below!

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/hWI5Q30ikgY/0.jpg)](https://youtu.be/hWI5Q30ikgY)


# PLEASE READ THIS:
THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. MISUSE OF THIS SOFTWARE COULD CAUSE SYSTEM INSTABILITY OR MALFUNCTION.

## Download
[Download](https://github.com/project-sbc/Handheld-Hardware-Tools/releases)

## Current Features.
### Power Controls
TDP, max cpu clock, max cores, EPP, and gpu clock (for amd 4000 and amd 6000 and newer devices).

### Display Controls
Change resolution, refresh rate, and display scaling quickly.

### System Controls
Change brightness and volume and toggle mute, wifi and BT.

### Customizable Home Page
The controls on the home can be rearranged in any order or disabled.

### Mouse Mode
Configure your controller to act as a mouse/keyboard. 

### Settings
Ability to change appearances such as menu on left or right and update settings. For Intel devices, you can change the TDP mode to best suit your device. Auto start and auto update are additional setting parameters. Ability to change max/min parameters can also be configured.

# Compatible Devices (Windows only)
## Aya
All aya devices are compatible with most features. Only newer devices such as the aya air and aya neo 2 will have fan control support.

## One Netbook
All one netbook devices are compatible with most features. Only new devices such as the mini pro AMD and one x player 2 will have fan support.

## GPD
All GPD devices are compatible with most features. Only new devices such as the Win Max 2 AMD and win 4 will have fan support.

## Steam
Steam Deck is compatible but some features may not work due to the controller implementation. No fan support.

## Anbernic
Win600. No fan support.

## Intel Laptops
4th generation or newer laptops or mini PCs. No fan support.

## AMD Laptops
Most ryzen laptops or mini PCs. No fan support.

# Donate
<p>If you feel this software has helped you, please consider donating. I enjoy doing this for the community and am a consumer of my own software, so I don't do it for the money, but I have easily put 100's of hours into this project to bring you the best possible experience. Thank you.</p>
https://ko-fi.com/project_sbc

https://www.paypal.com/donate?business=NQFQSSJBTTYY4&currency_code=USD

# Fix Error on Intel Devices Running Windows 11 22H2

Go to RegEdit to the following address:  Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config
change the DWORD value to 0 like the screenshot below.
Restart the computer

![Intel22H2](https://github.com/project-sbc/Power-Control-Panel-v2/blob/master/Intel%2022H2%20driver%20fix.jpg?raw=true "Intel 22H2 fix")

# Projects incorporated into this
<ul>
  <li>RyzenAdj</li>
  <li>GameLib.net</li>
  <li>QRes</li>
</ul>

# Special Thanks

<ul>
  <li>HMR / Handheld Master Race (Youtube - https://www.youtube.com/@hmr0/featured) - for help with the user interface design and layout</li>
  <li>Askarus (Discord)/Mystechry (Youtube - https://www.youtube.com/@mystechry) - for extensive help with testing early builds, suggesting several features seen in the current build</li>
  <li>Akaraah-3D (Discord) - for extensive testing and discovery of many bugs</li>
  <li>Hyatice (Discord) - for help with AutoTDP, amazing power saving profile, and some testing</li>
  <li>Frank (Motion Assistant Developer) - for help with fan controls</li>
  <li>Ben/CasperH (Handheld Companion Developers) - for feedback/general contributions. Check out Handheld Companion here for gyro support on windows handhelds https://github.com/Valkirie/HandheldCompanion</li>
  <li>JamesCJ (RyzenAdj Developer, UXTU Developer) - for Aya Neo 2 testing/general contributions. Check out his other projects here https://github.com/JamesCJ60/Playmate-Game-Launcher   https://github.com/JamesCJ60/Universal-x86-Tuning-Utility </li>
  <li>ETA Prime for the shoutout on Youtube</li>
  <li>Russian translation by ion2903 (Discord)</li>
  <li>Chinese translation by ProjectSBC, Kiki (formerly One Netbook employee), handtalker (Discord)</li>
  <li>Portugese translation by estefano.jol (Discord)</li>  
  <li>Japanese translation by tomoniutaou33 (Discord)</li>
  <li>Korean translation by motsa (Discord)</li>
  <li>Spanish translation by kikeminchas (Discord)</li>
</ul>


# Contact

You can email me about device support or other suggestions at handheld.hardware@outlook.com




