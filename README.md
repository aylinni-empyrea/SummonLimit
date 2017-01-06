# SummonLimit
Allows limiting the maximum allowed amount of summons per player.

Prevents minion hacks(long stardust dragons) by default.

Twins minions are taken into account. Each pair of
retinazer/spazmatism are considered one minion.

## Configuration

No configuration is required. **11** is set by default as it's the
[maximum possible amount in the vanilla game *as of 1.3.4.4*](http://terraria.gamepedia.com/Summon_weapons#Trivia).

Uses *EssentialsPlus-like* dynamic permissions,
which follow the `summonlimit.<number>` format.

>`summonlimit.*` would allow *65534* minions to be summoned at one time.

### Example

    /group addperm somegroup summonlimit.5
