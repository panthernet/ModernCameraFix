# FIXED AND IMPROVED VERSION
Changelog is in the bottom

# ModernCamera
Makes the camera more like an action MMO camera. This includes better control over the third person camera and a first person mode. Action mode allows you to toggle camera rotation so you don't have to always hold down the mouse to rotate the camera.

<details>
<summary>Examples</summary>
<ul>
<li><img src="https://i.imgur.com/bQVtdqg.jpg" alt="example 1"></img></li>
<li><a href="https://www.youtube.com/embed/tMhuAOtTez0">YouTubeVideo</a></li>
</ul>
</details>

YOU NEED LASTEST BEPINEX FOR 2.0 https://github.com/decaprime/VRising-Modding/releases


### Features
- Option and keybinding to enable/disable ModernCamera
- Option to use default build mode camera
- Option to always show a crosshair
- Option for field of view
- Keybinding to hide UI
- First person view
    - Option to enable/disable
    - Alway locks camera rotation
    - Offsets camera height when shapeshifted
- Third person view
    - Keybinding for action mode (action mode locks camera rotation)
    - Option to always show crosshair in action mode
        - _Helpful when using aimed abilities_
    - Option to lock pitch angle
    - Option to lock zoom distance
    - Options to adjust min/max pitch angles
    - Options to adjust min/max zoom
    - Option for over the shoulder offsets
        - _Use this to offset the camera. Useful to avoid always showing name/health on top of screen_
    - Option for aiming offsets
        - _Use this to adjust the aiming reticle position. This is helpful for aimed abilities_
    - Option to lock aim mode forward
        - _Default: will aim towards mouse when rotating the camera_
        - _Forward: will always aim forward when rotating the camera_


### Installation
- Install [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/)
- Extract release .zip content into V Rising root folder


### Configuration
All configuration is done with the in game options menu


### Known Issues
- Can see through floors/roofs from below (this cannot be fixed because objects are missing mesh faces)
- Shadows flicker when looking directly horizontal (Due to fake cloud shadows)


### FAQ
**Q: Why is my characters name and healthbar always shown at top of screen?**

**A:** Because the mouse is locked over the character. Use an over-the-shoulder offset in options to move the mouse off-center so that the mouse is not over the character.

**Q: Why can I see objects popping (loading) in and out in the distance now?**

**A:** This is done for performance by the game. Normally you don't see this because you are looking down at your character. ModernCamera cannot change this.

**Q: Why can't I rotate the camera after hiding the UI?**

**A:** You may have pressed "Enter" which causes the game to lock the camera because the chatbox should normally be open. To fix this, toggle the UI back on, click in the chat box, and press "Enter" again.


### Support
Join the [modding discord](https://vrisingmods.com/discord) for support and tag `@panthernet`


### Giving back
If you like the mod and want to give back, consider buying me a coffee [https://paypal.me/panthernet](https://paypal.me/panthernet)


### Contributors
- iZastic: `@iZastic#0365` on Discord
- Dimentox: `@Dimentox#1154` on Discord
- Kunogi: `@牧瀬紅莉栖#1570` on Discord


### SilkwormReborn
Source code for the SilkwormReborn library [https://github.com/panthernet/SilkwormReborn](https://github.com/panthernet/SilkwormReborn).


### Changelog
`2.0.2`
 - Added Action camera as toggle option (keybinds broken completely)
Action camera is buggy too, cursor isn't switching off I guess.

`2.0.1`
 - Added localization support (language files are stored in BepInEx/Config/ModernCamera)
 - Fixed excessive exception spam
 - Fixed Log call hook
 - Fixed options display in Settings menu

`2.0.0` 
- Updated for the new dlc / v1.0

</details>
