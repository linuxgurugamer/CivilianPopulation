Civilian Population
===================

![Civilian Population](https://github.com/adam-p/markdown-here/raw/master/src/common/images/icon48.png "Civilian Population")

Turn your Kerbal society into a Type 1 Civilization by adding civilian exploration! 
Your agency as research goes on gains the ability to have civilians take part on your exploration.

Once build, your ships, orbital or ground station will welcome civilian that will pay a rent to your agency, breed, grow their kids, get aged and die.
You will also be able to train and recruit civilian to become regular kerbonauts.

As this mod is still under development, all the functionnalities are not available yet.

Credits
-------

This mod is a fork from "Newbier Newb's Revamp of Civilian Population" mod.
It exists thanks to the collective work of :
- [@Tralfagar](http://forum.kerbalspaceprogram.com/index.php?/profile/150801-tralfagar/) that can be found [here](http://forum.kerbalspaceprogram.com/index.php?/topic/143823-120-newbier-newbs-revamp-of-civilian-population/).
- [@michaelhester07](http://forum.kerbalspaceprogram.com/index.php?/profile/96470-michaelhester07/) that can be found [here](http://forum.kerbalspaceprogram.com/index.php?/topic/101058-10x-civilian-population-14/).
- [@rabidninjawombat](http://forum.kerbalspaceprogram.com/index.php?/profile/108889-rabidninjawombat/) that can be found [here](http://forum.kerbalspaceprogram.com/index.php?/topic/111815-104civilian-population-1751-update-to-105-in-progress/).
- [@GGumby](http://forum.kerbalspaceprogram.com/index.php?/profile/122189-ggumby/) that can be found [here](http://forum.kerbalspaceprogram.com/index.php?/topic/140127-112ckan-civilian-populations-revived/).

It is licenced under [CC BY-NC 4.0](https://creativecommons.org/licenses/by-nc/4.0/)

TODO
----

- Add Netherdyne as an agency.

Electrical
----------

- CivPopReactor
- SmallCivPopReactor

Rebalance cost, conversion speed.
Big reator animation won't stop.
Turn the lights off when the reactors are off.

```
    [WRN 00:08:25.564] [ShipConstruct for CivPopReactor]: part cost (15000.0) is less than the cost of its resources (432500.0)
    [WRN 00:08:25.598] [ShipConstruct for SmallCivPopReactor]: part cost (8000.0) is less than the cost of its resources (173000.0)
```

Science
-------

-university

Almost ok, some EVA problems without ladder.

Utility
-------

- InsituKerbalRecruiterTest
- t1CivilizationGenerationShipQuartersMedium
- t1CivilizationGenerationShipQuartersLarge
- t1CivBiomassTank
- t1civGardenBiosphereMedium
- t1civGardenBiosphere
- megadrill.cfg
- pipeNetwork.cfg

All ok.

- civieDockingPort

Part is ok, behaviour to add (Modules CivilianDockGrowth & KerbalRecruitment).

- t1civWasteWaterTank

calibrate these to 1 day worth of waste

- t1CivWaterTank

calbriate these to 1 day worth of food/water

- stbiodomeFarmMk2

Almost ok, top attach point should be removed.

- surfaceAttachHouseSmall

Cannot go back in, no ladder.

```
    [LOG 08:21:59.524] Load(Model): CivilianPopulation/Models/Utility/surfaceAttachHouseSmall
    [ERR 08:21:59.599] Triggers on concave MeshColliders are not supported

    [ERR 08:21:59.600] Triggers on concave MeshColliders are not supported
```

- t1civSmallGardenModule

Part seems ok, animation KO (button toggle, status moving...)


Ground Parts
------------

Legs are broken, one do not deploy, the other deploys on load.

@see http://forum.kerbalspaceprogram.com/index.php?/topic/135250-landing-legs-in-11/&page=1

@see KerbalPlanetaryBaseSystems - GameData/PlanetaryBaseInc/BaseSystem/Parts/Wheels/LandingLeg.cfg

Structural Parts
----------------

- truss18x18NoCore
- truss6x18
- truss6x6Core
- truss6x6CoreL
- truss6x6CoreT
- truss6x6CoreX
- truss6x6NoCore
- bioSphereBaseNoWalls

All seems ok.

- bioSphereWindows
- bioSphereWindowsLarge
- bioSphereWindowsWide

Cannot go inside, should improve attach points.

- bioDomeBase
- bioDomeBaseLarge
- bioSphereBase
- bioSphereBaseWallRing
- parkbioDomeBase
- parkbioDomeBaseRock
- parkbioDomeBaseMetal

Attachment point in the air, cannot go "into" the structure.

Kerbalism Config
----------------
- t1civSmallGardenModule config done.

TODO : Kerbalism config for t1civGardenBiosphereMedium & t1civGardenBiosphere


Agency
------

TODO.

@see KerbalPlanetaryBaseSystems - GameData/PlanetaryBaseInc/BaseSystem/Agencies
@see KerbalPlanetaryBaseSystems - Sources/PlanetarySurfaceStructures/SurfaceStructuresCategoryFilter.cs
