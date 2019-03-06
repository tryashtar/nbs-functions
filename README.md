# <img src="https://i.imgur.com/kbKjing.png" width=48> NBS to Functions
### [Download link](https://github.com/tryashtar/nbs-functions/releases)

This is a simple Windows application that converts a [Note Block Studio](https://www.stuffbydavid.com/mcnbs) file into a bunch of [functions](https://minecraft.gamepedia.com/Function) which can be added to a data pack. You can play your entire song by activating a single command block chain.

### How to Use
Acquire a `.nbs` file somehow, by downloading it or creating it in Note Block Studio. The song is required to use only vanilla sounds and pitches. Download and open the application and click `Open NBS File`. Browse for your song and select it.

You can increase or decrease the BPM, which will default to whatever it was in the file. The max BPM, 1200, can play 20 adjacent notes every second.

You can also increase or decrease the number of functions to generate. The fewer functions you have, the more commands will be forced to run every tick. Maximum function count means it runs exactly one command for each note in a given tick. Since functions are cheap (especially when zipped in a data pack), I recommend leaving the function count high.

Click `Save Functions` and browse to your existing data pack, inside `(world)/datapacks/(your pack)/data`. Type any lowercase word in the box and save. All the function files will be saved to this data pack.

Click `Copy Command` and a command will be copied to your clipboard. Enter Minecraft and paste the command into chat. Press enter and you will receive a command block. Placing that command block will create a chain of one or more command blocks. The song will play while the blue one is receiving power.

### Foresight FAQ
> Why can't I use notes outside the vanilla pitch range?

Because unfortunately, `/playsound` is limited to the same pitches that note blocks are. You would need to create a custom sound (which is addressed below.

> Why can't I use custom sounds from a resource pack?

Because if you're using a resource pack, this entire application serves no purpose. You should instead make the entire song a single sound and simply use `/playsound` to play it directly.

> How do I make a data pack?

In the future, I'd like this application to automatically create a data pack for you if you simply choose to save in your world. Until then, a data pack goes world's `datapacks` folder. Create a folder with any name, then create a folder called `data` inside of that. Also create a file called `pack.mcmeta` with these contents: `{"pack":{"pack_format":1,"description":"NBS"}}`.
