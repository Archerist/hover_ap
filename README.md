# Hover Archipelago Client

You'll need [Hover](https://www.pcgamingwiki.com/wiki/Hover_(2017)), [MelonLoader 0.5.7](https://melonwiki.xyz/) and [Archipelago.MultiClient.Net](https://github.com/ArchipelagoMW/Archipelago.MultiClient.Net/releases)

For some reason MelonLoader 0.6.1 won't work.

Archipelago.MultiClient.Net is in packages and this is working with 5.0.5

Build events should copy the DLLs from `packages/Archipelago.MultiClient.Net.<version>/lib/net35` into `<HOVER>/UserLibs`, you can do it manually if it doesn't.

To develop this just clone it inside the Hover folder

Right now it lies to the AP Server by sending Timespinner info