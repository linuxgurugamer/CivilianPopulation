Civilian Population fork by Pamynx
==================================

TODO
----

- Add Netherdyne as an agency.
- Parts
    - Hydroponics : Hydroponics_large.cfg - Hydroponics_medium.cfg
    - Mining : laserdrill.cfg - megadrill.cfg
    - Recruitment : .cfg

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

Legs are broken, one do not deploy, the other deploy on load.

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

Cannot go inside, should improve attache attach points.

- bioDomeBase
- bioDomeBaseLarge
- bioSphereBase
- bioSphereBaseWallRing
- parkbioDomeBase
- parkbioDomeBaseRock
- parkbioDomeBaseMetal

Attachment point in the air, cannot go "into" the structure.

Agency
------

TODO.

@see KerbalPlanetaryBaseSystems - GameData/PlanetaryBaseInc/BaseSystem/Agencies
@see KerbalPlanetaryBaseSystems - Sources/PlanetarySurfaceStructures/SurfaceStructuresCategoryFilter.cs
