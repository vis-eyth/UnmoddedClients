## UnmoddedClients - BepInEx plugin for allowing unmodded clients to join

##### PLEASE NOTE: Due to the server sending the buildid to the client, i can't know what buildid the client is running on.

Allows you to invite people without mods while having a modded game. Does not enable quickplay or prismatic trials.

---

### Features
#### Fully configurable in the config.
Set your standard buildid to whatever you prefer:
- `steam` - Automatically uses unmodded buildid.
- `mod` - Automatically uses modded buildid.
- Anything else - uses that, as a buildid.

#### Multiple console functions:
- `build_id_mod` - Sets your buildid to one representing a modded game. No arguments.
- `build_id_steam` - Sets your buildid to one representing an unmodded game. No arguments.
- `build_id_custom` - Use this to set your game to a custom buildid, for example to play with friends using a different version of the game. (That's not recommended by the way) One argument.

---

##### PLEASE NOTE: This is _still_ very experimental, and I'd love to hear feedback. I'm on the [modding discord](https://discord.gg/hMdjd9y "Risk of Rain 2 Modding") - my handle is viseyth#3934.